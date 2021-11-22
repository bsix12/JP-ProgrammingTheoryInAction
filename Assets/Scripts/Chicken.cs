using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Food  // INHERITANCE - Chicken inherits from the Food class
{
    private Renderer _myRenderer;
    private AudioSource _myAudioSource;

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
        _myAudioSource = GetComponent<AudioSource>();

        myStartTemp = 60f;
        myIsCookedTemp = 120f;
        myIsBurnedTemp = 200f;
        myTemp = myStartTemp;
    }

    protected override void Update()  // POLYMORPHISM - override method.  Each food type has unique color pallette
    {
        SetFoodColor();  // ABSTRACTION, color setting details organized into a separate method.
        base.Update();   // Run Update() of base class.
    }

    private void SetFoodColor()
    {
        myRawColor = new Color32(240, 200, 200, 255);
        myCookedColor = new Color32(250, 250, 200, 255);

        if (!isCooked && iAm != "Raw Chicken")
        {
            _myRenderer.material.color = myRawColor;
            iAm = "Raw Chicken";
        }

        if (isCooked && iAm != "Cooked Chicken" && !isBurned)
        {
            _myRenderer.material.color = myCookedColor;
            _myAudioSource.PlayOneShot(cookConditionIndicator, .1f);
            iAm = "Cooked Chicken";
            
        }

        if (isBurned && iAm != "Burned Chicken")
        {
            _myRenderer.material.color = isBurnedColor;
            iAm = "Burned Chicken";
        }
    }
}
