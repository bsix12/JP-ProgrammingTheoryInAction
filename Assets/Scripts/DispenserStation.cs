using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserStation : MonoBehaviour
{
    public GameObject stationMaterial;
    public Transform spawnLoc;
    public AudioSource audioSource;
    public AudioClip dispenserSound;

    private bool _canDispense;


    private void Update()
    {
        if (_canDispense && Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(stationMaterial, spawnLoc);
            audioSource.PlayOneShot(dispenserSound, .3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TopPlate"))
        {
            _canDispense = true;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TopPlate"))
        {
            _canDispense = false;
        }
    }
}
