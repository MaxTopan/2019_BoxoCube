using UnityEngine;

public class SpikeCol : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(collision.gameObject.GetComponent<PlayerControl>().PlayerDeath());
            //Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(collision.gameObject.GetComponent<PlayerControl>().PlayerDeath());
        }
    }
}
