using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRandomizer : MonoBehaviour
{
    List<int> SceneBuildNums = new List<int>();

    private void Start()
    {
        InitializeSceneList();
        Debug.Log("Scene count: " + SceneManager.sceneCountInBuildSettings);
    }

    public void InitializeSceneList()
    {
        SceneBuildNums = new List<int>();
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            SceneBuildNums.Add(i);
        }
    }

    public int GetRandomSceneIndex()
    {
        if (SceneBuildNums.Count == 0)
        {
            InitializeSceneList();
        }
        int index = Random.Range(0, SceneBuildNums.Count);
        int rand = SceneBuildNums[index];
        SceneBuildNums.RemoveAt(index);
        return rand;
    }
}
