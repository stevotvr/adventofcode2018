using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                    Console.WriteLine($"{p.type} {p.hp} {p.y},{p.x}");
                }

                Console.WriteLine(rounds);
                Console.WriteLine();

                var queue = new Queue<Node>(players);
                while (queue.Count > 0)
                {
                    var player = queue.Dequeue();
                    if (player.type == '.')
                    {
                        continue;
                    }

                    var opp = player.type == 'E' ? 'G' : 'E';
                    if (!players.Any(x => x.type == opp))
                    {
                        return rounds * players.Select(x => x.hp).Sum();
                    }

                    if (Attack(player, out var victim))
                    {
                        if (victim.type == '.')
                        {
                            players.Remove(victim);
                        }

                        continue;
                    }

                    if (FindNearestAdjacent(player, x => x.type == opp, out var nearest))
                    {
                        FindNearestAdjacent(nearest, x => x == player, out var nextMove);
                        nextMove.type = player.type;
                        nextMove.hp = player.hp;
                        player.type = '.';
                        players.Remove(player);
                        players.Add(nextMove);

                        Attack(nextMove, out victim);
                        if (victim?.type == '.')
                        {
                            players.Remove(victim);
                        }
                    }
                }

                players = players.OrderBy(x => x.y).ThenBy(x => x.x).ToList();
                rounds++;
            }

        }

        private static int Part2(List<Node> players)
        {
            return 0;
        }

        private static bool Attack(Node player, out Node victim)
        {
            foreach (var e in player.edges.Where(x => x != null).OrderBy(x => x.hp).ThenBy(x => x.y).ThenBy(x => x.x))
            {
                if (e.type != '.' && e.type != player.type)
                {
                    e.hp -= player.attack;
                    if (e.hp <= 0)
                    {
                        e.type = '.';
                    }

                    victim = e;
                    return true;
                }
            }

            victim = null;
            return false;
        }

        private static Queue<Node> queue = new Queue<Node>();

        private static HashSet<Node> visited = new HashSet<Node>();

        private static Dictionary<Node, int> distances = new Dictionary<Node, int>();

        private static List<Node> matches = new List<Node>();

        private static bool FindNearestAdjacent(Node player, Func<Node, bool> cond, out Node nearest)
        {
            distances.Clear();
            distances[player] = 0;
            queue.Clear();
            queue.Enqueue(player);
            visited.Clear();
            visited.Add(player);
            matches.Clear();
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                foreach (var e in node.edges)
                {
                    if (e == null || visited.Contains(e))
                    {
                        continue;
                    }

                    if (cond(e))
                    {
                        matches.Add(node);
                        continue;
                    }

                    if (e.type != '.')
                    {
                        continue;
                    }

                    distances[e] = distances[node] + 1;
                    queue.Enqueue(e);
                    visited.Add(e);
                }
            }

            if (matches.Count() < 1)
            {
                nearest = null;
                return false;
            }

            nearest = matches.OrderBy(x => distances[x]).ThenBy(x => x.y).ThenBy(x => x.x).First();
            return true;
        }

        private class Node
        {
            public int x;
            public int y;
            public Node[] edges = new Node[4];
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

                    node.edges[0] = nodes[y - 1, x];
                    node.edges[1] = nodes[y, x - 1];
                    node.edges[2] = nodes[y, x + 1];
                    node.edges[3] = nodes[y + 1, x];
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
