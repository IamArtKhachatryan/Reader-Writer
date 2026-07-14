using System;
using System.IO;
using System.Threading;

namespace Reader
{
    class Program
    {
        static void Main()
        {
            string filePath = "C:\\Users\\DELL\\Desktop\\shared.txt";
            
            Console.WriteLine("Watching file: " + filePath);
            Console.WriteLine("Press Escape to stop.");
            Console.WriteLine();

            Console.WriteLine("Waiting for file to be created...");
            
            //while there is no such a file check every 300 miliseconds
            while (!File.Exists(filePath))
            {
                Thread.Sleep(300);
            }
            Console.WriteLine("File found. Watching for changes...");
            Console.WriteLine();

            FileTailReader tailer = new FileTailReader(filePath);

            while (true)
            {
                string newText = tailer.ReadNewContent();

                if (newText.Length > 0)
                {
                    Console.Write(newText);
                }

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }

                Thread.Sleep(500);
            }

            Console.WriteLine();
            Console.WriteLine("Stopped watching.");
        }
    }
}
