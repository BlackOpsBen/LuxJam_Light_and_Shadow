using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Illuminate : MonoBehaviour
{
    private GameObject darkness;
    private float maxFuel = 3f;
    private float fuel = 3f;
    private float fuelConsumptionRate = 1f;
    private bool disabled = false;

    private void Start()
    {
        darkness = GameObject.Find("Darkness");
    }

    private void Update()
    {
        if (!disabled)
        {
            if (fuel > float.Epsilon && Input.GetButton("Light"))
            {
                darkness.SetActive(false);
                fuel -= fuelConsumptionRate * Time.deltaTime;
            }
            else
            {
                darkness.SetActive(true);
            }
        }
    }

    public void ResetFuel()
    {
        fuel = maxFuel;
    }

    public void SetDisabled(bool tOrF)
    {
        disabled = tOrF;
    }
}
