using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelsText;
    private const string levelsTextPrefix = "Survived to level ";
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        scoreText.text = gameManager.GetCoinCount().ToString();
        levelsText.text = levelsTextPrefix + FindObjectOfType<SceneRandomizer>().GetPlayedScenes();
    }
}
