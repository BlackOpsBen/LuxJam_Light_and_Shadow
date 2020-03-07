﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public GameManager gameManager;

    private GameObject[] hearts;
    private int health = 3;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        hearts = new GameObject[] { GameObject.Find("Heart3"), GameObject.Find("Heart2"), GameObject.Find("Heart1") };
    }
    public void LoseHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i].activeSelf == true)
            {
                hearts[i].SetActive(false);
                health--;
                if (health == 0)
                {
                    gameManager.GameOver();
                }
                return;
            }
        }
    }

    public void ResetHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(true);
        }
    }
}
