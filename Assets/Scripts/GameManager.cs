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
    [SerializeField] GameObject bonusCoinsSword;

    [SerializeField] GameObject bonusCoinsFuel;

    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject diedScreen;
    [SerializeField] GameObject lootScreen;
    [SerializeField] float restartDelay = 1.5f;

    [SerializeField] GameObject prompt;

    private SceneRandomizer sceneRandomizer;

    [SerializeField] TextMeshProUGUI coinCounter;
    private int coins = 0;
    private int collectibleCoinAmount = 100;
    private int enemyCoinAmount = 50;
    private int swordCoinAmount = 10;
    private int fuelCoinAmount = 1;
    private float coinMultiplierBase = 1f;
    private float coinMultiplier = 1f;
    private float coinMultiplierIncrease = 0.1f;

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
        Instantiate(diedScreen, transform.position, Quaternion.identity);
        DisablePlayer();
        StartCoroutine(DelayCanRestart());
    }

    private IEnumerator DelayCanRestart()
    {
        yield return new WaitForSeconds(restartDelay);
        Instantiate(lootScreen, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(restartDelay);
        canRestart = true;
        Instantiate(prompt, transform.position, Quaternion.identity);
    }

    public void LevelComplete()
    {
        PlaySound(winSound);
        Instantiate(victoryScreen, transform.position, Quaternion.identity);
        Debug.Log("Level complete! PRESS ANY KEY TO CONTINUE"); // TODO make this happen in UI, not console.
        DisablePlayer();
        GainCoins("Fuel");
        if (hasSword)
        {
            GainCoins("Sword");
            SetHasSword(false); // TODO add "+coins" on screen where sword disappears
        }
        StartCoroutine(DelayCanAdvance());
    }

    private IEnumerator DelayCanAdvance()
    {
        yield return new WaitForSeconds(restartDelay);
        canAdvance = true;
        Instantiate(prompt, transform.position, Quaternion.identity);
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
        coinMultiplier += coinMultiplierIncrease;
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
        coinMultiplier = coinMultiplierBase;
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
                GameObject bonusTextFuel = Instantiate(bonusCoinsFuel, transform.position, Quaternion.identity);
                bonusTextFuel.GetComponentInChildren<TextMeshProUGUI>().text = "+" + amount.ToString();
                break;
            case "Sword":
                amount = swordCoinAmount;
                GameObject bonusTextSword = Instantiate(bonusCoinsSword, transform.position, Quaternion.identity);
                bonusTextSword.GetComponentInChildren<TextMeshProUGUI>().text = "+" + amount.ToString();
                break;
            default:
                Debug.LogWarning("Invalid coin amount/source");
                break;
        }
        coins += Mathf.RoundToInt(amount * coinMultiplier);
        coinCounter.text = coins.ToString();
    }

    public int GetCoinCount()
    {
        return coins;
    }

    public void ToggleSplashMode()
    {
        isSplashMode = !isSplashMode;
    }
}
