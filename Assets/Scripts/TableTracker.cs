using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TableTracker : MonoBehaviour
{
    ////////////////////////////
    /// Assign In Inspector
    ////////////////////////////

    public GameObject[] guests;
    public GameObject orderTicket;
    public AudioSource playerAudioSource;
    public AudioClip newOrderBell;

    public List<GameObject> onPlateGameObjects = new List<GameObject>();

    public TextMeshProUGUI maximumOrderScorePossibleTextUI;
    public TextMeshProUGUI orderTicketTextUI;
    public TextMeshProUGUI timeElapsedTextUI;
    public TextMeshPro orderText;
    public Image orderTicketBackground;

    ////////////////////////////

    public bool isOnTimer;
    public bool isLate;
    public bool isEmptyWithNoGuests = true;
    public float timeElapsed = 0;

    ////////////////////////////
 
    [SerializeField] protected float _timeRemainingUntilNextOrder;


    ////////////////////////////

    private List<int> _guestsIndexList;
    private List<string> _onlyFoodOrderedNames = new List<string>();

    [SerializeField] private int _chickenOrdered;
    [SerializeField] private int _beefRareOrdered;
    [SerializeField] private int _beefMediumOrdered;
    [SerializeField] private int _beefWellDoneOrdered;
    [SerializeField] private int _carrotsSteamedOrdered;
    [SerializeField] private int _broccoliSteamedOrdered;
    [SerializeField] private int _saladsOrdered;

    private int _maximumOrderScorePossible;
    private int _numberOfGuestsThisOrder;

    private float _timeGuestsStay;



    private void Start()
    {
        orderTicketTextUI.text = "";
    }


    protected virtual void Update()
    {
        OrderPrepElapsedTime();
    }


    public void BeginNewOrder()
    {
        _timeRemainingUntilNextOrder = Random.Range(5f, 15f); // debug timing ////////////////////////////////////////////////////////////////////
        _timeGuestsStay = _timeRemainingUntilNextOrder * .8f; // this is percent of delay that diners and food will remain at table before being cleared
        Invoke("GuestsLeaveAndClearTable", _timeGuestsStay);
    }


    public void GenerateAndPublishOrderDetails()
    {
            GameManager.Instance.GenerateOrderDetails(); // GameManagerGenerates the order              
            StoreOrderDetails();

            GameManager.Instance.ResetAfterOrderManagerStoresDetailsForTable();

            SeatGuests();
            PublishOrder();

            _onlyFoodOrderedNames.Clear();
            isOnTimer = true;
    }


    private void StoreOrderDetails()
    {
        _onlyFoodOrderedNames = GameManager.Instance.onlyFoodOrderedNames; // store results for Table
        _maximumOrderScorePossible = GameManager.Instance.maximumOrderScorePossible; // store results for Table

        _chickenOrdered = GameManager.Instance.chickenOrdered;
        _beefRareOrdered = GameManager.Instance.beefRareOrdered;
        _beefMediumOrdered = GameManager.Instance.beefMediumOrdered;
        _beefWellDoneOrdered = GameManager.Instance.beefWellDoneOrdered;
        _carrotsSteamedOrdered = GameManager.Instance.carrotsSteamedOrdered;
        _broccoliSteamedOrdered = GameManager.Instance.broccoliSteamedOrdered;
        _saladsOrdered = GameManager.Instance.saladsOrdered;
        _numberOfGuestsThisOrder = _chickenOrdered + _beefRareOrdered + _beefMediumOrdered + _beefWellDoneOrdered;
    }


    private void SeatGuests()
    {
        GameManager.Instance.numberOfSeatedGuests += _numberOfGuestsThisOrder;
        StatsTracker.Instance.guestsSeatedTotal += _numberOfGuestsThisOrder;

        _guestsIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        isEmptyWithNoGuests = false;

        for (int i = 0; i < _numberOfGuestsThisOrder; i++)
        {
            int _random = Random.Range(0, _guestsIndexList.Count - 1);

            guests[_guestsIndexList[_random]].gameObject.SetActive(true);
            _guestsIndexList.RemoveAt(_random); // needs to be non-repeating random index
        }
    }


    private void PublishOrder()
    {
        StatsTracker.Instance.ordersReceivedTotal += 1;
        orderText.text = ""; // clear the 'waiting for new order'
        orderTicketBackground.gameObject.SetActive(true);
        orderTicket.gameObject.SetActive(true);
        playerAudioSource.PlayOneShot(newOrderBell, .15f);
        maximumOrderScorePossibleTextUI.text = _maximumOrderScorePossible.ToString() + " points possible";

        for (int i = 0; i < _onlyFoodOrderedNames.Count; i++)
        {
            orderText.text += _onlyFoodOrderedNames[i] + "\n";
            orderTicketTextUI.text += _onlyFoodOrderedNames[i] + "\n";
        }
    }


    private void OrderPrepElapsedTime()
    {
       // timeElapsedTextUI.color = Color.black;

        if (isOnTimer)
        {
            timeElapsed += Time.deltaTime;

            string _roundTime = Mathf.Floor(timeElapsed / 60).ToString("0") + ":" + Mathf.FloorToInt(timeElapsed % 60).ToString("00");
            timeElapsedTextUI.text = _roundTime;

            if (timeElapsed > _numberOfGuestsThisOrder * GameManager.Instance.secondsOfPrepAllowedPerMain && !isLate)
            {
                timeElapsedTextUI.color = Color.red;
                isLate = true;
                GameManager.Instance.isLateNowTableCount += 1;
                StatsTracker.Instance.wasLateTableCountTotal += 1;
            }
        }
    }


    private void GuestsLeaveAndClearTable() // this is invoked from TableTracker/BeginNewOrder(), name of method must match
    {
        for (int i = 0; i < onPlateGameObjects.Count; i++)
        {
            Destroy(onPlateGameObjects[i].gameObject); // added .gameObject at end like wasteStation
        }

        for (int i = 0; i < guests.Length; i++)
        {
            guests[i].gameObject.SetActive(false);
        }

        GameManager.Instance.numberOfSeatedGuests -= _numberOfGuestsThisOrder; // _mainDishesOrdered = number of guests
        _numberOfGuestsThisOrder = 0;
        isEmptyWithNoGuests = true;
        onPlateGameObjects.Clear(); // list of GameObjects
        GameManager.Instance.reportCardToPost.Clear(); // list of strings

        GameManager.Instance.CloseTableThatWasWaitingToClose();
        GameManager.Instance.KitchenDoorControl();
    }


    public void AfterFoodIsServedActions()
    {
        GameManager.Instance.DetermineWhichTableWasServed();
        StatsTracker.Instance.deliveriesMadeTotal += 1;
        StatsTracker.Instance.elapsedTimeToServeOrder += timeElapsed;
        EvaluateDeliveryBeforeStartingNextOrder();   
        ResetTicket();
    }


    private void ResetTicket()
    {
        orderTicketBackground.gameObject.SetActive(false);
        orderTicket.gameObject.SetActive(false);
        
        isOnTimer = false;
        if (isLate)
        {            
            GameManager.Instance.isLateNowTableCount -= 1;
            timeElapsedTextUI.color = Color.black;
            isLate = false;
        }
        
        timeElapsed = 0;
        _maximumOrderScorePossible = 0;

        orderText.text = "Next Order Soon";
        orderTicketTextUI.text = "";
        maximumOrderScorePossibleTextUI.text = "----";
    }


    private void EvaluateDeliveryBeforeStartingNextOrder()
    {
        GameManager.Instance.isCalculatingScore = true;
        GameManager.Instance.chickenOrdered = _chickenOrdered; // AfterFoodIsServedActions() will compare Dilvered versus OrderedTable1 
        GameManager.Instance.beefRareOrdered = _beefRareOrdered;
        GameManager.Instance.beefMediumOrdered = _beefMediumOrdered;
        GameManager.Instance.beefWellDoneOrdered = _beefWellDoneOrdered;
        GameManager.Instance.carrotsSteamedOrdered = _carrotsSteamedOrdered;
        GameManager.Instance.broccoliSteamedOrdered = _broccoliSteamedOrdered;
        GameManager.Instance.saladsOrdered = _saladsOrdered;
        GameManager.Instance.maximumOrderScorePossible = _maximumOrderScorePossible;

        _chickenOrdered = 0;
        _beefRareOrdered = 0;
        _beefMediumOrdered = 0;
        _beefWellDoneOrdered = 0;
        _carrotsSteamedOrdered = 0;
        _broccoliSteamedOrdered = 0;
        _saladsOrdered = 0;

        GameManager.Instance.AfterFoodIsServedActions();
        BeginNewOrder();
    }
}

