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

    [SerializeField] TextMeshProUGUI levelCounter;
    private const string levelCounterPrefix = "Level: ";

    [SerializeField] Image swordIcon;
    [SerializeField] Image swordBrokenIcon;
    [SerializeField] GameObject bonusCoinsSword;

    [SerializeField] GameObject bonusCoinsFuel;

    [SerializeField] GameObject bonusCoinsStageCleared;

    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject diedScreen;
    [SerializeField] GameObject lootScreen;
    [SerializeField] float restartDelay = 1.5f;

    [SerializeField] GameObject prompt;

    private SceneRandomizer sceneRandomizer;

    private int collectiblesRemaining = 0;
    private bool stageCleared = false;
    private int stageClearedAmount = 100;
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
    private bool canContinue = false;
    private Health health;
    private int currentHealth = 3;
    private Illuminate illuminate;

    private bool hasSword = false;

    private bool isSplashMode = false;

    private void Start()
    {
        sceneRandomizer = FindObjectOfType<SceneRandomizer>();
        levelCounter.text = levelCounterPrefix + 1;
        illuminate = GetComponent<Illuminate>();
        ResetCollectiblesRemaining(GameObject.FindGameObjectsWithTag("Collectible").Length);
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
        GetContinue();

        // Cheat to beat level
        if (Input.GetKey(KeyCode.Backspace) && Input.GetKeyDown(KeyCode.L))
        {
            LevelComplete();
        }
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

    public void SetCanRestart()
    {
        canRestart = true;
        Instantiate(prompt, transform.position, Quaternion.identity);
    }

    private void GetContinue()
    {
        if (canContinue && Input.anyKeyDown)
        {
            canContinue = false;
            StartCoroutine(Continue());
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
        if (coins == 0)
        {
            StartCoroutine(DelayCanRestart());
        }
        else
        {
            StartCoroutine(DelayCanContinue());
        }
    }

    private IEnumerator DelayCanContinue()
    {
        yield return new WaitForSeconds(restartDelay);
        Instantiate(lootScreen, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(restartDelay);
        canContinue = true;
        Instantiate(prompt, transform.position, Quaternion.identity);
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
        DisablePlayer();
        GainCoins("Fuel");
        if (hasSword)
        {
            GainCoins("Sword");
            SetHasSword(false);
        }
        if (stageCleared)
        {
            GainCoins("StageCleared");
            stageCleared = false;
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
        levelCounter.text = levelCounterPrefix + sceneRandomizer.GetPlayedScenes();
        coinMultiplier += coinMultiplierIncrease;
        PlaySound(startSound);
        GetPlayer();
        ResetCollectiblesRemaining(GameObject.FindGameObjectsWithTag("Collectible").Length);
        illuminate.ResetFuel();
        illuminate.SetDisabled(false);
    }

    private IEnumerator RestartGame()
    {
        health.ResetHealth(true);
        currentHealth = 3;
        SetHasSword(false);
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

    private IEnumerator Continue()
    {
        SceneManager.LoadScene("Leaderboard");
        yield return null;
        PlaySound(winSound);
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
        float currentMultiplier = coinMultiplier;
        yield return new WaitForSeconds(0.5f);
        GameObject killLoot = Instantiate(bonusCoinsSword, transform.position, Quaternion.identity);
        killLoot.GetComponentInChildren<TextMeshProUGUI>().text = "+" + Mathf.RoundToInt(enemyCoinAmount * currentMultiplier).ToString();
        GainCoins("Enemy");
        swordBrokenIcon.enabled = false;
    }

    public void GainCoins(string source)
    {
        int amount = 0;
        switch (source)
        {
            case "Collectible":
                amount = collectibleCoinAmount;
                DecrementCollectiblesRemaining();
                break;
            case "Enemy":
                amount = enemyCoinAmount;
                break;
            case "Fuel":
                amount = fuelCoinAmount * Mathf.RoundToInt(illuminate.GetRemainingFuel()*100);
                GameObject bonusTextFuel = Instantiate(bonusCoinsFuel, transform.position, Quaternion.identity);
                bonusTextFuel.GetComponentInChildren<TextMeshProUGUI>().text = "+" + Mathf.RoundToInt(amount * coinMultiplier).ToString();
                break;
            case "Sword":
                amount = swordCoinAmount;
                GameObject bonusTextSword = Instantiate(bonusCoinsSword, transform.position, Quaternion.identity);
                bonusTextSword.GetComponentInChildren<TextMeshProUGUI>().text = "+" + Mathf.RoundToInt(amount * coinMultiplier).ToString();
                break;
            case "StageCleared":
                amount = stageClearedAmount;
                GameObject bonusTextStageCleared = Instantiate(bonusCoinsStageCleared, transform.position, Quaternion.identity);
                bonusTextStageCleared.GetComponentInChildren<TextMeshProUGUI>().text = "+" + Mathf.RoundToInt(amount * coinMultiplier).ToString() + "\n" + "Loot cleared!";
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

    public void SubmitScore(string username)
    {
        int score = coins;
        Leaderboard.AddNewScore(username, score);
    }

    public void ResetCollectiblesRemaining(int count)
    {
        collectiblesRemaining = count;
    }

    private void DecrementCollectiblesRemaining()
    {
        collectiblesRemaining--;
        if (collectiblesRemaining == 0)
        {
            stageCleared = true;
        }
    }
}
