using System;
using System.Collections.Generic;
using System.IO;

namespace AoC2018
{
    class Day19 : ISolution
    {
        private string[] input;

        private int[] registers;

        private Dictionary<string, Action<int, int, int>> instructions;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);

            instructions = new Dictionary<string, Action<int, int, int>>
            {
                { "addr", (a, b, c) => registers[c] = registers[a] + registers[b] },
                { "addi", (a, b, c) => registers[c] = registers[a] + b },
                { "mulr", (a, b, c) => registers[c] = registers[a] * registers[b] },
                { "muli", (a, b, c) => registers[c] = registers[a] * b },
                { "banr", (a, b, c) => registers[c] = registers[a] & registers[b] },
                { "bani", (a, b, c) => registers[c] = registers[a] & b },
                { "borr", (a, b, c) => registers[c] = registers[a] | registers[b] },
                { "bori", (a, b, c) => registers[c] = registers[a] | b },
                { "setr", (a, b, c) => registers[c] = registers[a] },
                { "seti", (a, b, c) => registers[c] = a },
                { "gtir", (a, b, c) => registers[c] = a > registers[b] ? 1 : 0 },
                { "gtri", (a, b, c) => registers[c] = registers[a] > b ? 1 : 0 },
                { "gtrr", (a, b, c) => registers[c] = registers[a] > registers[b] ? 1 : 0 },
                { "eqir", (a, b, c) => registers[c] = a == registers[b] ? 1 : 0 },
                { "eqri", (a, b, c) => registers[c] = registers[a] == b ? 1 : 0 },
                { "eqrr", (a, b, c) => registers[c] = registers[a] == registers[b] ? 1 : 0 },
            };
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
                ops.Add(() => instructions[words[0]](int.Parse(words[1]), int.Parse(words[2]), int.Parse(words[3])));
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
