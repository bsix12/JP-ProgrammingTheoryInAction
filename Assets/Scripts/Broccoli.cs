using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broccoli : Food
{
    private Renderer _myRenderer;

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
        myRawColor = new Color32(75, 200, 50, 255);
        myCookedColor = new Color32(25, 140, 0, 225);
       
        myStartTemp = 57f;
        myIsCookedTemp = 110f;
        myIsBurnedTemp = 160f;
        myTemp = myStartTemp;
    }

    protected override void Update()
    {
        SetFoodColor();
        base.Update();
    }

    private void SetFoodColor()
    {
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