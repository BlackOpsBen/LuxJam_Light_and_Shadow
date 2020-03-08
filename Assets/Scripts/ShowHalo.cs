using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHalo : MonoBehaviour
{
    [SerializeField] GameObject halo;

    private void Start()
    {
        Instantiate(halo, transform.position, Quaternion.identity);
    }
}
