using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private GameObject[] hearts;
    private int health = 3;

    private void Start()
    {
        hearts = new GameObject[] { GameObject.Find("Heart1"), GameObject.Find("Heart2"), GameObject.Find("Heart3") };
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
                    Die();
                }
                return;
            }
        }
    }

    private void Die()
    {
        Debug.LogWarning("You died!");
        gameObject.GetComponent<PlayerMovement>().enabled = false;
    }
}
