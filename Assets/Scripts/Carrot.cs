using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : Food  // INHERITANCE - Carrot inherits from the Food class
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

        myRawColor = new Color32(237, 145, 33, 255);
        myCookedColor = new Color32(255, 126, 0, 255);

        myRawName = "Raw Carrots";
        myCookedName = "Steamed Carrots";
        myRuinedName = "Ruined Carrots";
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
