using System.Text.RegularExpressions;
using static Resources;
class Gameloop
{
    internal static void GameLoop()
    {
        _availableChars.AddRange(allChars);
        var rand = new Random();
        int Randomindex = rand.Next(words.Length);
        string selectedWord = words[Randomindex];
        List<char> selectedWordUniqueOnly = new(selectedWord.Distinct().ToArray());
        Console.WriteLine(hangman_ASCII_Sprites[failedAttempts].ToString());
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
            Validation(KeyChar, selectedWord, out IsLetter, out IsAvailable, out IsInWord);
#if DEBUG
            if (IsLetter)
            {
                Console.WriteLine($"Debugging: IsLetter Check True, Target: {KeyChar}");
                if (IsAvailable)
                {
                    Console.WriteLine($"Debugging: IsAvaliable Check True, Target: {KeyChar}");
                    if (IsInWord)
                    {
                        _availableChars.Remove(KeyChar);
                        Console.WriteLine($"({string.Join(", ", _availableChars)})");
                        Console.WriteLine($"maximunFailedAttempts == {maximumFailedAttempts}, failedAttempts == {failedAttempts}");
                        selectedWordUniqueOnly.Remove(KeyChar);
                        Console.WriteLine(string.Join("", selectedWordUniqueOnly));
                        if (selectedWordUniqueOnly.Count == 0)
                        {
                            Console.WriteLine("You Win!");
                            break;
                        }
                        Console.WriteLine(hangman_ASCII_Sprites[failedAttempts].ToString());
                    }
                    else
                    {
                        _availableChars.Remove(KeyChar);
                        failedAttempts++;
                        if (failedAttempts >= maximumFailedAttempts)
                        {
                            Console.WriteLine("Game Over");
                            break;
                        }
                        Console.WriteLine($"maximunFailedAttempts == {maximumFailedAttempts}, failedAttempts == {failedAttempts}");
                        Console.WriteLine(hangman_ASCII_Sprites[failedAttempts].ToString());
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
                        UserInputResult(_availableChars, selectedWordUniqueOnly, failedAttempts, KeyChar, SUCCESSFUL);
                    }
                    else
                    {
                        UserInputResult(_availableChars, selectedWordUniqueOnly, failedAttempts, KeyChar, FAILED);
                    }
                }
                else
                {
                    i--;
                    Console.WriteLine($"The Character you entered is incorrect, the avaliable characters are: \n{string.Join(", ", _availableChars)}\n\n{hangman_ASCII_Sprites[failedAttempts]}");
                }
            }
            else
            {
                i--;
                Console.Clear();
                Console.WriteLine($"The Character you entered is incorrect, the avaliable characters are: \n{string.Join(", ", _availableChars)}\n\n{hangman_ASCII_Sprites[failedAttempts]}");
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
    internal static void Validation(char KeyChar, string selectedWord, out bool Check1, out bool Check2, out bool Check3)
    {
        Check1 = false;
        Check2 = false;
        Check3 = false;
        string pattern = @"^[a-zA-Z]+$";
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
                }
            }
        }
    }
    internal static void UserInputResult(char[] availableChars, List<char> selectedWordUniqueOnly, int failedAttempts, char KeyChar, int ResultSwitch)
    {
        switch (ResultSwitch)
        {
            case SUCCESSFUL:
            {
                availableChars.Remove(KeyChar);
                Console.WriteLine($" ({string.Join(", ", availableChars)})\n");
                selectedWordUniqueOnly.Remove(KeyChar);
                if (selectedWordUniqueOnly.Count == 0)
                {
                    Console.WriteLine("You Win!");
                    break;
                }
                Console.WriteLine(hangman_ASCII_Sprites[failedAttempts].ToString());
            }
            case FAILED:
            {
                _availableChars.Remove(KeyChar);
                Console.WriteLine($" ({string.Join(", ", _availableChars)})\n");
                if (failedAttempts == maximumFailedAttempts)
                {
                    Console.WriteLine("Game Over");
                    break;
                }
                failedAttempts++;
                Console.WriteLine(hangman_ASCII_Sprites[failedAttempts].ToString());
            }
        }
    }
}
