using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRandomizer : MonoBehaviour
{
    List<int> SceneBuildNums = new List<int>();
    private int playedScenes = 0;
    private int scenesSinceBonus = 0;
    private int bonusScenesInBuildSettings = 7; // This needs to be manually set to the number of bonus scenes I have included in build settings.
    private int numBonusScenes;
    private int arbitraryNumber = 3;
    private bool bonusIsEligible = false;

    private void Start()
    {
        InitializeSceneList();
    }

    public void InitializeSceneList()
    {
        playedScenes = 0;
        SceneBuildNums = new List<int>();
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings - 1; i++)
        {
            SceneBuildNums.Add(i);
        }
        numBonusScenes = bonusScenesInBuildSettings;
    }

    public int GetRandomSceneIndex()
    {
        if (SceneBuildNums.Count == 0)
        {
            InitializeSceneList();
        }

        int numToStartFrom;

        if (playedScenes < arbitraryNumber || scenesSinceBonus < arbitraryNumber)
        {
            bonusIsEligible = false;
        }
        else
        {
            bonusIsEligible = true;
        }

        if (SceneBuildNums.Count <= arbitraryNumber)
        {
            bonusIsEligible = true;
        }

        if (bonusIsEligible)
        {
            numToStartFrom = 0;
        }
        else
        {
            numToStartFrom = numBonusScenes;
        }

        int index = Random.Range(numToStartFrom, SceneBuildNums.Count);
        int rand = SceneBuildNums[index];
        SceneBuildNums.RemoveAt(index);

        if (index < numBonusScenes)
        {
            numBonusScenes--;
            scenesSinceBonus = 0;
        }
        else
        {
            scenesSinceBonus++;
        }

        playedScenes++;
        
        return rand;
    }

    public int GetPlayedScenes()
    {
        return playedScenes;
    }
}
