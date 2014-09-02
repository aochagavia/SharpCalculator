using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    double result = Calculator.Evaluate(Console.ReadLine());
                    Console.WriteLine("Result: {0}", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.Message);
                }
            }
        }
    }
}
