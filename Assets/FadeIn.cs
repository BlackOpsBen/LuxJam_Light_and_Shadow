using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeIn : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text1;
    [SerializeField] TextMeshProUGUI text2;
    [SerializeField] int maxAlpha = 255;

    private void Start()
    {
        StartCoroutine(FadeAlpha());
    }

    private IEnumerator FadeAlpha()
    {
        float alpha = 0f;
        while (alpha < 255)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            Debug.Log("Alpha: " + alpha);
            alpha += maxAlpha * Time.deltaTime;
            yield return null;
        }
    }
}
