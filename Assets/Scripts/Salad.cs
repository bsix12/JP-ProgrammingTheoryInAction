using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salad : Food
{
  
    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        myAudioSource = GetComponent<AudioSource>();
        myRb = GetComponent<Rigidbody>();
        myBoxCollider = GetComponent<BoxCollider>();
        
        myStartTemp = 57f;
        myIsCookedTemp = 57f;
        myIsRuinedTemp = 70f;  // wilted
        myTemp = myStartTemp;

        myRawColor = new Color32(150, 210, 115, 255);
        myCookedColor = myRawColor;
        
        myRawName = "Salad";
        myRuinedName = "Ruined Salad";
    }

    protected override void SetFoodGameObjectProperties()
    {
        if (iAm != myRawName && iAm != myRuinedName)
        {
            myRenderer.material.color = myRawColor;
            myCurrentColor = myRawColor;
            iAm = myRawName;
        }

        if (isRuined && iAm != myRuinedName)
        {
            myRenderer.material.color = isRuinedColor;
            myCurrentColor = isRuinedColor;
            myAudioSource.PlayOneShot(GameManager.Instance.negativeDeliveryAudio, .2f);
            ClearCookIndicators();
            Instantiate(indicatorRuinedPrefab, transform);
            iAm = myRuinedName;
        }
    }


    protected override void OnTriggerEnter(Collider other) // POLYMORPHISM - override method
    {
        if (!isRuined && (other.CompareTag("Grill") || (other.CompareTag("Steamer"))))
        {
            isRuined = true;
        }
        base.OnTriggerEnter(other);
    }    
}