using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public GameManager gameManager;

    private GameObject[] hearts;
    //private int health;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Debug.LogWarning("Game manager found");
        hearts = new GameObject[] { GameObject.Find("Heart3"), GameObject.Find("Heart2"), GameObject.Find("Heart1") };
    }
    public void LoseHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i].GetComponent<Image>().enabled)
            {
                hearts[i].GetComponent<Image>().enabled = false;
                gameManager.LoseHealth();
                if (gameManager.GetHealth() == 0)
                {
                    gameManager.GameOver();
                }
                return;
            }
        }
    }

    public void ResetHealth(bool full)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].GetComponent<Image>().enabled = full;
        }
    }
}
