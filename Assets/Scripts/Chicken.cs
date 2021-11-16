using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Food  // INHERITANCE - Chicken inherits from the Food class
{
    private Renderer _myRenderer;

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();

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
