using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2018
{
    class Day25 : ISolution
    {
        private string[] input;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);
        }

        public object Part1()
        {
            var points = new List<(int x, int y, int z, int t)>();
            foreach (var l in input)
            {
                var point = l.Split(',').Select(x => int.Parse(x)).ToArray();
                points.Add((point[0], point[1], point[2], point[3]));
            }

            var stack = new Stack<(int x, int y, int z, int t)>();
            var count = 0;
            while (points.Count > 0)
            {
                stack.Push(points[0]);
                while (stack.Count > 0)
                {
                    var point = stack.Pop();
                    points.Remove(point);
                    foreach (var p in points)
                    {
                        if (Math.Abs(point.x - p.x) + Math.Abs(point.y - p.y) + Math.Abs(point.z - p.z) + Math.Abs(point.t - p.t) <= 3)
                        {
                            stack.Push(p);
                        }
                    }
                }

                count++;
            }

            return count;
        }

        public object Part2()
        {
            return null;
        }
    }
}
