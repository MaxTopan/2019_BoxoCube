using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField]
    private bool shooting = false; // is the spike going across the screen?

    [SerializeField]
    private float delay;
    public float Delay { get { return delay; } set { delay = value; } }

    public float Speed { get; set; }
    public Transform PlayerTrans { get; set; }
    float destroyTimeDelay = 3;
    Animator anim;
    Rigidbody2D rb;
    public bool tutSpike = false;
    private AudioSource source;
  

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        Speed = 4;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (tutSpike == true)
        {
            StartCoroutine(StartTimer(delay));
        }
    }

    public void Initialize(float _delay, float _speed, Transform pt)
    {
        delay = _delay;
        Speed = _speed;
        PlayerTrans = pt;
    }

    private void FixedUpdate()
    {
        if (shooting)
        {
            rb.velocity = transform.up * Speed;
        }
    }

    public IEnumerator StartTimer(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        anim.SetBool("flashing", true);
        yield return new WaitForSeconds(0.8f);
        PlayAudio();
        shooting = true;
        anim.SetBool("flashing", false);
        yield return new WaitForSeconds(destroyTimeDelay);
        Destroy(gameObject);
    }

    void PlayAudio()
    {
        source.pitch = Random.Range(0.8f, 1.2f);
        source.Play();
    }
}