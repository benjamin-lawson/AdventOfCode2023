using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2023
{
    public class Day12Solution : ISolution
    {
        private List<int> _validGroups;
        private int _maxOfValidGroups;
        private int _sumOfValidGroups;
        private HashSet<string> _validPatterns = new HashSet<string>();

        public override string SolvePart1(string inputPath, bool debug)
        {
            string[] lines = GetProblemInput(inputPath);

            int total = 0;
            foreach (string line in lines)
            {
                string pattern = line.Split(' ')[0].Trim();
                _validGroups = line.Split(' ')[1].Split(',').Select(int.Parse).ToList();
                _maxOfValidGroups = _validGroups.Max();
                _sumOfValidGroups = _validGroups.Sum();

                total += RecursePattern(pattern, debug);
            }


            return total.ToString();
        }

        public override string SolvePart2(string inputPath, bool debug)
        {
            string[] lines = GetProblemInput(inputPath);

            int total = 0;
            foreach (string line in lines)
            {
                string pattern = line.Split(' ')[0].Trim();
                if (debug) Console.WriteLine($"Original Pattern: {pattern}");

                pattern = $"{pattern}?{pattern}?{pattern}?{pattern}?{pattern}"; 
                if (debug) Console.WriteLine($"Unfolded Pattern: {pattern}");

                _validGroups = line.Split(' ')[1].Split(',').Select(int.Parse).ToList();
                if (debug) Console.WriteLine($"Original Groups: {string.Join(',', _validGroups)}");
                _validGroups = UnfoldGroups(_validGroups);
                if (debug) Console.WriteLine($"Unfolded Groups: {string.Join(',', _validGroups)}");

                _maxOfValidGroups = _validGroups.Max();
                _sumOfValidGroups = _validGroups.Sum();

                int arrangements = RecursePattern(pattern, debug);
                if (debug) Console.WriteLine($"Arrangements: {arrangements}");

                total += arrangements;
            }

            return total.ToString();
        }

        public bool IsPatternValid(string pattern)
        {
            if (pattern.Contains('?'))
            {
                Console.Error.WriteLine("Valid patterns should only contain '.' and '#'");
                return false;
            }

            List<int> patternGroups = GetPatternGroups(pattern);
            return Enumerable.SequenceEqual(patternGroups, _validGroups);
        }

        public List<int> GetPatternGroups(string pattern)
        {
            List<int> groups = new List<int>();
            int groupCount = 0;
            char lastChar = '.';

            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] == '#')
                {
                    if (groups.Count <= groupCount) groups.Add(1);
                    else groups[groupCount]++;
                }
                else
                {
                    if (lastChar == '#') groupCount++;
                }
                lastChar = pattern[i];
            }

            return groups;
        }

        public int RecursePattern(string pattern, bool debug = false)
        {
            if (_validPatterns.Contains(pattern)) return 1;

            if (!pattern.Contains('?'))
            {
                if (IsPatternValid(pattern))
                {
                    _validPatterns.Add(pattern);
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            if (pattern.Count(c => c == '#') > _sumOfValidGroups)
            {
                return 0;
            }

            List<int> currentGroups = GetPatternGroups(pattern);
            if (currentGroups.Count > 0 && currentGroups.Max() > _maxOfValidGroups)
            {
                return 0;
            }

            int replaceIndex = pattern.IndexOf('?');
            return RecursePattern(pattern.Remove(replaceIndex, 1).Insert(replaceIndex, "#"), debug) + RecursePattern(pattern.Remove(replaceIndex, 1).Insert(replaceIndex, "."), debug);
        }

        public string UnfoldPattern(string pattern)
        {
            return $"{pattern}?{pattern}?{pattern}?{pattern}?{pattern}";
        }

        public List<int> UnfoldGroups(List<int> groups)
        {
            List<int> newGroups = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                newGroups.AddRange(groups);
            }
            return newGroups;
        }
    }
}
