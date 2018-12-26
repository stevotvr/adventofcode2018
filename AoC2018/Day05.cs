using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace AoC2018
{
    class Day05 : ISolution
    {
        private string input;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllText(files[0]).Trim();
        }

        public object Part1()
        {
            return React(input).Length;
        }

        private static string React(string input)
        {
            var sb = new StringBuilder();
            var stop = false;
            while (!stop)
            {
                var lower = input.ToLower();
                stop = true;
                for (var i = 0; i < input.Length - 1; i++)
                {
                    if (lower[i] == lower[i + 1])
                    {
                        if ((input[i] >= 'A' && input[i] <= 'Z' && input[i + 1] >= 'a' && input[i + 1] <= 'z') || (input[i] >= 'a' && input[i] <= 'z' && input[i + 1] >= 'A' && input[i + 1] <= 'Z'))
                        {
                            stop = false;
                            sb.Append(input.Substring(i + 2));
                            break;
                        }
                    }

                    sb.Append(input[i]);
                }

                if (stop)
                {
                    sb.Append(input[input.Length - 1]);
                }

                input = sb.ToString();
                sb.Clear();
            }

            return input;
        }

        public object Part2()
        {
            var handles = new WaitHandle[26];
            var lengths = new int[26];
            input = React(input);
            var lower = input.ToLower();
            for (var i = 0; i < 26; i++)
            {
                handles[i] = new AutoResetEvent(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(TryPoly), new object[] { input, i, lower, lengths, handles[i] });
            }

            WaitHandle.WaitAll(handles);

            return lengths.Min();
        }

        private void TryPoly(object state)
        {
            var data = (object[])state;
            var input = (string)data[0];
            var c = ((int)data[1]) + 'a';
            var lower = (string)data[2];
            var sb = new StringBuilder();
            for (var i = 0; i < input.Length; i++)
            {
                if (lower[i] != c)
                {
                    sb.Append(input[i]);
                }
            }

            ((int[])data[3])[c - 'a'] = React(sb.ToString()).Length;
            ((AutoResetEvent)data[4]).Set();
        }
    }
}
