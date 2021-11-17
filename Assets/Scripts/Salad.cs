using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salad : Food
{
    private Renderer _myRenderer;
    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();

        myStartTemp = 57f;
        myIsBurnedTemp = 70f;  // wilted
        myTemp = myStartTemp;
    }
    protected override void Update()  // POLYMORPHISM - override method.  Each food type has unique color pallette
    {
        SetFoodColor();  // ABSTRACTION, color setting details organized into a separate method.
        base.Update();   // Run Update() of base class.
    }

    private void SetFoodColor()
    {
        myRawColor = new Color32(150, 210, 115, 225);

        if (!isCooked)
        {
            _myRenderer.material.color = myRawColor;
        }

        if (isBurned)
        {
            _myRenderer.material.color = isBurnedColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBurned && other.CompareTag("CookStation"))
        {
            isBurned = true;
        }
    }
}