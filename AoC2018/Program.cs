using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2018
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = 0;

            while (true)
            {
                if (day == 0)
                {
                    Console.Clear();
                    Console.Write("Enter day: ");
                    var input = Console.ReadLine();
                    if (!int.TryParse(input,out day) || day < 1 || day > 25)
                    {
                        day = 0;
                        Console.WriteLine("Invalid day");
                    }
                    else
                    {
                        Console.WriteLine($"Day {day}");
                    }
                }
                else
                {
                    var solution = Activator.CreateInstance(Type.GetType($"AoC2018.Day{day:d2}")) as ISolution;
                    if (solution == null)
                    {
                        Console.WriteLine("Error loading solution");
                        day = 0;
                        continue;
                    }

                    solution.LoadInput(Directory.GetFiles("Input", $"Day{day:d2}?.txt"));

                    Console.WriteLine("Press 1 for part 1, 2 for part 2, or ESC to choose another day...");
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            Console.WriteLine("Running part 1...");
                            Console.WriteLine($"Answer: {solution.Part1()}");
                            break;
                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            Console.WriteLine("Running part 2...");
                            Console.WriteLine($"Answer: {solution.Part2()}");
                            break;
                        case ConsoleKey.Escape:
                            day = 0;
                            break;
                    }
                }
            }
        }
    }
}
