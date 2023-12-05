
namespace AdventOfCode2023
{
    internal class Day3Solution : ISolution
    {
        private List<Tuple<int, int>> _surroundOffsets = new List<Tuple<int, int>>() 
        {
            new Tuple<int, int>(-1, -1),
            new Tuple<int, int>(-1, 0),
            new Tuple<int, int>(-1, 1),
            new Tuple<int, int>(0, -1),
            new Tuple<int, int>(0, 1),
            new Tuple<int, int>(1, -1),
            new Tuple<int, int>(1, 0),
            new Tuple<int, int>(1, 1),
        };

        private int[][] GenerateEmptyMap(int width, int height)
        {
            int[][] map = new int[height][];
            for (int y = 0; y < height; y++)
            {
                map[y] = new int[width];
                for (int x = 0; x < width; x++)
                {
                    map[y][x] = 0;
                }
            }
            return map;
        }

        public override string SolvePart1(string inputPath, bool debug)
        {
            string[] lines = File.ReadAllLines(inputPath);
            int height = lines.Length;
            int width = lines[0].Length;

            int[][] map = GenerateEmptyMap(width, height);
            for (int y = 0; y < height; y++)
            {
                string line = lines[y];
                for (int x = 0; x < width; x++)
                {
                    char c = line[x];
                    if (c == '.') continue;
                    else if (Char.IsDigit(c)) map[y][x] = 1;
                    else map[y][x] = -1;
                }
            }

            int[][] validGroupsMap = GenerateEmptyMap(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[y][x] == -1)
                    {
                        foreach (var offset in _surroundOffsets)
                        {
                            if (x + offset.Item1 < 0 || x + offset.Item1 >= width || y + offset.Item2 < 0 || y + offset.Item2 >= height) continue;
                            validGroupsMap[y + offset.Item2][x + offset.Item1] += 1;
                        }
                    } 
                }
            }

            string buffer = "";
            bool addBuffer = false;
            int result = 0;
            for (int y = 0; y < height; y++)
            {
                string line = lines[y];
                for (int x = 0; x < width; x++)
                {
                    if (map[y][x] != 1) 
                    {
                        if (addBuffer)
                        {
                            result += int.Parse(buffer);
                            addBuffer = false;
                        }
                        buffer = "";
                    }
                    else if (map[y][x] == 1) {
                        buffer += line[x];
                        if (validGroupsMap[y][x] > 0) addBuffer = true;
                    }
                    continue;
                }
            }

            return $"{result}";
        }

        public override string SolvePart2(string inputPath, bool debug)
        {
            string[] lines = File.ReadAllLines(inputPath);
            int height = lines.Length;
            int width = lines[0].Length;

            int[][] numMap = GenerateEmptyMap(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (Char.IsDigit(lines[y][x])) numMap[y][x] = 1;
                }
            }

            int[][] gearMap = GenerateEmptyMap(width, height);
            List<Tuple<int, int>> validGearPoints = new List<Tuple<int, int>>();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (lines[y][x] == '*')
                    {
                        int surroundCount = 0;
                        foreach (var offset in _surroundOffsets)
                        {
                            if (x + offset.Item1 < 0 || x + offset.Item1 >= width || y + offset.Item2 < 0 || y + offset.Item2 >= height) continue;
                            if (Char.IsDigit(lines[y + offset.Item2][x + offset.Item1])) surroundCount++;
                        }
                        if (surroundCount == 2) 
                        { 
                            gearMap[y][x] = 1;
                            validGearPoints.Add(new Tuple<int, int>(x, y));
                        }
                    }
                }
            }

            int[][] validGroupAreas = GenerateEmptyMap(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (gearMap[y][x] == 0) continue;

                    foreach (var offset in _surroundOffsets)
                    {
                        if (x + offset.Item1 < 0 || x + offset.Item1 >= width || y + offset.Item2 < 0 || y + offset.Item2 >= height) continue;
                        validGroupAreas[y + offset.Item2][x + offset.Item1] = 1;
                    }
                }
            }


            if (debug)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (numMap[y][x] == 0) Console.Write('.');
                        if (numMap[y][x] == 1) Console.Write(lines[y][x]);
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (gearMap[y][x] == 0) Console.Write('.');
                        if (gearMap[y][x] == 1) Console.Write('*');
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (validGroupAreas[y][x] == 0) Console.Write('.');
                        if (validGroupAreas[y][x] == 1) Console.Write('*');
                    }
                    Console.WriteLine();
                }
            }

            string buffer = "";
            bool addBuffer = false;
            int result = 0;
            

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (numMap[y][x] == 1)
                    {
                        buffer += lines[y][x];
                        if (validGroupAreas[y][x] == 1) addBuffer = true;
                        continue;
                    }
                    else
                    {
                        if (addBuffer)
                        {
                            continue;
                        }
                    }
                }
            }

            return $"{result}";
        }
    }
}
