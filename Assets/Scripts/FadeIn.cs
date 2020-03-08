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
    private float maxAlpha = 1f;
    private float r;
    private float g;
    private float b;

    private void Start()
    {
        r = text1.color.r;
        g = text1.color.g;
        b = text1.color.b;
        StartCoroutine(FadeAlpha());
    }

    private void SetAlpha(float alpha)
    {
        image.color = new Color(0f, 0f, 0f, alpha);
        text1.color = new Color(r, g, b, alpha);
        text2.color = new Color(r, g, b, alpha);
    }

    private IEnumerator FadeAlpha()
    {
        float alpha = 0f;
        while (alpha < maxAlpha)
        {
            SetAlpha(alpha);
            alpha += maxAlpha * 0.5f * Time.deltaTime;
            yield return null;
        }
    }
}
