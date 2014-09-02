using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc
{
    class Parser
    {
        public static Expression Parse(Token[] tokens)
        {
            // Strangely, the enumerator over Token[] yields `object` instead of `Token`
            // We use a list to avoid this behavior
            var ts = new List<Token>(tokens).GetEnumerator();

            if (ts.MoveNext() && ts.Current is LPar)
                return innerParse(ts);
            else
                throw new Exception("Parentheses not present or wrongly formatted");
        }

        private static Expression innerParse(IEnumerator<Token> ts)
        {
            if (!ts.MoveNext()) throw new Exception("Unexpected end of token stream");

            Operator op = ts.Current as Operator;
            if (op == null) throw new Exception(String.Format("Unexpected token '{0}' expecting an operator", ts.Current));

            var args = new List<Expression>();

            while (ts.MoveNext())
            {
                // Begin of an expression
                if (ts.Current is LPar)
                {
                    args.Add(innerParse(ts));
                }
                // End of the current expression
                else if (ts.Current is RPar)
                {
                    return new OperatorExpression(op, args.ToArray());
                }
                // A number
                else if (ts.Current is Literal)
                {
                    args.Add(new NumberExpression((ts.Current as Literal).Value));
                }
                // A name
                else if (ts.Current is Name)
                {
                    throw new Exception("Names are not yet supported");
                }
                else
                {
                    throw new Exception(String.Format("Unexpected token '{0}', expecting an expression", ts.Current));
                }
            }

            // This point should never be reached
            throw new Exception("Unexpected end of token stream");
        }
    }

    abstract class Expression
    {
        public abstract double Evaluate();
    }

    class OperatorExpression : Expression
    {
        protected Operator op;
        protected Expression[] args;

        public OperatorExpression(Operator o, Expression[] es) { this.op = o; this.args = es; }

        public override double Evaluate()
        {
            return op.Evaluate(this.args);
        }
    }

    class NumberExpression : Expression
    {
        protected double value;

        public NumberExpression(double v) { this.value = v; }

        public override double Evaluate() { return this.value; }
    }
}
