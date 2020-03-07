using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Illuminate : MonoBehaviour
{
    [SerializeField] GameObject igniteSound;
    [SerializeField] GameObject burnoutSound;
    [SerializeField] AudioSource flameSound;

    [SerializeField] Image torchSprite;
    [SerializeField] RectTransform fuelMeterFill;
    private float fuelMeterMaxSize = 32f;
    private float fuelMeterCurrentSize;

    private GameObject darkness;
    private float maxFuel = 3f;
    private float fuel = 3f;
    private float fuelConsumptionRate = 1f;
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
    }

    private void GetUseTorch()
    {
        if (!disabled)
        {
            if (fuel > float.Epsilon && Input.GetButtonDown("Light"))
            {
                PlaySound(igniteSound);
            }
            if (fuel > float.Epsilon && Input.GetButton("Light"))
            {
                if (!isLit)
                {
                    flameSound.Play();
                }
                isLit = true;
                darkness.SetActive(false);
                torchSprite.enabled = true;
                fuel -= fuelConsumptionRate * Time.deltaTime;
            }
            else
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
        }
    }

    private void UpdateFuelMeter()
    {
        fuelMeterCurrentSize = fuel * fuelMeterMaxSize / maxFuel;
        fuelMeterFill.localScale = new Vector3(fuelMeterCurrentSize, 1f, 1f);
    }

    public void ResetFuel()
    {
        fuel = maxFuel;
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
