using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlate : MonoBehaviour
{
    private float _turnSpeed = 150f;

    // Update is called once per frame
    void Update()
    {
        RotatePlateTop();
    }

    private void RotatePlateTop()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Time.deltaTime * _turnSpeed * Vector3.down);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Time.deltaTime * _turnSpeed * Vector3.up);
        }
    }
}
