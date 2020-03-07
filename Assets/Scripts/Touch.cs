using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Touch : MonoBehaviour
{
    private Health health;

    private void Start()
    {
        health = GetComponentInParent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("End"))
        {
            health.gameManager.LevelComplete();
        }
        else if (collision.CompareTag("Enemy"))
        {
            health.ResetHealth(false);
            health.gameManager.GameOver();
        }
    }
}
