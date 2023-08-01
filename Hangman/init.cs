class Init
{
    internal static void Main()
    {
        Console.WriteLine("Welcome to the game 'Hangman'");
        Console.WriteLine("You write a letter, and if that letter is in the word, then you can guess more, until you reveal the word. \nIf you guess 8 letters wrong, you lose the game.");
        Console.WriteLine("Choose a letter to guess, only english letters are allowed.\n");
        Gameloop.GameLoop();
    }
}