﻿using System.Collections;
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
    private bool isInputReady = false;
    private bool isValidUsername = false;

    private void Start()
    {
        inputUI.SetActive(false);
        StartCoroutine(ShowInputUI());
    }

    private void Update()
    {
        if (isInputReady && Input.GetKeyDown(KeyCode.Escape))
        {
            PressNo();
        }
        if (isInputReady && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            PressOk();
        }

        SetUserName();

        if (string.IsNullOrEmpty(username))
        {
            isValidUsername = false;
            okButton.interactable = false;
        }
        else
        {
            isValidUsername = true;
            okButton.interactable = true;
        }
    }

    private IEnumerator ShowInputUI()
    {
        yield return null;
        gameManager = FindObjectOfType<GameManager>();
        inputUI.SetActive(true);
        inputField.ActivateInputField();
        noButton.onClick.AddListener(delegate () { PressNo(); });
        okButton.onClick.AddListener(delegate () { PressOk(); });
        isInputReady = true;
    }

    public void PressNo()
    {
        HideInputUI();
        Leaderboard.OnlyGetScores();
        StartCoroutine(AllowRestart());
    }

    public void PressOk()
    {
        if (isValidUsername)
        {
            HideInputUI();

            Leaderboard.AddNewScore(username, gameManager.GetCoinCount());

            StartCoroutine(AllowRestart());
        }
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

    private IEnumerator AllowRestart()
    {
        yield return new WaitForSeconds(1.5f);
        gameManager.SetCanRestart();
    }
}

// TODO remove cheat
// TODO add intro text
// TODO Make continue prompt only specific key (so people can screenshot