using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject, 1f);
    }
}
