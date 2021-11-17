using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : Food  // INHERITANCE - Carrot inherits from the Food class
{
    private Renderer _myRenderer;
    public string iAm;
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
        myRawColor = new Color32(237, 145, 33, 225);
        myCookedColor = new Color32(255, 126, 0, 255); 

        if (!isCooked)
        {
            _myRenderer.material.color = myRawColor;
            iAm = "Raw Carrots";
        }

        if (isCooked)
        {
            _myRenderer.material.color = myCookedColor;
            iAm = "Steamed Carrots";
        }

        if (isBurned)
        {
            _myRenderer.material.color = isBurnedColor;
        }
    }
}
