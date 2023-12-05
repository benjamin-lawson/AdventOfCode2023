namespace AdventOfCode2023
{
    public class SeedRange
    {
        public long SeedRangeStart;
        public long Length;
        public long SeedRangeEnd => SeedRangeStart + Length;

    }
    public class MapEntry
    {
        public long SourceRangeStart;
        public long DestinationRangeStart;
        public long Length;
        public long SourceRangeEnd => SourceRangeStart + Length;
        public long DestinationRangeEnd => DestinationRangeStart + Length;
    }

    public class Map
    {
        public string SourceCategory;
        public string DestinationCategory;
        public List<MapEntry> MapEntries = new List<MapEntry>();
        public Map DestinationMap;
        public Map SourceMap;

        public long GetDestinationValue(long sourceVal)
        {
            foreach (MapEntry entry in MapEntries)
            {
                if (sourceVal >= entry.SourceRangeStart && sourceVal <= entry.SourceRangeEnd)
                {
                    return entry.DestinationRangeStart + (sourceVal - entry.SourceRangeStart);
                }
            }
            return sourceVal;
        }

        public long GetSourceValue(long destVal) 
        {
            foreach (MapEntry entry in MapEntries)
            {
                if (destVal >= entry.DestinationRangeStart && destVal <= entry.DestinationRangeEnd)
                {
                    return entry.SourceRangeStart + (destVal - entry.DestinationRangeStart);
                }
            }
            return destVal;
        }

        public long TraverseMap(long sourceVal)
        {
            if (DestinationMap is null) return GetDestinationValue(sourceVal);

            return DestinationMap.TraverseMap(GetDestinationValue(sourceVal));
        }

        public long TraverseMapReverse(long destVal)
        {
            if (SourceMap is null) return GetSourceValue(destVal);

            return SourceMap.TraverseMapReverse(GetSourceValue(destVal));
        }
    }

    public class Day5Solution : ISolution
    {
        public override string SolvePart1(string inputPath, bool debug)
        {
            string[] lines = File.ReadAllLines(inputPath);
            List<long> seeds = lines[0].Replace("seeds: ", "").Trim().Split(' ').Select(long.Parse).ToList();
            Dictionary<string, Map> maps = ParseMaps(lines);

            long closestLocation = long.MaxValue;
            Map seedMap = maps["seed"];

            foreach (long seedVal in seeds)
            {
                long locVal = seedMap.TraverseMap(seedVal);
                if (locVal < closestLocation) closestLocation = locVal;
            }

            return closestLocation.ToString();
        }


        public override string SolvePart2(string inputPath, bool debug)
        {
            string[] lines = File.ReadAllLines(inputPath);
            Dictionary<string, Map> maps = ParseMaps(lines);
            List<SeedRange> ranges = ParseSeedRange(lines[0]);

            Map locMap = maps.Values.Where(m => m.DestinationCategory == "location").First();
            for (long locVal = 0; locVal < long.MaxValue; locVal++)
            {
                long seedVal = locMap.TraverseMapReverse(locVal);
                foreach (SeedRange range in ranges)
                {
                    if (seedVal >= range.SeedRangeStart && seedVal <= range.SeedRangeEnd) return locVal.ToString();
                }
            }

            return "";
        }

        public static List<SeedRange> ParseSeedRange(string seedLine)
        {
            List<SeedRange> ranges = new List<SeedRange>();

            string seedStr = seedLine.Replace("seeds: ", "").Trim();
            string[] seedSplit = seedStr.Split(' ');
            
            for (int i = 0; i < seedSplit.Length; i += 2)
            {
                long startSeedValue = long.Parse(seedSplit[i]);
                long seedRange = long.Parse(seedSplit[i + 1]);
                ranges.Add(new SeedRange() { SeedRangeStart = startSeedValue, Length = seedRange });
            }

            return ranges;
        }

        public static Dictionary<string, Map> ParseMaps(string[] lines)
        {
            Dictionary<string, Map> maps = new Dictionary<string, Map>();
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                if (string.IsNullOrEmpty(line)) continue;

                if (line.Trim().EndsWith("map:"))
                {
                    string mappingStr = line.Trim().Replace(" map:", "");
                    var map = new Map()
                    {
                        SourceCategory = mappingStr.Split('-')[0],
                        DestinationCategory = mappingStr.Split("-")[2]
                    };

                    i++;

                    if (i >= lines.Length - 1) break;


                    while (!string.IsNullOrEmpty(line) && i < lines.Length)
                    {
                        line = lines[i];

                        if (string.IsNullOrEmpty(line)) continue;

                        map.MapEntries.Add(new MapEntry()
                        {
                            SourceRangeStart = long.Parse(line.Trim().Split(' ')[1]),
                            DestinationRangeStart = long.Parse(line.Trim().Split(' ')[0]),
                            Length = long.Parse(line.Trim().Split(' ')[2])
                        });
                        ;

                        i++;
                    }

                    maps.Add(map.SourceCategory, map);

                }
            }

            foreach (Map map in maps.Values)
            {
                if (map.DestinationCategory != "location") map.DestinationMap = maps[map.DestinationCategory];
                if (map.SourceCategory != "seed") map.SourceMap = maps.Values.Where(m => m.DestinationCategory == map.SourceCategory).First();
            }

            return maps;
        }
    }
}
