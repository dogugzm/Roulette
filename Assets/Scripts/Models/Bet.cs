using System;

namespace Models
{
    [Serializable]
    public class Bet
    {
        public int Amount;
        public BetType BetType;
        public int[] Numbers;

        public Bet(int amount, BetType betType, int[] numbers)
        {
            Amount = amount;
            BetType = betType;
            Numbers = numbers;
        }
    }
}
