namespace Helper
{
    public enum BetType
    {
        // Inside Bets
        Straight,
        Split,
        Street,
        Corner,
        SixLine,

        // Outside Bets
        Red,
        Black,
        Even,
        Odd,
        Low, // Bet on numbers 1-18
        High, // Bet on numbers 19-36
        Dozen1, // Bet on numbers 1-12
        Dozen2, // Bet on numbers 13-24
        Dozen3, // Bet on numbers 25-36
        Column1,
        Column2,
        Column3
    }
}