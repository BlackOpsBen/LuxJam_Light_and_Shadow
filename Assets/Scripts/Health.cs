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
        StartCoroutine(DelayedGetGameManager());
        hearts = new GameObject[] { GameObject.Find("Heart3"), GameObject.Find("Heart2"), GameObject.Find("Heart1") };
    }

    private IEnumerator DelayedGetGameManager()
    {
        yield return null;
        gameManager = FindObjectOfType<GameManager>();
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
