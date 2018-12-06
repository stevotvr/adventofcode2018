using System;
using System.IO;

namespace Day06
{
    class Program
    {
        private struct Point
        {
            public int X;
            public int Y;
        }

        private static int Part1(string[] input)
        {
            var points = new Point[input.Length];
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var maxX = 0;
            var maxY = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var x = int.Parse(input[i].Substring(0, input[i].IndexOf(',')));
                var y = int.Parse(input[i].Substring(input[i].IndexOf(' ') + 1));
                minX = Math.Min(minX, x);
                minY = Math.Min(minY, y);
                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);
                points[i] = new Point
                {
                    X = x,
                    Y = y,
                };
            }

            var areas = new int[points.Length];
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minX; y <= maxY; y++)
                {
                    var best = int.MaxValue;
                    var idx = -1;
                    for (var i = 0; i < points.Length; i++)
                    {
                        var dist = Math.Abs(x - points[i].X) + Math.Abs(y - points[i].Y);
                        if (dist < best)
                        {
                            idx = i;
                            best = dist;
                        }
                        else if (dist == best)
                        {
                            idx = -1;
                        }
                    }

                    if (idx > -1)
                    {
                        areas[idx]++;
                    }
                }
            }

            var largest = 0;
            for (var i = 0; i < points.Length; i++)
            {
                if (points[i].X == maxX || points[i].X == minX || points[i].Y == maxX || points[i].Y == minY)
                {
                    continue;
                }

                largest = Math.Max(largest, areas[i]);
            }

            return largest;
        }

        private static int Part2(string[] input)
        {
            var points = new Point[input.Length];
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var maxX = 0;
            var maxY = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var x = int.Parse(input[i].Substring(0, input[i].IndexOf(',')));
                var y = int.Parse(input[i].Substring(input[i].IndexOf(' ') + 1));
                minX = Math.Min(minX, x);
                minY = Math.Min(minY, y);
                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);
                points[i] = new Point
                {
                    X = x,
                    Y = y,
                };
            }

            var area = 0;
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minX; y <= maxY; y++)
                {
                    var size = 0;
                    for (var i = 0; i < points.Length; i++)
                    {
                        size += Math.Abs(x - points[i].X) + Math.Abs(y - points[i].Y);
                    }

                    if (size < 10000)
                    {
                        area++;
                    }
                }
            }

            return area;
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
