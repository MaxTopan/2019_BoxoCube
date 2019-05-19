using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardProximityDetector : MonoBehaviour
{
    MeshRenderer rend;
    private List<Spike> childSpikes;
    private bool timerStarted = false;

    private void Awake()
    {
        //rend = gameObject.AddComponent<Renderer>();
    }

    /// <summary>
    /// Look at each Spike script this go has as a child, and add them to a list.
    /// </summary>
    public void SetChildSpikes()
    {
        rend = gameObject.AddComponent<MeshRenderer>();
        childSpikes = new List<Spike>();

        foreach (Spike s in GetComponentsInChildren<Spike>())
        {
            childSpikes.Add(s);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerStarted && rend.isVisible)
        {
            for (int i = 0; i < childSpikes.Count; i++)
            {
                StartCoroutine(childSpikes[i].StartTimer(childSpikes[i].Delay));
            }
            timerStarted = true;
        }

        // once the spikes timers have all been called, and the spikes have destroyed themselves, destroy this
        //if (timerStarted && transform.childCount == 0)
        //{
        //    Destroy(gameObject);
        //}
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
