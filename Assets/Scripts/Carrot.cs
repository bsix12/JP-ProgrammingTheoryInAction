using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : Food  // INHERITANCE - Carrot inherits from the Food class
{
    private Renderer _myRenderer;
    private AudioSource _myAudioSource;



    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
        _myAudioSource = GetComponent<AudioSource>();
        myRb = GetComponent<Rigidbody>();
        myBoxCollider = GetComponent<BoxCollider>();

        myStartTemp = 57f;
        myIsCookedTemp = 110f;
        myIsBurnedTemp = 160f;
        myTemp = myStartTemp;
    }

    protected override void Update()  // POLYMORPHISM - override method.  Each food type has unique color pallette
    {
        SetFoodColor();  // ABSTRACTION, color setting details organized into a separate method.
        base.Update();   // Run Update() of base class.
    }

    private void SetFoodColor()
    {
        myRawColor = new Color32(237, 145, 33, 255);
        myCookedColor = new Color32(255, 126, 0, 255); 

        if (!isCooked && iAm != "Raw Carrots")
        {
            _myRenderer.material.color = myRawColor;
            myCurrentColor = myRawColor;
            iAm = "Raw Carrots";
        }

        if (isCooked && iAm != "Steamed Carrots" && !isBurned)
        {
            _myRenderer.material.color = myCookedColor;
            myCurrentColor = myCookedColor;
            _myAudioSource.PlayOneShot(cookConditionIndicator, 5f);
            iAm = "Steamed Carrots";
        }

        if (isBurned && iAm != "Burned Carrots")
        {
            _myRenderer.material.color = isBurnedColor;
            myCurrentColor = isBurnedColor;
            iAm = "Burned Carrots";
        }
    }
}
