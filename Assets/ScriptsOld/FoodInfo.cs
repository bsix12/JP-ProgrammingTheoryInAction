using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInfo : MonoBehaviour
{
    [SerializeField] private float _myTemperature = 60f;
    [SerializeField] private float _heatRateApplied;
    [SerializeField] private float _coolingRate = 0.1f;
   
    public Color myColor; 
    
    public bool isCooking;
    public bool isCooked;
    public bool isCookedRare;
    public bool isCookedMedium;
    public bool isCookedWellDone;
    public bool isBurned;

    private Renderer _myRender;
    private float _myStartTemperature = 60f;
    
    void Awake()
    {
        _myRender = GetComponent<Renderer>();
        _myTemperature = _myStartTemperature;
        isCooked = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFoodTemperature();
        MonitorCookedCondition();
        SetFoodColor();
    }

    public void ApplyHeat(float stationHeatingRate)
    {
        _heatRateApplied = stationHeatingRate;
    }

    private void UpdateFoodTemperature()
    {
        if (isCooking)
        {
            _myTemperature += _heatRateApplied * Time.deltaTime;
        }
        else
        {
            _myTemperature -= _coolingRate * Time.deltaTime;
        }
    }

    private void MonitorCookedCondition()
    {
    float _myIsRareTemperature = 120f;
    float _myIsMediumTemperature = 140f;
    float _myIsWellDoneTemperature = 170f;
    float _myIsBurnedTemperature = 200f;

        if (isCookedWellDone && _myTemperature >= _myIsBurnedTemperature)   // remains isBurned even if temperature drops below myIsBurnedTemperature
        {
            isBurned = true;
            isCookedWellDone = false;
        }

        else if (isCookedMedium && _myTemperature >= _myIsWellDoneTemperature)
        {
            isCookedWellDone = true;
            isCookedMedium = false;
        }
        else if (isCookedRare && _myTemperature >= _myIsMediumTemperature)
        {
            isCookedMedium = true;
            isCookedRare = false;
        }
        else if (!isCooked && _myTemperature >= _myIsRareTemperature)
        {
            isCookedRare = true;
            isCooked = true;
        }
    }

    private void SetFoodColor()
    {
        if (!isCooked)
        {
            _myRender.material.color = new Color32(225, 145, 145, 255);
        }

        if (isCookedRare)
        {
            _myRender.material.color = new Color32(245, 125, 100, 255);
        }

        if (isCookedMedium)
        {
            _myRender.material.color = new Color32(175, 100, 50, 255);
        }

        if (isCookedWellDone)
        {
            _myRender.material.color = new Color32(125, 75, 25, 255);
        }

        if (isBurned)
        {
            _myRender.material.color = new Color32(25, 25, 0, 255);
        }
    }
}
