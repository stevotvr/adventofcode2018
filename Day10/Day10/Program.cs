using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day10
{
    class Program
    {
        private static void Part1(string[] input)
        {
            var points = new Point[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                var parts = input[i].Split(new string[] { "=<", "> " }, StringSplitOptions.None);
                var x = int.Parse(parts[1].Substring(0, parts[1].IndexOf(',')).Trim());
                var y = int.Parse(parts[1].Substring(parts[1].IndexOf(',') + 1).Trim());
                var vx = int.Parse(parts[3].Substring(0, parts[3].IndexOf(',')).Trim());
                var vy = int.Parse(parts[3].Substring(parts[3].IndexOf(',') + 1, parts[3].Length - parts[3].IndexOf(',') - 2).Trim());
                points[i] = new Point
                {
                    x = x,
                    y = y,
                    vx = vx,
                    vy = vy,
                };
            }

            var h = int.MaxValue;
            while (true)
            {
                var minX = int.MaxValue;
                var minY = int.MaxValue;
                var maxX = int.MinValue;
                var maxY = int.MinValue;
                foreach (var p in points)
                {
                    p.x += p.vx;
                    p.y += p.vy;
                    minX = Math.Min(minX, p.x);
                    minY = Math.Min(minY, p.y);
                    maxX = Math.Max(maxX, p.x);
                    maxY = Math.Max(maxY, p.y);
                }

                if (maxY - minY > h)
                {
                    var grid = new bool[maxY - minY + 1, maxX - minX + 1];
                    foreach (var p in points)
                    {
                        grid[p.y - p.vy - minY, p.x - p.vx - minX] = true;
                    }

                    for (var i = 0; i < grid.GetLength(0); i++)
                    {
                        for (var j = 0; j < grid.GetLength(1); j++)
                        {
                            Console.Write(grid[i, j] ? '#' : ' ');
                        }
                        Console.WriteLine();
                    }

                    break;
                }

                h = maxY - minY;
            }
        }

        private static int Part2(string[] input)
        {
            var points = new Point[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                var parts = input[i].Split(new string[] { "=<", "> " }, StringSplitOptions.None);
                var y = int.Parse(parts[1].Substring(parts[1].IndexOf(',') + 1).Trim());
                var vy = int.Parse(parts[3].Substring(parts[3].IndexOf(',') + 1, parts[3].Length - parts[3].IndexOf(',') - 2).Trim());
                points[i] = new Point
                {
                    y = y,
                    vy = vy,
                };
            }

            var h = int.MaxValue;
            var t = 0;
            while (true)
            {
                var minY = int.MaxValue;
                var maxY = int.MinValue;
                foreach (var p in points)
                {
                    p.y += p.vy;
                    minY = Math.Min(minY, p.y);
                    maxY = Math.Max(maxY, p.y);
                }

                if (maxY - minY > h)
                {
                    return t;
                }

                h = maxY - minY;
                t++;
            }
        }

        private class Point
        {
            public int x;
            public int y;
            public int vx;
            public int vy;
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
                        Part1(input);
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
