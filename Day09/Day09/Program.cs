using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day09
{
    class Program
    {
        private static long Part1(int players, int final)
        {
            var marbles = new LinkedList<int>();
            var scores = new long[players];

            marbles.AddFirst(0);

            for (var i = 1; i <= final; i++)
            {
                if ((i % 23) == 0)
                {
                    for (var j = 0; j < 7; j++)
                    {
                        var last = marbles.Last;
                        marbles.RemoveLast();
                        marbles.AddFirst(last);
                    }

                    scores[i % scores.Length] += i + marbles.First();
                    marbles.RemoveFirst();
                }
                else
                {
                    for (var j = 0; j < 2; j++)
                    {
                        var first = marbles.First;
                        marbles.RemoveFirst();
                        marbles.AddLast(first);
                    }

                    marbles.AddFirst(i);
                }
            }

            return scores.Max();
        }

        private static long Part2(int players, int final)
        {
            return Part1(players, final * 100);
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllText("Input.txt").Split(' ');
            var players = int.Parse(input[0]);
            var final = int.Parse(input[6]);

            while (true)
            {
                Console.WriteLine("Press 1 for part 1, 2 for part 2, Esc to exit...");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("Running part 1...");
                        Console.WriteLine($"Answer: {Part1(players, final)}");
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("Running part 2...");
                        Console.WriteLine($"Answer: {Part2(players, final)}");
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}
