using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
namespace Compil
{
    class Program
    {
        static void Main()
        {
            InputOutput.Init("test.pas");
            SyntaxAnalyzer sa = new SyntaxAnalyzer();
            sa.Analyze();
            InputOutput.End();
        }
    }
}