using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{
    [SerializeField] GameObject[] itemOptions;

    private void Start()
    {
        int rand = Random.Range(0, itemOptions.Length);
        Instantiate(itemOptions[rand], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
