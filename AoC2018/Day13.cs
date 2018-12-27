using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2018
{
    class Day13 : ISolution
    {
        private char[,] tracks;

        private List<Cart> carts = new List<Cart>();

        public void LoadInput(params string[] files)
        {
            var input = File.ReadAllLines(files[0]);
            tracks = new char[input.Length, input[0].Length];
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[i].Length; j++)
                {
                    switch (input[i][j])
                    {
                        case ' ':
                            continue;
                        case '<':
                            tracks[i, j] = '-';
                            carts.Add(new Cart
                            {
                                x = j,
                                y = i,
                                dx = -1,
                            });
                            break;
                        case '>':
                            tracks[i, j] = '-';
                            carts.Add(new Cart
                            {
                                x = j,
                                y = i,
                                dx = 1,
                            });
                            break;
                        case '^':
                            tracks[i, j] = '|';
                            carts.Add(new Cart
                            {
                                x = j,
                                y = i,
                                dy = -1,
                            });
                            break;
                        case 'v':
                            tracks[i, j] = '|';
                            carts.Add(new Cart
                            {
                                x = j,
                                y = i,
                                dy = 1,
                            });
                            break;
                        default:
                            tracks[i, j] = input[i][j];
                            break;
                    }
                }
            }
        }

        public object Part1()
        {
            while (true)
            {
                foreach (var cart in carts.OrderBy((Cart c) => c.y).ThenBy((Cart c) => c.x))
                {
                    cart.Tick(tracks);
                    if (cart.FindCollision(carts, out var collision))
                    {
                        return $"{cart.x},{cart.y}";
                    }
                }
            }
        }

        public object Part2()
        {
            while (carts.Count > 1)
            {
                foreach (var cart in carts.OrderBy((Cart c) => c.y).ThenBy((Cart c) => c.x))
                {
                    if (cart.FindCollision(carts, out var collision))
                    {
                        carts.Remove(cart);
                        carts.Remove(collision);
                    }

                    cart.Tick(tracks);
                }
            }

            return $"{carts[0].x},{carts[0].y}";
        }

        private class Cart
        {
            public int x;
            public int y;
            public int dx;
            public int dy;
            private int turn;

            public void Tick(char[,] tracks)
            {
                switch (tracks[y, x])
                {
                    case '+':
                        Turn();
                        break;
                    case '/':
                        if (dx == 0)
                        {
                            dx = -dy;
                            dy = 0;
                        }
                        else
                        {
                            dy = -dx;
                            dx = 0;
                        }
                        break;
                    case '\\':
                        if (dx == 0)
                        {
                            dx = dy;
                            dy = 0;
                        }
                        else
                        {
                            dy = dx;
                            dx = 0;
                        }
                        break;
                }

                x += dx;
                y += dy;
            }

            public bool FindCollision(List<Cart> carts, out Cart collision)
            {
                foreach (var cart in carts)
                {
                    if (cart == this)
                    {
                        continue;
                    }

                    if (cart.x == x && cart.y == y)
                    {
                        collision = cart;
                        return true;
                    }
                }

                collision = null;
                return false;
            }

            private void Turn()
            {
                if (turn == 0)
                {
                    if (dx == 0)
                    {
                        dx = dy;
                        dy = 0;
                    }
                    else
                    {
                        dy = -dx;
                        dx = 0;
                    }
                }
                else if (turn == 2)
                {
                    if (dx == 0)
                    {
                        dx = -dy;
                        dy = 0;
                    }
                    else
                    {
                        dy = dx;
                        dx = 0;
                    }
                }

                turn = (turn + 1) % 3;
            }
        }
    }
}
