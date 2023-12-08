using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    public class Card
    {
        public static readonly char[] CardValues = new char[14] { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
        public char Label;
        public int Strength => Array.IndexOf(CardValues, Label);

        public Card(char label) 
        { 
            Label = label;
        }

        public override string ToString()
        {
            return $"{Label}";
        }
    }

    public enum HandType
    {
        HIGH_CARD = 0,
        ONE_PAIR = 1,
        TWO_PAIR = 2,
        THREE_OF_A_KIND = 3,
        FULL_HOUSE = 4,
        FOUR_OF_A_KIND = 5,
        FIVE_OF_A_KIND = 6
    }

    public class Hand : IComparable<Hand>
    {
        private Card[] cards = new Card[5];
        private const int HAND_SIZE = 5;
        private int _bid;
        private HandType _type;

        public HandType Type => _type;
        public int Bid => _bid;

        public Hand(string handStr, int bid) 
        {
            cards = handStr.Select(c => new Card(c)).ToArray();
            _type = GetHandType(cards);
            _bid = bid;

            
            if (handStr.Contains('J') && Type != HandType.FIVE_OF_A_KIND)
            {
                foreach (char c in GetLetterCounts(cards).Keys)
                {
                    if (c == 'J') continue;

                    string newHandStr = handStr.Replace('J', c);
                    Hand newHand = new Hand(newHandStr, 0);
                    if ((int)newHand.Type > (int)Type) _type = newHand.Type;
                }
            }
        }

        public static Dictionary<char, int> GetLetterCounts(Card[] cards)
        {
            var letterCounts = new Dictionary<char, int>();
            foreach (Card c in cards)
            {
                if (letterCounts.ContainsKey(c.Label)) letterCounts[c.Label]++;
                else letterCounts[c.Label] = 1;
            }
            return letterCounts;
        }

        public static HandType GetHandType(Card[] cards)
        {
            var letterCounts = new Dictionary<char, int>();
            foreach (Card c in cards)
            {
                if (letterCounts.ContainsKey(c.Label)) letterCounts[c.Label]++;
                else letterCounts[c.Label] = 1;
            }

            switch (letterCounts.Count)
            {
                case 1:
                    return HandType.FIVE_OF_A_KIND;
                case 2:
                    return letterCounts.Values.Any(i => i == 4) ? HandType.FOUR_OF_A_KIND : HandType.FULL_HOUSE;
                case 3:
                    return letterCounts.Values.Any(i => i == 3) ? HandType.THREE_OF_A_KIND : HandType.TWO_PAIR;
                case 4:
                    return HandType.ONE_PAIR;
                default:
                    return HandType.HIGH_CARD;
            }
        }

        public Card CardAt(int index)
        {
            return cards[index];
        }

        public int CompareTo(Hand? other)
        {
            if (other == null) return 1;

            if ((int)Type > (int)other.Type) return 1;
            else if ((int)Type < (int)other.Type) return -1;

            for (int i = 0; i < HAND_SIZE; i++)
            {
                if (CardAt(i).Strength > other.CardAt(i).Strength) return 1;
                else if (CardAt(i).Strength < other.CardAt(i).Strength) return -1;
            }

            return 0;
        }

        public override string ToString()
        {
            return $"{GetCardsString(cards)} {Bid} -> {Type}";
        }

        public static string GetCardsString(Card[] cards)
        {
            return string.Join("", cards.Select(c => c.ToString()));
        }
    }


    public class Day7Solution : ISolution
    {
        public override string SolvePart1(string inputPath, bool debug)
        {
            string[] input = GetProblemInput(inputPath);
            List<Hand> hands = input.Select(line => new Hand(line.Split(' ')[0], int.Parse(line.Split(' ')[1]))).ToList();
            hands.Sort();

            int total = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                total += (i + 1) * hands[i].Bid;
            }

            return total.ToString();
        }

        public override string SolvePart2(string inputPath, bool debug)
        {
            string[] input = GetProblemInput(inputPath);

            List<Hand> hands = input.Select(line => new Hand(line.Split(' ')[0], int.Parse(line.Split(' ')[1]))).ToList();
            hands.Sort();

            if (debug)
            {
                foreach (Hand hand in hands)
                {
                    Console.WriteLine(hand);
                }
            }

            int total = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                total += (i + 1) * hands[i].Bid;
            }

            return total.ToString();
        }
    }
}
