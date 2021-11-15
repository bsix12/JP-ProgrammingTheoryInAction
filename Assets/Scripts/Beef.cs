using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beef : Food
{
    public bool isCookedRare;
    public bool isCookedMedium;
    public bool isCookedWellDone;

    private Renderer _myRenderer;
    

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
        myRawColor = new Color32(225, 145, 145, 255);
        
        myStartTemp = 60f;
        myIsBurnedTemp = 200f;
        myTemp = myStartTemp;        
    }

    protected override void Update()
    {
        SetFoodColor();
        base.Update();
    }

    protected override void MonitorCookedCondition()
    {
        float _myIsRareTemp = 120f;
        float _myIsMediumTemp = 140f;
        float _myIsWellDoneTemp = 170f;
        

        if (isCookedWellDone && myTemp >= myIsBurnedTemp)   // remains isBurned even if temperature drops below myIsBurnedTemperature
        {
            isBurned = true;
            isCookedWellDone = false;
        }

        else if (isCookedMedium && myTemp >= _myIsWellDoneTemp)
        {
            isCookedWellDone = true;
            isCookedMedium = false;
        }
        else if (isCookedRare && myTemp >= _myIsMediumTemp)
        {
            isCookedMedium = true;
            isCookedRare = false;
        }
        else if (!isCooked && myTemp >= _myIsRareTemp)
        {
            isCookedRare = true;
            isCooked = true;
        }
    }

    private void SetFoodColor()
    {
        if (!isCooked)
        {
            _myRenderer.material.color = myRawColor;
        }

        if (isCookedRare)
        {
            _myRenderer.material.color = new Color32(245, 125, 100, 255);
        }

        if (isCookedMedium)
        {
            _myRenderer.material.color = new Color32(175, 100, 50, 255);
        }

        if (isCookedWellDone)
        {
            _myRenderer.material.color = new Color32(125, 75, 25, 255);
        }

        if (isBurned)
        {
            _myRenderer.material.color = isBurnedColor;
        }
    }
}
