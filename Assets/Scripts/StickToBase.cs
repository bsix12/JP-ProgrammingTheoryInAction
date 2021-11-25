using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToBase : MonoBehaviour
{
    public Transform baseTransform;

    // Update is called once per frame
    void Update()
    {
        transform.position = baseTransform.position;    
    }
}
