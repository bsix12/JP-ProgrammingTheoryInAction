using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasteStation : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            Destroy(other);
        }
    }
}
