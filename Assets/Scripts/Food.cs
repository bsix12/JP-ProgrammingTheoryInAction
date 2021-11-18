using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float _myTemp; // ENCAPSULATION - backing field for myTemp.  Treating the food temperature like a 'health bar' that is not directly accessed
    [SerializeField] protected bool isCooking;
    [SerializeField] protected bool isCooked;
    [SerializeField] protected bool isBurned;
    
    protected float myTemp {get{ return _myTemp;} set{_myTemp = value;}} // ENCAPSULATION - accessible property
    protected float myStartTemp;
    protected float myIsBurnedTemp;
    protected float myIsCookedTemp;

    protected string iAm;

    protected Color32 myRawColor;
    protected Color32 myCookedColor;
    protected Color32 myCurrentColor;
    protected Color32 isBurnedColor = new Color32(25, 25, 0, 255);

    private float _roomTemperature = 72;
    private static float _stationHeatingRate;

    public GameObject onHeatPrefab;
 
    protected virtual void Update()
    {
        UpdateFoodTemperature();    // ABSTRACTION - method name indicates Update() action, details in separate method
        MonitorCookedCondition();   // ABSTRACTION - method name indicates Update() action, details in separate method
    }
    
    private void UpdateFoodTemperature()
    {
        float _warmingRate = .1f;
        float _coolingRate = .2f;
        
        if (isCooking)
        {
            _myTemp += Time.deltaTime * _stationHeatingRate; // cooking at some heatRate
            
        }
        else
        {
            if (_myTemp > _roomTemperature)
            {
                _myTemp -= Time.deltaTime * _coolingRate; // cooling at some coolRate
            }
            
            if(_myTemp < _roomTemperature)
            {
                _myTemp += Time.deltaTime * _warmingRate;
            }
        }
    }

    public static float GetHeatingRate(float stationHeatingRate)
    {
        _stationHeatingRate = stationHeatingRate;
        return _stationHeatingRate;
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

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CookStation"))
        {
            Instantiate(onHeatPrefab, transform);
        }


        if (other.gameObject.CompareTag("Customer"))
        {
            OrderManager.Instance.foodDelivered.Add(iAm);
        }
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
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        } 
    }


    private void OnCollisionStay(Collision other) // example: attaches to plate when dispensed
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

