using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Food  // INHERITANCE - Chicken inherits from the Food class
{

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        myAudioSource = GetComponent<AudioSource>();
        myRb = GetComponent<Rigidbody>();
        myBoxCollider = GetComponent<BoxCollider>();

        myStartTemp = 60f;
        myIsCookedTemp = 120f;
        myIsRuinedTemp = 200f;
        myTemp = myStartTemp;

        myRawColor = new Color32(240, 200, 200, 255);
        myCookedColor = new Color32(250, 250, 200, 255);

        myRawName = "Raw Chicken";
        myCookedName = "Cooked Chicken";
        myRuinedName = "Ruined Chicken";

    }


    protected override void OnTriggerEnter(Collider other) // POLYMORPHISM - override method.  
    {
        if (other.gameObject.CompareTag("Steamer"))
        {
            isOnWrongCookStation = true;
        }

        if (other.gameObject.CompareTag("Grill"))
        {
            myAudioSource.Play(); // Apply 'sizzleSound' for meats only && on grill only
        }

        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other) // POLYMORPHISM - override method.
    {
        if (other.gameObject.CompareTag("Steamer"))
        {
            isOnWrongCookStation = false;
        }

        if (other.gameObject.CompareTag("Grill"))
        {
            myAudioSource.Stop(); // Apply 'sizzleSound' for meats only && on grill only
        }

        base.OnTriggerExit(other);
    }
}

