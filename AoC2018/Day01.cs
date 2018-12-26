using System.Collections.Generic;
using System.IO;

namespace AoC2018
{
    class Day01 : ISolution
    {
        private string[] input;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);
        }

        public object Part1()
        {
            var output = 0;

            foreach (var l in input)
            {
                output += int.Parse(l);
            }

            return output;
        }

        public object Part2()
        {
            var output = 0;

            var list = new HashSet<int>();
            while (true)
            {
                foreach (var l in input)
                {
                    output += int.Parse(l);
                    if (!list.Add(output))
                    {
                        return output;
                    }
                }
            }
        }
    }
}
