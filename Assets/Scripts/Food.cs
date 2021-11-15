using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float _myTemp; // backing field for myTemp
    [SerializeField] protected bool isCooking;
    [SerializeField] protected bool isCooked;
    [SerializeField] protected bool isBurned;
    
    protected float myTemp {get{ return _myTemp;} set{_myTemp = value;}} // accessible property
    protected float myStartTemp;
    protected float myIsBurnedTemp;
    protected float myIsCookedTemp;

    protected Color32 myRawColor;
    protected Color32 myCookedColor;
    protected Color32 myCurrentColor;
    protected Color32 isBurnedColor = new Color32(25, 25, 0, 255);

    protected virtual void Update()
    {
        UpdateFoodTemperature();
        MonitorCookedCondition();
    }

    private void UpdateFoodTemperature()
    {
        if (isCooking)
        {
            _myTemp += Time.deltaTime * 10; // cooking at some heatRate
        }
        else
        {
            _myTemp -= Time.deltaTime * .1f; // cooling at some coolRate
        }
    }

    protected virtual void MonitorCookedCondition()
    {
        if (!isBurned && _myTemp >= myIsBurnedTemp)
        {
            isBurned = true;
        }
        else if (!isCooked && _myTemp >= myIsCookedTemp)
        {
            isCooked = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CookStation"))
        {
            isCooking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CookStation"))
        {
            isCooking = false;
        }
    }


    private void OnCollisionEnter(Collision other) // example: attaches to plate when dispensed
    {
        if(other.gameObject.CompareTag("Player"))
        {
            transform.parent = other.transform;
        }
        
    }

    private void OnCollisionExit(Collision other) // example: stays on floor if falls off plate
    {
        transform.parent = null;
    }


}

