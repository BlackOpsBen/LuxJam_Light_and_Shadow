using UnityEngine;

public class PersistNoDupesByTag : MonoBehaviour
{
    private void Awake()
    {
        if (this.GetComponent<GameManager>())
        {
            Debug.Log("Awake called on " + this.name);
        }
        
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
