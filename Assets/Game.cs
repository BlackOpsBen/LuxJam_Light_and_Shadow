using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private string username;
    private int score;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            username = GetUsername();
            score = GetScore();
            Leaderboard.AddNewScore(username, score);
        }
    }

    public string GetUsername()
    {
        // Replace this temp code for randomly selecting name

        int whichType = Random.Range(0, 2);

        // Type 0
        string[] names = { "Ben", "Daniel", "Jordon" };

        // Type 1
        string alphabet = "qwertyuiopasdfghjklzxcvbnm";
        string randomName = "";

        if (whichType == 0)
        {
            return names[Random.Range(0, names.Length)];
        }
        else
        {
            for (int i = 0; i < Random.Range(5, 10); i++)
            {
                randomName += alphabet[Random.Range(0, alphabet.Length)];
            }
            return randomName;
        }
    }

    public int GetScore()
    {
        // Replace this temp code for randomly generating score
        int rand = Random.Range(0, 2500);
        print("Random score: " + rand);
        return rand;
    }
}
