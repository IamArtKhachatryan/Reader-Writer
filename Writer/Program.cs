class Program
{
    static void Main()
    {
        string filePath = "C:\\Users\\DELL\\Desktop\\shared.txt";
        //string filePath = "C:\\Users\\user\\Desktop\\shared.txt";

        bool fileHasContent = File.Exists(filePath) && new FileInfo(filePath).Length > 0;

        string[][] stepOptions;
        string[] stepTitles;

        if (fileHasContent)
        {
            stepTitles = ["File already has content. What to do?", "Choose flush mode:"];
            stepOptions =
            [
                ["Append (keep old content)", "Overwrite (clear file)"],
                ["Auto (flush after every line)", "Manual (flush only on command)"]
            ];
        }
        else
        {
            stepTitles = ["Choose flush mode:"];
            stepOptions =
            [
                ["Auto (flush after every line)", "Manual (flush only on command)"]
            ];
        }

        int[] answers = ConsoleMenu.RunMenuSteps(stepTitles, stepOptions);

        bool overwrite = false;
        bool autoFlush;

        if (fileHasContent)
        {
            overwrite = answers[0] == 1;
            autoFlush = answers[1] == 0;
        }
        else
        {
            autoFlush = answers[0] == 0;
        }

        string modeName = autoFlush ? "Auto" : "Manual";

        Console.WriteLine("Hotkeys: Esc - exit" + (autoFlush ? "" : ", F2 - flush"));
        Console.WriteLine();

        using (StreamWriter writer = new StreamWriter(filePath, !overwrite))
        {
            while (true)
            {
                Console.Write("[" + modeName + "] > ");

                bool exitRequested;
                bool flushRequested;
                string line = ReadLineWithHotkeys(autoFlush, out exitRequested, out flushRequested);

                if (exitRequested)
                {
                    Console.WriteLine();
                    break;
                }

                if (flushRequested)
                {
                    writer.Flush();
                    Console.WriteLine();
                    Console.WriteLine("[flushed to file]");
                    continue;
                }

                if (line.Length > 0)
                {
                    writer.WriteLine(line);

                    if (autoFlush)
                        writer.Flush();
                }
            }
        }
    }

    static string ReadLineWithHotkeys(bool autoFlush, out bool exitRequested, out bool flushRequested)
    {
        exitRequested = false;
        flushRequested = false;
        string buffer = "";

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                exitRequested = true;
                return buffer;
            }

            if (!autoFlush && keyInfo.Key == ConsoleKey.F2)
            {
                flushRequested = true;
                return buffer;
            }

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return buffer;
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (buffer.Length > 0)
                {
                    buffer = buffer.Substring(0, buffer.Length - 1);
                    Console.Write("\b \b");
                }
                continue;
            }

            if (!char.IsControl(keyInfo.KeyChar))
            {
                buffer = buffer + keyInfo.KeyChar;
                Console.Write(keyInfo.KeyChar);
            }
        }
    }
}