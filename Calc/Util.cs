using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc
{
    static class Util
    {
        public static string ReadUntil(this StringReader reader, Func<char, bool> stop)
        {
            var builder = new StringBuilder();
            while (reader.Peek() != -1 && !stop((char)reader.Peek()))
                builder.Append((char)reader.Read());

            return builder.ToString();
        }
    }
}
