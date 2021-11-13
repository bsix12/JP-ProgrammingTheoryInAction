using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInfo : MonoBehaviour
{
    [SerializeField] private float _myTemperature = 60f;
    // [SerializeField private float _heatingRate = 5f;

    public float _myIsCookedTemperature = 160f;
    public float _myBurnedTemperature = 300f;
    public bool isCooking;
    public bool isCooked;
    public bool isBurned;

    [SerializeField] private float heatRate;
    [SerializeField] private float coolingRate = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooking)
        {
            _myTemperature += heatRate * Time.deltaTime;
        }
        else
        {
            _myTemperature -= coolingRate * Time.deltaTime;
        }

        if (isCooked == false && _myTemperature >= _myIsCookedTemperature)
        {
            isCooked = true;
        }

        if(!isBurned && _myTemperature >= _myBurnedTemperature)
        {
            isBurned = true;
        }

    }

    public void Cooking(float stationHeatingRate)
    {
        heatRate = stationHeatingRate;
    }
}
