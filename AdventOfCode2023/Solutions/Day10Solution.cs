using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public enum PieceType
    {
        VERTICAL_PIPE,
        HORIZONTAL_PIPE,
        NE_BEND,
        NW_BEND,
        SE_BEND,
        SW_BEND,
        GROUND,
        STARTING,
        NONE
    }

    public class MapTile
    {
        private static readonly Dictionary<char, PieceType> PieceMapping = new Dictionary<char, PieceType>()
        {
            { '|', PieceType.VERTICAL_PIPE },
            { '-', PieceType.HORIZONTAL_PIPE },
            { 'L', PieceType.NE_BEND },
            { 'J', PieceType.NW_BEND },
            { '7', PieceType.SW_BEND },
            { 'F', PieceType.SE_BEND },
            { '.', PieceType.GROUND },
            { 'S', PieceType.STARTING },
        };

        private int _x;
        private int _y;
        private char _pieceChar;
        private PipeMap _parentMap;
        private Tuple<int, int> _id;

        public int X => _x;
        public int Y => _y;
        public Tuple<int, int> Id => _id;
        public char PieceChar => _pieceChar;
        public PieceType PieceType => PieceMapping[_pieceChar];
        public bool TopConnected => _pieceChar == '|' || _pieceChar == 'L' || _pieceChar == 'J';
        public bool BottomConnected => _pieceChar == '|' || _pieceChar == '7' || _pieceChar == 'F';
        public bool LeftConnected => _pieceChar == '-' || _pieceChar == 'J' || _pieceChar == '7';
        public bool RightConnected => _pieceChar == '-' || _pieceChar == 'L' || _pieceChar == 'F';

        public MapTile? RightTile => _parentMap.GetTile(_x + 1, _y);
        public MapTile? LeftTile => _parentMap.GetTile(_x - 1, _y);
        public MapTile? BottomTile => _parentMap.GetTile(_x, _y + 1);
        public MapTile? TopTile => _parentMap.GetTile(_x, _y - 1);

        public MapTile(int x, int y, char pieceChar, PipeMap parentMap)
        {
            _x = x;
            _y = y;
            _id = new Tuple<int, int>(x, y);
            _pieceChar = pieceChar;
            _parentMap = parentMap;
        }

        public void DeterminePieceTypeFromNeighbors()
        {
            if (TopTile is not null && TopTile.BottomConnected) 
            {
                if (BottomTile is not null && BottomTile.TopConnected) _pieceChar = '|';
                if (LeftTile is not null && LeftTile.RightConnected) _pieceChar = 'J';
                if (RightTile is not null && RightTile.LeftConnected) _pieceChar = 'L';
            }

            if (BottomTile is not null && BottomTile.TopConnected)
            {
                if (LeftTile is not null && LeftTile.RightConnected) _pieceChar = '7';
                if (RightTile is not null && RightTile.LeftConnected) _pieceChar = 'F';
            }

            if (RightTile is not null && RightTile.LeftConnected && LeftTile is not null && LeftTile.RightConnected) _pieceChar = '-';
        }

        public void SetPieceChar(char newChar)
        {
            _pieceChar = newChar;
        }
    }

    public class PipeMap
    {
        private MapTile[][] _map;
        private int _startingX;
        private int _startingY;
        private static readonly List<Tuple<int, int>> NeighborOffsets = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(-1, 0),
            new Tuple<int, int>(1, 0),
            new Tuple<int, int>(0, -1),
            new Tuple<int, int>(0, 1)
        };

        public int Width => _map[0].Length;
        public int Height => _map.Length;

        public int StartingX => _startingX;
        public int StartingY => _startingY;

        public PipeMap(string[] input)
        {
            _map = new MapTile[input.Length][];
            for (int y = 0; y < input.Length; y++)
            {
                _map[y] = new MapTile[input[y].Length];
                for (int x = 0; x < input[y].Length; x++)
                {
                    _map[y][x] = new MapTile(x, y, input[y][x], this);
                    if (input[y][x] == 'S')
                    {
                        _startingX = x;
                        _startingY = y;
                    }
                }
            }
        }

        public void Init()
        {
            _map[StartingY][StartingX].DeterminePieceTypeFromNeighbors();
        }

        public MapTile? GetTile(int x, int y) => (x < 0 || x >= Width || y < 0 || y >= Height) ? null : _map[y][x];
        public MapTile? GetTileById(Tuple<int, int> id)
        {
            return GetTile(id.Item1, id.Item2);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++) 
                {
                    sb.Append($"{_map[y][x].PieceChar}");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public bool IsTileInsideLoop(int x, int y)
        {
            int wallCount = 0;
            for (int pX = 0; pX < x; pX++)
            {
                if (_map[y][pX].PieceChar == '|' || _map[y][pX].PieceChar == 'J' || _map[y][pX].PieceChar == 'L') wallCount++;
            }
            return wallCount % 2 == 1;
        }
    }

    public class Day10Solution : ISolution
    {
        private Dictionary<Tuple<int, int>, int> _mapDistances;

        public override string SolvePart1(string inputPath, bool debug)
        {
            _mapDistances = new Dictionary<Tuple<int, int>, int>();
            string[] lines = GetProblemInput(inputPath);
            PipeMap map = new PipeMap(lines);
            map.Init();
            TraverseMap(map);

            if (debug)
            {
                Console.WriteLine($"Width: {map.Width} | Height: {map.Height}");
                Console.WriteLine($"Starting: ({map.StartingX}, {map.StartingY})");
                Console.WriteLine(map);
                PrintDistances(map);
            }

            return _mapDistances.Max(kv => kv.Value).ToString();
        }

        public void TraverseMap(PipeMap map)
        {
            Queue<MapTile> tileQueue = new Queue<MapTile>();
            Queue<int> distQueue = new Queue<int>();

            tileQueue.Enqueue(map.GetTile(map.StartingX, map.StartingY));
            distQueue.Enqueue(0);

            while (tileQueue.Count > 0)
            {
                MapTile tile = tileQueue.Dequeue();
                int dist = distQueue.Dequeue();

                if (!_mapDistances.ContainsKey(tile.Id) || _mapDistances[tile.Id] > dist)
                {
                    _mapDistances[tile.Id] = dist;
                    if (tile.TopTile is not null && tile.TopConnected)
                    {
                        tileQueue.Enqueue(tile.TopTile);
                        distQueue.Enqueue(dist + 1);
                    }
                    if (tile.BottomTile is not null && tile.BottomConnected)
                    {
                        tileQueue.Enqueue(tile.BottomTile);
                        distQueue.Enqueue(dist + 1);
                    }
                    if (tile.LeftTile is not null && tile.LeftConnected)
                    {
                        tileQueue.Enqueue(tile.LeftTile);
                        distQueue.Enqueue(dist + 1);
                    }
                    if (tile.RightTile is not null && tile.RightConnected)
                    {
                        tileQueue.Enqueue(tile.RightTile);
                        distQueue.Enqueue(dist + 1);
                    }
                }
            }
        }

        public void PrintDistances(PipeMap map)
        {
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    MapTile tile = map.GetTile(x, y);
                    if (_mapDistances.ContainsKey(tile.Id))
                    {
                        sb.Append($"{_mapDistances[tile.Id]}");
                    }
                    else
                    {
                        sb.Append(".");
                    }
                    
                }
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }

        public override string SolvePart2(string inputPath, bool debug)
        {
            _mapDistances = new Dictionary<Tuple<int, int>, int>();
            string[] lines = GetProblemInput(inputPath);
            PipeMap map = new PipeMap(lines);
            map.Init();
            TraverseMap(map);

            int total = 0;
            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    if (!_mapDistances.ContainsKey(new Tuple<int, int>(x, y))) map.GetTile(x, y).SetPieceChar('.');
                }
            }
            string[] cleanedData = new string[lines.Length];
            for (int y = 0; y < map.Height; y++)
            {
                StringBuilder sb = new();
                for (int x = 0; x < map.Width; x++)
                {
                    sb.Append(map.GetTile(x, y).PieceChar);
                }

                cleanedData[y] = Regex.Replace(Regex.Replace(sb.ToString(), "F-*7|L-*J", string.Empty), "F-*J|L-*7", "|");
            }

            foreach (var line in cleanedData)
            {
                int parity = 0;
                foreach (char piecChar in line)
                {
                    if (piecChar == '|') parity++;
                    if (piecChar == '.' && parity % 2 == 1) total++;
                }
            }

            return total.ToString();
        }
    }
}
