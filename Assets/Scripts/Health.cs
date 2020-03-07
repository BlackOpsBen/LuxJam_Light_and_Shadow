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
        //health = gameManager.GetHealth();
        gameManager = FindObjectOfType<GameManager>();
        hearts = new GameObject[] { GameObject.Find("Heart3"), GameObject.Find("Heart2"), GameObject.Find("Heart1") };
    }
    public void LoseHealth()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i].GetComponent<Image>().enabled)
            {
                hearts[i].GetComponent<Image>().enabled = false;
                //health--;
                gameManager.LoseHealth();
                if (gameManager.GetHealth() == 0)
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
            hearts[i].GetComponent<Image>().enabled = true;
        }
    }
}
