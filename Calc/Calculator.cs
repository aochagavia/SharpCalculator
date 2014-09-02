using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc
{
    class Calculator
    {
        public static double Evaluate(string s)
        {
            Token[] tokens = Scanner.Scan(s);
            Expression expr = Parser.Parse(tokens);
            return expr.Evaluate();
        }
    }
}
