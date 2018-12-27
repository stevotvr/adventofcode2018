using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AoC2018
{
    class Day19 : ISolution
    {
        private static readonly ReadOnlyDictionary<string, Action<int[], int, int, int>> instructions = new ReadOnlyDictionary<string, Action<int[], int, int, int>>(new Dictionary<string, Action<int[], int, int, int>>
        {
            { "addr", (r, a, b, c) => r[c] = r[a] + r[b] },
            { "addi", (r, a, b, c) => r[c] = r[a] + b },
            { "mulr", (r, a, b, c) => r[c] = r[a] * r[b] },
            { "muli", (r, a, b, c) => r[c] = r[a] * b },
            { "banr", (r, a, b, c) => r[c] = r[a] & r[b] },
            { "bani", (r, a, b, c) => r[c] = r[a] & b },
            { "borr", (r, a, b, c) => r[c] = r[a] | r[b] },
            { "bori", (r, a, b, c) => r[c] = r[a] | b },
            { "setr", (r, a, b, c) => r[c] = r[a] },
            { "seti", (r, a, b, c) => r[c] = a },
            { "gtir", (r, a, b, c) => r[c] = a > r[b] ? 1 : 0 },
            { "gtri", (r, a, b, c) => r[c] = r[a] > b ? 1 : 0 },
            { "gtrr", (r, a, b, c) => r[c] = r[a] > r[b] ? 1 : 0 },
            { "eqir", (r, a, b, c) => r[c] = a == r[b] ? 1 : 0 },
            { "eqri", (r, a, b, c) => r[c] = r[a] == b ? 1 : 0 },
            { "eqrr", (r, a, b, c) => r[c] = r[a] == r[b] ? 1 : 0 },
        });

        private string[] input;

        private int[] registers;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);
        }

        public object Part1()
        {
            registers = new int[6];
            var pcr = int.Parse(input[0].Substring(input[0].IndexOf(' ')));
            var pc = 0;
            var ops = new List<Action>();
            for (var i = 1; i < input.Length; i++)
            {
                var words = input[i].Split(' ');
                ops.Add(() => instructions[words[0]](registers, int.Parse(words[1]), int.Parse(words[2]), int.Parse(words[3])));
            }

            while (registers[pcr] >= 0 && registers[pcr] < ops.Count)
            {
                registers[pcr] = pc;
                ops[pc]();
                pc = registers[pcr];
                pc++;
            }

            return registers[0];
        }

        public object Part2()
        {
            var x = int.Parse(input[22].Split(' ')[2]);
            var y = int.Parse(input[24].Split(' ')[2]);
            var n = 836 + x * 22 + y + 10550400;
            var sr = (int)Math.Sqrt(n);
            var total = n % sr == 0 ? sr : 0;
            for (var i = 1; i < sr; i++)
            {
                if (n % i == 0)
                {
                    total += i + n / i;
                }
            }

            return total;
        }
    }
}
