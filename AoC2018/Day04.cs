using System;
using System.Collections.Generic;
using System.IO;

namespace AoC2018
{
    class Day04 : ISolution
    {
        private string[] input;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);
        }

        public object Part1()
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

        public object Part2()
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
    }
}
