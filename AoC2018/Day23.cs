using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2018
{
    class Day23 : ISolution
    {
        private List<(int x, int y, int z, int r)> bots;

        public void LoadInput(params string[] files)
        {
            var input = File.ReadAllLines(files[0]);
            bots = new List<(int x, int y, int z, int r)>();
            foreach (var l in input)
            {
                var m = Regex.Match(l, @"^pos=<(\-?[0-9]+),(\-?[0-9]+),(\-?[0-9]+)>, r=([0-9]+)$");
                var bot = (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));

                bots.Add(bot);
            }
        }

        public object Part1()
        {
            var strongest = bots.First(x => x.r == bots.Max(y => y.r));
            return bots.Count(x => Math.Abs(strongest.x - x.x) + Math.Abs(strongest.y - x.y) + Math.Abs(strongest.z - x.z) <= strongest.r);
        }

        public object Part2()
        {
            var minX = bots.Min(x => x.x);
            var maxX = bots.Max(x => x.x);
            var minY = bots.Min(x => x.y);
            var maxY = bots.Max(x => x.y);
            var minZ = bots.Min(x => x.z);
            var maxZ = bots.Max(x => x.z);
            var best = new int[4];
            for (var div = 10000000; div > 0; div /= 10)
            {
                for (var x = maxX; x >= minX; x -= div)
                {
                    for (var y = maxY; y >= minY; y -= div)
                    {
                        for (var z = maxZ; z >= minZ; z -= div)
                        {
                            var count = bots.Count(b => Math.Abs(x - b.x) + Math.Abs(y - b.y) + Math.Abs(z - b.z) <= b.r);
                            if (count > best[3] || (count == best[3] && Math.Abs(x) + Math.Abs(y) + Math.Abs(z) < Math.Abs(best[0]) + Math.Abs(best[1]) + Math.Abs(best[2])))
                            {
                                best[0] = x;
                                best[1] = y;
                                best[2] = z;
                                best[3] = count;
                            }
                        }
                    }
                }

                minX = best[0] - div / 2;
                maxX = best[0] + div / 2;
                minY = best[1] - div / 2;
                maxY = best[1] + div / 2;
                minZ = best[2] - div / 2;
                maxZ = best[2] + div / 2;
                best[3] = 0;
            }

            return Math.Abs(best[0]) + Math.Abs(best[1]) + Math.Abs(best[2]);
        }
    }
}
