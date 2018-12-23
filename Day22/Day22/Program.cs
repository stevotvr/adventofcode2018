using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day22
{
    class Program
    {
        private static int Part1(int depth, int targetX, int targetY)
        {
            var erosion = new int[targetX + 1, targetY + 1];
            var risk = 0;
            for (var y = 0; y <= targetY; y++)
            {
                for (var x = 0; x <= targetX; x++)
                {
                    var geo = 0;
                    if ((x == 0 && y == 0) || (x == targetX && y == targetY))
                    {
                        geo = 0;
                    }
                    else if (y == 0)
                    {
                        geo = x * 16807;
                    }
                    else if (x == 0)
                    {
                        geo = y * 48271;
                    }
                    else
                    {
                        geo = erosion[x - 1, y] * erosion[x, y - 1];
                    }

                    erosion[x, y] = (geo + depth) % 20183;
                    risk += erosion[x, y] % 3;
                }
            }

            return risk;
        }

        private static int Part2(int depth, int targetX, int targetY)
        {
            var bounds = (int)(Math.Max(targetX, targetY) * 1.2);
            var neighbors = new int[][] { new int[] { 0, -1 }, new int[] { -1, 0 }, new int[] { 1, 0 }, new int[] { 0, 1 } };

            var start = new State
            {
                node = new Node(null, 0, 0, depth, targetX, targetY),
                tool = Tool.Torch,
            };

            var grid = new List<List<Node>>
            {
                new List<Node>
                {
                    start.node,
                },
            };
            var maxX = 0;
            var maxY = 0;

            var times = new Dictionary<State, int>
            {
                { start, 0 },
            };
            var queue = new Queue<State>();
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                var state = queue.Dequeue();
                if (state.node.x >= bounds || state.node.y >= bounds)
                {
                    continue;
                }

                if (maxX < state.node.x + 1)
                {
                    maxX++;
                    for (var y = 0; y <= maxY; y++)
                    {
                        grid[y].Add(new Node(grid, maxX, y, depth, targetX, targetY));
                    }
                }

                if (maxY < state.node.y + 1)
                {
                    maxY++;
                    grid.Add(new List<Node>());
                    for (var x = 0; x <= maxX; x++)
                    {
                        grid[maxY].Add(new Node(grid, x, maxY, depth, targetX, targetY));
                    }
                }

                var newStates = new List<State>();
                foreach (var n in neighbors)
                {
                    if (state.node.x + n[0] < 0 || state.node.y + n[1] < 0)
                    {
                        continue;
                    }

                    var edge = grid[state.node.y + n[1]][state.node.x + n[0]];
                    switch (state.node.type)
                    {
                        case Terrain.Rocky:
                            switch (edge.type)
                            {
                                case Terrain.Rocky:
                                    newStates.Add(new State(edge, Tool.Torch));
                                    newStates.Add(new State(edge, Tool.Gear));
                                    break;
                                case Terrain.Wet:
                                    newStates.Add(new State(edge, Tool.Gear));
                                    break;
                                case Terrain.Narrow:
                                    newStates.Add(new State(edge, Tool.Torch));
                                    break;
                            }

                            break;
                        case Terrain.Wet:
                            switch (edge.type)
                            {
                                case Terrain.Rocky:
                                    newStates.Add(new State(edge, Tool.Gear));
                                    break;
                                case Terrain.Wet:
                                    newStates.Add(new State(edge, Tool.Neither));
                                    newStates.Add(new State(edge, Tool.Gear));
                                    break;
                                case Terrain.Narrow:
                                    newStates.Add(new State(edge, Tool.Neither));
                                    break;
                            }

                            break;
                        case Terrain.Narrow:
                            switch (edge.type)
                            {
                                case Terrain.Rocky:
                                    newStates.Add(new State(edge, Tool.Torch));
                                    break;
                                case Terrain.Wet:
                                    newStates.Add(new State(edge, Tool.Neither));
                                    break;
                                case Terrain.Narrow:
                                    newStates.Add(new State(edge, Tool.Neither));
                                    newStates.Add(new State(edge, Tool.Torch));
                                    break;
                            }

                            break;
                    }
                }

                foreach (var newState in newStates)
                {
                    var time = times[state] + (newState.tool == state.tool ? 1 : 8);
                    if (!times.ContainsKey(newState) || time < times[newState])
                    {
                        times[newState] = time;
                        queue.Enqueue(newState);
                    }
                }
            }

            return times.Where(x => x.Key.node.x == targetX && x.Key.node.y == targetY && x.Key.tool == Tool.Torch).Min(x => x.Value);
        }

        private struct State : IEquatable<State>
        {
            public Node node;
            public Tool tool;

            public State(Node node, Tool tool)
            {
                this.node = node;
                this.tool = tool;
            }

            public bool Equals(State other)
            {
                return this.node == other.node && this.tool == other.tool;
            }
        }

        private class Node
        {
            public int x;
            public int y;
            public int erosion;
            public Terrain type;

            public Node(List<List<Node>> grid, int x, int y, int depth, int targetX, int targetY)
            {
                this.x = x;
                this.y = y;

                var geo = 0;
                if ((x == 0 && y == 0) || (x == targetX && y == targetY))
                {
                    geo = 0;
                }
                else if (y == 0)
                {
                    geo = x * 16807;
                }
                else if (x == 0)
                {
                    geo = y * 48271;
                }
                else
                {
                    geo = grid[y][x - 1].erosion * grid[y - 1][x].erosion;
                }

                this.erosion = (geo + depth) % 20183;
                this.type = (Terrain)(this.erosion % 3);
            }
        }

        private enum Tool
        {
            Neither,
            Torch,
            Gear,
        }

        private enum Terrain : int
        {
            Rocky,
            Wet,
            Narrow,
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input.txt");
            var depth = int.Parse(input[0].Substring(input[0].IndexOf(' ')));
            var target = input[1].Substring(input[1].IndexOf(' ')).Split(',').Select(x => int.Parse(x)).ToArray();

            while (true)
            {
                Console.WriteLine("Press 1 for part 1, 2 for part 2, Esc to exit...");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("Running part 1...");
                        Console.WriteLine($"Answer: {Part1(depth, target[0], target[1])}");
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("Running part 2...");
                        Console.WriteLine($"Answer: {Part2(depth, target[0], target[1])}");
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}
