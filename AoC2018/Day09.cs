using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2018
{
    class Day09 : ISolution
    {
        private int players;

        private int final;

        public void LoadInput(params string[] files)
        {
            var input = File.ReadAllText(files[0]).Split(' ');
            players = int.Parse(input[0]);
            final = int.Parse(input[6]);
        }

        public object Part1()
        {
            var marbles = new LinkedList<int>();
            var scores = new long[players];

            marbles.AddFirst(0);

            for (var i = 1; i <= final; i++)
            {
                if ((i % 23) == 0)
                {
                    for (var j = 0; j < 7; j++)
                    {
                        var last = marbles.Last;
                        marbles.RemoveLast();
                        marbles.AddFirst(last);
                    }

                    scores[i % scores.Length] += i + marbles.First();
                    marbles.RemoveFirst();
                }
                else
                {
                    for (var j = 0; j < 2; j++)
                    {
                        var first = marbles.First;
                        marbles.RemoveFirst();
                        marbles.AddLast(first);
                    }

                    marbles.AddFirst(i);
                }
            }

            return scores.Max();
        }

        public object Part2()
        {
            final *= 100;
            return Part1();
        }
    }
}
