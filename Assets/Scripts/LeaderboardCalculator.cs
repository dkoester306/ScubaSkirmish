using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LeaderboardCalculator
{
    // return if playercount is greater than a leaderboardCount
    public static bool leaderboardEligible(Player player, List<Player> leaderboard)
    {
        if (leaderboard.Count == 0 || leaderboard == null)
        {
            return true;
        }
        else
        {
            if ((player.fishCount > leaderboard[leaderboard.Count - 1].fishCount))
            {
                return true;
            }
            return false;
        }
    }

    public static List<Player> sortLeadboard(List<Player> leaderboard)
    {
        leaderboard.Sort((player, player1) => player1.fishCount.CompareTo(player.fishCount));
        return leaderboard;
    }

    // add player count to leaderboardDict
    public static List<Player> insertIntoLeaderBoard(Player newPlayer, List<Player> leaderboard)
    {
        if (leaderboard == null || leaderboard.Count == 0)
        {
            leaderboard.Add(newPlayer);
        }
        else if (leaderboardEligible(newPlayer, leaderboard))
        {
            leaderboard[leaderboard.Count - 1] = newPlayer;
            leaderboard.Sort((player, player1) => player1.fishCount.CompareTo(player.fishCount));
        }
        return leaderboard;
    }

}
