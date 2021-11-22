using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserStation : MonoBehaviour
{
    public GameObject stationMaterial;
    public Transform spawnLoc;
    public AudioSource audioSource;
    public AudioClip dispenserSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(stationMaterial, spawnLoc);
            audioSource.PlayOneShot(dispenserSound, .3f); 
        }        
    }
}
