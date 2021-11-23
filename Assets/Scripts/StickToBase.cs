using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToBase : MonoBehaviour
{
    public Transform baseTransform;
    private Vector3 _offset = new Vector3(0, .05f, 0);

    // Update is called once per frame
    void Update()
    {
        transform.position = baseTransform.position + _offset;    
    }
}
