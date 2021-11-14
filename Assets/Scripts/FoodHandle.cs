using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodHandle : MonoBehaviour
{
    private Transform myTransform;

    private void Awake()
    {
        myTransform = GetComponent<Transform>();
    }
    private void OnCollisionEnter(Collision other)  // falls onto plate, from dispenser
    {
        myTransform.parent  = other.transform;
    }

    private void OnCollisionExit(Collision other)  // falls off plate
    {
        myTransform.parent = null;   
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space) && other.CompareTag("Station"))
        {
            transform.position = other.transform.position;
            //myTransform.parent = other.transform;
        }
    }
}
