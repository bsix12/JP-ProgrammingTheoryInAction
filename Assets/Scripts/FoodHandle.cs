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
    private void OnCollisionEnter(Collision other)
    {
        myTransform.parent  = other.transform;
    }
    private void OnCollisionExit(Collision other)
    {
        myTransform.parent = null;   
    }
}
