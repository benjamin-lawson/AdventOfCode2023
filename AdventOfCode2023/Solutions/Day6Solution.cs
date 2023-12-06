using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Day6Solution : ISolution
    {
        public override string SolvePart1(string inputPath, bool debug)
        {
            string[] lines = GetProblemInput(inputPath);

            long[] times = lines[0]
                .Replace("Time:", "")
                .Trim()
                .Split(' ')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(long.Parse)
                .ToArray();

            long[] distances = lines[1]
                .Replace("Distance:", "")
                .Trim()
                .Split(' ')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(long.Parse)
                .ToArray();

            long total = 1;
            for (long i = 0; i < times.Length; i++)
            {
                total = total * CountWinningHoldTimes(times[i], distances[i]);
            }

            return total.ToString();
        }

        public override string SolvePart2(string inputPath, bool debug)
        {
            string[] lines = GetProblemInput(inputPath);

            long time = long.Parse(
                lines[0]
                .Replace("Time:", "")
                .Trim()
                .Replace(" ", "")
            );

            long distance = long.Parse(
                lines[1]
                .Replace("Distance:", "")
                .Trim()
                .Replace(" ", "")
            );

            if (debug)
            {
                Console.WriteLine($"Race Time: {time} | Distance: {distance}");
            }

            return CountWinningHoldTimes(time, distance).ToString();
        }

        public long CountWinningHoldTimes(long raceTime, long previousWinningDistance)
        {
            int total = 0;
            for (long holdTime = 1; holdTime < raceTime + 1; holdTime++)
            {
                if (SimulateRace(holdTime, raceTime) > previousWinningDistance) total++;
            }
            return total;
        }

        public long SimulateRace(long buttonHoldTime, long totalTime)
        {
            return (totalTime - buttonHoldTime) * buttonHoldTime;
        }
    }
}
