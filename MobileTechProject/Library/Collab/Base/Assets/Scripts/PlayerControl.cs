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

        //if (Input.GetButton("Fire1"))
        //{
        //    if (canShoot)
        //    {
        //        Vector2 aim = GetDir().normalized;

        //        //Vector2 aim = (Vector2)transform.position - tPos.normalized;

        //        gunCont.Shoot(aim * -4 * gunCont.FirePower);
        //        rb.AddForceAtPosition(aim * gunCont.FirePower, transform.position, ForceMode2D.Impulse);
        //    }
        //}

        if (Input.GetButton("Fire1"))
        {
            if (canShoot)
            {
                Vector2 aim = Input.mousePosition.x < Screen.width / 2 ? (transform.right + transform.up).normalized : (-transform.right + transform.up).normalized;  // (Vector2)transform.position - (Vector2.right + Vector2.down).normalized : (Vector2)transform.position - (Vector2.left + Vector2.down).normalized;
                Debug.Log(aim);
                gunCont.Shoot(aim * -4 * gunCont.FirePower);
                rb.AddForceAtPosition(aim * gunCont.FirePower, transform.position, ForceMode2D.Impulse);
            }
        }

    }

    Vector2 GetDir()
    {
        return (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
