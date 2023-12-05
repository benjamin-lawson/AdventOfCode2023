
namespace AdventOfCode2023
{
    public class ScratchCard
    {
        public int CardNumber;
        public int CardIndex => CardNumber - 1;
        public List<int> WinningNumbers;
        public List<int> GameNumbers;

        public ScratchCard(string gameStr)
        {
            CardNumber = int.Parse(gameStr.Split(':')[0].Replace("Card ", "").Trim());
            WinningNumbers = gameStr.Trim().Split(':')[1].Trim().Split('|')[0].Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList();
            GameNumbers = gameStr.Trim().Split(':')[1].Trim().Split('|')[1].Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(int.Parse).ToList();
        }

        public int GetMatchingNumbersCount()
        {
            return GameNumbers.Where(n => WinningNumbers.Contains(n)).Count();
        }

        public int GetPoints()
        {
            int totalNums = GetMatchingNumbersCount();
            return totalNums > 0 ? (int)Math.Pow(2, totalNums - 1) : 0;
        }
    }

    public class Day4Solution : ISolution
    {
        public override string SolvePart1(string inputPath, bool debug)
        {
            string[] lines = File.ReadAllLines(inputPath);
            int sum = 0;

            foreach (string line in lines)
            {
                var card = new ScratchCard(line);
                sum += card.GetPoints();
            }

            return sum.ToString();
        }

        private void PrintCardProcessingList(List<ScratchCard> cards)
        {
            foreach (ScratchCard card in cards)
            {
                Console.Write($"{card.CardNumber}, ");
            }
            Console.WriteLine();
        }

        public override string SolvePart2(string inputPath, bool debug)
        {
            List<ScratchCard> cards = File.ReadAllLines(inputPath).Select(l => new ScratchCard(l)).ToList();
            var cardProcessingList = new List<ScratchCard>(cards.ToList());
            int currentIndex = 0;

            while (currentIndex < cardProcessingList.Count)
            {
                var card = cardProcessingList[currentIndex];

                int matchingCount = card.GetMatchingNumbersCount();
                for (int i = card.CardNumber; i < card.CardNumber + matchingCount; i++)
                {
                    cardProcessingList.Add(cards[i]);
                }

                currentIndex++;
            }


            return $"{cardProcessingList.Count}";
        }
    }
}
