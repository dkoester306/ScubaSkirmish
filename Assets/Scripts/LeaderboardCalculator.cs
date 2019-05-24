using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LeaderboardCalculator
{
    // return if playercount is greater than a leaderboardCount
    public static bool leaderboardEligible(Player newPlayer, List<Player> leaderboard)
    {
        if ((leaderboard[3] == null) || (newPlayer.fishCount > leaderboard[3].fishCount))
        {
            return true;
        }
        return false;
    }

    // add player count to leaderboardDict
    public static List<Player> insertOntoLeaderBoard(Player newPlayer, List<Player> leaderboard)
    {
        if (leaderboard == null)
        {
            leaderboard.Add(newPlayer);
            return leaderboard;
        }
            
        else if (leaderboardEligible(newPlayer, leaderboard))
        {
            leaderboard[3] = newPlayer;
            leaderboard.Sort((player, player1) => player.fishCount.CompareTo(player1.fishCount));
            return leaderboard;
        }
        return leaderboard;
    }

}
