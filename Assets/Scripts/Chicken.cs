using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Food
{
    private Renderer _myRenderer;

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
        myRawColor = new Color32(240, 200, 200, 255);
        myCookedColor = new Color32(250, 250, 200, 255);
        myStartTemp = 60f;
        myIsCookedTemp = 120f;
        myIsBurnedTemp = 200f;
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
