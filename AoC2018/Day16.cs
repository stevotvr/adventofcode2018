using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2018
{
    class Day16 : ISolution
    {
        private string[] input1;

        private string[] input2;

        private int[] registers = new int[4];

        private Dictionary<string, Action<int, int, int>> instructions;

        public void LoadInput(params string[] files)
        {
            input1 = File.ReadAllText(files[0]).Split(new string[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            input2 = File.ReadAllLines(files[1]);

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
            var keys = new int[] { 0, 1, 2, 3 };
            var count = 0;
            foreach (var sample in input1)
            {
                var lines = sample.Split('\n');
                var before = lines[0].Substring(9, 10).Split(',').Select(x => int.Parse(x.Trim())).ToArray();
                var test = lines[1].Split(' ').Select(x => int.Parse(x)).ToArray();
                var after = lines[2].Substring(9, 10).Split(',').Select(x => int.Parse(x.Trim())).ToArray();

                var c = 0;
                foreach (var instruction in instructions.Values)
                {
                    before.CopyTo(registers, 0);
                    instruction(test[1], test[2], test[3]);
                    if (keys.All(x => registers[x] == after[x]))
                    {
                        c++;
                        if (c >= 3)
                        {
                            count++;
                            break;
                        }
                    }
                }
            }

            return count;
        }

        public object Part2()
        {
            var keys = new int[] { 0, 1, 2, 3 };
            var candidates = new Dictionary<int, HashSet<string>>();
            var instructionMap = new Dictionary<int, string>();
            for (var i = 0; i < 16; i++)
            {
                candidates[i] = new HashSet<string>(instructions.Keys);
            }

            foreach (var sample in input1)
            {
                var lines = sample.Split('\n');
                var before = lines[0].Substring(9, 10).Split(',').Select(x => int.Parse(x.Trim())).ToArray();
                var test = lines[1].Split(' ').Select(x => int.Parse(x)).ToArray();
                var after = lines[2].Substring(9, 10).Split(',').Select(x => int.Parse(x.Trim())).ToArray();

                foreach (var instruction in instructions)
                {
                    if (candidates[test[0]].Count == 0)
                    {
                        continue;
                    }

                    before.CopyTo(registers, 0);
                    instruction.Value(test[1], test[2], test[3]);
                    if (!keys.All(x => registers[x] == after[x]))
                    {
                        candidates[test[0]].Remove(instruction.Key);
                        if (candidates[test[0]].Count == 1)
                        {
                            instructionMap[test[0]] = candidates[test[0]].First();
                            foreach (var hs in candidates.Values)
                            {
                                hs.Remove(instructionMap[test[0]]);
                            }
                        }
                    }
                }
            }

            registers = new int[4];
            foreach (var line in input2)
            {
                var op = line.Split(' ').Select(x => int.Parse(x)).ToArray();
                instructions[instructionMap[op[0]]](op[1], op[2], op[3]);
            }

            return registers[0];
        }
    }
}
