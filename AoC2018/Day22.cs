using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2018
{
    class Day22 : ISolution
    {
        private int depth;

        private int targetX;

        private int targetY;

        public void LoadInput(params string[] files)
        {
            var input = File.ReadAllLines(files[0]);
            depth = int.Parse(input[0].Substring(input[0].IndexOf(' ')));
            var target = input[1].Substring(input[1].IndexOf(' ')).Split(',').Select(x => int.Parse(x)).ToArray();
            targetX = target[0];
            targetY = target[1];
        }

        public object Part1()
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

        public object Part2()
        {
            var neighbors = new (int dx, int dy, bool switchTool)[] { (0, -1, false), (-1, 0, false), (1, 0, false), (0, 1, false), (0, 0, true) };
            var switchTo = new Tool[][] { new Tool[] { 0, Tool.Gear, Tool.Torch }, new Tool[] { Tool.Gear, 0, Tool.Neither }, new Tool[] { Tool.Torch, Tool.Neither, 0 } };

            var start = (new Node(null, 0, 0, depth, targetX, targetY), Tool.Torch);

            var grid = new List<List<Node>>
            {
                new List<Node>
                {
                    start.Item1,
                },
            };
            var maxX = 0;
            var maxY = 0;

            var times = new Dictionary<(Node node, Tool tool), int>
            {
                { start, 0 },
            };
            var queue = new PriorityQueue<(Node node, Tool tool)>();
            queue.Push(start, 0);
            while (!queue.Empty)
            {
                var state = queue.Pop();
                if (state.node.x == targetX && state.node.y == targetY && state.tool == Tool.Torch)
                {
                    return times[state];
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

                foreach (var (dx, dy, switchTool) in neighbors)
                {
                    if (state.node.x + dx < 0 || state.node.y + dy < 0)
                    {
                        continue;
                    }

                    (Node node, Tool tool) newState;
                    var time = times[state];
                    if (switchTool)
                    {
                        newState = (state.node, switchTo[(int)state.node.type][(int)state.tool]);
                        time += 7;
                    }
                    else
                    {
                        var edge = grid[state.node.y + dy][state.node.x + dx];
                        if ((int)state.tool == (int)edge.type)
                        {
                            continue;
                        }

                        newState = (edge, state.tool);
                        time++;
                    }

                    if (!times.ContainsKey(newState) || time < times[newState])
                    {
                        times[newState] = time;
                        queue.Push(newState, time);
                    }
                }
            }

            return 0;
        }

        private class PriorityQueue<T>
        {
            private List<(T item, int priority)> items = new List<(T, int)>();

            public bool Empty { get => items.Count == 0; }

            public void Push(T item, int priority)
            {
                this.items.Add((item, priority));

                for (var i = this.items.Count - 1; i > 0; i = (i - 1) / 2)
                {
                    if (this.items[i].priority > this.items[(i - 1) / 2].priority)
                    {
                        break;
                    }

                    var temp = this.items[(i - 1) / 2];
                    this.items[(i - 1) / 2] = this.items[i];
                    this.items[i] = temp;
                }
            }

            public T Pop()
            {
                var item = items[0];
                this.items.RemoveAt(0);
                return item.item;
            }
        }

        private struct Node
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

        private enum Terrain
        {
            Rocky,
            Wet,
            Narrow,
        }
    }
}
