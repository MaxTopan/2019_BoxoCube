using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    private Vector2 minPos;
    private Vector2 maxPos;
    private float moveDuration;
    private float pauseTime;

    private bool moving = true;

    public void Initialise(float _minY, float _maxY, float _moveDuration)
    {
        minPos = new Vector2(transform.position.x, _minY);
        maxPos = new Vector2(transform.position.x, _maxY);

        moveDuration = _moveDuration;
        pauseTime = moveDuration * 2;

        moving = false;
    }

    private void Update()
    {
        if (!moving)
        {
            StartCoroutine(Move(minPos, maxPos, moveDuration));
        }
    }

    private IEnumerator Move(Vector2 aPos, Vector2 bPos, float moveTime)
    {
        moving = true;
        float timeTaken = 0;

        while (timeTaken <= moveTime)
        {
            timeTaken += Time.deltaTime;
            float percent = Mathf.Clamp01(timeTaken / moveTime);

            transform.position = Vector2.Lerp(aPos, bPos, percent);

            yield return null;
        }
        transform.position = bPos;

        minPos = bPos;
        maxPos = aPos;

        yield return new WaitForSeconds(pauseTime);
        moving = false;
    }
}
