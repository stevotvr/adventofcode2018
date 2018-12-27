using System;
using System.Diagnostics;
using System.IO;

namespace AoC2018
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
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
                    ISolution solution;
                    try
                    {
                        var solutionType = Type.GetType($"AoC2018.Day{day:d2}");
                        sw.Restart();
                        solution = (ISolution)Activator.CreateInstance(solutionType);
                        sw.Stop();
                    }
                    catch
                    {
                        Console.WriteLine("Error loading solution");
                        day = 0;
                        continue;
                    }

                    try
                    {
                        var files = Directory.GetFiles("Input", $"Day{day:d2}?.txt");
                        sw.Start();
                        solution.LoadInput(files);
                        sw.Stop();
                    }
                    catch
                    {
                        Console.WriteLine("Error loading solution input file");
                        day = 0;
                        continue;
                    }

                    Console.WriteLine("Press 1 for part 1, 2 for part 2, or ESC to choose another day...");
                    object answer;
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            Console.WriteLine("Running part 1...");
                            sw.Start();
                            answer = solution.Part1();
                            sw.Stop();
                            Console.WriteLine($"Answer: {answer}");
                            Console.WriteLine($"{sw.ElapsedMilliseconds}ms");
                            break;
                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            Console.WriteLine("Running part 2...");
                            sw.Start();
                            answer = solution.Part2();
                            sw.Stop();
                            Console.WriteLine($"Answer: {answer}");
                            Console.WriteLine($"{sw.ElapsedMilliseconds}ms");
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
