using System;
using System.Collections.Generic;
using Models;

[Serializable]
public class GameData
{
    public int PlayerBalance;
    public List<Bet> CurrentBets;
    public StatisticData Statistics;
}