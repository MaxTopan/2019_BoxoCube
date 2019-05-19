using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    private float xPos, farX;
    public float yPos, zPos;
    [SerializeField]
    Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            xPos = target.position.x; // update player x position
            farX = farX < xPos ? xPos : farX;//, 0, Mathf.Infinity); // far x is the highest x has been, and not less than 0
            transform.position = new Vector3(farX, yPos, zPos);
        }
    }
}
