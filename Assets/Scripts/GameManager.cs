using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject youDiedSound;
    [SerializeField] GameObject winSound;
    [SerializeField] GameObject startSound;

    [SerializeField] Image swordIcon;
    [SerializeField] Image swordBrokenIcon;

    private SceneRandomizer sceneRandomizer;

    [SerializeField] TextMeshProUGUI coinCounter;
    private int coins = 0;
    private int collectibleCoinAmount = 100;
    private int enemyCoinAmount = 50;
    private int swordCoinAmount = 10;
    private int fuelCoinAmount = 1;

    private PlayerMovement player;
    private bool canRestart = false;
    private bool canAdvance = false;
    private Health health;
    private int currentHealth = 3;
    private Illuminate illuminate;

    private bool hasSword = false;

    private bool isSplashMode = false;

    private void Start()
    {
        sceneRandomizer = FindObjectOfType<SceneRandomizer>();
        illuminate = GetComponent<Illuminate>();
        GetPlayer();
        PlaySound(startSound);

        swordIcon.enabled = false;
        swordBrokenIcon.enabled = false;
    }

    private void GetPlayer()
    {
        health = FindObjectOfType<Health>();
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        GetAdvanceLevel();
        GetRestartGame();
    }

    private void GetAdvanceLevel()
    {
        if (canAdvance && Input.anyKeyDown)
        {
            canAdvance = false;
            StartCoroutine(LoadNextLevel());
        }
    }

    private void GetRestartGame()
    {
        if (canRestart && Input.anyKeyDown)
        {
            canRestart = false;
            StartCoroutine(RestartGame());
        }
    }

    public void LoseHealth()
    {
        currentHealth--;
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public void GameOver()
    {
        PlaySound(youDiedSound);
        Debug.LogWarning("You died! PRESS ANY KEY TO RESTART"); // TODO make this happen in UI, not console.
        DisablePlayer();
        canRestart = true;
    }

    public void LevelComplete()
    {
        PlaySound(winSound);
        Debug.Log("Level complete! PRESS ANY KEY TO CONTINUE"); // TODO make this happen in UI, not console.
        DisablePlayer();
        GainCoins("Fuel");
        canAdvance = true;
    }

    public void DisablePlayer()
    {
        player.SetDisabled(true);
        illuminate.SetDisabled(true);
    }

    private IEnumerator LoadNextLevel()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Instead of loading the first scene, switching to loading a random scene starting the game over.
        SceneManager.LoadScene(sceneRandomizer.GetRandomSceneIndex());
        yield return null;
        PlaySound(startSound);
        GetPlayer();
        illuminate.ResetFuel();
        illuminate.SetDisabled(false);
    }

    private IEnumerator RestartGame()
    {
        health.ResetHealth(true);
        currentHealth = 3;
        ResetCoins();
        //SceneManager.LoadScene(0); // Instead of loading the first scene, switching to loading a random scene starting the game over.
        sceneRandomizer.InitializeSceneList();
        SceneManager.LoadScene(sceneRandomizer.GetRandomSceneIndex());
        yield return null;
        PlaySound(startSound);
        GetPlayer();
        illuminate.ResetFuel();
        illuminate.SetDisabled(false);
    }

    private void ResetCoins()
    {
        coins = 0;
        coinCounter.text = coins.ToString();
    }

    public void PlaySound(GameObject sound)
    {
        Instantiate(sound, transform.position, Quaternion.identity);
    }

    public bool GetHasSword()
    {
        return hasSword;
    }

    public void SetHasSword(bool tOrF)
    {
        swordIcon.enabled = tOrF;
        hasSword = tOrF;
    }

    public IEnumerator UseSword()
    {
        SetHasSword(false);
        swordBrokenIcon.enabled = true;
        yield return new WaitForSeconds(0.5f);
        swordBrokenIcon.enabled = false;
    }

    public void GainCoins(string source)
    {
        int amount = 0;
        switch (source)
        {
            case "Collectible":
                amount = collectibleCoinAmount;
                break;
            case "Enemy":
                amount = enemyCoinAmount;
                break;
            case "Fuel":
                amount = fuelCoinAmount * Mathf.RoundToInt(illuminate.GetRemainingFuel()*100);
                break;
            default:
                Debug.LogWarning("Invalid coin amount/source");
                break;
        }
        coins += amount;
        coinCounter.text = coins.ToString();
    }

    public void ToggleSplashMode()
    {
        isSplashMode = !isSplashMode;
    }
}
