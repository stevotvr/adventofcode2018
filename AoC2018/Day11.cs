using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace AoC2018
{
    class Day11 : ISolution
    {
        private int input;

        public void LoadInput(params string[] files)
        {
            input = int.Parse(File.ReadAllText(files[0]));
        }

        public object Part1()
        {
            var max = int.MinValue;
            var maxX = 0;
            var maxY = 0;
            for (var y = 1; y < 299; y++)
            {
                for (var x = 1; x < 299; x++)
                {
                    var total = 0;
                    for (var i = 0; i < 3; i++)
                    {
                        var id = x + i + 10;
                        for (var j = 0; j < 3; j++)
                        {
                            var power = (id * (y + j) + input) * id;
                            power = power < 100 ? 0 : int.Parse(power.ToString().Reverse().ToArray()[2].ToString());
                            total += power - 5;
                        }
                    }

                    if (total > max)
                    {
                        max = total;
                        maxX = x;
                        maxY = y;
                    }
                }
            }

            return $"{maxX},{maxY}";
        }

        public object Part2()
        {
            var grid = new int[300, 300];
            for (var x = 0; x < 300; x++)
            {
                var id = x + 11;
                for (var y = 0; y < 300; y++)
                {
                    var power = (id * (y + 1) + input) * id;
                    power = power < 100 ? 0 : int.Parse(power.ToString().Reverse().ToArray()[2].ToString());
                    grid[x, y] = power - 5;
                }
            }

            var results = new int[300];
            var bestBlock = new (int x, int y)[300];
            var handles = new WaitHandle[64];

            for (var i = 0; i < 64; i++)
            {
                handles[i] = new AutoResetEvent(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(TrySize), new object[] { grid, i * 64, 64, results, bestBlock, handles[i] });
            }

            WaitHandle.WaitAll(handles);

            var size = Array.IndexOf(results, results.Max());
            return $"{bestBlock[size].x},{bestBlock[size].y},{size}";
        }

        private static void TrySize(object state)
        {
            var data = (object[])state;
            var grid = (int[,])data[0];
            var start = (int)data[1];
            var end = start + (int)data[2];
            var results = (int[])data[3];
            var bestBlock = ((int x, int y)[])data[4];
            end = Math.Min(end, results.Length - 1);

            for (var s = start; s < end; s++)
            {
                results[s] = int.MinValue;
                for (var y = 0; y <= 300 - s; y++)
                {
                    for (var x = 0; x <= 300 - s; x++)
                    {
                        var total = 0;
                        for (var i = 0; i < s; i++)
                        {
                            for (var j = 0; j < s; j++)
                            {
                                total += grid[x + i, y + j];
                            }
                        }

                        if (total > results[s])
                        {
                            results[s] = total;
                            bestBlock[s] = (x + 1, y + 1);
                        }
                    }
                }
            }

            ((AutoResetEvent)data[5]).Set();
        }
    }
}
