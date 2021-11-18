using UnityEngine;

public class RotateObject : MonoBehaviour
{
    private float _rotationSpeed = 100f;

 
    void Update()
    {
        transform.Rotate(transform.up, _rotationSpeed * Time.deltaTime);  
    }
}
