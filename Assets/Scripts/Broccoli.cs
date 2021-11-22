using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broccoli : Food  // INHERITANCE - Broccoli inherits from the Food class
{
    private Renderer _myRenderer;
    private AudioSource _myAudioSource;

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
        _myAudioSource = GetComponent<AudioSource>();

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
        myRawColor = new Color32(75, 200, 50, 255);
        myCookedColor = new Color32(25, 140, 0, 225);

        if (!isCooked && iAm != "Raw Broccoli")
        {
            _myRenderer.material.color = myRawColor;
            iAm = "Raw Broccoli";
        }

        if (isCooked && iAm != "Steamed Broccoli" && !isBurned)
        {
            _myRenderer.material.color = myCookedColor;
            _myAudioSource.PlayOneShot(cookConditionIndicator, .1f);
            iAm = "Steamed Broccoli";
        }

        if (isBurned && iAm != "Burned Broccoli")
        {
            _myRenderer.material.color = isBurnedColor;
            iAm = "Burned Broccoli";
        }
    }
}