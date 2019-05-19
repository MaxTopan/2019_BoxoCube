using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "MainCamera")
        {
            Destroy(gameObject);
        }
    }
}
