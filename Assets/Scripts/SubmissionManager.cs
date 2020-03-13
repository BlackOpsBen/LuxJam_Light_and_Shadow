using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubmissionManager : MonoBehaviour
{
    [SerializeField] Button noButton;
    [SerializeField] Button okButton;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject inputUI;
    private GameManager gameManager;
    private string username;

    private void Start()
    {
        inputUI.SetActive(false);
        StartCoroutine(ShowInputUI());
    }

    private IEnumerator ShowInputUI()
    {
        yield return null;
        gameManager = FindObjectOfType<GameManager>();
        inputUI.SetActive(true);
        noButton.onClick.AddListener(delegate () { PressNo(); });
        okButton.onClick.AddListener(delegate () { PressOk(); });
        Debug.Log("Coins: " + gameManager.GetCoinCount());
    }

    public void PressNo()
    {
        Debug.Log("No pressed.");
        HideInputUI();

        Leaderboard.OnlyGetScores();
    }

    public void PressOk()
    {
        Debug.Log("OK pressed.");
        HideInputUI();

        SetUserName();
        Debug.Log("Username: " + username);

        Leaderboard.AddNewScore(username, gameManager.GetCoinCount());
    }

    private void HideInputUI()
    {
        noButton.enabled = false;
        okButton.enabled = false;
        inputUI.SetActive(false);
    }

    public void SetUserName()
    {
        username = inputField.text;
    }
}


// TODO Validate entry before accepting OK
// TODO Make keyboard Enter and Esc trigger buttons also. 
// TODO Check for existing user and confirm entry