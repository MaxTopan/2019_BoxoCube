using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    [SerializeField]
    private FuelManager fm; // stored reference to Fuel Manager
    [SerializeField]
    private int fuelUp = 50; // amount the player gains back upon refuelling

    private float ceilingBound;

    /// <summary>
    /// Assign Fuel Manager, maximium Y bounds for spawning, rigidbody.
    /// </summary>
    public void Initialise(FuelManager fuelManager, float cb)
    {
        fm = fuelManager;
        ceilingBound = cb;
        StartCoroutine(CheckSpawnPos());
    }

    /// <summary>
    /// Check to see if the pickup spawned inside another object; if it did, move it and check again.
    /// </summary>
    private IEnumerator CheckSpawnPos()
    {
        if (Physics2D.OverlapBox(transform.position, new Vector2(0.4f, 0.4f), 0))
        {
            yield return new WaitForEndOfFrame();

            Debug.Log("Respawning Fuel Pickup from: " + transform.position.x);
            float x = Random.Range(-5f, 5f);
            float y = Random.Range(-ceilingBound, ceilingBound);

            transform.position = new Vector3(transform.position.x + x, y);
            Debug.Log("New pos: " + transform.position.x);
            
            CheckSpawnPos();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player") // when the player collides with this object
        {
            fm.Refuel(fuelUp); // give the player fuelUp amount of fuel
            // spawn a fuelup indicator
            Destroy(gameObject); // destroy this object
        }
    }
}
