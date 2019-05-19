using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FuelManager : MonoBehaviour
{
    public int Fuel { get; private set; } // current amount of fuel player has
    [SerializeField]
    private int fuelMax; // maximum fuel player can have
    public Text fuelText; // display text for fuel amount
    private Animator fuelTextAnim;
    public PlayerControl pc;

    private void Awake()
    {
        fuelTextAnim = fuelText.gameObject.GetComponent<Animator>();
        Fuel = fuelMax; // give player maximum fuel
        UpdateFuelText(); // update fuel display
    }
    
    /// <summary>
    /// Reduces fuel count by 1.
    /// </summary>
    public void UseFuel()
    {
        Fuel--;

        if (Fuel == 0)
        {
            StartCoroutine(OutOfFuel());
        }

        UpdateFuelText();
    }

    /// <summary>
    /// Increases the player's fuel count.
    /// </summary>
    /// <param name="fuelUp">Amount to refuel by.</param>
    public void Refuel(int fuelUp)
    {
        if (Fuel == 0) { StartCoroutine(pc.ShotDelay()); } // prevent canShoot getting stuck on false
        Fuel = Fuel + fuelUp > fuelMax ? fuelMax : Fuel + fuelUp; // if the player's fuel count would go over max, set it to max
        UpdateFuelText();
        fuelTextAnim.Play("FuelTextUpdate");
    }

    /// <summary>
    /// Updates display text to show how much fuel the player has.
    /// </summary>
    public void UpdateFuelText()
    {
        fuelText.text = "FUEL: " + Fuel;
    }

    private IEnumerator OutOfFuel()
    {
        yield return new WaitForSeconds(2f);
        if (Fuel == 0)
        {
            StartCoroutine(pc.PlayerDeath());
        }
    }
}
