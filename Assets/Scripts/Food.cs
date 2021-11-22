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

    public AudioClip cookConditionIndicator; // rings each time food is cooked or next stage of isCooked
    public GameObject onHeatPrefab;

    private float _roomTemperature = 72;
    private static float _stationHeatingRate;

    protected virtual void Update()
    {
        UpdateFoodTemperature();    // ABSTRACTION - method name indicates Update() action, details in separate method
        MonitorCookedCondition();   // ABSTRACTION - method name indicates Update() action, details in separate method
    }
    
    private void UpdateFoodTemperature()
    {
        float _warmingRate = .1f;
        float _coolingRate = .2f; // implemented but no use yet.  will support 'food served cold' customer comments
        
        if (isCooking)
        {
            _myTemp += Time.deltaTime * _stationHeatingRate;
            
        }
        else
        {
            // when not cooking, food is cooling or warming toward room temperature
            if (_myTemp > _roomTemperature)
            {
                _myTemp -= Time.deltaTime * _coolingRate;
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
        // provide OnHeat indicator when food touching cook surface
        if (other.gameObject.CompareTag("CookStation"))
        {
            Instantiate(onHeatPrefab, transform);
        }

        // tally up the food delivered, each food objects adds itself (iAm) to list
        if (other.gameObject.CompareTag("ServeToCustomer"))
        {
            GameManager.Instance.foodDelivered.Add(iAm);
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
        // remove OnHeat idicators from food when exiting cook area
        if (other.gameObject.CompareTag("CookStation"))
        {
            isCooking = false;
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        } 
    }


    private void OnCollisionStay(Collision other) 
    {   
        // attaches to player when dispensed
        // attaches to customer plate when served
        // food as children allows for destroy all children to clear
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("CustomerPlate"))
        {
            transform.parent = other.transform;
        }        
    }

    private void OnCollisionExit(Collision other)
    {
        // remains on floor if falls off plate/player
        transform.parent = null;
    }


}

