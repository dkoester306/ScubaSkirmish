using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

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
    private List<Player> players = new List<Player>();
    private List<Player> leaderboards = new List<Player>();


    // Start is called before the first frame update
    private void Start()
    {
        NewHighScorePanel.SetActive(false);
        CalculateNewLeaderboard();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void FillLeaderboards(List<Player> _players)
    {
        _players.Add(new Player { fishCount = 10, playerName = "Davey Jones" });
        _players.Add(new Player { fishCount = 20, playerName = "Scuba Steve" });
        _players.Add(new Player { fishCount = 35, playerName = "Flying Duchman" });
        _players.Add(new Player { fishCount = 50, playerName = "Ghost of Captain Cutler" });
        LeaderboardCalculator.sortLeadboard(_players);
    }

    //: Create Player object with Name and FishCount
    //@ InputField PlayerName: NewHighScore.InputField.Text
    //@ Int FishCount: GM.FishCount
    public Player CreateNewPlayer(int fishCount)
    {
        Player newPlayer = new Player();
        newPlayer.playerName = "Player Name";
        newPlayer.fishCount = fishCount;
        return newPlayer;
    }

    public void CalculateNewLeaderboard()
    {
        players = ReadInLeaderboardFile(players);

        UpdateLeaderboardUI();

        // Create PlayerRef
        playerRef = new Player {fishCount = playerfishCount, playerName = "New Player"};
        playerRef = CreateNewPlayer(playerfishCount);

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
            LeaderboardCalculator.insertIntoLeaderBoard(playerRef, players);

            // write down to leaderboard.json
            WriteToLeaderboardFile(players);

            // update UI assets
            UpdateLeaderboardUI();

            // deactivate highscore panel
            NewHighScorePanel.SetActive(false);
        }
    }

    private void UpdateLeaderboardUI()
    {
        for (int i = leaderboardTextObjects.Count - 1; i >= 0; i--)
        {
            leaderboardTextObjects[i].text 
                = (i+1).ToString() + ". " + players[i].playerName + " Fish Caught: " + players[i].fishCount;
        }
    }

    // find if file exists: return true else false
    private bool FindLeadboardFile()
    {
        bool fileExists = File.Exists("leaderboards.json");
        if (fileExists) return true;
        return false;
    }

    // read in last leaderboard.json file
    private List<Player> ReadInLeaderboardFile(List<Player> _players)
    {
        if (FindLeadboardFile())
        {
            string playerListString = File.ReadAllText("leaderboards.json");
            Leaderboards readInleaderboard = JsonConvert.DeserializeObject<Leaderboards>(playerListString);
            _players = readInleaderboard.LeaderboardplayerList;
        }
        else
        {
            _players = new List<Player>();
            FillLeaderboards(_players);
        }
        return _players;
    }

    // try and write to a leaderboard.json
    private static void WriteToLeaderboardFile(List<Player> newleaderboards)
    {
        using (var localLeaderboard = File.CreateText("leaderboards.json"))
        {
            Leaderboards newLeaderboards = new Leaderboards();
            newLeaderboards.LeaderboardplayerList = newleaderboards;
            localLeaderboard.Write(JsonConvert.SerializeObject(newLeaderboards));
        }
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
    public List<Player> LeaderboardplayerList;
    public TimeSpan LeaderboparTimeStamp;
}

[Serializable]
public class Player
{
    public int fishCount;
    public string playerName;
}

#endregion