using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Day9Solution : ISolution
    {
        public override string SolvePart1(string inputPath, bool debug)
        {
            string[] lines = GetProblemInput(inputPath);
            int total = 0;
            foreach (string line in lines)
            {
                List<int> nums = line.Split(' ').Select(s => int.Parse(s)).ToList();
                TraversDiffRecurse(nums);
                total += nums.Last();
            }


            return total.ToString();
        }

        public void TraversDiffRecurse(List<int> nums, int level = 0, bool forwards = true)
        {
            if (nums.All(x => x == 0))
            {
                nums.Add(0);
                return;
            }

            List<int> diffNums = new List<int>();
            for (int i = 0; i < nums.Count - 1; i++) 
            {
                diffNums.Add(nums[i + 1] - nums[i]);
            }

            TraversDiffRecurse(diffNums, level + 1, forwards);
            if (forwards)
            {
                nums.Add(nums.Last() + diffNums.Last());
            }
            else
            {
                nums.Insert(0, nums[0] - diffNums[0]);
            }
            
        }

        public override string SolvePart2(string inputPath, bool debug)
        {
            string[] lines = GetProblemInput(inputPath);
            int total = 0;
            foreach (string line in lines)
            {
                List<int> nums = line.Split(' ').Select(s => int.Parse(s)).ToList();
                TraversDiffRecurse(nums, forwards: false);
                total += nums[0];
            }


            return total.ToString();
        }
    }
}
