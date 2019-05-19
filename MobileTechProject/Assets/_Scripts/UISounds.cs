using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    public AudioSource audioS;

    void PlayClip(AudioClip clip)
    {
        audioS.PlayOneShot(clip);
    }
}
