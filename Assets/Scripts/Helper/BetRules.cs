using System.Linq;
using UnityEngine;

namespace Helper
{
    public static class BetRules
    {
        // Payout ratios (e.g., Straight bet pays 35 to 1, so the multiplier is 35)
        public static int GetPayout(BetType betType)
        {
            switch (betType)
            {
                case BetType.Straight:
                    return 35;
                case BetType.Split:
                    return 17;
                case BetType.Street:
                    return 11;
                case BetType.Corner:
                    return 8;
                case BetType.SixLine:
                    return 5;
                case BetType.Red:
                case BetType.Black:
                case BetType.Even:
                case BetType.Odd:
                case BetType.Low:
                case BetType.High:
                    return 1;
                case BetType.Dozen1:
                case BetType.Dozen2:
                case BetType.Dozen3:
                case BetType.Column1:
                case BetType.Column2:
                case BetType.Column3:
                    return 2;
                default:
                    return 0;
            }
        }

        // Number sets for outside bets
        private static readonly int[] RedNumbers =
            { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };

        private static readonly int[] BlackNumbers =
            { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 };

        public static Color GetNumberColor(int number)
        {
            if (number == 0) return Color.green;
            if (RedNumbers.Contains(number)) return Color.red;
            if (BlackNumbers.Contains(number)) return Color.yellow;
            return Color.white;
        }

        public static int[] GetBetNumbers(BetType betType)
        {
            switch (betType)
            {
                case BetType.Red:
                    return RedNumbers;
                case BetType.Black:
                    return BlackNumbers;
                case BetType.Even:
                    return Enumerable.Range(1, 36).Where(n => n % 2 == 0).ToArray();
                case BetType.Odd:
                    return Enumerable.Range(1, 36).Where(n => n % 2 != 0).ToArray();
                case BetType.Low:
                    return Enumerable.Range(1, 18).ToArray();
                case BetType.High:
                    return Enumerable.Range(19, 18).ToArray();
                case BetType.Dozen1:
                    return Enumerable.Range(1, 12).ToArray();
                case BetType.Dozen2:
                    return Enumerable.Range(13, 12).ToArray();
                case BetType.Dozen3:
                    return Enumerable.Range(25, 12).ToArray();
                case BetType.Column1:
                    return Enumerable.Range(1, 36).Where(n => n % 3 == 1).ToArray();
                case BetType.Column2:
                    return Enumerable.Range(1, 36).Where(n => n % 3 == 2).ToArray();
                case BetType.Column3:
                    return Enumerable.Range(1, 36).Where(n => n % 3 == 0).ToArray();
                default:
                    return new int[] { }; // Inside bets are defined by player selection, not pre-defined sets
            }
        }
    }
}