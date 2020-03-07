using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Touch : MonoBehaviour
{
    [SerializeField] GameObject collectibleSound;
    [SerializeField] GameObject swordFoundSound;
    [SerializeField] GameObject attackSound;
    [SerializeField] GameObject enemyTouchSound;

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
            if (health.gameManager.GetHasSword())
            {
                health.gameManager.PlaySound(attackSound);
                collision.gameObject.GetComponent<OnDeath>().isBeingKilled = true;
                Destroy(collision.gameObject);
                StartCoroutine(health.gameManager.UseSword());
            }
            else
            {
                health.gameManager.PlaySound(enemyTouchSound);
                health.ResetHealth(false);
                health.gameManager.GameOver();
            }
        }
        else if (collision.CompareTag("Collectible"))
        {
            Debug.Log("You found a collectible!");
            health.gameManager.PlaySound(collectibleSound);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Sword"))
        {
            Debug.Log("You found a sword!");
            health.gameManager.PlaySound(swordFoundSound);
            health.gameManager.SetHasSword(true);
            Destroy(collision.gameObject);
        }
    }
}
