using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Day14
{
    class Program
    {
        private static int Part1(int input)
        {
            var scores = new List<int>(new int[] { 3, 7 });
            var e1 = 0;
            var e2 = 1;
            while (scores.Count < input + 10)
            {
                var sum = scores[e1] + scores[e2];
                if (sum > 9)
                {
                    scores.Add(1);
                    scores.Add(sum - 10);
                }
                else
                {
                    scores.Add(sum);
                }

                e1 = (e1 + scores[e1] + 1) % scores.Count;
                e2 = (e2 + scores[e2] + 1) % scores.Count;
            }

            return int.Parse(string.Join("", scores.GetRange(input, 10)));
        }

        private static int Part2(int input)
        {
            var scores = new List<int>(new int[] { 3, 7 });
            var search = input.ToString();
            var sb = new StringBuilder("37");
            var e1 = 0;
            var e2 = 1;
            var i = 0;
            while (true)
            {
                var sum = scores[e1] + scores[e2];
                sb.Append(sum);
                if (sum > 9)
                {
                    scores.Add(1);
                    scores.Add(sum - 10);
                }
                else
                {
                    scores.Add(sum);
                }

                if (i % 100000 == 0)
                {
                    var index = sb.ToString().IndexOf(search);
                    if (index > -1)
                    {
                        return index;
                    }
                }

                e1 = (e1 + scores[e1] + 1) % scores.Count;
                e2 = (e2 + scores[e2] + 1) % scores.Count;
                i++;
            }
        }

        static void Main(string[] args)
        {
            var input = int.Parse(File.ReadAllText("Input.txt"));

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
