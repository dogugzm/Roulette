public enum BetType
{
    // Inside Bets
    Straight, // Bet on a single number
    Split, // Bet on two adjacent numbers
    Street, // Bet on a row of three numbers
    Corner, // Bet on four numbers that form a square
    SixLine, // Bet on two adjacent rows of three numbers each

    // Outside Bets
    Red, // Bet on all red numbers
    Black, // Bet on all black numbers
    Even, // Bet on all even numbers
    Odd, // Bet on all odd numbers
    Low, // Bet on numbers 1-18
    High, // Bet on numbers 19-36
    Dozen1, // Bet on numbers 1-12
    Dozen2, // Bet on numbers 13-24
    Dozen3, // Bet on numbers 25-36
    Column1, // Bet on the first column of 12 numbers
    Column2, // Bet on the second column of 12 numbers
    Column3 // Bet on the third column of 12 numbers
}