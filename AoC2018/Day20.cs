using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace AoC2018
{
    class Day20 : ISolution
    {
        private static readonly ReadOnlyDictionary<char, int> Directions = new ReadOnlyDictionary<char, int>(new Dictionary<char, int>
        {
            { 'N', 0 },
            { 'E', 1 },
            { 'S', 2 },
            { 'W', 3 },
        });

        private string input;

        public void LoadInput(params string[] files)
        {
            input = File.ReadAllText(files[0]);
        }

        public object Part1()
        {
            return GetDistances(input).Max();
        }

        public object Part2()
        {
            return GetDistances(input).Count(x => x >= 1000);
        }

        private static int[] GetDistances(string input)
        {
            input = input.Substring(1, input.Length - 2);
            var start = new Room();
            var stack = new Stack<Room>();
            var current = start;
            var i = 0;
            while (i < input.Length)
            {
                switch (input[i])
                {
                    case '(':
                        stack.Push(current);
                        break;
                    case ')':
                        current = stack.Pop();
                        break;
                    case '|':
                        current = stack.Peek();
                        break;
                    default:
                        var index = Directions[input[i]];
                        var newRoom = current.rooms[index] = current.rooms[index] ?? new Room();
                        newRoom.rooms[(index + 2) % 4] = current;
                        current = newRoom;
                        break;
                }

                i++;
            }

            var distances = new Dictionary<Room, int>();
            distances[start] = 0;
            var visited = new HashSet<Room>();
            stack.Push(start);
            while (stack.Count > 0)
            {
                var room = stack.Pop();
                visited.Add(room);
                foreach (var r in room.rooms)
                {
                    if (r == null || visited.Contains(r))
                    {
                        continue;
                    }

                    distances[r] = distances[room] + 1;
                    stack.Push(r);
                }
            }

            return distances.Values.ToArray();
        }

        private class Room
        {
            public Room[] rooms = new Room[4];
        }
    }
}
