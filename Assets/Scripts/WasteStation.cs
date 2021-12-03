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
            Destroy(other.gameObject);  // Destroy(other) didn't work
                                        // looked like object was removed but it was instead reset and falling
                                        // need destroy the gameObject the script is attached to
            Debug.Log("Hit Waste");
            GameManager.Instance.ApplyWastedFoodPenalty();
        }
    }
}
