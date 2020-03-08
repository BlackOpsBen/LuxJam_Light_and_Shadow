using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    private SceneRandomizer sceneRandomizer;
    private bool canStart = false;
    [SerializeField] float inputDelay = 2f;
    [SerializeField] GameObject prompt;

    private void Start()
    {
        sceneRandomizer = FindObjectOfType<SceneRandomizer>();
        sceneRandomizer.InitializeSceneList();
        StartCoroutine(DelayCanStart());
    }

    private IEnumerator DelayCanStart()
    {
        yield return new WaitForSeconds(inputDelay);
        canStart = true;
        yield return new WaitForSeconds(1f);
        Instantiate(prompt, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if (canStart && Input.anyKeyDown)
        {
            SceneManager.LoadScene(sceneRandomizer.GetRandomSceneIndex());            
        }
    }
}
