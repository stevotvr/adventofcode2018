using System;
using System.Collections.Generic;
using System.IO;

namespace AoC2018
{
    class Day21 : ISolution
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
            ParseProgram(out var ops, out var pcr, out int testReg);
            registers = new int[6];
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
            registers = new int[6];
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
                ops.Add(() => instructions[words[0]](int.Parse(words[1]), int.Parse(words[2]), int.Parse(words[3])));

                if (i == 29)
                {
                    testReg = int.Parse(words[1]);
                }
            }
        }
    }
}
