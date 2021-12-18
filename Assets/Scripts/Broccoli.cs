using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broccoli : Food  // INHERITANCE - Broccoli inherits from the Food class
{


    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        myAudioSource = GetComponent<AudioSource>();
        myRb = GetComponent<Rigidbody>();
        myBoxCollider = GetComponent<BoxCollider>();

        myStartTemp = 57f;
        myIsCookedTemp = 110f;
        myIsRuinedTemp = 160f;
        myTemp = myStartTemp;

        myRawColor = new Color32(75, 200, 50, 255);
        myCookedColor = new Color32(25, 140, 0, 225);

        myRawName = "Raw Broccoli";
        myCookedName = "Steamed Broccoli";
        myRuinedName = "Ruined Broccoli";
    }


    protected override void OnTriggerEnter(Collider other) // POLYMORPHISM - override method.  
    {
        if (other.gameObject.CompareTag("Grill"))
        {
            isOnWrongCookStation = true;
        }

        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other) // POLYMORPHISM - override method.  
    {
        if (other.gameObject.CompareTag("Grill"))
        {
            isOnWrongCookStation = false;
        }

        base.OnTriggerExit(other);
    }
}

