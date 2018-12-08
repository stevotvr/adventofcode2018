using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day08
{
    class Program
    {
        private static int index;

        private static int Part1(int[] input)
        {
            index = 0;
            return SumMeta(input);
        }

        private static int SumMeta(int[] data)
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

        private static int Part2(int[] input)
        {
            index = 0;
            return SumNode(BuildTree(input));
        }

        private static Node BuildTree(int[] data)
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

        static void Main(string[] args)
        {
            var input = File.ReadAllText("Input.txt").Split(' ').Select((string s) => int.Parse(s)).ToArray();

            while (true)
            {
                Console.WriteLine("Press 1 for part 1, 2 for part 2, Esc to exit...");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("Running part 1...");
                        Console.WriteLine($"Answer: {Part1(input)}");
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("Running part 2...");
                        Console.WriteLine($"Answer: {Part2(input)}");
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}
