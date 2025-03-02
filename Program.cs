using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Parser
{
    class MainClass
    {

        public static void Main(string[] args)
        {

            if (args.Length < 1)
            {
                Console.WriteLine("Usage; {0} [-t | <filename>]", Process.GetCurrentProcess().ProcessName);
                return;
            }

            try
            {
                StreamReader input;

                if (args[0] == "-t")
                {
                    input = new StreamReader(Console.OpenStandardInput());
                }
                else
                {
                    input = new StreamReader(args[0]);
                }

                string prg = input.ReadToEnd();
                byte[] data = Encoding.ASCII.GetBytes(prg); MemoryStream stream = new MemoryStream(data, 0, data.Length);
                Scanner l = new Scanner(stream);
                Parser p = new Parser(l);
                TypeChecker typeCheck = new TypeChecker();

                if (p.Parse())
                {
                    if (p.Program.Accept(typeCheck).Equals(Type.OK))
                        Console.WriteLine("pass");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            /*byte[] data = Encoding.ASCII.GetBytes("// fail 6 1\r\n\r\nvoid main() {\r\n  int x; x = f();\r\n}\r\nint f() {}");
            MemoryStream stream = new MemoryStream(data, 0, data.Length);
            Scanner l = new Scanner(stream);
            Parser p = new Parser(l);
            TypeChecker typeCheck = new TypeChecker();

            if (p.Parse())
            {
                if (p.Program.Accept(typeCheck).Equals(Type.OK))
                    Console.WriteLine("pass");
            }*/
        }
    }
}
