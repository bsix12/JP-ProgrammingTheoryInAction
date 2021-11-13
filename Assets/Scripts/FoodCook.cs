using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCook : MonoBehaviour
{
    [SerializeField] private float stationHeatingRate = 10;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            other.GetComponent<FoodInfo>().Cooking(stationHeatingRate);
            other.GetComponent<FoodInfo>().isCooking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            other.GetComponent<FoodInfo>().isCooking = false;
        }
    }
}
