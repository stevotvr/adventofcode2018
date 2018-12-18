using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day17
{
    class Program
    {
        private static int Part1(string[] input)
        {
            Fill(input, out var water, out var grid);

            var total = 0;
            foreach (var w in water)
            {
                if (w)
                {
                    total++;
                }
            }

            return total;
        }

        private static int Part2(string[] input)
        {
            Fill(input, out var water, out var grid);

            for (var y = water.GetLength(1) - 1; y >= 0; y--)
            {
                var done = false;
                while (!done)
                {
                    done = true;
                    for (var x = water.GetLength(0) - 2; x > 0; x--)
                    {
                        if (water[x, y] && !((water[x - 1, y] || grid[x - 1, y]) && (water[x + 1, y] || grid[x + 1, y])))
                        {
                            water[x, y] = false;
                            done = false;
                        }
                    }
                }
            }

            var total = 0;
            foreach (var w in water)
            {
                if (w)
                {
                    total++;
                }
            }

            return total;
        }

        private static void Fill(string[] input, out bool[,] water, out bool[,] grid)
        {
            var stack = new Stack<int[]>();
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;
            foreach (var line in input)
            {
                var parts = line.Split(new string[] { "=", ", " }, StringSplitOptions.RemoveEmptyEntries);
                var c1 = int.Parse(parts[1]);
                var c2 = parts[3].Split(new string[] { ".." }, StringSplitOptions.None).Select(e => int.Parse(e)).ToArray();
                var x = 0;
                var y = 0;
                if (parts[0] == "x")
                {
                    x = c1;
                    maxX = Math.Max(maxX, x);
                }
                else
                {
                    y = c1;
                    minY = Math.Min(minY, y);
                    maxY = Math.Max(maxY, y);
                }
                for (var i = c2[0]; i <= c2[1]; i++)
                {
                    if (parts[0] == "x")
                    {
                        y = i;
                        minY = Math.Min(minY, y);
                        maxY = Math.Max(maxY, y);
                    }
                    else
                    {
                        x = i;
                        maxX = Math.Max(maxX, x);
                    }

                    stack.Push(new int[] { x, y });
                }
            }

            grid = new bool[maxX * 2, maxY + 2];
            water = new bool[maxX * 2, maxY + 2];
            while (stack.Count > 0)
            {
                var block = stack.Pop();
                grid[block[0], block[1]] = true;
            }

            stack.Push(new int[] { 500, minY });
            while (stack.Count > 0)
            {
                var block = stack.Pop();
                var x = block[0];
                var y = block[1];

                while (y < maxY && !grid[x, y + 1] && !water[x, y])
                {
                    water[x, y] = true;
                    y++;
                }

                water[x, y] = true;
                if (y >= maxY)
                {
                    continue;
                }

                var spill = false;
                while (!spill)
                {
                    x = block[0];
                    while (!grid[x, y])
                    {
                        if (!grid[x, y + 1] && !water[x, y + 1])
                        {
                            spill = true;
                            if (grid[x + 1, y + 1])
                            {
                                stack.Push(new int[] { x, y });
                            }

                            break;
                        }

                        water[x, y] = true;

                        x--;
                    }

                    x = block[0];
                    while (!grid[x, y])
                    {
                        if (!grid[x, y + 1] && !water[x, y + 1])
                        {
                            spill = true;
                            if (grid[x - 1, y + 1])
                            {
                                stack.Push(new int[] { x, y });
                            }

                            break;
                        }

                        water[x, y] = true;

                        x++;
                    }

                    y--;
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
