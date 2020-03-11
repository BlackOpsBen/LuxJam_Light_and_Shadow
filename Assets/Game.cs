using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int score = Random.Range(0, 2000);
            string username = "";
            string alphabet = "qwertyuiopasdfghjklzxcvbnm";

            for (int i = 0; i < Random.Range(5,10); i++)
            {
                username += alphabet[Random.Range(0, alphabet.Length)];
            }

            Leaderboard.AddNewScore(username, score);
        }
    }
}
