﻿using AdventOfCode2023;

ISolution solution = new Day6Solution();

Console.WriteLine("=== PART 1 ===");
string testAnswer = solution.Solve("TestInput/day6-part1.txt", ProblemPart.PART1, true);
Console.WriteLine($"Test Answer: {testAnswer}");

string answer = solution.Solve("Input/day6-part1.txt", ProblemPart.PART1);
Console.WriteLine($"Answer: {answer}");

Console.WriteLine("=== PART 2 ===");
testAnswer = solution.Solve("TestInput/day6-part1.txt", ProblemPart.PART2, true);
Console.WriteLine($"Test Answer: {testAnswer}");

answer = solution.Solve("Input/day6-part1.txt", ProblemPart.PART2);
Console.WriteLine($"Answer: {answer}");

