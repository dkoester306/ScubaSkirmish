using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

//using Newtonsoft.Json;      //! actual issue

public class LeaderboardController : MonoBehaviour
{
    // when of game over

    // read in(if so)the pre-existing local leaderboard file

    // if not create new list and give permission to add new highscore
    // then writes back to the local leaderboard file

    // if so check the list of the leaderlist, if yes, check the list and 
    // add the score at that index, pushing all elements back one
    // removing the last element in the list
    // then writes back to the local leaderboard file

    // activate highscore panel
    public GameObject NewHighScorePanel;
    public InputField playerInputField;
    private int leaderboardfishCount = 0;
    private bool newhighscore = false;
    List<Player> players = new List<Player>();

    // update UI elements

    // Start is called before the first frame update
    void Start()
    {
        NewHighScorePanel.SetActive(false);

        try
        {
            using (var creds = File.CreateText("leaderboards.json"))
            {
                Dictionary<int, string> leaderboards = new Dictionary<int, string>();
                leaderboards.Add(100, "John");
                leaderboards.Add(89, "Adam");
                leaderboards.Add(56, "Louis");
                leaderboards.Add(34, "Beck");
                //string message = JsonConvert.SerializeObject(leaderboards.ToString());
                //creds.Write(message);
            }
        }
        catch
        {
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //: Create Player object with Name and FishCount
    //@ InputField PlayerName: NewHighScore.InputField.Text
    //@ Int FishCount: GM.FishCount
    Player CreateNewPlayer(int fishCount)
    {
        return new Player() {fishCount = fishCount, playerName = "New Player"};
    }

    private Player playerRef;
    void CalculateNewLeaderboard()
    {
        // read in last leaderboard.json file
        if (FindLeadboardFile())
        {
            var leaderboards = JsonUtility.FromJson<Leaderboards>("leaderboards.json");
            players = leaderboards.leaderboardplayerList;
        }
        // gets fishcount
        // calculates if potential leaderboard
        playerRef = CreateNewPlayer(leaderboardfishCount);
        if (LeaderboardCalculator.leaderboardEligible(playerRef, players))
        {
            newhighscore = true;
        }
        
    }

    public void UserSubmitedHighScore()
    {
        if (newhighscore)
        {
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
        throw new NotImplementedException();
    }

    static bool FindLeadboardFile()
    {
        //@ find if file exists: return true
        bool creds = File.Exists("leaderboards.json");
        if (creds)
        {
            return true;
        }
        return false;
    }

    static bool WriteToLeaderboardFile(object newleaderboards)
    {
        //@ try and write to a leaderboard.json
        using (StreamWriter localLeaderboard = File.CreateText("leaderboards.json"))
        {
            string message = JsonConvert.SerializeObject(newleaderboards);
            localLeaderboard.Write(message);
        }
        return false;
    }

    public void GetFishCount(int fishCount)
    {
        leaderboardfishCount = fishCount;
    }
}

#region DataStructures

[System.Serializable]
    public class Leaderboards
    {
        public TimeSpan LeaderboparTimeStamp;
        public List<Player> leaderboardplayerList;
    }

    [System.Serializable]
    public class Player
    {
        public string playerName;
        public int fishCount;
    }

#endregion






