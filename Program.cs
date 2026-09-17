using Snake;

public class Program
{
    public Program() { }

    public static void Main(string[] args)
    {
        //Console.WriteLine("Skane is runnnig");
        Game game = new Game("Snake", 60, 800, 800);
        game.LoadGame();
    }
}