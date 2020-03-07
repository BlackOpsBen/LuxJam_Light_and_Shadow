using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private GameObject spawn;

    void Start()
    {
        spawn = GameObject.FindGameObjectWithTag("Spawn");
        transform.position = spawn.transform.position;
    }
}
