using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using static Resources;
class Gameloop
{
    internal static void GameLoop()
    {
        _availableChars.AddRange(allChars);
        var rand = new Random();
        int randomIndex = rand.Next(words.Length);
        string selectedWord = words[randomIndex];
        List<char> selectedWordUniqueOnly = new(selectedWord.Distinct().ToArray());
        Console.WriteLine(hangman_ASCII_Sprites[failedAttempts].ToString());
        char[] DiscoveredChars = new char[selectedWord.Length];

        if (Locale == "Русский")
        {
            randomIndex = rand.Next(russianWords.Length);
            selectedWord = russianWords[randomIndex];
            allChars = allRussianChars;
            _availableChars.Clear();
            _availableChars.AddRange(allRussianChars);
            DiscoveredChars = new char[selectedWord.Length];
            selectedWordUniqueOnly = new(selectedWord.Distinct().ToArray());
        }
        Fill(DiscoveredChars, '_');
        for (int i = 0; i < allChars.Length; i++)
        {
#if DEBUG
            Console.WriteLine(selectedWord);
#endif
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            char KeyChar = char.ToLower(keyInfo.KeyChar);
            bool IsLetter;
            bool IsAvailable;
            bool IsInWord;
            Validation(KeyChar, selectedWord, out IsLetter, out IsAvailable, out IsInWord, DiscoveredChars);
#if DEBUG
            if (IsLetter)
            {
                Console.WriteLine($"Debugging: IsLetter Check True, Target: {KeyChar}");
                if (IsAvailable)
                {
                    Console.WriteLine($"Debugging: IsAvaliable Check True, Target: {KeyChar}");
                    if (IsInWord)
                    {
                        UserInputResult(_availableChars, selectedWordUniqueOnly, failedAttempts, KeyChar, ResultSwitchStates.SUCCESSFUL, DiscoveredChars);
                    }
                    else
                    {
                        failedAttempts++;
                        UserInputResult(_availableChars, selectedWordUniqueOnly, failedAttempts, KeyChar, ResultSwitchStates.FAILED, DiscoveredChars);
                    }
                }
                else
                {
                    Console.WriteLine($"Debugging: IsAvaliable Check False, Target: {KeyChar}");
                    i--;
                    Console.WriteLine($"The Character you entered is incorrect, the avaliable characters are: \n{string.Join(", ", _availableChars)}\n\n{hangman_ASCII_Sprites[failedAttempts]}");
                }
            }
            else
            {
                Console.WriteLine($"Debugging: IsLetter Check False, Target: {KeyChar}");
                i--;
                Console.WriteLine($"The Character you entered is incorrect, the avaliable characters are: \n{string.Join(", ", _availableChars)}\n\n{hangman_ASCII_Sprites[failedAttempts]}");
            }
#else
            if (IsLetter)
            {
                if (IsAvailable)
                {
                    if (IsInWord)
                    {
                        UserInputResult(_availableChars, selectedWordUniqueOnly, failedAttempts, KeyChar, ResultSwitchStates.SUCCESSFUL, DiscoveredChars);
                    }
                    else
                    {
                        failedAttempts++;
                        UserInputResult(_availableChars, selectedWordUniqueOnly, failedAttempts, KeyChar, ResultSwitchStates.FAILED, DiscoveredChars);
                    }
                }
                else
                {
                    i--;
                    Console.Clear();
                    if (Locale == "Русский")
                    {
                        Console.WriteLine($"\nВведенный вами символ недопустим, допустимыми являются следующие символы: \n{string.Join(", ", _availableChars)}\n\n{hangman_ASCII_Sprites[failedAttempts]}");
                    }
                    else
                    {
                        Console.WriteLine($"The Character you entered is incorrect, the avaliable characters are: \n{string.Join(", ", _availableChars)}\n\n{hangman_ASCII_Sprites[failedAttempts]}");
                    }
                }
            }
            else
            {
                i--;
                Console.Clear();
                if (Locale == "Русский")
                {
                    Console.WriteLine($"\nВведенный вами символ недопустим, допустимыми являются следующие символы: \n{string.Join(", ", _availableChars)}\n\n{hangman_ASCII_Sprites[failedAttempts]}");
                }
                else
                {
                    Console.WriteLine($"The Character you entered is invalid, the avaliable characters are: \n{string.Join(", ", _availableChars)}\n\n{hangman_ASCII_Sprites[failedAttempts]}");
                }
            }
#endif
        }
    }
    /// <summary>
    /// Method <c>Validation</c> contains 3 checks, isLetter, isAvailable and isInWord.
    /// </summary>
    /// <param name="KeyChar">The character the user has entered</param>
    /// <param name="selectedWord">The chosen word for Hangman</param>
    /// <param name="Check1">The output of the first check, checks if a letter is a english letter</param>
    /// <param name="Check2">The output of the second check, checks if a letter is available</param>
    /// <param name="Check3">The output of the third check, checks if a letter is in the selected word</param>
    internal static void Validation(char KeyChar, string selectedWord, out bool Check1, out bool Check2, out bool Check3, char[] DiscoveredChars)
    {
        Check1 = false;
        Check2 = false;
        Check3 = false;
        string pattern = @"^[a-zA-Z]+$";
        if (Locale == "Русский")
        {
            pattern = @"^[а-яА-Я]+$";
        }
        bool isLetter = Regex.IsMatch(KeyChar.ToString(), pattern);
        bool isAvailable = _availableChars.Contains(KeyChar);
        bool isInWord = selectedWord.Contains(KeyChar);
        if (isLetter)
        {
            Check1 = true;
            if (isAvailable)
            {
                Check2 = true;
                if (isInWord)
                {
                    Check3 = true;
                    DiscoveredCharsDisplay(selectedWord, KeyChar, DiscoveredChars);
                }
            }
        }
    }
    internal static void UserInputResult(List<char> availableChars, List<char> selectedWordUniqueOnly, int failedAttempts, char KeyChar, ResultSwitchStates ResultSwitch, char[] DiscoveredChars)
    {
        switch (ResultSwitch)
        {
            case ResultSwitchStates.SUCCESSFUL:
            {
                availableChars.Remove(KeyChar);
                selectedWordUniqueOnly.Remove(KeyChar);
                if (selectedWordUniqueOnly.Count == 0)
                {
                    TextReader originalInput = Console.In;
                    TextWriter originalOutput = Console.Out;
                    if (Locale == "Русский")
                    {

                        Console.WriteLine("Ты выиграл!");
                        Console.WriteLine("Игра закроется через пять секунд");
                        Console.SetIn(TextReader.Null);
                        Console.SetOut(TextWriter.Null);
                        Console.ReadKey();
                        Thread.Sleep(5000);
                        Environment.Exit(0);
                    }
                    Console.WriteLine("You Win!");
                    Console.WriteLine("The game will close in five seconds");
                    Console.SetIn(TextReader.Null);
                    Console.SetOut(TextWriter.Null);
                    Console.ReadKey();
                    Thread.Sleep(5000);
                    Environment.Exit(0);
                }
                Console.WriteLine($" ({string.Join(", ", availableChars)})\n");
                Console.WriteLine(hangman_ASCII_Sprites[failedAttempts].ToString());
                Console.WriteLine($"\n{string.Join("\0", DiscoveredChars)}");
                if (Locale == "Русский")
                {
                    Console.WriteLine($"\nКоличество неудачных попыток: {failedAttempts} из {maximumFailedAttempts}.");
                    break;
                }
                Console.WriteLine($"\nNumber of failed attempts: {failedAttempts} out of {maximumFailedAttempts}.");
                break;
            }
            case ResultSwitchStates.FAILED:
            {
                availableChars.Remove(KeyChar);
                if (failedAttempts >= maximumFailedAttempts)
                {
                    TextReader originalInput = Console.In;
                    TextWriter originalOutput = Console.Out;
                    if (Locale == "Русский")
                    {
                        Console.WriteLine("Игра окончена");
                        Console.WriteLine("Игра закроется через пять секунд");
                        Console.SetIn(TextReader.Null);
                        Console.SetOut(TextWriter.Null);
                        Console.ReadKey();
                        Thread.Sleep(5000);
                        Environment.Exit(0);
                    }    
                    Console.WriteLine("Game Over");
                    Console.WriteLine("The game will close in five seconds");
                    Console.SetIn(TextReader.Null);
                    Console.SetOut(TextWriter.Null);
                    Console.ReadKey();
                    Thread.Sleep(5000);
                    Environment.Exit(0);

                }
                Console.WriteLine($" ({string.Join(", ", _availableChars)})\n");
                if (Locale == "Русский")
                {
                    Console.WriteLine("Выбранный вами символ является неправильным!");
                }
                Console.WriteLine("The Character you have chosen is incorrect!");
                Console.WriteLine(hangman_ASCII_Sprites[failedAttempts].ToString());
                Console.WriteLine($"\n{string.Join("\0", DiscoveredChars)}");
                if (Locale == "Русский")
                {
                    Console.WriteLine($"\nКоличество неудачных попыток: {failedAttempts} из {maximumFailedAttempts}.");
                    break;
                }
                Console.WriteLine($"\nNumber of failed attempts: {failedAttempts} out of {maximumFailedAttempts}.");
                break;
            }
            default:
                if (Locale == "Русский")
                {
                    Console.WriteLine($"Значение недействительно ({ResultSwitch})");
                    break;
                }
                Console.WriteLine($"The Value is invalid ({ResultSwitch})");
                break;
        }
    }
    internal static char[] Fill(char[] array, char fill)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = fill;
        }
        return array;
    }
    public static List<int> FindOccurrences(string input, char character)
    {
        List<int> occurrences = new List<int>();
        int index = -1;

        do
        {
            index = input.IndexOf(character, index + 1);
            if (index != -1)
            {
                occurrences.Add(index);
            }
        } while (index != -1);

        return occurrences;
    }
    public static void DiscoveredCharsDisplay(string selectedWord, char KeyChar, char[] DiscoveredChars)
    {
        List<int> indexList = FindOccurrences(selectedWord, KeyChar);
        for (int i = 0; i < DiscoveredChars.Length; i++)
        {
            if (indexList.Count == i)
            {
                break;
            }
            DiscoveredChars[indexList[i]] = selectedWord[indexList[i]];
        }
    }
}
