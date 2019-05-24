using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LeaderboardCalculator
{
    // return if playercount is greater than a leaderboardCount
    static bool leaderboardEligible(int playercount, int leadercount)
    {
        if (playercount > leadercount)
        {
            return true;
        }
        return false;
    }

    // add player count to leaderboardDict
    static void insertOntoLeaderBoard(List<Player> leaderboard, int playerCount, int index)
    {
        foreach (Player leaderboardObject in leaderboard)
        {
            if (playerCount > leaderboardObject.fishCount)
            {
                
            }
        }
    }

    static bool findLeadboardFile()
    {
        //@ find if file exists: return true
        return false;
    }

    static bool writeToLeaderboardFile()
    {
        //@ try and write to a leaderboard.json
        return false;
    }

}
