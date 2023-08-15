using static Resources;
class Init
{
    internal static void Main()
    {
        Console.WriteLine("Select Locale / Выберите язык:\n English\n Русский");
        Locale = Console.ReadLine();
        if (Locale != "English" && Locale != "Русский")
        {
            Console.WriteLine("Invaild Input! / Неправельный выбор!");
            Main();
        }
        if (Locale == "Русский")
        {
            Console.WriteLine("Добро пожаловать в игру 'Виселица'");
            Console.WriteLine("Вы пишете букву, и если эта буква есть в слове, то вы можете угадывать еще, пока не раскроете слово. \nЕсли вы угадаете 9 букв неправильно, вы проиграете в игре.");
            Console.WriteLine("Выберите букву для отгадывания, разрешены только русские буквы.\n");
            Gameloop.GameLoop();
        }
        Console.WriteLine("Welcome to the game 'Hangman'");
        Console.WriteLine("You write a letter, and if that letter is in the word, then you can guess more, until you reveal the word. \nIf you guess 9 letters wrong, you lose the game.");
        Console.WriteLine("Choose a letter to guess, only english letters are allowed.\n");
        Gameloop.GameLoop();
    }
}