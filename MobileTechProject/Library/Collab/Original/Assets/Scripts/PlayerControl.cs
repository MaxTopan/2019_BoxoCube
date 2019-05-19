using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;
    private KeyCode nextGun = KeyCode.Period; // USED FOR TESTING
    public GunController gunCont;
    // Touch touch;
    public bool canShoot = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //touch = Input.GetTouch(0);

        if (Input.GetKeyUp(nextGun))
        {
            gunCont.NextGun();
        }

        if (Input.touchCount > 0 || Input.GetButton("Fire1"))
        {
            if (canShoot)
            {
                Vector2 aim = Input.mousePosition.x < Screen.width / 2 ? (transform.right + transform.up).normalized : (-transform.right + transform.up).normalized;  // if the input is on the left of the screen, shoot left and downwards at a 45 degree angle, vice versa for the right side of the screen
                Debug.Log(aim);
                gunCont.Shoot(aim * -4 * gunCont.FirePower);
                rb.AddForceAtPosition(aim * gunCont.FirePower, transform.position, ForceMode2D.Impulse);
            }
        }

    }
}
