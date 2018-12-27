using System;
using System.IO;

namespace AoC2018
{
    class Day06 : ISolution
    {
        private string[] input;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);
        }

        public object Part1()
        {
            var points = new (int x, int y)[input.Length];
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
                points[i] = (x, y);
            }

            var areas = new int[points.Length];
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minX; y <= maxY; y++)
                {
                    var nearest = int.MaxValue;
                    var idx = -1;
                    for (var i = 0; i < points.Length; i++)
                    {
                        var dist = Math.Abs(x - points[i].x) + Math.Abs(y - points[i].y);
                        if (dist < nearest)
                        {
                            idx = i;
                            nearest = dist;
                        }
                        else if (dist == nearest)
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
                if (points[i].x == maxX || points[i].x == minX || points[i].y == maxX || points[i].y == minY)
                {
                    continue;
                }

                largest = Math.Max(largest, areas[i]);
            }

            return largest;
        }

        public object Part2()
        {
            var points = new (int x, int y)[input.Length];
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
                points[i] = (x, y);
            }

            var area = 0;
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minX; y <= maxY; y++)
                {
                    var size = 0;
                    for (var i = 0; i < points.Length; i++)
                    {
                        size += Math.Abs(x - points[i].x) + Math.Abs(y - points[i].y);
                    }

                    if (size < 10000)
                    {
                        area++;
                    }
                }
            }

            return area;
        }
    }
}
