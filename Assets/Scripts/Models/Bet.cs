
using Helper;

namespace Models
{
    public class Bet
    {
        public int Amount { get; private set; }
        public BetType BetType { get; private set; }
        public int[] Numbers { get; private set; }

        public Bet(int amount, BetType betType, int[] numbers)
        {
            Amount = amount;
            BetType = betType;
            Numbers = numbers;
        }
    }
}
