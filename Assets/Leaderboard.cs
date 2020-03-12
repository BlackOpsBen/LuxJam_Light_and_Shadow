using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Leaderboard : MonoBehaviour
{
    const string privateCode = "1m_7Ka99GEyyhUZUXqFKtw7QYkVTg1oUGzuGmE7ElQZA";
    const string publicCode = "5e67d122fe232612b89ba932";
    const string webURL = "http://dreamlo.com/lb/";

    private Highscore[] highscoreList;
    private Highscore singleHighscore;

    private bool isPreviousScore;

    static Leaderboard instance;
    DisplayScores displayScores;

    private void Awake()
    {
        instance = this;
        displayScores = GetComponent<DisplayScores>();
    }
    public static void AddNewScore(string username, int score)
    {
        instance.isPreviousScore = true;
        //instance.StartCoroutine(instance.DownloadPlayer(username)); // Including this inside UploadNewScore to ensure it is complete before upload.
        instance.StartCoroutine(instance.UploadNewScore(username, score));
    }
    IEnumerator UploadNewScore(string username, int score)
    {
        #region Check for player first, and save any existing score
        WWW wwwD = new WWW(webURL + publicCode + "/pipe-get/" + username);
        yield return wwwD;

        if (string.IsNullOrEmpty(wwwD.error))
        {
            FormatSingleScore(wwwD.text);
            displayScores.SetPlayerExists(true);
            displayScores.OnSingleScoreDownloaded(singleHighscore, isPreviousScore);
        }
        else
        {
            displayScores.SetPlayerExists(false);
            print("Error downloading: " + wwwD.error + ". Player doesn't exist yet?");
        }
        isPreviousScore = false;
        #endregion

        #region Then add their new score
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            print("Upload Successful");
            GetScores();
        }
        else
        {
            print("Error uploading: " + www.error);
        }
        isPreviousScore = false; // TODO <- is this where I put "isPreviousScore = false;" ?
        #endregion
    }

    public void GetScores()
    {
        instance.StartCoroutine(instance.DownloadScores());
    }

    IEnumerator DownloadScores()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatScores(www.text);
            displayScores.OnScoresDownloaded(highscoreList);
        }
        else
        {
            print("Error downloading: " + www.error);
        }
    }

    void FormatScores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoreList = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] {'|'});
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            int rank = int.Parse(entryInfo[5]);
            highscoreList[i] = new Highscore(username, score, rank);
            print(highscoreList[i].username + ": " + highscoreList[i].score);
        }
    }

    void FormatSingleScore(string textStream)
    {
        string entry = textStream;
        singleHighscore = new Highscore();

        string[] entryInfo = entry.Split(new char[] { '|' });
        string username = entryInfo[0];
        int score = int.Parse(entryInfo[1]);
        int rank = int.Parse(entryInfo[5]);
        singleHighscore = new Highscore(username, score, rank);
        print(singleHighscore.rank + ". " + singleHighscore.username + ": " + singleHighscore.score);
    }

    //public static bool CheckListForName(string username)
    //{
    //    instance.StartCoroutine(instance.DownloadPlayer(username));
    //    return instance.playerExists;
    //}

    //IEnumerator DownloadPlayer(string username) // Including this in the Upload
    //{
    //    WWW wwwD = new WWW(webURL + publicCode + "/pipe-get/" + username);
    //    yield return wwwD;

    //    if (string.IsNullOrEmpty(wwwD.error))
    //    {
    //        FormatSingleScore(wwwD.text);
    //        displayScores.SetPlayerExists(true);
    //        displayScores.OnSingleScoreDownloaded(singleHighscore, isPreviousScore);
    //    }
    //    else
    //    {
    //        displayScores.SetPlayerExists(false);
    //        print("Error downloading: " + wwwD.error + ". Player doesn't exist yet?");
    //    }
    //    isPreviousScore = false;
    //}

    //public static Highscore GetSingleHighscore()
    //{
    //    return instance.singleHighScore;
    //}
}

public struct Highscore
{
    public string username;
    public int score;
    public int rank;

    public Highscore(string _username, int _score, int _rank)
    {
        username = _username;
        score = _score;
        rank = _rank;
    }
}