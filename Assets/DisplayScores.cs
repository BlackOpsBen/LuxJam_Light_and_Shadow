using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayScores : MonoBehaviour
{
    private const string altHeaderTop = "- You are the mightiest in the land! -";
    private const string altHeaderNewBest = "- New personal best! -";
    private const string altHeaderStillTop = "- You are STILL the mightiest in the land! -";
    private const string altHeaderTiedBest = "- You tied your previous best! -";
    private const string altHeaderOldBest = "- Try to beat your previous best! -";

    [SerializeField] Color highlightColor = Color.magenta;
    [SerializeField] Color topColor = Color.yellow;

    public TextMeshProUGUI[] usernameTexts;
    public TextMeshProUGUI[] scoreTexts;
    public TextMeshProUGUI secondaryHeader;
    public TextMeshProUGUI yourUsername;
    public TextMeshProUGUI yourScore;

    Leaderboard highscoreManager;

    private Highscore previousHighscore;
    private Highscore currentHighscore;

    private bool alreadyChecked = false;
    private bool usernameExists;

    void Start()
    {
        GetTop10Scores();
    }

    private void GetTop10Scores()
    {
        secondaryHeader.text = "";
        yourScore.text = "";
        yourUsername.text = "";

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

    public void OnSingleScoreDownloaded(Highscore singleHighscore, bool isPrevious)
    {
        if (isPrevious)
        {
            previousHighscore = singleHighscore;
        }
        else
        {
            currentHighscore = singleHighscore;
        }

        if (!usernameExists)
        {
            ShowNewBest();
        }
        else // if username DOES exist
        {
            if (currentHighscore.score > previousHighscore.score)
            {
                ShowNewBest();
            }
            else if (currentHighscore.score == previousHighscore.score)
            {
                ShowTiedBest();
            }
            else if (currentHighscore.score < previousHighscore.score)
            {
                ShowOldBest();
            }
        }
    }

    private void ShowNewBest()
    {
        if (currentHighscore.rank == 0)
        {
            secondaryHeader.text = altHeaderTop;
            yourUsername.text = "";
            yourScore.text = "";
        }
        else if (currentHighscore.rank < 10)
        {
            usernameTexts[currentHighscore.rank].color = highlightColor;
            scoreTexts[currentHighscore.rank].color = highlightColor;

            secondaryHeader.text = altHeaderNewBest;
            yourUsername.text = "";
            yourScore.text = "";
        }
        else
        {
            secondaryHeader.text = altHeaderNewBest;
            yourUsername.text = (currentHighscore.rank + 1) + ". " + currentHighscore.username;
            yourScore.text = currentHighscore.score.ToString();
        }
    }

    private void ShowTiedBest()
    {
        if (currentHighscore.rank == 0)
        {
            secondaryHeader.text = altHeaderStillTop;
            yourUsername.text = "";
            yourScore.text = "";
        }
        else if (currentHighscore.rank < 10)
        {
            usernameTexts[currentHighscore.rank].color = highlightColor;
            scoreTexts[currentHighscore.rank].color = highlightColor;

            secondaryHeader.text = altHeaderTiedBest;
            yourUsername.text = "";
            yourScore.text = "";
        }
        else
        {
            secondaryHeader.text = altHeaderTiedBest;
            yourUsername.text = (currentHighscore.rank + 1) + ". " + currentHighscore.username;
            yourScore.text = currentHighscore.score.ToString();
        }
    }

    private void ShowOldBest()
    {
        if (currentHighscore.rank == 0)
        {
            secondaryHeader.text = altHeaderStillTop;
            yourUsername.text = "";
            yourScore.text = "";
        }
        else if (currentHighscore.rank < 10)
        {
            usernameTexts[currentHighscore.rank].color = highlightColor;
            scoreTexts[currentHighscore.rank].color = highlightColor;

            secondaryHeader.text = altHeaderOldBest;
            yourUsername.text = "";
            yourScore.text = "";
        }
        else
        {
            secondaryHeader.text = altHeaderOldBest;
            yourUsername.text = (currentHighscore.rank + 1) + ". " + currentHighscore.username;
            yourScore.text = currentHighscore.score.ToString();
        }
    }

    public void SetPlayerExists(bool isPlayerListed)
    {
        alreadyChecked = false;
        if (!alreadyChecked)
        {
            usernameExists = isPlayerListed;
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
