using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody _playerRb;
	//public Transform _plateCenter;
	public GameObject pickupScrubbi;
	public GameObject pickupTopPlate;
	private bool _isFirstTimePickingUpPlatter = true;

	private float _moveForce = 15f; // will vary based on mass and result desired
	private float _turnSpeed = 150f;
	private bool _isFirstTimeUsingAD = true;
	private bool _isFirstTimeUsingWS = true;

	private void Awake()
	{
		_playerRb = GetComponent<Rigidbody>();  // caching during awake
		pickupTopPlate.gameObject.SetActive(true);
		transform.GetChild(2).gameObject.SetActive(false);
	}

    private void Update()
    {
		RotatePlayer();
    }

	private void FixedUpdate()
	{
		MovePlayer(); // move with physics in FixedUpdate
	}

	private void MovePlayer()
	{
		if (Input.GetKey(KeyCode.W) && GameManager.Instance.moveWithWSUnlocked)
		{
			_playerRb.AddForce(_moveForce * transform.forward);
			if (_isFirstTimeUsingWS)
			{
				_isFirstTimeUsingWS = false;
				GameManager.Instance.messageText.text = "Pick up your serving platter.";
			}
		}

		if (Input.GetKey(KeyCode.S) && GameManager.Instance.moveWithWSUnlocked)
		{
			_playerRb.AddForce(_moveForce * -transform.forward);  // give back
			if (_isFirstTimeUsingWS)
			{
				_isFirstTimeUsingWS = false;
				GameManager.Instance.messageText.text = "Pick up your serving platter.";
			}
		}
	}

	private void RotatePlayer()
	{
		if (Input.GetKey(KeyCode.A) && GameManager.Instance.moveWithADUnlocked)
		{
			transform.Rotate(Time.deltaTime * _turnSpeed * Vector3.down);
			GameManager.Instance.moveWithWSUnlocked = true; // not sure if bad to write value every time instead of just once
			if (_isFirstTimeUsingAD)
            {
				_isFirstTimeUsingAD = false;
				GameManager.Instance.messageText.text = "Use <u>W</u> or <u>S</u> to move forward or backward.";
			}
		}

		if (Input.GetKey(KeyCode.D) && GameManager.Instance.moveWithADUnlocked)
		{
			transform.Rotate(Time.deltaTime * _turnSpeed * Vector3.up);
			GameManager.Instance.moveWithWSUnlocked  = true;
			if (_isFirstTimeUsingAD)
			{
				_isFirstTimeUsingAD = false;
				GameManager.Instance.messageText.text = "Use <u>W</u> or <u>S</u> to move forward or backward.";
			}
		}
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickupTopPlate"))
        {
            if (_isFirstTimePickingUpPlatter)
            {
				_isFirstTimePickingUpPlatter = false;
				GameManager.Instance.messageText.text = "To begin seating guests, select dining room tables to open.";
			}

			else if (!_isFirstTimePickingUpPlatter)
            {
				pickupScrubbi.gameObject.SetActive(true);
			}
			
			pickupTopPlate.gameObject.SetActive(false);
			transform.GetChild(4).gameObject.SetActive(false);
			transform.GetChild(2).gameObject.SetActive(true);
		}

        if (other.gameObject.CompareTag("PickupScrubbi"))
        {
            if (!_isFirstTimePickingUpPlatter)
            {
				pickupScrubbi.gameObject.SetActive(false);
				pickupTopPlate.gameObject.SetActive(true);
				transform.GetChild(2).gameObject.SetActive(false);
				transform.GetChild(4).gameObject.SetActive(true);
            }
		}

		if (other.gameObject.CompareTag("DiningRoom"))
        {
			if(GameManager.Instance.isFirstTimeServingCustomers && transform.GetChild(2).childCount > 0)
            {
				GameManager.Instance.messageText.text = "Approach table and press <u>spacebar</u> to serve food to guests.";
			}
			
			GameManager.Instance.playerInDiningRoom = true;
        }

		if (other.gameObject.CompareTag("Dispenser") && GameManager.Instance.isFirstTimeTriggeringDispenser)
        {
			GameManager.Instance.messageText.text = "Press <u>spacebar</u> to dispense food.";
		}
    }

    private void OnTriggerExit(Collider other)
    {
		if (other.gameObject.CompareTag("DiningRoom"))
		{
			GameManager.Instance.playerInDiningRoom = false;
			GameManager.Instance.KitchenDoorControl();
		}

		if(other.gameObject.CompareTag("Dispenser") && GameManager.Instance.isFirstTimeTriggeringDispenser)
        {
			GameManager.Instance.messageText.text = "";

		}
		if (GameManager.Instance.isFirstTimeServingCustomers && transform.GetChild(2).childCount > 0)
		{
			GameManager.Instance.messageText.text = "";
		}
	}
}

