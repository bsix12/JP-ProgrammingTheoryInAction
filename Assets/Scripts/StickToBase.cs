using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToBase : MonoBehaviour
{
    public Transform baseTransform;

    // Update is called once per frame
    private void Update()
    {
        transform.position = baseTransform.position;    
    }
}
