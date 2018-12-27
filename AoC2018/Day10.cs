using System;
using System.IO;
using System.Text;

namespace AoC2018
{
    class Day10 : ISolution
    {
        private string[] input;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);
        }

        public object Part1()
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

            var sb = new StringBuilder().AppendLine();
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
                            sb.Append(grid[i, j] ? '#' : ' ');
                        }
                        sb.AppendLine();
                    }

                    break;
                }

                h = maxY - minY;
            }

            return sb.ToString();
        }

        public object Part2()
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
    }
}
