using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves tutorial hazards to the right places dependant on ceiling and wall height
/// </summary>
public class TutorialHazards : MonoBehaviour
{
    private LevelGenerator lg;
    private float yPos;

    public bool onFloor;
    public bool wall;

    void Awake()
    {
        lg = FindObjectOfType<LevelGenerator>();
        if (!wall) // if the object is not a wall
        {
            yPos = onFloor ? -lg.CeilingHeight : lg.CeilingHeight; // return floor Y position if onFloor is true, ceiling otherwise
        }
        else
        {
            yPos = onFloor ? -lg.WallHazardPos : lg.WallHazardPos;
        }

        transform.position = new Vector2(transform.position.x, yPos);
    }
}
