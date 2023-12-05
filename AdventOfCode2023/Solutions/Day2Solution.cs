using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Day2Solution : ISolution
    {
        public override string SolvePart1(string inputPath, bool debug)
        {
            string[] inputData = File.ReadAllLines(inputPath);

            int result = 0;
            foreach (string line in inputData)
            {
                int gameId = int.Parse(line.Split(':')[0].Replace("Game ", ""));
                bool addGame = true;
                foreach (string game in line.Split(':')[1].Split(';'))
                {
                    foreach (string marbleStr in game.Split(','))
                    {
                        string color = marbleStr.Trim().Split(' ')[1];
                        int count = int.Parse(marbleStr.Trim().Split(' ')[0]);

                        if (count > 12 && color == "red") addGame = false;
                        else if (count > 13 && color == "green") addGame = false;
                        else if (count > 14 && color == "blue") addGame = false;
                    }
                }
                if (addGame) result += gameId;
            }
            return $"{result}";
        }

        public override string SolvePart2(string inputPath, bool debug)
        {
            string[] inputData = File.ReadAllLines(inputPath);

            int result = 0;
            foreach (string line in inputData)
            {
                int gameId = int.Parse(line.Split(':')[0].Replace("Game ", ""));
                Dictionary<string, int> minimumValues = new Dictionary<string, int>()
                {
                    {"red", 0},
                    {"green", 0},
                    {"blue", 0},
                };

                foreach (string game in line.Split(':')[1].Split(';'))
                {
                    foreach (string marbleStr in game.Split(','))
                    {
                        string color = marbleStr.Trim().Split(' ')[1];
                        int count = int.Parse(marbleStr.Trim().Split(' ')[0]);

                        if (count > minimumValues[color]) minimumValues[color] = count;
                    }
                }

                int power = minimumValues["red"] * minimumValues["green"] * minimumValues["blue"];

                if (debug)
                {
                    Console.WriteLine($"Game: {gameId}");
                    Console.WriteLine($"Red: {minimumValues["red"]}");
                    Console.WriteLine($"Green: {minimumValues["green"]}");
                    Console.WriteLine($"Blue: {minimumValues["blue"]}");
                    Console.WriteLine($"Power: {power}");
                }

                result += power;
            }
            return $"{result}";
        }
    }
}
