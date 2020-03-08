using UnityEngine;

public class PersistNoDupesByTag : MonoBehaviour
{
    private void Awake()
    {
        int numObjects = GameObject.FindGameObjectsWithTag(gameObject.tag).Length;
        if (numObjects > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
