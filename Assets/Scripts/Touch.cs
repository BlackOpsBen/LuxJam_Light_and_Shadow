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
    [SerializeField] GameObject armorBreakSound;
    [SerializeField] GameObject errorSound;
    [SerializeField] GameObject healSound;
    [SerializeField] GameObject grabTorchSound;
    [SerializeField] GameObject grabItemSound;

    private Health health;
    private Armor armor;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        armor = GetComponentInParent<Armor>();
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
                }
                else if (armor.GetIsArmored())
                {
                    health.gameManager.SetIsArmored(false);
                    playerMovement.transform.position = playerMovement.GetPreviousPosition();
                    armor.SetIsArmored(false);
                    Instantiate(armorBreakSound, transform.position, Quaternion.identity);
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
                    PlayErrorSound();
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
                    PlayErrorSound();
                }
                break;
            case "TorchPickup":
                if (!FindObjectOfType<Illuminate>().GetHasBackupTorch() && health.gameManager.GetCoinCount() >= 1500)
                {
                    Instantiate(grabTorchSound, transform.position, Quaternion.identity);
                    health.gameManager.GainCoins("ItemPurchase");
                    FindObjectOfType<Illuminate>().SetHasBackupTorch(true);
                    Destroy(collision.gameObject);
                }
                else
                {
                    PlayErrorSound();
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
                    PlayErrorSound();
                }
                break;
            case "Armor":
                if (!armor.GetIsArmored() && health.gameManager.GetCoinCount() >= 1500)
                {
                    GainArmor();
                    Destroy(collision.gameObject);
                }
                else
                {
                    PlayErrorSound();
                }
                break;
            default:
                break;
        }
    }

    public void GainArmor() // TODO set to private when removing all cheats
    {
        Instantiate(grabItemSound, transform.position, Quaternion.identity);
        armor.SetIsArmored(true);
        health.gameManager.GainCoins("ItemPurchase");
        health.gameManager.SetIsArmored(true);
    }

    private void PlayErrorSound()
    {
        Instantiate(errorSound, transform.position, Quaternion.identity);
    }
}
