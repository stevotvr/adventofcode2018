using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day15
{
    class Program
    {
        private static int Part1(List<Node> players, Node[,] input)
        {
            var rounds = 0;
            while (true)
            {
                for (var y = 0; y < input.GetLength(0); y++)
                {
                    for (var x = 0; x < input.GetLength(1); x++)
                    {
                        Console.Write(input[y, x] == null ? '#' : input[y, x].type);
                    }

                    Console.WriteLine();
                }

                foreach (var p in players)
                {
                    Console.WriteLine(p.hp);
                }

                Console.WriteLine(rounds);
                Console.WriteLine();

                for (var i = 0; i < players.Count; i++)
                {
                    if (players[i].type == '.')
                    {
                        continue;
                    }

                    if (!players.Any(x => x.type == (players[i].type == 'E' ? 'G' : 'E')))
                    {
                        return rounds * players.Where(x => x.type != '.').Select(x => x.hp).Sum();
                    }

                    if (Attack(players[i]))
                    {
                        continue;
                    }

                    if (FindNearest(players[i], out var nearest))
                    {
                        FindNearest(nearest, out var nextMove);
                        nextMove.type = players[i].type;
                        nextMove.hp = players[i].hp;
                        players[i].type = '.';
                        players[i] = nextMove;
                    }

                    Attack(players[i]);
                }

                players.RemoveAll(x => x.type == '.');
                players = players.OrderBy(x => x.y).ThenBy(x => x.x).ToList();
                rounds++;
            }

        }

        private static int Part2(List<Node> players)
        {
            return 0;
        }

        private static bool Attack(Node player)
        {
            foreach (var n in player.nodes.Where(x => x != null).OrderBy(x => x.hp).ThenBy(x => x.y).ThenBy(x => x.x))
            {
                if (n.type != '.' && n.type != player.type)
                {
                    n.hp -= player.attack;
                    if (n.hp <= 0)
                    {
                        n.type = '.';
                    }

                    return true;
                }
            }

            return false;
        }

        private static Queue<Node> queue = new Queue<Node>();

        private static HashSet<Node> visited = new HashSet<Node>();

        private static Dictionary<Node, int> distances = new Dictionary<Node, int>();

        private static bool FindNearest(Node player, out Node nearest)
        {
            distances.Clear();
            distances[player] = 0;
            queue.Clear();
            queue.Enqueue(player);
            visited.Clear();
            visited.Add(player);
            var list = new List<Node>();
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                foreach (var n in node.nodes)
                {
                    if (n == null || n.type == player.type || visited.Contains(n))
                    {
                        continue;
                    }

                    if (n.type != '.')
                    {
                        list.Add(node);
                        continue;
                    }

                    distances[n] = distances[node] + 1;
                    queue.Enqueue(n);
                    visited.Add(n);
                }
            }

            if (list.Count() < 1)
            {
                nearest = null;
                return false;
            }

            nearest = list.Distinct().OrderBy(x => distances[x]).ThenBy(x => x.y).ThenBy(x => x.x).First();
            return true;
        }

        private class Node
        {
            public int x;
            public int y;
            public Node[] nodes = new Node[4];
            public char type;
            public int hp = 200;
            public int attack = 3;
        }

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input.txt");
            var nodes = new Node[input.Length, input[0].Length];
            var players = new List<Node>();
            for (var y = 1; y < input.Length - 1; y++)
            {
                for (var x = 1; x < input[y].Length - 1; x++)
                {
                    if (input[y][x] == '#')
                    {
                        continue;
                    }

                    var node = nodes[y, x];
                    if (node == null)
                    {
                        node = new Node
                        {
                            x = x,
                            y = y,
                            type = input[y][x],
                        };
                        nodes[y, x] = node;
                    }

                    if (input[y][x] != '.')
                    {
                        players.Add(node);
                    }
                }
            }

            for (var y = 1; y < input.Length - 1; y++)
            {
                for (var x = 1; x < input[y].Length - 1; x++)
                {
                    var node = nodes[y, x];
                    if (node == null)
                    {
                        continue;
                    }

                    node.nodes[0] = nodes[y - 1, x];
                    node.nodes[1] = nodes[y, x - 1];
                    node.nodes[2] = nodes[y, x + 1];
                    node.nodes[3] = nodes[y + 1, x];
                }
            }

            while (true)
            {
                Console.WriteLine("Press 1 for part 1, 2 for part 2, Esc to exit...");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("Running part 1...");
                        Console.WriteLine($"Answer: {Part1(players, nodes)}");
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("Running part 2...");
                        Console.WriteLine($"Answer: {Part2(players)}");
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }
    }
}
