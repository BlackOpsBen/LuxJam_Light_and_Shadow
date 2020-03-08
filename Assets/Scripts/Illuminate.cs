using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Illuminate : MonoBehaviour
{
    [SerializeField] GameObject igniteSound;
    [SerializeField] GameObject burnoutSound;
    [SerializeField] AudioSource flameSound;
    [SerializeField] GameObject eerieSound;
    private bool playedEerieSound = false;

    [SerializeField] Image torchSprite;
    [SerializeField] RectTransform fuelMeterFill;
    private float fuelMeterMaxSize = 32f;
    private float fuelMeterCurrentSize;

    private GameObject darkness;
    private float maxFuel = 3f;
    private float fuel = 3f;
    private float fuelConsumptionRate = 1f;
    private float fuelLightCost = 0.1f;
    private bool disabled = false;
    private bool isLit = false;

    private void Start()
    {
        darkness = GameObject.Find("Darkness");
    }

    private void Update()
    {
        GetUseTorch();
        UpdateFuelMeter();
        if (!playedEerieSound)
        {
            if (!(fuel > float.Epsilon))
            {
                Instantiate(eerieSound, transform.position, Quaternion.identity);
                playedEerieSound = true;
                Debug.Log("Empty fuel meter");
                fuelMeterCurrentSize = 0f;
                fuelMeterFill.localScale = new Vector3(fuelMeterCurrentSize, 1f, 1f);
            }
        }
    }

    private void GetUseTorch()
    {
        if (!disabled)
        {
            if (fuel > float.Epsilon && Input.GetButtonDown("Light"))
            {
                PlaySound(igniteSound);
                fuel = Mathf.Clamp(fuel - fuelLightCost, 0f, maxFuel);
                darkness.SetActive(false);
            }
            if (fuel > float.Epsilon && Input.GetButton("Light"))
            {
                if (!isLit)
                {
                    flameSound.Play();
                }
                isLit = true;
                torchSprite.enabled = true;
                fuel = Mathf.Clamp(fuel - fuelConsumptionRate * Time.deltaTime, 0f, maxFuel);
            }
            else
            {
                PutOutTorch();
            }
        }
        else
        {
            PutOutTorch();
        }
    }

    private void PutOutTorch()
    {
        if (isLit)
        {
            PlaySound(burnoutSound);
            flameSound.Stop();
        }
        isLit = false;
        darkness.SetActive(true);
        torchSprite.enabled = false;
    }

    private void UpdateFuelMeter()
    {
        if (!disabled)
        {
            fuelMeterCurrentSize = fuel * fuelMeterMaxSize / maxFuel;
            fuelMeterFill.localScale = new Vector3(fuelMeterCurrentSize, 1f, 1f);
        }
        else
        {
            fuelMeterCurrentSize = 0f;
            fuelMeterFill.localScale = new Vector3(fuelMeterCurrentSize, 1f, 1f);
        }
        
    }

    public void ResetFuel()
    {
        fuel = maxFuel;
        playedEerieSound = false;
    }

    public float GetRemainingFuel()
    {
        return fuel;
    }

    public void SetDisabled(bool tOrF)
    {
        disabled = tOrF;
    }

    public void PlaySound(GameObject sound)
    {
        Instantiate(sound, transform.position, Quaternion.identity);
    }
}
