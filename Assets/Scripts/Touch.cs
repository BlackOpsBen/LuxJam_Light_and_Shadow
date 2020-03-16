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
    [SerializeField] GameObject errorSound;
    [SerializeField] GameObject healSound;
    [SerializeField] GameObject grabTorchSound;
    [SerializeField] GameObject grabItemSound;

    private Health health;

    private void Start()
    {
        health = GetComponentInParent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "End":
                health.gameManager.LevelComplete();
                break;
            case "Enemy":
                if (health.gameManager.GetHasSword())
                {
                    health.gameManager.PlaySound(attackSound);
                    collision.gameObject.GetComponent<OnDeath>().isBeingKilled = true;
                    Destroy(collision.gameObject);
                    StartCoroutine(health.gameManager.UseSword());
                    //health.gameManager.GainCoins("Enemy"); // delegating this to the Game Manager so it happens after sword breaks
                }
                else
                {
                    health.gameManager.PlaySound(enemyTouchSound);
                    health.ResetHealth(false);
                    health.gameManager.GameOver();
                }
                break;
            case "Collectible":
                health.gameManager.PlaySound(collectibleSound);
                Destroy(collision.gameObject);
                health.gameManager.GainCoins("Collectible");
                break;
            case "Sword":
                if (!health.gameManager.GetHasSword())
                {
                    Instantiate(swordFoundSound, transform.position, Quaternion.identity);
                    //health.gameManager.PlaySound(swordFoundSound);
                    health.gameManager.SetHasSword(true);
                    Destroy(collision.gameObject);
                }
                else
                {
                    Instantiate(errorSound, transform.position, Quaternion.identity);
                }
                break;
            case "HeartPickup":
                if (health.gameManager.GetHealth() < 4 && health.gameManager.GetCoinCount() >= 1500)
                {
                    Instantiate(healSound, transform.position, Quaternion.identity);
                    health.GainHealth();
                    health.gameManager.GainCoins("ItemPurchase");
                    Destroy(collision.gameObject);
                }
                else
                {
                    Instantiate(errorSound, transform.position, Quaternion.identity);
                }
                break;
            case "TorchPickup":
                if (!FindObjectOfType<Illuminate>().GetHasBackupTorch() && health.gameManager.GetCoinCount() >= 1500)
                {
                    Instantiate(grabTorchSound, transform.position, Quaternion.identity);
                    health.gameManager.GainCoins("ItemPurchase");
                    FindObjectOfType<Illuminate>().SetHasBackupTorch();
                    Destroy(collision.gameObject);
                }
                else
                {
                    Instantiate(errorSound, transform.position, Quaternion.identity);
                }
                break;

            case "AmuletOfWayfinding":
                if (health.gameManager.GetCoinCount() >= 1500)
                {
                    Instantiate(grabItemSound, transform.position, Quaternion.identity);
                    health.gameManager.GainCoins("ItemPurchase");
                    Destroy(collision.gameObject);
                }
                else
                {
                    Instantiate(errorSound, transform.position, Quaternion.identity);
                }
                break;
            default:
                break;
        }

        //if (collision.CompareTag("End"))
        //{
        //    health.gameManager.LevelComplete();
        //}
        //else if (collision.CompareTag("Enemy"))
        //{
        //    if (health.gameManager.GetHasSword())
        //    {
        //        health.gameManager.PlaySound(attackSound);
        //        collision.gameObject.GetComponent<OnDeath>().isBeingKilled = true;
        //        Destroy(collision.gameObject);
        //        StartCoroutine(health.gameManager.UseSword());
        //        //health.gameManager.GainCoins("Enemy"); // delegating this to the Game Manager so it happens after sword breaks
        //    }
        //    else
        //    {
        //        health.gameManager.PlaySound(enemyTouchSound);
        //        health.ResetHealth(false);
        //        health.gameManager.GameOver();
        //    }
        //}
        //else if (collision.CompareTag("Collectible"))
        //{
        //    health.gameManager.PlaySound(collectibleSound);
        //    Destroy(collision.gameObject);
        //    health.gameManager.GainCoins("Collectible");
        //}
        //else if (collision.CompareTag("Sword"))
        //{
        //    if (!health.gameManager.GetHasSword())
        //    {
        //        health.gameManager.PlaySound(swordFoundSound);
        //        health.gameManager.SetHasSword(true);
        //        Destroy(collision.gameObject);
        //    }
        //    else
        //    {
        //        Instantiate(errorSound, transform.position, Quaternion.identity);
        //    }
        //}
    }
}
