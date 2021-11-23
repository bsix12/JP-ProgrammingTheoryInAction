using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beef : Food  // INHERITANCE - Beef inherits from the Food class
{
    public bool isCookedRare;
    public bool isCookedMedium;
    public bool isCookedWellDone;

    private Renderer _myRenderer;
    private AudioSource _myAudioSource;
    

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();
        _myAudioSource = GetComponent<AudioSource>();
         
        myStartTemp = 60f;
        myIsBurnedTemp = 200f;
        myTemp = myStartTemp;        
    }

    protected override void Update()  // POLYMORPHISM - override method.  Each food type has unique color pallette
    {
        SetFoodColor();  // ABSTRACTION - method name indicates Update() action, details in separate method
        base.Update();   // Run Update() of base class.
    }

    protected override void MonitorCookedCondition()  // POLYMORPHISM - Beef has rare, medium, and well-done conditions
    {
        float _myIsRareTemp = 120f;
        float _myIsMediumTemp = 140f;
        float _myIsWellDoneTemp = 170f;
        

        if (isCookedWellDone && myTemp >= myIsBurnedTemp)
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
        else if (!isCooked && myTemp >= _myIsRareTemp) // anything below rare is considered not cooked (!isCooked)
        {
            isCookedRare = true;
            isCooked = true;
        }
    }

    private void SetFoodColor()
    {
        myRawColor = new Color32(225, 145, 145, 255);


        if (!isCooked && iAm != "Raw Beef")
        {
            _myRenderer.material.color = myRawColor;
            iAm = "Raw Beef";
        }

        if (isCookedRare && iAm != "Beef: Rare" && !isCookedMedium && !isCookedWellDone && !isBurned)
        {
            _myRenderer.material.color = new Color32(245, 125, 100, 255);
            _myAudioSource.PlayOneShot(cookConditionIndicator, .5f);
            iAm = "Beef: Rare";
        }

        if (isCookedMedium && iAm != "Beef: Medium" && !isCookedRare && !isCookedWellDone && !isBurned)
        {
            _myRenderer.material.color = new Color32(175, 100, 50, 255);
            _myAudioSource.PlayOneShot(cookConditionIndicator, .5f);
            iAm = "Beef: Medium";
        }

        if (isCookedWellDone && iAm != "Beef: Well-Done" && !isCookedRare && !isCookedMedium && !isBurned)
        {
            _myRenderer.material.color = new Color32(125, 75, 25, 255);
            _myAudioSource.PlayOneShot(cookConditionIndicator, .5f);
            iAm = "Beef: Well-Done";
        }

        if (isBurned && iAm != "Burned Beef")
        {
            _myRenderer.material.color = isBurnedColor;
            iAm = "Burned Beef";
        }
    }

    protected override void OnTriggerEnter(Collider other) // POLYMORPHISM - override method.  
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
