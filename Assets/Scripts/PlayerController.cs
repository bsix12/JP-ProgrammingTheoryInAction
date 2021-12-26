using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	////////////////////////////
	/// Assign In Inspector
	////////////////////////////

	public GameObject pickupScrubbi;
	public GameObject pickupTopPlate;

	public AudioSource scrubbiAudioSource;

	////////////////////////////

	private Rigidbody _playerRb;
	private AudioSource _myAudioSource;

	private bool _isFirstTimeUsingAD = true;
	private bool _isFirstTimeUsingWS = true;
	private bool _isFirstTimePickingUpPlatter = true;
	private bool _isFootstepAudioPlaying = false;

	private float _moveForce = 15f; // will vary based on mass and result desired
	private float _turnSpeed = 150f;
	


	private void Awake()
	{
		_playerRb = GetComponent<Rigidbody>();  // caching during awake
		_myAudioSource = GetComponent<AudioSource>();
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
		PlayFootsteps();
	}


	private void MovePlayer()
	{
		if (Input.GetKey(KeyCode.W) && GameManager.Instance.isMoveWithWSUnlocked)
		{
			_playerRb.AddForce(_moveForce * transform.forward);
			if (_isFirstTimeUsingWS)
			{
				_isFirstTimeUsingWS = false;
				GameManager.Instance.messageText.text = "Pick up the round serving platter\n\nleaning against the wall.";	
			}
		}

		if (Input.GetKey(KeyCode.S) && GameManager.Instance.isMoveWithWSUnlocked)
		{
			_playerRb.AddForce(_moveForce * -transform.forward);  // give back
			if (_isFirstTimeUsingWS)
			{
				_isFirstTimeUsingWS = false;
				GameManager.Instance.messageText.text = "Pick up the round serving platter\n\nleaning against the wall.";
			}
		}
	}


	private void RotatePlayer()
	{
		if (Input.GetKey(KeyCode.A) && GameManager.Instance.isMoveWithADUnlocked)
		{
			transform.Rotate(Time.deltaTime * _turnSpeed * Vector3.down);
			GameManager.Instance.isMoveWithWSUnlocked = true; // not sure if bad to write value every time instead of just once
			if (_isFirstTimeUsingAD)
            {
				_isFirstTimeUsingAD = false;
				GameManager.Instance.messageText.text = "Use <u>W</u> or <u>S</u> to move forward or backward.";
			}
		}

		if (Input.GetKey(KeyCode.D) && GameManager.Instance.isMoveWithADUnlocked)
		{
			transform.Rotate(Time.deltaTime * _turnSpeed * Vector3.up);
			GameManager.Instance.isMoveWithWSUnlocked  = true;
			if (_isFirstTimeUsingAD)
			{
				_isFirstTimeUsingAD = false;
				GameManager.Instance.messageText.text = "Use <u>W</u> or <u>S</u> to move forward or backward.";
			}
		}
	}


	private void PlayFootsteps()
    {
		if(!_isFootstepAudioPlaying && GameManager.Instance.isMoveWithWSUnlocked && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
		{
			_isFootstepAudioPlaying = true;
			_myAudioSource.Play();

        }
  
		if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
			_isFootstepAudioPlaying = false;
			_myAudioSource.Stop();
		}
	}


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickupTopPlate"))
        {
            if (_isFirstTimePickingUpPlatter)
            {
				_isFirstTimePickingUpPlatter = false;
				GameManager.Instance.playerAudioSource.PlayOneShot(GameManager.Instance.positiveDeliveryAudio, 0.1f);
				GameManager.Instance.messageText.text = "Open Table 1 to begin seating guests.";
				GameManager.Instance.EnableDiningTablesUI();
			}

			else if (!_isFirstTimePickingUpPlatter)
            {
				pickupScrubbi.gameObject.SetActive(true);
			}
			
			pickupTopPlate.gameObject.SetActive(false);
			transform.GetChild(2).gameObject.SetActive(true);
			transform.GetChild(4).gameObject.SetActive(false);
			GameManager.Instance.isUsingScrubbi = false;
		}

        if (other.gameObject.CompareTag("PickupScrubbi"))
        {
            if (!_isFirstTimePickingUpPlatter)
            {
				pickupScrubbi.gameObject.SetActive(false);
				pickupTopPlate.gameObject.SetActive(true);
				transform.GetChild(2).gameObject.SetActive(false);
				transform.GetChild(4).gameObject.SetActive(true);
				GameManager.Instance.isUsingScrubbi = true;
            }
		}

		if (other.gameObject.CompareTag("DiningRoom"))
        {
			GameManager.Instance.playerInDiningRoom = true;
			
			if(GameManager.Instance.isFirstTimeServingCustomers && transform.GetChild(2).childCount > 0)
            {
				GameManager.Instance.messageText.text = "Approach table and press <u>spacebar</u> to serve food to guests.";
			}			
        }
    }


    private void OnTriggerExit(Collider other)
    {
		if (other.gameObject.CompareTag("DiningRoom"))
		{
			GameManager.Instance.playerInDiningRoom = false;
			GameManager.Instance.KitchenDoorControl();
	
			if (GameManager.Instance.isFirstTimeServingCustomers && transform.GetChild(2).childCount > 0)
			{
				GameManager.Instance.messageText.text = "";
			}
		}
	}
}

