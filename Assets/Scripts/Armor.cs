using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite armoredSprite;
    private GameManager gameManager;

    private bool isArmored = false;

    private void Start()
    {
        StartCoroutine(GetGameManager());
    }

    public bool GetIsArmored()
    {
        return isArmored;
    }

    public void SetIsArmored(bool value)
    {
        gameManager.SetIsArmored(value);
        isArmored = value;
        if (isArmored)
        {
            playerSprite.sprite = armoredSprite;
        }
        else
        {
            playerSprite.sprite = defaultSprite;
        }
    }

    private IEnumerator GetGameManager()
    {
        yield return null;
        gameManager = FindObjectOfType<GameManager>();
        SetIsArmored(gameManager.GetIsArmored());
    }
}
