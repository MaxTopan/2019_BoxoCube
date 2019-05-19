using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves walls to edges of screen
public class WallBounds : MonoBehaviour
{
    private Transform[] walls;
    private float wallWidth = 0.8f;
    public float Bound { get; private set; } // reference point for spawning hazards

    // Use this for initialization
    void Awake()
    {
        walls = GetComponentsInChildren<Transform>(); // 0 = Camera, 1 = Ceiling, 3 = Floor, 5 = LeftWall
        wallWidth = walls[1].GetComponent<SpriteRenderer>().bounds.size.y; // stores the width of the ceilings

        //Debug.Log("1: " + walls[1].gameObject.name);
        //Debug.Log("3: " + walls[3].gameObject.name);
        //Debug.Log("5: " + walls[5].gameObject.name);

        // set position of ceiling
        Vector3 ceilingPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 1));
        ceilingPos += new Vector3(0, -wallWidth / 2, 0);

        // set position of floor
        Vector3 floorPos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 1));
        floorPos += new Vector3(0, wallWidth / 2, 0);

        // set position of left wall
        Vector3 wallPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 1));
        wallPos -= new Vector3(wallWidth / 2, 0, 0);

        // set height for hazards to spawn at
        Bound = ceilingPos.y - wallWidth / 2;
        Debug.Log(Bound);

        // Ceiling
        walls[1].position = ceilingPos;

        // Floor
        walls[3].position = floorPos;

        // Left Wall
        walls[5].position = wallPos;
    }

    /// <summary>
    /// Make the perimiter fatal to touch.
    /// </summary>
    public IEnumerator DeadlyWalls()
    {
        walls[1].GetComponent<Animator>().SetBool("flashing", true);
        walls[3].GetComponent<Animator>().SetBool("flashing", true);
        walls[5].GetComponent<Animator>().SetBool("flashing", true);

        yield return new WaitForSeconds(0.8f);

        walls[1].gameObject.AddComponent<SpikeCol>();
        walls[3].gameObject.AddComponent<SpikeCol>();
        walls[5].gameObject.AddComponent<SpikeCol>();
    }
}
