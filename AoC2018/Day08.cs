using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2018
{
    class Day08 : ISolution
    {
        private int[] input;

        private int index;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllText(files[0]).Split(' ').Select((string s) => int.Parse(s)).ToArray();
        }

        public object Part1()
        {
            index = 0;
            return SumMeta(input);
        }

        public object Part2()
        {
            index = 0;
            return SumNode(BuildTree(input));
        }

        private int SumMeta(int[] data)
        {
            var children = data[index++];
            var entries = data[index++];
            var total = 0;
            for (var i = 0; i < children; i++)
            {
                total += SumMeta(data);
            }

            for (var i = 0; i < entries; i++)
            {
                total += data[index++];
            }

            return total;
        }

        private Node BuildTree(int[] data)
        {
            var node = new Node
            {
                Children = new List<Node>(),
                Meta = new List<int>(),
            };

            var children = data[index++];
            var entries = data[index++];
            for (var i = 0; i < children; i++)
            {
                node.Children.Add(BuildTree(data));
            }

            for (var i = 0; i < entries; i++)
            {
                node.Meta.Add(data[index++]);
            }

            return node;
        }

        private static int SumNode(Node node)
        {
            if (node.Children.Count == 0)
            {
                return node.Meta.Sum();
            }
            else
            {
                var sum = 0;
                foreach (var i in node.Meta)
                {
                    if (i > 0 && i <= node.Children.Count)
                    {
                        sum += SumNode(node.Children[i - 1]);
                    }
                }

                return sum;
            }
        }

        private struct Node
        {
            public List<Node> Children;
            public List<int> Meta;
        }
    }
}
