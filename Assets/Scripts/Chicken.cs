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
            Debug.Log(iAm);
        }

        if (isCooked && iAm != "Cooked Chicken" && !isBurned)
        {
            _myRenderer.material.color = myCookedColor;
            _myAudioSource.PlayOneShot(cookConditionIndicator, .5f);
            iAm = "Cooked Chicken";            
        }

        if (isBurned && iAm != "Burned Chicken")
        {
            _myRenderer.material.color = isBurnedColor;
            iAm = "Burned Chicken";
        }
    }

    protected override void OnTriggerEnter(Collider other) // POLYMORPHISM - override method.  Meats sizzle on grill.
    {
        if (other.gameObject.CompareTag("Grill"))
        {
            _myAudioSource.Play(); // Apply 'sizzleSound' for meats only && on grill only
        }
                
        base.OnTriggerEnter(other);       
    }

    protected override void OnTriggerExit(Collider other) // POLYMORPHISM - override method.
    {
        if (other.gameObject.CompareTag("Grill"))
        {
            _myAudioSource.Stop(); // Apply 'sizzleSound' for meats only && on grill only
        }
        
        base.OnTriggerExit(other);
    }
}
