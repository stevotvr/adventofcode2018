using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Day02
{
    class Program
    {
        private static int Part1(string[] input)
        {
            var two = 0;
            var three = 0;
            var counts = new Dictionary<char, int>();
            foreach (var l in input)
            {
                foreach (var c in l.ToCharArray())
                {
                    counts.TryGetValue(c, out int i);
                    counts[c] = i + 1;
                }

                if (counts.ContainsValue(2))
                {
                    two++;
                }

                if (counts.ContainsValue(3))
                {
                    three++;
                }

                counts.Clear();
            }

            return two * three;
        }

        private static string Part2(string[] input)
        {
            var d = 0;
            var sb = new StringBuilder();
            foreach (var a in input)
            {
                foreach (var b in input)
                {
                    for (var i = 0; i < a.Length; i++)
                    {
                        if (a[i] != b[i])
                        {
                            if (++d > 1)
                            {
                                break;
                            }
                        }
                        else
                        {
                            sb.Append(a[i]);
                        }
                    }

                    if (d == 1)
                    {
                        return sb.ToString();
                    }

                    d = 0;
                    sb.Clear();
                }
            }

            return "error";
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
