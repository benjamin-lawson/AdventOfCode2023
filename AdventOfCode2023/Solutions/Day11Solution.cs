using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Drawing;
using System.Diagnostics.Metrics;

namespace AdventOfCode2023
{
    public class Day11Solution : ISolution
    {
        public override string SolvePart1(string inputPath, bool debug)
        {
            string[] galaxyMap = GetProblemInput(inputPath);
            List<Tuple<int, int>> originalGalaxyPositions = FindAllGalaxyPositions(galaxyMap);
            var emptyRows = GetEmptyRowIndexes(galaxyMap);
            var emptyCols = GetEmptyColumnIndexes(galaxyMap);


            return CompareAllDistances(originalGalaxyPositions, emptyRows, emptyCols, 1).ToString();
        }

        public List<Tuple<int, int>> FindAllGalaxyPositions(string[] galaxyMap)
        {
            var galaxyPositions = new List<Tuple<int, int>>();
            for (int y = 0; y < galaxyMap.Length; y++)
            {
                for (int x = 0; x < galaxyMap[y].Length; x++)
                {
                    if (galaxyMap[y][x] == '#') galaxyPositions.Add(new Tuple<int, int>(x, y));
                }
            }
            return galaxyPositions;
        }

        public override string SolvePart2(string inputPath, bool debug)
        {
            string[] galaxyMap = GetProblemInput(inputPath);
            List<Tuple<int, int>> originalGalaxyPositions = FindAllGalaxyPositions(galaxyMap);
            var emptyRows = GetEmptyRowIndexes(galaxyMap);
            var emptyCols = GetEmptyColumnIndexes(galaxyMap);

            
            return CompareAllDistances(originalGalaxyPositions, emptyRows, emptyCols, 999999).ToString();
        }

        public long CompareAllDistances(List<Tuple<int, int>> galaxyPositions, HashSet<int> emptyRows, HashSet<int> emptyColumns, int expansionValue = 1)
        {
            long total = 0;

            for (int i = 0; i < galaxyPositions.Count; i++)
            {
                Tuple<int, int> galaxy1 = galaxyPositions[i];

                long new1X = galaxy1.Item1 + (expansionValue * emptyColumns.Count(e => e < galaxy1.Item1));
                long new1Y = galaxy1.Item2 + (expansionValue * emptyRows.Count(e => e < galaxy1.Item2));

                for (int j = i + 1; j < galaxyPositions.Count; j++)
                {
                    Tuple<int, int> galaxy2 = galaxyPositions[j];

                    long new2X = galaxy2.Item1 + (expansionValue * emptyColumns.Count(e => e < galaxy2.Item1));
                    long new2Y = galaxy2.Item2 + (expansionValue * emptyRows.Count(e => e < galaxy2.Item2));

                    total += Math.Abs(new1X - new2X) + Math.Abs(new1Y - new2Y);
                }
            }

            return total;
        }

        public HashSet<int> GetEmptyRowIndexes(string[] galaxyMap)
        {
            var emptyRows = new HashSet<int>();
            for (int i = 0; i < galaxyMap.Length; i++)
            {
                if (galaxyMap[i].All(c => c == '.')) emptyRows.Add(i);
            }
            return emptyRows;
        }

        public HashSet<int> GetEmptyColumnIndexes(string[] galaxyMap)
        {
            var emptyColumns = new HashSet<int>();
            for (int i = 0; i < galaxyMap[0].Length; i++)
            {
                if (galaxyMap.All(r => r[i] == '.')) emptyColumns.Add(i);
            }
            return emptyColumns;
        }
    }
}
