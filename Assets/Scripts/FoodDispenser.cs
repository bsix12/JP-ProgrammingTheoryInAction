using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDispenser : MonoBehaviour
{
    public GameObject stationMaterial;
    public Transform spawnLoc;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(stationMaterial, spawnLoc);
        }        
    }
}
