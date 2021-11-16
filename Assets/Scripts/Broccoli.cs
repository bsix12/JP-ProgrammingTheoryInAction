using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broccoli : Food  // INHERITANCE - Broccoli inherits from the Food class
{
    private Renderer _myRenderer;

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
               
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

        if (!isCooked)
        {
            _myRenderer.material.color = myRawColor;
        }

        if (isCooked)
        {
            _myRenderer.material.color = myCookedColor;
        }

        if (isBurned)
        {
            _myRenderer.material.color = isBurnedColor;
        }
    }
}