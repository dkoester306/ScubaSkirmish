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
    static void insertOntoLeaderBoard(Dictionary<int, string> leaderboard, int playerCount, int index)
    {
        foreach (KeyValuePair<int, string> keyValuePair in leaderboard)
        {

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
