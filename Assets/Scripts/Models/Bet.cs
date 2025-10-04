using System;
using Helper;

namespace Models
{
    [Serializable]
    public class Bet
    {
        public int Amount;
        public BetType BetType;
        public int[] Numbers;
        public string ChipId;

        public Bet(int amount, BetType betType, int[] numbers, string chipId)
        {
            Amount = amount;
            BetType = betType;
            Numbers = numbers;
            ChipId = chipId;
        }
    }
}