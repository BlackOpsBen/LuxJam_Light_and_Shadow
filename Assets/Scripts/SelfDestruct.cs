using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float deathTime = 1f;
    void Awake()
    {
        Destroy(gameObject, deathTime);
    }
}
