using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        scoreText.text = gameManager.GetCoinCount().ToString();
    }
}
