using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeFood : MonoBehaviour
{
    private Transform _itemToServeTransform;
    private Rigidbody _itemToServeRb;
    private Vector3 _transferFoodVector;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.isInServiceArea && !OrderManager.Instance.isDoneServingTable1)
        {
            ServeFoodToTable();
            TransferObjectsReadyToServeListMoveToOnPlateList();

            OrderManager.Instance.AfterFoodIsServedActions();
            GameManager.Instance.readyToServeGameObjects.Clear();

            GameManager.Instance.foodDeliveredNames.Clear();
            OrderManager.Instance.dinersClearedTable1 = false;
        }
    }

    private void ServeFoodToTable()
    {
        for (int i = 0; i < GameManager.Instance.readyToServeGameObjects.Count; i++)
        {
            //Debug.Log("ServeFoodToTable Called")
            //Debug.Log("I am element number: " + i);                
            _itemToServeRb = GameManager.Instance.readyToServeGameObjects[i].GetComponent<Rigidbody>();
            _itemToServeTransform = GameManager.Instance.readyToServeGameObjects[i].GetComponent<Transform>();
            //Debug.Log("my start position is: " + _itemToServeTransform.position);
            _itemToServeRb.velocity = Vector3.zero;
            _itemToServeRb.angularVelocity = Vector3.zero;
            _transferFoodVector = GameManager.Instance.serveTableLocation - transform.position;
            _itemToServeTransform.position += _transferFoodVector;
            //Debug.Log("my final position is: " + _itemToServeTransform.position);
        }

        
    }

    private void TransferObjectsReadyToServeListMoveToOnPlateList()
    {
        for (int i= 0; i<GameManager.Instance.readyToServeGameObjects.Count; i++)
        {
            GameManager.Instance.onPlateGameObjectsTable1.Add(GameManager.Instance.readyToServeGameObjects[i]);               
        }

        GameManager.Instance.readyToServeGameObjects.Clear();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            GameManager.Instance.isInServiceArea = true;
            GameManager.Instance.atTableName = other.GetComponent<TableIAm>().atTableName; // use this value as a condition for other methods/actions to differentiate 'at which table'
            GameManager.Instance.serveTableLocation = other.transform.position;   // Create a reference to the correct transform to be used in _transferFoodVector
                                                                            // before this change, the transform.position for ServeFood script never updated after
                                                                            // the first time a value was assigned.  food was moving from player location to 
                                                                            // the same transform location each time.
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Table"))
        {
            GameManager.Instance.isInServiceArea = false;
        }
    }
}

//////////////////////////////////////////////////////////////////////////////////

    /*
        private void OnTriggerEnter(Collider other)                             // this was first pass - food transfered to table automatically

        {                                                                       // was a problem when person approaches slowly and food on front
            if (other.CompareTag("Food"))                                       // of plate is transfered and triggers the evaluation but, there is 
            {                                                                   // food to be served that was 'too slow' to make it in range for transfer
                _otherRb = other.GetComponent<Rigidbody>();
                MoveFoodToPlate();
            }
        }

        // review and post results, allow new order to be generated
        // isDoneServing to prevent player triggering duplicate report
        if (other.CompareTag("Player") && GameManager.Instance.isDoneServing == false)
        {
             GameManager.Instance.AfterFoodIsServed();
        }
    */

    /*
        private void MoveFoodToPlate(Rigidbody _otherRb)                        // this was second pass - key press transfers food.  problem was if large
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

