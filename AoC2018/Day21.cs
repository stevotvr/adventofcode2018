using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AoC2018
{
    class Day21 : ISolution
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

        private int[] registers = new int[6];

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);
        }

        public object Part1()
        {
            ParseProgram(out var ops, out var pcr, out int testReg);
            var pc = 0;
            while (registers[pcr] >= 0 && registers[pcr] < ops.Count)
            {
                registers[pcr] = pc;
                ops[pc]();
                pc = registers[pcr];

                if (pc == 28)
                {
                    return registers[testReg];
                }

                pc++;
            }

            return 0;
        }

        public object Part2()
        {
            ParseProgram(out var ops, out var pcr, out int testReg);
            var states = new HashSet<int>();
            var pc = 0;
            var prev = 0;
            while (pc >= 0 && pc < ops.Count)
            {
                registers[pcr] = pc;
                ops[pc]();
                pc = registers[pcr];

                if (pc == 28)
                {
                    if (!states.Add(registers[testReg]))
                    {
                        return prev;
                    }

                    prev = registers[testReg];
                }

                pc++;
            }

            return 0;
        }

        private void ParseProgram(out List<Action> ops, out int pcr, out int testReg)
        {
            ops = new List<Action>();
            pcr = int.Parse(input[0].Substring(input[0].IndexOf(' ')));
            testReg = -1;
            for (var i = 1; i < input.Length; i++)
            {
                var words = input[i].Split(' ');
                ops.Add(() => instructions[words[0]](registers, int.Parse(words[1]), int.Parse(words[2]), int.Parse(words[3])));

                if (i == 29)
                {
                    testReg = int.Parse(words[1]);
                }
            }
        }
    }
}
