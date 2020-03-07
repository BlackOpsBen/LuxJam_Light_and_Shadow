using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponentInParent<PlayerMovement>().TriggerDetected(this, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObject.GetComponentInParent<PlayerMovement>().TriggerReset(this);
    }
}
