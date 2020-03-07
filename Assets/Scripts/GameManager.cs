using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject youDiedSound;
    [SerializeField] GameObject winSound;
    [SerializeField] GameObject startSound;

    private PlayerMovement player;
    private bool canRestart = false;
    private bool canAdvance = false;
    private Health health;
    private int currentHealth = 3;
    private Illuminate illuminate;

    private void Start()
    {
        illuminate = GetComponent<Illuminate>();
        GetPlayer();
        PlaySound(startSound);
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
        Debug.Log("Game Over.");
        PlaySound(youDiedSound);
        Debug.LogWarning("You died! PRESS ANY KEY TO RESTART");
        DisablePlayer();
        canRestart = true;
    }

    public void LevelComplete()
    {
        PlaySound(winSound);
        Debug.Log("Level complete! PRESS ANY KEY TO CONTINUE");
        DisablePlayer();
        canAdvance = true;
    }

    public void DisablePlayer()
    {
        player.SetDisabled(true);
        illuminate.SetDisabled(true);
    }

    private IEnumerator LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        SceneManager.LoadScene(0);
        yield return null;
        PlaySound(startSound);
        GetPlayer();
        illuminate.ResetFuel();
        illuminate.SetDisabled(false);
    }

    public void PlaySound(GameObject sound)
    {
        Debug.Log("Playing sound: " + sound.name);
        Instantiate(sound, transform.position, Quaternion.identity);
    }
}
