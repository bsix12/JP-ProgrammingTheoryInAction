using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteStation : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip wasteSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            audioSource.PlayOneShot(wasteSound);
            Destroy(other);

        }
    }
}
