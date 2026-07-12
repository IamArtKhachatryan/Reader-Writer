static class ConsoleMenu
{

    public static int[] RunMenuSteps(string[] titles, string[][] optionsPerStep)
    {
        int stepCount = titles.Length;
        int[] answers = new int[stepCount];
        for (int i = 0; i < stepCount; i++)
            answers[i] = 0;

        int currentStep = 0;

        while (currentStep < stepCount)
        {
            bool canGoBack = currentStep > 0;
            int result = ShowMenu(titles[currentStep], optionsPerStep[currentStep], answers[currentStep], canGoBack);

            if (result == -1)
            {
                currentStep = currentStep - 1;
            }
            else
            {
                answers[currentStep] = result;
                currentStep = currentStep + 1;
            }
        }

        return answers;
    }

    public static int ShowMenu(string title, string[] options, int initialSelected, bool canGoBack)
    {
        int selected = initialSelected;
        ConsoleKey key;

        Console.CursorVisible = false;

        do
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selected)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("> " + options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("  " + options[i]);
                }
            }

            Console.WriteLine();
            Console.WriteLine(canGoBack ? "(Escape - back)" : "(Escape - exit)");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            key = keyInfo.Key;

            if (key == ConsoleKey.UpArrow)
            {
                selected = selected - 1;
                if (selected < 0)
                    selected = options.Length - 1;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                selected = selected + 1;
                if (selected > options.Length - 1)
                    selected = 0;
            }
            else if (key == ConsoleKey.Escape)
            {
                Console.CursorVisible = true;
                Console.Clear();

                if (canGoBack)
                    return -1;

                Environment.Exit(0);
            }

        } while (key != ConsoleKey.Enter);

        Console.CursorVisible = true;
        Console.Clear();

        return selected;
    }
}