using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2018
{
    class Day18 : ISolution
    {
        private string[] input;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);
        }

        public object Part1()
        {
            var state = input.Select(x => x.ToCharArray()).ToArray();
            for (var i = 0; i < 10; i++)
            {
                state = NextState(state);
            }

            return GetResourceValue(state);
        }

        public object Part2()
        {
            var state = input.Select(x => x.ToCharArray()).ToArray();
            var values = new List<int>();
            while (true)
            {
                state = NextState(state);
                var value = GetResourceValue(state);
                var index = values.LastIndexOf(value);
                if (index > -1)
                {
                    var prev = index - (values.Count - index);
                    if (prev >= 0 && values[prev] == value)
                    {
                        return values.GetRange(index, values.Count - index)[(1000000000 - index - 1) % (values.Count - index)];
                    }
                }

                values.Add(value);
            }
        }

        private static char[][] NextState(char[][] state)
        {
            var newState = new char[state.Length][];
            for (var r = 0; r < state.Length; r++)
            {
                newState[r] = new char[state[r].Length];
                for (var c = 0; c < state[r].Length; c++)
                {
                    var open = 0;
                    var trees = 0;
                    var lumber = 0;
                    for (var r2 = -1; r2 <= 1; r2++)
                    {
                        if (r + r2 < 0 || r + r2 >= state.Length)
                        {
                            continue;
                        }

                        for (var c2 = -1; c2 <= 1; c2++)
                        {
                            if (r2 == 0 && c2 == 0)
                            {
                                continue;
                            }

                            if (c + c2 < 0 || c + c2 >= state[r + r2].Length)
                            {
                                continue;
                            }

                            switch (state[r + r2][c + c2])
                            {
                                case '.':
                                    open++;
                                    break;
                                case '|':
                                    trees++;
                                    break;
                                case '#':
                                    lumber++;
                                    break;
                            }
                        }
                    }

                    switch (state[r][c])
                    {
                        case '.':
                            newState[r][c] = trees >= 3 ? '|' : '.';
                            break;
                        case '|':
                            newState[r][c] = lumber >= 3 ? '#' : '|';
                            break;
                        case '#':
                            newState[r][c] = (lumber > 0 && trees > 0) ? '#' : '.';
                            break;
                    }
                }

            }

            return newState;
        }

        private static int GetResourceValue(char[][] state)
        {
            return state.Sum(x => x.Sum(y => y == '|' ? 1 : 0)) * state.Sum(x => x.Sum(y => y == '#' ? 1 : 0));
        }
    }
}
