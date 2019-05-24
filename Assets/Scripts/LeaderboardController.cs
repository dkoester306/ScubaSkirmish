using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
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
    public GameObject inputNewHighScore;

    // update UI elements

    // Start is called before the first frame update
    void Start()
    {
        inputNewHighScore.SetActive(false);

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
}

#region DataStructures

    [System.Serializable]
    public class Leaderboards
    {
        public TimeSpan LeaderboparTimeStamp;
        //@ List or Dictionary????
        public List<Player> leaderboardDictionary;
    }

    [System.Serializable]
    public class Player
    {
        public string playerName;
        public int fishCount;
    }

#endregion






