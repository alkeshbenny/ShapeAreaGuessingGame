using System;
using System.Threading.Tasks;

public abstract class Shape
{
    public abstract double GetArea();
    public abstract string GetDescription();
}

public class Circle : Shape
{
    public double Radius { get; set; }

    public Circle(double radius) => Radius = radius;

    public override double GetArea() => Math.PI * Radius * Radius;

    public override string GetDescription() => $"Circle with Radius = {Radius}";
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public override double GetArea() => Width * Height;

    public override string GetDescription() => $"Rectangle with Width = {Width}, Height = {Height}";
}

public class Triangle : Shape
{
    public double BaseLength { get; set; }
    public double Height { get; set; }

    public Triangle(double baseLength, double height)
    {
        BaseLength = baseLength;
        Height = height;
    }

    public override double GetArea() => 0.5 * BaseLength * Height;

    public override string GetDescription() => $"Triangle with Base = {BaseLength}, Height = {Height}";
}

class Program
{
    static async Task Main()
    {
        Console.WriteLine("🎮 Welcome to the Shape Area Guessing Game!");
        Console.WriteLine("✅ Correct Guess = +10 points");
        Console.WriteLine("❌ Wrong Guess or Time Up = -5 points");
        Console.WriteLine("⏰ You have 30 seconds per round");
        Console.WriteLine("⚠️ Invalid Input = 0 points (skipped)");
        Console.WriteLine("💡 Please enter numbers only (e.g., 12.34)\n");

        Console.WriteLine("Press Enter to start the game...");
        while (Console.ReadKey(true).Key != ConsoleKey.Enter)
        {
            // wait for Enter without printing anything
        }

        Random rand = new Random();
        int score = 0;

        for (int round = 1; round <= 5; round++)
        {
            Shape shape = GetRandomShape(rand);
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($"Round {round}:");
            Console.WriteLine(shape.GetDescription());

            // Start timer and input immediately without waiting for extra Enter
            Console.Write("Your guess for the area: ");
            string input = await ReadLineWithTimeout(30);

            if (input == null)
            {
                double actualArea = Math.Round(shape.GetArea(), 2);
                Console.WriteLine($"\n⏰ Time's up! The correct area was {actualArea:F2}. -5 points");
                score -= 5;
            }
            else if (double.TryParse(input, out double userGuess))
            {
                double actualArea = Math.Round(shape.GetArea(), 2);
                double difference = Math.Abs(actualArea - userGuess);

                if (difference < 0.1)
                {
                    Console.WriteLine("🎉 Correct! +10 points");
                    score += 10;
                }
                else
                {
                    Console.WriteLine($"❌ Incorrect. The correct area is {actualArea:F2}. -5 points");
                    score -= 5;
                }
            }
            else
            {
                Console.WriteLine("⚠️ Invalid input. No points.");
            }

            Console.WriteLine($"Current Score: {score}");
        }


        Console.WriteLine(new string('=', 40));
        Console.WriteLine($"🏁 Game Over! Final Score: {score}");
        if (score == 50) Console.WriteLine("🔥 Perfect Score! You're a geometry master!");
        else if (score >= 30) Console.WriteLine("👏 Great job!");
        else if (score >= 0) Console.WriteLine("👍 Keep practicing!");
        else Console.WriteLine("😅 Don't worry, try again!");
    }

    static Shape GetRandomShape(Random rand)
    {
        int choice = rand.Next(3);
        return choice switch
        {
            0 => new Circle(rand.Next(1, 11)),
            1 => new Rectangle(rand.Next(2, 11), rand.Next(2, 11)),
            _ => new Triangle(rand.Next(2, 11), rand.Next(2, 11))
        };
    }

    static async Task<string> ReadLineWithTimeout(int seconds)
    {
        string input = "";
        DateTime endTime = DateTime.Now.AddSeconds(seconds);

        while (DateTime.Now < endTime)
        {
            int timeLeft = (int)(endTime - DateTime.Now).TotalSeconds;
            if (timeLeft < 0) timeLeft = 0;

            Console.Write($"\rYour guess for the area: {input}");
            Console.Write(new string(' ', 10)); // clear residual chars
            Console.Write($"  Time left: {timeLeft,2} sec ");

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return input;
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input[..^1];
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    input += key.KeyChar;
                }
            }

            await Task.Delay(100);
        }

        Console.WriteLine();
        return null; // timed out
    }
}
