using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeFood : MonoBehaviour
{
    private Rigidbody _otherRb;
    private Transform _otherFoodTransform;
    private float _offsetDistance = .5f;
    private Vector3 _offset;
    private Vector3 _moveFoodVector;
    private List<GameObject> _itemsToServe;
    public Transform _playerTransform;

    private void Start()
    {
        _itemsToServe = GameObject.FindGameObjectWithTag("CollectFoodTrigger").GetComponent<CollectFood>().itemsToServe;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.inServiceArea = true;            
        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.inServiceArea = false;
        }

    }

    private void Update()
    {
        if (GameManager.Instance.inServiceArea && GameManager.Instance.isDoneServing == false && Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < _itemsToServe.Count; i++)
            {
                // _otherRb = _itemsToServe[i].GetComponent<Rigidbody>();
                // MoveFoodToPlate(_otherRb);
                _otherFoodTransform = _itemsToServe[i].GetComponent<Transform>();
                MoveFoodToPlate(_otherFoodTransform);
            }
            GameManager.Instance.AfterFoodIsServed();
        }
    }

    private void MoveFoodToPlate(Transform _otherFoodTransform)
    {

        // add vector3 math here to calculate new positions to move food
        // top center of TopPlate to top center of CustomerPlate is the vector...then apply this to each food item in _itemsToServe
        _moveFoodVector = transform.position - _playerTransform.position;
        _otherFoodTransform.position += _moveFoodVector;
    }


    /*
    private void OnTriggerEnter(Collider other)                             // this was first pass - food transfered to table automatically
    {                                                                       // was a problem when person approaches slowly and food on front
        if (other.CompareTag("Food"))                                       // of plate is transfered and triggers the evaluation but, there is 
        {                                                                   // food to be served that was 'too slow' to make it in range for transfer
            _otherRb = other.GetComponent<Rigidbody>();
            MoveFoodToPlate();
        }

        // review and post results, allow new order to be generated
        // isDoneServing to prevent player triggering duplicate report
        if (other.CompareTag("Player") && GameManager.Instance.isDoneServing == false)
        {
             GameManager.Instance.AfterFoodIsServed();
        }
    }
    */


    /*
    void MoveFoodToPlate(Rigidbody _otherRb)                                // this was second pass - key press transfers food.  problem was if large
    {                                                                       // quantity of food transferred at once, they collide into eachoter at new
        // remove player movement transfering with food                     // location and food goes all over the place and on floor.
        OnPlatePosition();                                                  // could do something like turn off colliders or make kinematic and then set a 
        _otherRb.transform.position = transform.position + _offset;         // y axis position they could not drop below to simulate stopping on plate
        _otherRb.velocity = Vector3.zero;                                   // instead, plan is to use vector3 comparison and move food from TopPlate to 
        _otherRb.angularVelocity = Vector3.zero;                            // customer plate in same relative positions
    }

    void OnPlatePosition()
    {
        // generate random locations on customer plate to deposit food
        _offset.x = Random.Range(-_offsetDistance, _offsetDistance);
        _offset.y = 0f;
        _offset.z = Random.Range(-_offsetDistance, _offsetDistance);
    }

    */
}
