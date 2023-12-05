using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public class Day1Solution : ISolution
    {
        public override string SolvePart1(string inputPath, bool debug)
        {
            string[] inputData = File.ReadAllLines(inputPath);

            int sum = 0;
            foreach (string line in inputData)
            {
                string replacedString = Regex.Replace(line, @"[^0-9]+", "");
                if (debug) Console.WriteLine($"Final - {replacedString.First()}{replacedString.Last()}");
                sum += int.Parse($"{replacedString.First()}{replacedString.Last()}");
            }

            return sum.ToString();
        }

        public override string SolvePart2(string inputPath, bool debug)
        {
            string[] inputData = File.ReadAllLines(inputPath);
            

            int sum = 0;
            foreach (string line in inputData)
            {

                string numString = $"{GetFirstNumber(line)}{GetLastNumber(line)}";
                if (debug) Console.WriteLine(numString);
                sum += int.Parse(numString);
            }

            return sum.ToString();
        }

        private Dictionary<string, string> _digits = new Dictionary<string, string>() {
                { "one", "1" },
                { "two", "2" },
                { "three", "3" },
                { "four", "4" },
                { "five", "5" },
                { "six", "6" },
                { "seven", "7" },
                { "eight", "8" },
                { "nine", "9" },
            };

        private string GetFirstNumber(string input)
        {
            string buffer = "";
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (Char.IsLetter(c))
                {
                    buffer += c;
                }
                else
                {
                    return c.ToString();
                }
                string? digit = _digits.Where(kv => buffer.Contains(kv.Key)).Select(kv => kv.Value).FirstOrDefault();

                if (digit is not null)
                {
                    return digit;
                }
            }

            return "";
        }

        private string GetLastNumber(string input)
        {
            string buffer = "";
            for (int i = input.Length - 1; i >= 0; i--)
            {
                char c = input[i];
                if (Char.IsLetter(c))
                {
                    buffer = c + buffer;
                }
                else
                {
                    return c.ToString();
                }
                string? digit = _digits.Where(kv => buffer.Contains(kv.Key)).Select(kv => kv.Value).FirstOrDefault();

                if (digit is not null)
                {
                    return digit;
                }
            }

            return "";
        }
    }
}
