using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AoC2018
{
    class Day14 : ISolution
    {
        private int input;

        public void LoadInput(params string[] files)
        {
            input = int.Parse(File.ReadAllText(files[0]));
        }

        public object Part1()
        {
            var scores = new List<int>(new int[] { 3, 7 });
            var e1 = 0;
            var e2 = 1;
            while (scores.Count < input + 10)
            {
                var sum = scores[e1] + scores[e2];
                if (sum > 9)
                {
                    scores.Add(1);
                    scores.Add(sum - 10);
                }
                else
                {
                    scores.Add(sum);
                }

                e1 = (e1 + scores[e1] + 1) % scores.Count;
                e2 = (e2 + scores[e2] + 1) % scores.Count;
            }

            return int.Parse(string.Join("", scores.GetRange(input, 10)));
        }

        public object Part2()
        {
            var scores = new List<int>(new int[] { 3, 7 });
            var search = input.ToString();
            var sb = new StringBuilder("37");
            var e1 = 0;
            var e2 = 1;
            var i = 0;
            while (true)
            {
                var sum = scores[e1] + scores[e2];
                sb.Append(sum);
                if (sum > 9)
                {
                    scores.Add(1);
                    scores.Add(sum - 10);
                }
                else
                {
                    scores.Add(sum);
                }

                if (i % 100000 == 0)
                {
                    var index = sb.ToString().IndexOf(search);
                    if (index > -1)
                    {
                        return index;
                    }
                }

                e1 = (e1 + scores[e1] + 1) % scores.Count;
                e2 = (e2 + scores[e2] + 1) % scores.Count;
                i++;
            }
        }
    }
}
