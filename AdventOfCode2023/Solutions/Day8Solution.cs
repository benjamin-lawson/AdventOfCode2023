using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Node
    {
        public string Name;
        public string LeftName;
        public string RightName;
        public Dictionary<string, Node> ParentDict;

        public Node LeftNode => ParentDict[LeftName];
        public Node RightNode => ParentDict[RightName];
    }

    public static class MathHelpers
    {
        public static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
        {
            while (b != T.Zero)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        public static T LeastCommonMultiple<T>(T a, T b) where T : INumber<T>
            => a / GreatestCommonDivisor(a, b) * b;

        public static T LeastCommonMultiple<T>(this IEnumerable<T> values) where T : INumber<T>
            => values.Aggregate(LeastCommonMultiple);
    }

    public class Day8Solution : ISolution
    {
        private Regex leftRightRegex = new Regex(@"\((\w{3}), (\w{3})\)");
        public override string SolvePart1(string inputPath, bool debug)
        {
            string[] lines = GetProblemInput(inputPath);
            string instructionSet = lines[0];
            var nodes = GetNodesFromInput(lines);

            Node node = nodes["AAA"];
            int stepCount = 0;
            while (node.Name != "ZZZ")
            {
                char instruction = instructionSet[stepCount % instructionSet.Length];
                node = instruction == 'R' ? node.RightNode : node.LeftNode;
                stepCount++;
            }

            return stepCount.ToString();
        }

        public override string SolvePart2(string inputPath, bool debug)
        {
            string[] lines = GetProblemInput(inputPath);
            string instructionSet = lines[0];
            var nodes = GetNodesFromInput(lines);

            if (debug)
            {
                foreach (var node in nodes.Values)
                {
                    Console.WriteLine($"{node.Name} = ({node.LeftName}, {node.RightName})");
                }
            }

            Node[] startingNodes = nodes.Where(kv => kv.Key.EndsWith("A")).Select(kv => kv.Value).ToArray();
            Console.WriteLine($"Nodes to Match: {startingNodes.Length}");
            var cycles = new Dictionary<Node, int>();
            foreach (Node node in startingNodes)
            {
                Node currNode = node;
                int stepCount = 0;
                while (!currNode.Name.EndsWith("Z"))
                {
                    currNode = instructionSet[stepCount % instructionSet.Length] == 'R' ? currNode.RightNode : currNode.LeftNode;
                    stepCount++;
                }
                cycles[node] = stepCount;
                Console.WriteLine($"{node.Name} - {stepCount}");
            }

            long test = MathHelpers.LeastCommonMultiple<long>(cycles.Values.Select(v => (long)v));

            return test.ToString();
        }

        public Dictionary<string, Node> GetNodesFromInput(string[] lines)
        {
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            for (int i = 2; i < lines.Length; i++)
            {
                string name = lines[i].Split(" = ")[0].Trim();
                var match = leftRightRegex.Match(lines[i].Split(" = ")[1].Trim());
                string leftName = match.Groups[1].Value;
                string rightName = match.Groups[2].Value;

                nodes.Add(name, new Node() { Name = name, LeftName = leftName, RightName = rightName, ParentDict = nodes });
            }
            return nodes;
        }
    }
}
