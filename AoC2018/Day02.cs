using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AoC2018
{
    class Day02 : ISolution
    {
        private string[] input;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);
        }

        public object Part1()
        {
            var two = 0;
            var three = 0;
            var counts = new Dictionary<char, int>();
            foreach (var l in input)
            {
                foreach (var c in l.ToCharArray())
                {
                    counts.TryGetValue(c, out int i);
                    counts[c] = i + 1;
                }

                if (counts.ContainsValue(2))
                {
                    two++;
                }

                if (counts.ContainsValue(3))
                {
                    three++;
                }

                counts.Clear();
            }

            return two * three;
        }

        public object Part2()
        {
            var d = 0;
            var sb = new StringBuilder();
            foreach (var a in input)
            {
                foreach (var b in input)
                {
                    for (var i = 0; i < a.Length; i++)
                    {
                        if (a[i] != b[i])
                        {
                            if (++d > 1)
                            {
                                break;
                            }
                        }
                        else
                        {
                            sb.Append(a[i]);
                        }
                    }

                    if (d == 1)
                    {
                        return sb.ToString();
                    }

                    d = 0;
                    sb.Clear();
                }
            }

            return "error";
        }
    }
}
