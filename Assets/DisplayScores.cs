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
    [SerializeField] Color defaultColor = Color.red;

    public TextMeshProUGUI[] usernameTexts;
    public TextMeshProUGUI[] scoreTexts;
    public TextMeshProUGUI secondaryHeader;
    public TextMeshProUGUI yourUsername;
    public TextMeshProUGUI yourScore;

    Leaderboard highscoreManager;

    private Highscore previousHighscore;
    private int previousHighscoreScore;
    private Highscore newHighscore;
    private int newHighscoreScore;
    private int scoreThisRun;

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

        //StartCoroutine(RefreshScores());
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
        ResetColors();
        if (isPrevious)
        {
            previousHighscore = singleHighscore;
            previousHighscoreScore = previousHighscore.score;
        }
        else
        {
            newHighscore = singleHighscore;
            newHighscoreScore = newHighscore.score;

            if (!usernameExists)
            {
                print("Username does not exist yet.");
                print("previousHighscore.score = " + previousHighscore.score);
                print("newHighscore.score = " + newHighscore.score);
                print("scoreThisRun = " + scoreThisRun);
                ShowNewBest();
            }
            else // if username DOES exist
            {
                print("Username DOES exist & new is greater than previous.");
                print("previousHighscore.score = " + previousHighscore.score);
                print("newHighscore.score = " + newHighscore.score);
                print("scoreThisRun = " + scoreThisRun);
                if (newHighscore.score > previousHighscore.score)
                {
                    ShowNewBest();
                }
                else if (scoreThisRun == previousHighscore.score)
                {
                    ShowTiedBest();
                }
                else if (scoreThisRun < previousHighscore.score)
                {
                    ShowOldBest();
                }
                else
                {
                    Debug.LogWarning("Invalid score comparrison state reached!");
                    Debug.LogWarning("newHighscore: " + newHighscore.score + "\n" + "previousHighscore: " + previousHighscore.score + "\n" + "scoreThisRun: " + scoreThisRun); ;
                }
            }
        }
    }

    private void ShowNewBest()
    {
        if (newHighscore.rank == 0)
        {
            secondaryHeader.text = altHeaderTop;
            yourUsername.text = "";
            yourScore.text = "";
        }
        else if (newHighscore.rank < 10)
        {
            usernameTexts[newHighscore.rank].color = highlightColor;
            scoreTexts[newHighscore.rank].color = highlightColor;

            secondaryHeader.text = altHeaderNewBest;
            yourUsername.text = "";
            yourScore.text = "";
        }
        else
        {
            secondaryHeader.text = altHeaderNewBest;
            yourUsername.text = (newHighscore.rank + 1) + ". " + newHighscore.username;
            yourScore.text = newHighscore.score.ToString();
        }
    }

    private void ShowTiedBest()
    {
        if (newHighscore.rank == 0)
        {
            secondaryHeader.text = altHeaderStillTop;
            yourUsername.text = "";
            yourScore.text = "";
        }
        else if (newHighscore.rank < 10)
        {
            usernameTexts[newHighscore.rank].color = highlightColor;
            scoreTexts[newHighscore.rank].color = highlightColor;

            secondaryHeader.text = altHeaderTiedBest;
            yourUsername.text = "";
            yourScore.text = "";
        }
        else
        {
            secondaryHeader.text = altHeaderTiedBest;
            yourUsername.text = (newHighscore.rank + 1) + ". " + newHighscore.username;
            yourScore.text = newHighscore.score.ToString();
        }
    }

    private void ShowOldBest()
    {
        if (newHighscore.rank == 0)
        {
            secondaryHeader.text = altHeaderStillTop;
            yourUsername.text = "";
            yourScore.text = "";
        }
        else if (newHighscore.rank < 10)
        {
            usernameTexts[newHighscore.rank].color = highlightColor;
            scoreTexts[newHighscore.rank].color = highlightColor;

            secondaryHeader.text = altHeaderOldBest;
            yourUsername.text = "";
            yourScore.text = "";
        }
        else
        {
            secondaryHeader.text = altHeaderOldBest;
            yourUsername.text = (newHighscore.rank + 1) + ". " + newHighscore.username;
            yourScore.text = newHighscore.score.ToString();
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
            highscoreManager.GetScores(null);
            yield return new WaitForSeconds(30f);
        }
    }

    public void ClearSecondaryFields()
    {
        secondaryHeader.text = "";
        yourUsername.text = "";
        yourScore.text = "";
    }

    public void ResetColors()
    {
        usernameTexts[0].color = topColor;
        scoreTexts[0].color = topColor;
        for (int i = 1; i < usernameTexts.Length; i++)
        {
            usernameTexts[i].color = defaultColor;
            scoreTexts[i].color = defaultColor;
        }
    }

    public void ResetHighscores()
    {
        previousHighscore = new Highscore();
        previousHighscoreScore = 0;
        newHighscore = new Highscore();
        newHighscoreScore = 0;
        scoreThisRun = 0;
    }

    public void SetThisRunsScore(int score)
    {
        scoreThisRun = score;
    }
}
