using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC2018
{
    class Day07 : ISolution
    {
        private string[] input;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);
        }

        public object Part1()
        {
            var requirements = new List<(char step, char requires)>();
            var steps = new HashSet<char>();
            foreach (var l in input)
            {
                var step = l[36];
                var req = l[5];
                requirements.Add((step, req));

                steps.Add(step);
                steps.Add(req);
            }

            var sorted = steps.ToList();
            sorted.Sort();
            var sb = new StringBuilder();
            while (sorted.Count > 0)
            {
                var ready = sorted.Find((char s) => !requirements.Any(r => r.step == s));
                sb.Append(ready);
                sorted.Remove(ready);
                requirements.RemoveAll(r => r.requires == ready);
            }

            return sb.ToString();
        }

        public object Part2()
        {
            var requirements = new List<(char step, char requires)>();
            var steps = new HashSet<char>();
            foreach (var l in input)
            {
                var step = l[36];
                var req = l[5];
                requirements.Add((step, req));

                steps.Add(step);
                steps.Add(req);
            }

            var sorted = steps.ToList();
            sorted.Sort();
            var time = 0;
            var tasks = new int[5];
            var finished = new Dictionary<char, int>();
            while (sorted.Count > 0 || tasks.Any((int t) => t > time))
            {
                var ready = sorted.FindAll((char s) => !requirements.Any(r => r.step == s));
                for (var t = 0; t < tasks.Length && ready.Count > 0; t++)
                {
                    if (tasks[t] <= time)
                    {
                        tasks[t] = (ready[0] - 'A' + 1) + 60 + time;
                        finished[ready[0]] = tasks[t];
                        sorted.Remove(ready[0]);
                        ready.RemoveAt(0);
                    }
                }

                time++;

                foreach (var k in finished.Keys.ToArray())
                {
                    if (finished[k] <= time)
                    {
                        requirements.RemoveAll(r => r.requires == k);
                        finished.Remove(k);
                    }
                }
            }

            return time;
        }
    }
}
