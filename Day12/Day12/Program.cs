using System;
using System.Collections.Generic;
using System.IO;

namespace Day12
{
    class Program
    {
        private static int Part1(char[] initial, Dictionary<string, char> patterns)
        {
            var state = initial;
            var next = new List<char>();
            var p = new char[5];
            for (var i = 0; i < 20; i++)
            {
                for (var j = - 3; j < state.Length + 3; j++)
                {
                    for (var k = -2; k <= 2; k++)
                    {
                        if (j + k >= 0 && j + k < state.Length)
                        {
                            p[k + 2] = state[j + k];
                        }
                        else
                        {
                            p[k + 2] = '.';
                        }
                    }

                    next.Add(patterns[string.Join("", p)]);
                }

                state = next.ToArray();
                next.Clear();
            }

            var sum = 0;
            for (var i = 0; i < state.Length; i++)
            {
                if (state[i] == '#')
                {
                    sum += i - 60;
                }
            }

            return sum;
        }

        private static long Part2(char[] initial, Dictionary<string, char> patterns)
        {
            var state = initial;
            var next = new List<char>();
            var p = new char[5];
            var prevSum = 0;
            var prevDiff = 0;
            var repeats = 0;
            var gen = 0;
            while (repeats < 100)
            {
                gen++;
                for (var i = -3; i < state.Length + 3; i++)
                {
                    for (var k = -2; k <= 2; k++)
                    {
                        if (i + k >= 0 && i + k < state.Length)
                        {
                            p[k + 2] = state[i + k];
                        }
                        else
                        {
                            p[k + 2] = '.';
                        }
                    }

                    next.Add(patterns[string.Join("", p)]);
                }

                state = next.ToArray();
                next.Clear();

                var sum = 0;
                for (var i = 0; i < state.Length; i++)
                {
                    if (state[i] == '#')
                    {
                        sum += i - (3 * gen);
                    }
                }

                if (sum - prevSum == prevDiff)
                {
                    repeats++;
                }
                else
                {
                    repeats = 0;
                    prevDiff = sum - prevSum;
                }

                prevSum = sum;
            }

            return prevSum + (prevDiff * (50000000000 - gen));
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input.txt");
            var initial = input[0].Substring(input[0].IndexOf(':') + 1).Trim().ToCharArray();
            var patterns = new Dictionary<string, char>();
            for (var i = 2; i < input.Length; i++)
            {
                var parts = input[i].Split(new string[] { " => " }, StringSplitOptions.None);
                patterns[parts[0]] = parts[1][0];
            }
            
            while (true)
            {
                Console.WriteLine("Press 1 for part 1, 2 for part 2, Esc to exit...");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("Running part 1...");
                        Console.WriteLine($"Answer: {Part1(initial, patterns)}");
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("Running part 2...");
                        Console.WriteLine($"Answer: {Part2(initial, patterns)}");
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}
