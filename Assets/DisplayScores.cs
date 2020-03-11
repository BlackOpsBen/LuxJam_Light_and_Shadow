using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayScores : MonoBehaviour
{
    public TextMeshProUGUI[] usernameTexts;
    public TextMeshProUGUI[] scoreTexts;
    Leaderboard highscoreManager;

    void Start()
    {
        for (int i = 0; i < usernameTexts.Length; i++)
        {
            usernameTexts[i].text = i + 1 + ". Fetching...";
            scoreTexts[i].text = "...";
        }

        highscoreManager = GetComponent<Leaderboard>();

        StartCoroutine(RefreshScores());
    }

    public void OnScoresDownloaded(Highscore[] highscoreList)
    {
        for (int i = 0; i < usernameTexts.Length; i++)
        {
            usernameTexts[i].text = i + 1 + ". ";
            if (highscoreList.Length > i)
            {
                usernameTexts[i].text += highscoreList[i].username;
                scoreTexts[i].text = highscoreList[i].score.ToString();
            }
        }
    }

    IEnumerator RefreshScores()
    {
        while (true)
        {
            highscoreManager.GetScores();
            yield return new WaitForSeconds(30f);
        }
    }
}
