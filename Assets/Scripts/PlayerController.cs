using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody _playerRb;

	private float _moveForce = 15f; // will vary based on mass and result desired
	private float _turnSpeed = 120f;

	private void Awake()
	{
		_playerRb = GetComponent<Rigidbody>();  // caching during awake
	}

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
			transform.Rotate(Time.deltaTime* _turnSpeed * Vector3.down);
        }
		
			if (Input.GetKey(KeyCode.D))
        {
			transform.Rotate(Time.deltaTime* _turnSpeed * Vector3.up);
        }
    }


	private void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.W))
        {
			_playerRb.AddForce(_moveForce * transform.forward);
        }

		if (Input.GetKey(KeyCode.S))
        {
			_playerRb.AddForce(_moveForce * -transform.forward);  // give back
        }
	}
}