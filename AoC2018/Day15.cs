using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2018
{
    class Day15 : ISolution
    {
        private string[] input;

        private Node[,] nodes;

        private List<Node> players;

        private bool allowElfDeath = true;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllLines(files[0]);
            nodes = new Node[input.Length, input[0].Length];
            players = new List<Node>();
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
        }

        public object Part1()
        {
            var rounds = 0;
            while (true)
            {
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
                        if (victim.hp <= 0)
                        {
                            if (!allowElfDeath && victim.type == 'E')
                            {
                                return 0;
                            }

                            players.Remove(victim);
                            victim.type = '.';
                        }

                        continue;
                    }

                    if (FindNearestAdjacent(player, x => x.type == opp, out var nearest))
                    {
                        FindNearestAdjacent(nearest, x => x == player, out var nextMove);
                        nextMove.type = player.type;
                        nextMove.hp = player.hp;
                        nextMove.attack = player.attack;
                        player.type = '.';
                        players.Remove(player);
                        players.Add(nextMove);

                        Attack(nextMove, out victim);
                        if (victim?.hp <= 0)
                        {
                            if (!allowElfDeath && victim.type == 'E')
                            {
                                return 0;
                            }

                            players.Remove(victim);
                            victim.type = '.';
                        }
                    }
                }

                players = players.OrderBy(x => x.y).ThenBy(x => x.x).ToList();
                rounds++;
            }
        }

        public object Part2()
        {
            allowElfDeath = false;
            var elfAttack = 4;
            var elfCount = players.Count(x => x.type == 'E');
            while (true)
            {
                players.Clear();
                for (var y = 1; y < input.Length - 1; y++)
                {
                    for (var x = 1; x < input[y].Length - 1; x++)
                    {
                        var node = nodes[y, x];
                        if (node == null)
                        {
                            continue;
                        }

                        node.type = input[y][x];
                        node.hp = 200;
                        node.attack = node.type == 'E' ? elfAttack : 3;
                        if (node.type != '.')
                        {
                            players.Add(node);
                        }
                    }
                }

                var result = (int)Part1();
                if (result > 0)
                {
                    return result;
                }

                elfAttack++;
            }
        }

        private static bool Attack(Node player, out Node victim)
        {
            foreach (var e in player.edges.Where(x => x != null).OrderBy(x => x.hp).ThenBy(x => x.y).ThenBy(x => x.x))
            {
                if (e.type != '.' && e.type != player.type)
                {
                    e.hp -= player.attack;
                    victim = e;
                    return true;
                }
            }

            victim = null;
            return false;
        }

        private Queue<Node> queue = new Queue<Node>();

        private HashSet<Node> visited = new HashSet<Node>();

        private Dictionary<Node, int> distances = new Dictionary<Node, int>();

        private List<Node> matches = new List<Node>();

        private bool FindNearestAdjacent(Node player, Func<Node, bool> cond, out Node nearest)
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
    }
}
