using System;
using System.Collections.Generic;
using System.IO;

namespace Day04
{
    class Program
    {
        private static int Part1(string[] input)
        {
            Array.Sort(input);
            var sleepTime = new Dictionary<int, int>();
            var sleepMinutes = new Dictionary<int, Dictionary<int, int>>();
            var guard = 0;
            var start = 0; ;
            foreach (var l in input)
            {
                var parts = l.Split(' ');
                switch (parts[2])
                {
                    case "Guard":
                        guard = int.Parse(parts[3].Substring(1));
                        if (!sleepTime.ContainsKey(guard))
                        {
                            sleepTime[guard] = 0;
                        }

                        if (!sleepMinutes.ContainsKey(guard))
                        {
                            sleepMinutes[guard] = new Dictionary<int, int>();
                        }

                        break;
                    case "falls":
                        start = int.Parse(parts[1].Substring(3, 2));
                        break;
                    case "wakes":
                        var end = int.Parse(parts[1].Substring(3, 2));
                        sleepTime[guard] += end - start;
                        for (var i = start; i <= end; i++)
                        {
                            sleepMinutes[guard].TryGetValue(i, out var m);
                            sleepMinutes[guard][i] = m + 1;
                        }

                        break;
                }
            }

            var max = 0;
            foreach (var t in sleepTime)
            {
                if (t.Value > max)
                {
                    max = t.Value;
                    guard = t.Key;
                }
            }

            max = 0;
            var minute = 0;
            foreach (var m in sleepMinutes[guard])
            {
                if (m.Value > max)
                {
                    max = m.Value;
                    minute = m.Key;
                }
            }

            return guard * minute;
        }

        private static int Part2(string[] input)
        {
            Array.Sort(input);
            var sleepMinutes = new Dictionary<int, Dictionary<int, int>>();
            var guard = 0;
            var start = 0; ;
            foreach (var l in input)
            {
                var parts = l.Split(' ');
                switch (parts[2])
                {
                    case "Guard":
                        guard = int.Parse(parts[3].Substring(1));
                        if (!sleepMinutes.ContainsKey(guard))
                        {
                            sleepMinutes[guard] = new Dictionary<int, int>();
                        }

                        break;
                    case "falls":
                        start = int.Parse(parts[1].Substring(3, 2));
                        break;
                    case "wakes":
                        var end = int.Parse(parts[1].Substring(3, 2));
                        for (var i = start; i <= end; i++)
                        {
                            sleepMinutes[guard].TryGetValue(i, out var m);
                            sleepMinutes[guard][i] = m + 1;
                        }

                        break;
                }
            }

            var max = 0;
            var minute = 0;
            foreach (var g in sleepMinutes)
            {
                foreach (var m in g.Value)
                {
                    if (m.Value > max)
                    {
                        max = m.Value;
                        guard = g.Key;
                        minute = m.Key;
                    }
                }
            }

            return guard * minute;
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
