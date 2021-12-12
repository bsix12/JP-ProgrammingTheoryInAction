using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beef : Food  // INHERITANCE - Beef inherits from the Food class
{
    public bool isCookedRare;
    public bool isCookedMedium;
    public bool isCookedWellDone;


    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        myAudioSource = GetComponent<AudioSource>();
        myRb = GetComponent<Rigidbody>();
        myBoxCollider = GetComponent<BoxCollider>();
         
        myStartTemp = 60f;
        myIsRuinedTemp = 200f;
        myTemp = myStartTemp;
    }


    protected override void MonitorCookedCondition()  // POLYMORPHISM - Beef has rare, medium, and well-done conditions
    {
        float _myIsRareTemp = 120f;
        float _myIsMediumTemp = 140f;
        float _myIsWellDoneTemp = 170f;
        

        if (isCookedWellDone && myTemp >= myIsRuinedTemp)
        {
            isRuined = true;
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

    protected override void SetFoodGameObjectProperties()
    {
        myRawColor = new Color32(225, 145, 145, 255);        
        Color32 myRareColor = new Color32(245, 125, 100, 255);
        Color32 myMediumColor = new Color32(175, 100, 50, 255);
        Color32 myWellDoneColor = new Color32(125, 75, 25, 255);

        if (!isCooked && iAm != "Raw Beef")
        {
            myRenderer.material.color = myRawColor;
            myCurrentColor = myRawColor;
            iAm = "Raw Beef";
        }

        if (isCookedRare && iAm != "Beef: Rare" && !isCookedMedium && !isCookedWellDone && !isRuined)
        {
            myRenderer.material.color = myRareColor;
            myCurrentColor = myRareColor;
            myAudioSource.PlayOneShot(cookConditionIndicator, 5f);
            iAm = "Beef: Rare";
        }

        if (isCookedMedium && iAm != "Beef: Medium" && !isCookedRare && !isCookedWellDone && !isRuined)
        {
            myRenderer.material.color = myMediumColor;
            myCurrentColor = myMediumColor;
            myAudioSource.PlayOneShot(cookConditionIndicator, 5f);
            iAm = "Beef: Medium";
        }

        if (isCookedWellDone && iAm != "Beef: Well-Done" && !isCookedRare && !isCookedMedium && !isRuined)
        {
            myRenderer.material.color = myWellDoneColor;
            myCurrentColor = myWellDoneColor;
            myAudioSource.PlayOneShot(cookConditionIndicator, 5f);
            iAm = "Beef: Well-Done";
        }

        if (isRuined && iAm != "Ruined Beef")
        {
            myRenderer.material.color = isRuinedColor;
            myCurrentColor = isRuinedColor;
            iAm = "Ruined Beef";
        }
    }

    protected override void OnTriggerEnter(Collider other) // POLYMORPHISM - override method.  
    {
        if (other.gameObject.CompareTag("Grill"))
        {
            myAudioSource.Play(); // Apply 'sizzleSound' for meats only && on grill only
        }

        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other) // POLYMORPHISM - override method.
    {
        if (other.gameObject.CompareTag("Grill"))
        {
            myAudioSource.Stop(); // Apply 'sizzleSound' for meats only && on grill only
        }
        
        base.OnTriggerExit(other);        
    }
}
