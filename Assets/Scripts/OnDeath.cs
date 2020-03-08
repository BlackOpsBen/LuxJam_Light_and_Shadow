using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeath : MonoBehaviour
{
    [SerializeField] GameObject onDeathSound;
    public bool isBeingKilled = false;
    private void OnDestroy()
    {
        if (isBeingKilled)
        {
            Instantiate(onDeathSound, transform.position, Quaternion.identity);
        }
    }
}
