using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//using Newtonsoft.Json;

public class LeaderboardController : MonoBehaviour
{
    public List<Text> leaderboardTextObjects = new List<Text>();

    private bool newhighscore;
    // when of game over

    // read in(if so)the pre-existing local leaderboard file

    // if not create new list and give permission to add new highscore
    // then writes back to the local leaderboard file

    // if so check the list of the leaderlist, if yes, check the list and 
    // add the score at that index, pushing all elements back one
    // removing the last element in the list
    // then writes back to the local leaderboard file

    // activate highscore panel

    // update UI elements

    public GameObject NewHighScorePanel;
    private int playerfishCount;
    public InputField playerInputField;

    private Player playerRef;
    private readonly List<Player> players = new List<Player>();


    // Start is called before the first frame update
    private void Start()
    {
        NewHighScorePanel.SetActive(true);
        var leaderboards = new List<Player>();
        leaderboards.Add(new Player {fishCount = 0, playerName = "New Player"});
        leaderboards.Add(new Player {fishCount = 0, playerName = "New Player"});
        leaderboards.Add(new Player {fishCount = 0, playerName = "New Player"});
        leaderboards.Add(new Player {fishCount = 0, playerName = "New Player"});
        WriteToLeaderboardFile(leaderboards);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    //: Create Player object with Name and FishCount
    //@ InputField PlayerName: NewHighScore.InputField.Text
    //@ Int FishCount: GM.FishCount
    public Player CreateNewPlayer(int fishCount)
    {
        var newPlayer = new Player();
        newPlayer.playerName = "Player Name";
        newPlayer.fishCount = fishCount;
        return newPlayer;
    }

    public void CalculateNewLeaderboard()
    {
        ReadLeaderboardFile(players);

        // Create PlayerRef
        playerRef = new Player {fishCount = playerfishCount, playerName = "New Player"};
        // Calculate if PlayerRef is Leaderboard Eligible
        if (LeaderboardCalculator.leaderboardEligible(playerRef, players))
        {
            newhighscore = true;
            NewHighScorePanel.SetActive(true);
        }
    }

    public void UserSubmitedHighScore()
    {
        if (newhighscore)
        {
            // set playerName inputField
            playerRef.playerName = playerInputField.text;

            // edit leaderboard with new player
            LeaderboardCalculator.insertOntoLeaderBoard(playerRef, players);

            // write down to leaderboard.json
            WriteToLeaderboardFile(players);

            // update UI assets
            UpdateLeaderboardUI();
        }
    }

    private void UpdateLeaderboardUI()
    {
        for (var i = 0; i < players.Count; i++)
            leaderboardTextObjects[i].text = players[i].playerName + " Fish Caught: " + players[i].fishCount;
    }

    private static bool FindLeadboardFile()
    {
        //@ find if file exists: return true
        var fileExists = File.Exists("leaderboards.json");
        if (fileExists) return true;
        return false;
    }

    private static void ReadLeaderboardFile(List<Player> _players)
    {
        // read in last leaderboard.json file
        if (FindLeadboardFile())
        {
            var leaderboards = JsonUtility.FromJson<Leaderboards>("leaderboards.json");
            _players = leaderboards.leaderboardplayerList;
        }
    }

    private static bool WriteToLeaderboardFile(List<Player> newleaderboards)
    {
        //@ try and write to a leaderboard.json
        using (var localLeaderboard = File.CreateText("leaderboards.json"))
        {
            var newLeaderboards = new Leaderboards();
            newLeaderboards.leaderboardplayerList = newleaderboards;
            var leaderboards = JsonUtility.ToJson(newLeaderboards.leaderboardplayerList);
            localLeaderboard.Write(leaderboards);
        }

        return false;
    }

    public void GetFishCount(int fishCount)
    {
        playerfishCount = fishCount;
    }
}

#region DataStructures

[Serializable]
public class Leaderboards
{
    public List<Player> leaderboardplayerList;
    public TimeSpan LeaderboparTimeStamp;
}

[Serializable]
public class Player
{
    public int fishCount;
    public string playerName;
}

#endregion