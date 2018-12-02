using System;
using System.Collections.Generic;
using System.IO;

namespace Day01
{
    class Program
    {
        private static int Part1(string[] input)
        {
            var output = 0;

            foreach (var l in input)
            {
                if (int.TryParse(l, out int i))
                {
                    output += i;
                }
            }

            return output;
        }

        private static int Part2(string[] input)
        {
            var output = 0;

            var list = new HashSet<int>();
            while (true)
            {
                foreach (var l in input)
                {
                    if (int.TryParse(l, out int i))
                    {
                        output += i;
                        if (!list.Add(output))
                        {
                            return output;
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input.txt");

            while (true)
            {
                Console.WriteLine("Press 1 for part 1, 2 for part 2, Esc to exit...");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("Running part 1...");
                        Console.WriteLine($"Answer: {Part1(input)}");
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("Running part 2...");
                        Console.WriteLine($"Answer: {Part2(input)}");
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}
