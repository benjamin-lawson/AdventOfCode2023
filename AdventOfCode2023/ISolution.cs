using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public enum ProblemPart
    {
        PART1,
        PART2
    }

    public abstract class ISolution
    {
        public string Solve(string inputPath, ProblemPart part, bool debug = false)
        {
            Console.WriteLine($"Solving for {inputPath} - {part}");
            if (debug) Console.WriteLine("*** DEBUG ENABLED");
            string answer = part == ProblemPart.PART1 ? SolvePart1(inputPath, debug) : SolvePart2(inputPath, debug);
            if (debug) Console.WriteLine("*** DEBUG DISABLED");
            return answer;
        }

        public abstract string SolvePart1(string inputPath, bool debug);
        public abstract string SolvePart2(string inputPath, bool debug);
    }
}
