using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20
{
    class Program
    {
        private static int Part1(string input)
        {
            return GetDistances(input).Max();
        }

        private static int Part2(string input)
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
                        var index = directions[input[i]];
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

        private static readonly Dictionary<char, int> directions = new Dictionary<char, int>
        {
            { 'N', 0 },
            { 'E', 1 },
            { 'S', 2 },
            { 'W', 3 },
        };

        static void Main(string[] args)
        {
            var input = File.ReadAllText("Input.txt");

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
