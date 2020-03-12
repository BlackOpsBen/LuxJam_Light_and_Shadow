using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public string username;
    public int score;

    private bool usernameExists;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            username = GetUsername();
            score = GetScore();

            Leaderboard.GoGetScores();

            // Need to ensure this is set AFTER the get
            usernameExists = Leaderboard.CheckListForName(username);

            if (!usernameExists)
            {
                Leaderboard.AddNewScore(username, score);
                if (Leaderboard.GetSingleHighscore().rank == 1)
                {

                }
            }

            Leaderboard.AddNewScore(username, score);
        }
    }

    public string GetUsername()
    {
        // Replace this temp code for randomly selecting name
        string[] names = { "Ben", "Daniel", "Jordon" };
        // Random fake name:
        //string alphabet = "qwertyuiopasdfghjklzxcvbnm";
        //for (int i = 0; i < Random.Range(5,10); i++)
        //{
        //    username += alphabet[Random.Range(0, alphabet.Length)];
        //}

        return names[Random.Range(0, names.Length)];
    }

    public int GetScore()
    {
        // Replace this temp code for randomly generating score
        return Random.Range(0, 2500);
    }
}
