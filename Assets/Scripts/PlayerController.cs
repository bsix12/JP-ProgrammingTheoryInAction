using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody playerRb;

	private float moveForce = 15f; // will vary based on mass and result desired
	private float turnSpeed = 30f;

	private void Awake()
	{
		playerRb = GetComponent<Rigidbody>();  // caching during awake
	}

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
			transform.Rotate(Time.deltaTime* turnSpeed * Vector3.down);
        }
		
			if (Input.GetKey(KeyCode.D))
        {
			transform.Rotate(Time.deltaTime* turnSpeed * Vector3.up);
        }
    }


	private void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.W))
        {
			playerRb.AddForce(moveForce * transform.forward);
        }

		if (Input.GetKey(KeyCode.S))
        {
			playerRb.AddForce(moveForce * -transform.forward);  // give back
        }
	}
}
