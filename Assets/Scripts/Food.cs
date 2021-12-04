using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private float _myTemp; // ENCAPSULATION - backing field for myTemp.  Treating the food temperature like a 'health bar' that is not directly accessed

    public GameObject isSmashedPrefab;

    [SerializeField] protected bool isCooking;
    [SerializeField] protected bool isCooked;
    [SerializeField] protected bool isBurned;
    [SerializeField] protected bool isOnFloor;
    [SerializeField] protected bool hasBeenOnFloor;
    [SerializeField] protected bool isSmashed;
    public bool hasBeenMovedToPlate = false; // work around.  prevent double move in ServeFood/ServeFoodToTable()/_transferFoodVector.  have not found way to prevent
    protected float myTemp {get{ return _myTemp;} set{_myTemp = value;}} // ENCAPSULATION - accessible property
    protected float myStartTemp;
    protected float myIsBurnedTemp;
    protected float myIsCookedTemp;
    protected float onFloorTime;
    protected float smashedSinkSpeed = .2f;

    public string iAm;
    
    protected Color32 myRawColor;
    protected Color32 myCookedColor;
    protected Color32 myCurrentColor;
    protected Color32 isBurnedColor = new Color32(25, 25, 0, 255);

    public AudioClip cookConditionIndicator; // rings each time food is cooked or next stage of isCooked
    public GameObject indicatorGrillPrefab;
    public GameObject indicatorSteamerPrefab;

    protected Rigidbody myRb;
    protected Collider myBoxCollider;
    private float _roomTemperature = 72;
    private static float _stationHeatingRate;


    protected virtual void Update()
    {
        UpdateFoodTemperature();    // ABSTRACTION - method name indicates Update() action, details in separate method
        MonitorCookedCondition();   // ABSTRACTION - method name indicates Update() action, details in separate method
        MonitorTimeOnFloor();
    }

    
    private void UpdateFoodTemperature()
    {
        float _warmingRate = .1f;
        float _coolingRate = .2f; // implemented but no use yet.  will support 'food served cold' customer comments
        
        if (isCooking)
        {
            _myTemp += Time.deltaTime * _stationHeatingRate;
            
        }
        else
        {
            // when not cooking, food is cooling or warming toward room temperature
            if (_myTemp > _roomTemperature)
            {
                _myTemp -= Time.deltaTime * _coolingRate;
            }
            
            if(_myTemp < _roomTemperature)
            {
                _myTemp += Time.deltaTime * _warmingRate;
            }
        }
    }


    public static float GetHeatingRate(float stationHeatingRate)
    {
        _stationHeatingRate = stationHeatingRate;
        return _stationHeatingRate;
    }


    protected virtual void MonitorCookedCondition()
    {
        if (!isBurned && _myTemp >= myIsBurnedTemp)
        {
            isBurned = true;
        }
        else if (!isCooked && _myTemp >= myIsCookedTemp)
        {
            isCooked = true;
        }
    }


    protected void MonitorTimeOnFloor()
    {
        if (isOnFloor && !isSmashed)
        {
            onFloorTime += Time.deltaTime;
        }

        if (onFloorTime > 5 && !isSmashed) // play test value
        {
            isSmashed = true;
            ReplaceFoodWithSmashed();
            GameManager.Instance.ApplySmashedFoodPenalty();
        }

        if (isSmashed)
        {
            myRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            myRb.isKinematic = true;
            myRb.detectCollisions = false;
            transform.Translate(Vector3.down * Time.deltaTime * smashedSinkSpeed);
        }
    }
   

    protected void ReplaceFoodWithSmashed()
    {
        GameObject foodSmashed = Instantiate(isSmashedPrefab, transform.position + new Vector3(0, Random.Range(-.085f, -.1f), 0), transform.rotation);
        foodSmashed.GetComponent<FoodSmashed>().myColor = myCurrentColor;        
    }


    protected virtual void OnTriggerEnter(Collider other)
    {
        // provide OnHeat indicator when food touching cook surface
        if (other.gameObject.CompareTag("Grill"))
        {
            Instantiate(indicatorGrillPrefab, transform);
        }

        if (other.gameObject.CompareTag("Steamer"))
        {
            Instantiate(indicatorSteamerPrefab, transform);
        }

        // tally up the food delivered, each food objects adds itself (iAm) to list
        if (other.gameObject.CompareTag("CollectFoodTrigger"))
        {
            GameManager.Instance.foodDeliveredNames.Add(iAm);
            GameManager.Instance.readyToServeGameObjects.Add(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Grill") || other.gameObject.CompareTag("Steamer"))
        {
            isCooking = true;            
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        bool removedOne = false;
        
        // remove OnHeat idicators from food when exiting cook area
        if (other.gameObject.CompareTag("Grill") || other.gameObject.CompareTag("Steamer"))
        {
            isCooking = false;
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        } 

        if (other.gameObject.CompareTag("CollectFoodTrigger"))
        {
            for (int i = 0; i < GameManager.Instance.foodDeliveredNames.Count; i++)
            {
                if (GameManager.Instance.foodDeliveredNames[i] == iAm && !removedOne)
                {
                    GameManager.Instance.foodDeliveredNames.RemoveAt(i);
                    GameManager.Instance.readyToServeGameObjects.RemoveAt(i);
                    removedOne = true;
                }
            }
        }
    }

    // Note to future self - learned that parent objects should have the Rigidbody and Collision scripts
    // Colliders on child objects effectively extend the parent with regard to physics.
    // In this project, this is why the Player GameObject has the rididbody and OnCollision methods instead of the TopPlate GameObject
    // This is why dispensed Food lands on the TopPlate surface/mesh collider even though there is no Rigidbody and script is on the parent/Player
    // Adding Rigidbody to the TopPlate will actually make the physics not work properly
    // Also, the collision is detected on the parent...so in this case we need to take extra steps to have the TopPlate child object become the new parent of objects in collision

    // attaches to player when dispensed
    // this assigns the 3rd child (index 2; starts with zero) of 'other' as the new parent
    // this seems like an potential failure point where code will break if the hierarchy gets changed
    // maybe a Debug.Log message can be added to indicate which GameObject was assigned as new parent
    // a getChildWithTag type of option might be more robust; I don't see this option
    // maybe logic equivalent to if(other.transform.GetChild(2).gameObject.Tag != "TopPlate) {Debug.Log("message here")}


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            isOnFloor = true;
            hasBeenOnFloor = true;
        }
    }        

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent = other.transform.GetChild(2);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        // remains on floor if falls off plate/player
        transform.parent = null;

        if (other.gameObject.CompareTag("Floor"))
        {
            isOnFloor = false;
        }
    }
}

