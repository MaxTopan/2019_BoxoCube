using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //////FOR TESTING//////
    DataCollector dc;
    //////FOR TESTING//////

    Rigidbody2D rb;
    // Touch touch;
    private bool canShoot;
    KeyCode left = KeyCode.A, right = KeyCode.D;

    private float shotDelayTimer = 0.135f; // time elapsed between each 'shot'
    private float firePower = 4f; // impulse power of each shot
    private float bulletSpeed = 4f; // speed of bullet
    public GameObject bullet; // game object fired out of player
    public GameObject explosion;
    float screenWidth;
    private Vector2 rightShot, leftShot; // directions for the character to be propelled by either shooting left or right
    private AudioSource shootSource;
    private StatsSaver stats;
    private SpriteRenderer sr;

    [SerializeField]
    private FuelManager fm;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        stats = FindObjectOfType<StatsSaver>();
        dc = GameObject.FindGameObjectWithTag("DataCollector").GetComponent<DataCollector>();

        canShoot = true;

        shootSource = GetComponent<AudioSource>();

        // initialise propulsion directions
        rightShot = (transform.right + (transform.up * 1.17f)).normalized; // 49.4794 degrees
        leftShot = ((-transform.right * 1.5f) + transform.up).normalized; // 146.31 degrees

        screenWidth = Screen.width; // cache screen width
        rb = GetComponent<Rigidbody2D>(); // cache rigidbody2D
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fm.Fuel <= 0) // player cannot shoot once they run out of fuel
        {
            canShoot = false;
        }

        //#if UNITY_ANDROID
        if (Input.touchCount > 0 || Input.GetButton("Fire1"))
        {
            if (canShoot)
            {
                Vector2 aim = Input.mousePosition.x < screenWidth / 2 ? rightShot : leftShot; // if the player is touching the left side of the screen, set aim to right shot, and vice versa
                Shoot(aim); // propel player in direction of aim
            }
        }
        //#elif UNITY_STANDALONE
        if (Input.GetKey(right) && canShoot) // travel right
        {
            Shoot(rightShot); // propel player in direction of rightShot
#if UNITY_EDITOR
            dc.FuelUsed(true); // tracks fuel used
#endif
        }
        if (Input.GetKey(left) && canShoot) // travel left
        {
            Shoot(leftShot); // propel player in direction of leftShot
#if UNITY_EDITOR
            dc.FuelUsed(false); // tracks fuel used
#endif
        }
        //#endif
    }

    public void Shoot(Vector2 aim)
    {
        GameObject bul = Instantiate(bullet, gameObject.transform); // create and store bullet
        rb.AddForceAtPosition(aim * firePower, transform.position, ForceMode2D.Impulse); // propel the player in the direction of aim 
        bul.GetComponent<Rigidbody2D>().velocity = aim * -bulletSpeed * firePower; // propel the bullet in the opposite direction to the player
        fm.UseFuel(); // use a unit of fuel
        PlaySound(shootSource);
        StartCoroutine(ShotDelay()); // set canShoot to false for a few frames
    }

    /// <summary>
    /// set canShoot to false for shotDelayTimer seconds.
    /// </summary>
    public IEnumerator ShotDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shotDelayTimer);
        canShoot = true;
    }

    private void PlaySound(AudioSource source)
    {
        source.pitch = Random.Range(0.8f, 1.2f);
        source.Play();
    }

    private bool deathCalled = false;
    public IEnumerator PlayerDeath()
    {
        if (!deathCalled)
        {
            deathCalled = true;
            canShoot = false;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            GameObject exp = Instantiate(explosion);
            exp.transform.position = transform.position;

            sr.enabled = false;
            this.enabled = false;
            yield return new WaitForSeconds(0.6f);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        

        Debug.Log("DEAD");
    }
}