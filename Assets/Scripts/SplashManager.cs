using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    private GameManager gameManager;
    private SceneRandomizer sceneRandomizer;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        sceneRandomizer = gameManager.GetComponent<SceneRandomizer>();
    }

    private void Update()
    {
        sceneRandomizer.InitializeSceneList();
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(sceneRandomizer.GetRandomSceneIndex());            
        }
    }
}
