using UnityEngine;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
    void Update()
    {
        Snap();
    }

    private void Snap()
    {
        transform.position = new Vector3(
            Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.y),
            0f
            );
    }
}
