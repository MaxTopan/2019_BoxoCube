using UnityEngine;

public class OffscreenCleanup : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
