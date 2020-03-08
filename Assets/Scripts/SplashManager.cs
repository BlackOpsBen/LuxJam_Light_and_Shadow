using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    private SceneRandomizer sceneRandomizer;

    private void Start()
    {
        sceneRandomizer = FindObjectOfType<SceneRandomizer>();
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
