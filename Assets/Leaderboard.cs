using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Leaderboard : MonoBehaviour
{
    const string privateCode = "1m_7Ka99GEyyhUZUXqFKtw7QYkVTg1oUGzuGmE7ElQZA";
    const string publicCode = "5e67d122fe232612b89ba932";
    const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highScoresList;
    static Leaderboard instance;
    DisplayScores highscoresDisplay;

    private void Awake()
    {
        instance = this;
        highscoresDisplay = GetComponent<DisplayScores>();
    }
    public static void AddNewScore(string username, int score)
    {
        instance.StartCoroutine(instance.UploadNewScore(username, score));
    }
    IEnumerator UploadNewScore(string username, int score)
    {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        //UnityWebRequest www = new UnityWebRequest(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score);
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
    }

    public void GetScores()
    {
        StartCoroutine(DownloadScores());
    }

    IEnumerator DownloadScores()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        //UnityWebRequest www = new UnityWebRequest(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatScores(www.text);
            highscoresDisplay.OnScoresDownloaded(highScoresList);
        }
        else
        {
            print("Error downloading: " + www.error);
        }
    }

    void FormatScores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highScoresList = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] {'|'});
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highScoresList[i] = new Highscore(username, score);
            print(highScoresList[i].username + ": " + highScoresList[i].score);
        }
    }
}

public struct Highscore
{
    public string username;
    public int score;

    public Highscore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
}