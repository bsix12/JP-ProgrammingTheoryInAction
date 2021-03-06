using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookStation : MonoBehaviour
{
    [SerializeField] private float myHeatingRate = 8f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Food.GetHeatingRate(myHeatingRate);
        }
    }
}
