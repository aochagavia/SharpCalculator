using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc
{
    class Scanner
    {
        public static Token[] Scan(string s)
        {
            var reader = new StringReader(s);
            var tokens = new List<Token>();

            while (reader.Peek() != -1)
            {
                discardWhitespace(reader);

                // PrimaryToken
                Token t = PrimaryToken.FromReader(reader);
                if (t != null)
                {
                    tokens.Add(t);
                    continue;
                }

                // SecondaryToken
                t = SecondaryToken.FromReader(reader);
                if (t != null)
                    tokens.Add(t);
                else
                    throw new Exception(String.Format("Unrecognized token: {0}", (char)reader.Peek()));
            }

            // Return the token array when there are no more characters to read
            return tokens.ToArray();
        }

        private static void discardWhitespace(StringReader reader)
        {
            while (true)
            {
                if (Char.IsWhiteSpace((char)reader.Peek()))
                    reader.Read();
                else
                    break;
            }
        }
    }

    abstract class Token { }
    class PrimaryToken : Token
    {
        public static Token FromReader(StringReader reader)
        {
            Token t = null;
            switch ((char)reader.Peek())
            {
                case '(': t = new LPar(); break;
                case ')': t = new RPar(); break;
            }

            if (t != null) reader.Read();

            return t;
        }
    }
    class LPar : PrimaryToken { }
    class RPar : PrimaryToken { }

    class SecondaryToken : Token
    {
        public static Token FromReader(StringReader reader)
        {
            string word = reader.ReadUntil(c => Char.IsWhiteSpace(c) || c == '(' || c == ')');

            // Try to read an operator
            Operator o = Operator.FromString(word);
            if (o != null)
                return o;

            // Try to read a literal (a double)
            double x;
            if (Double.TryParse(word, out x))
                return new Literal(x);

            // Otherwise this is a name
            return new Name(word);
        }
    }

    class Literal : SecondaryToken
    {
        public double Value { get; protected set; }
        public Literal(double d) { this.Value = d; }
    }

    class Name : SecondaryToken
    {
        public String Value { get; protected set; }
        public Name(string v) { this.Value = v; }
    }

    // Operators
    abstract class Operator : SecondaryToken
    {
        protected abstract double processArgs(Expression[] args);

        public double Evaluate(Expression[] arguments)
        {
            return this.processArgs(arguments);
        }

        public static Operator FromString(string s)
        {
            switch (s)
            {
                case "+": return new Add();
                case "-": return new Sub();
                case "*": return new Mul();
                case "/": return new Div();
                default: return null;
            }
        }
    }

    class Add : Operator
    {
        protected override double processArgs(Expression[] args)
        {
            return args.Aggregate(0d, (a, b) => a + b.Evaluate());
        }
    }

    class Sub : Operator
    {
        protected override double processArgs(Expression[] args)
        {
            if (args.Length == 0)
                throw new Exception("Subtraction requires at least one argument");

            double first = args[0].Evaluate();
            return args.Skip(1).Aggregate(first, (a, b) => a - b.Evaluate());
        }
    }

    class Mul : Operator
    {
        protected override double processArgs(Expression[] args)
        {
            return args.Aggregate(1d, (a, b) => a * b.Evaluate());
        }
    }

    class Div : Operator
    {
        protected override double processArgs(Expression[] args)
        {
            if (args.Length != 2)
                throw new Exception("Division requires at least two arguments");

            return args[0].Evaluate() / args[1].Evaluate();
        }
    }
}
