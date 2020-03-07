using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerMovement player;
    private bool canRestart = false;
    private bool canAdvance = false;
    private Health health;

    private void Start()
    {
        GetPlayer();
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

    public void GameOver()
    {
        Debug.Log("Game Over.");
        Debug.LogWarning("You died! PRESS ANY KEY TO RESTART");
        DisablePlayer();
        canRestart = true;
    }

    public void LevelComplete()
    {
        Debug.Log("Level complete! PRESS ANY KEY TO CONTINUE");
        DisablePlayer();
        canAdvance = true;
    }

    public void DisablePlayer()
    {
        player.SetDisabled(true);
    }

    private IEnumerator LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
        GetPlayer();
    }

    private IEnumerator RestartGame()
    {
        health.ResetHealth();
        SceneManager.LoadScene(0);
        yield return null;
        GetPlayer();
    }

}
