using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    [SerializeField] protected float _timeRemainingUntilNextOrder;
    private float _timeGuestsStay;
    //public bool isReadyForNewOrder = false;
    //public bool isDoneServing = false;
    private List<string> _onlyFoodOrderedNames = new List<string>();
    public bool isOnTimer;
    public List<GameObject> onPlateGameObjects = new List<GameObject>();

    private int _maximumOrderScorePossible;

    [SerializeField] private int _chickenOrdered;
    [SerializeField] private int _beefRareOrdered;
    [SerializeField] private int _beefMediumOrdered;
    [SerializeField] private int _beefWellDoneOrdered;
    [SerializeField] private int _carrotsSteamedOrdered;
    [SerializeField] private int _broccoliSteamedOrdered;
    [SerializeField] private int _saladsOrdered;
    private int _mainDishesOrdered;

    private List<int> _guestsIndexList;
    public GameObject[] guests;

    public TextMeshPro orderText;
    public Image orderTicketBackground;
    public GameObject orderTicket;
    public TextMeshProUGUI maximumOrderScorePossibleTextUI;
    public TextMeshProUGUI orderTicketTextUI;
    public bool isLate;

    public TextMeshProUGUI timeElapsedTextUI;
    public float timeElapsed = 0;


    //protected bool isGeneratingOrderTable1; // order generator can only be used by one table at a time
    //protected bool isGeneratingOrderTable2;
    //protected bool isGeneratingOrderTable3;

    //protected bool isCalculatingScore;



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
        Invoke("GuestsLeaveAndClear", _timeGuestsStay);
    }


    public void GenerateOrderForTable()
    {
        if (GameManager.Instance.isActiveTable1 && GameManager.Instance.isDoneServingTable1 && GameManager.Instance.isGeneratingOrderTable1)
        {
            GenerateAndPublishOrderDetails();
            GameManager.Instance.isDoneServingTable1 = false;
            GameManager.Instance.isGeneratingOrderTable1 = false;
        }

        if (GameManager.Instance.isActiveTable2 && GameManager.Instance.isDoneServingTable2 && GameManager.Instance.isGeneratingOrderTable2)
        {
            GenerateAndPublishOrderDetails();
            GameManager.Instance.isDoneServingTable2 = false;
            GameManager.Instance.isGeneratingOrderTable2 = false;
        }

        if (GameManager.Instance.isActiveTable3 && GameManager.Instance.isDoneServingTable3 && GameManager.Instance.isGeneratingOrderTable3)
        {
            GenerateAndPublishOrderDetails();
            GameManager.Instance.isDoneServingTable3 = false;
            GameManager.Instance.isGeneratingOrderTable3 = false;
        }
    }


    private void GenerateAndPublishOrderDetails()
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
        _mainDishesOrdered = _chickenOrdered + _beefRareOrdered + _beefMediumOrdered + _beefWellDoneOrdered;
    }

    private void SeatGuests()
    {
        int _numberOfGuests = _mainDishesOrdered;
        GameManager.Instance.numberOfSeatedGuests += _numberOfGuests;
        _guestsIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };

        for (int i = 0; i < _numberOfGuests; i++)
        {
            int _random = Random.Range(0, _guestsIndexList.Count - 1);

            guests[_guestsIndexList[_random]].gameObject.SetActive(true);
            _guestsIndexList.RemoveAt(_random); // needs to be non-repeating random index
        }
    }

    private void PublishOrder()
    {
        orderText.text = ""; // clear the 'waiting for new order'
        orderTicketBackground.gameObject.SetActive(true);
        orderTicket.gameObject.SetActive(true);
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

            if (timeElapsed > _mainDishesOrdered * GameManager.Instance.secondsOfPrepAllowedPerMain && !isLate)
            {
                timeElapsedTextUI.color = Color.red;
                isLate = true;
                GameManager.Instance.isLateTableCount += 1;
            }
        }
    }
    private void GuestsLeaveAndClear()
    {
        for (int i = 0; i < onPlateGameObjects.Count; i++)
        {
            Destroy(onPlateGameObjects[i].gameObject); // added .gameObject at end like wasteStation
        }

        for (int i = 0; i < guests.Length; i++)
        {
            guests[i].gameObject.SetActive(false);
        }

        GameManager.Instance.numberOfSeatedGuests -= _mainDishesOrdered; // _mainDishesOrdered = number of guests
        _mainDishesOrdered = 0;

        onPlateGameObjects.Clear(); // list of GameObjects
        GameManager.Instance.reportCardToPost.Clear(); // list of strings

        if (!GameManager.Instance.isActiveTable1 && GameManager.Instance.isDoneServingTable1)
        {
            GameManager.Instance.reservedTable1.gameObject.SetActive(true);
        }
        
        if (!GameManager.Instance.isActiveTable2 && GameManager.Instance.isDoneServingTable2)
        {
            GameManager.Instance.reservedTable2.gameObject.SetActive(true);
        }
        
        if (!GameManager.Instance.isActiveTable3 && GameManager.Instance.isDoneServingTable3)
        {
            GameManager.Instance.reservedTable3.gameObject.SetActive(true);
        }

        GameManager.Instance.KitchenDoorControl();
    }


    public void AfterFoodIsServedActions()
    {
        if (GameManager.Instance.atTableName == "Table1") // string must match Inspector, DiningRoom/DiningTableX/inServiceAreaTrigger/TableIAm.cs
        {
            GameManager.Instance.isDoneServingTable1 = true;
            GameManager.Instance.isReadyForNewOrderTable1 = true;
            ResetTicket();
            EvaluateDeliveryBeforeStartingNextOrder();
        }

        else if (GameManager.Instance.atTableName == "Table2") // string must match Inspector, DiningRoom/DiningTableX/inServiceAreaTrigger/TableIAm.cs
        {
            GameManager.Instance.isDoneServingTable2 = true;
            GameManager.Instance.isReadyForNewOrderTable2 = true;
            ResetTicket();
            EvaluateDeliveryBeforeStartingNextOrder();
        }

        else if (GameManager.Instance.atTableName == "Table3") // string must match Inspector, DiningRoom/DiningTableX/inServiceAreaTrigger/TableIAm.cs
        {
            GameManager.Instance.isDoneServingTable3 = true;
            GameManager.Instance.isReadyForNewOrderTable3 = true;
            ResetTicket();
            EvaluateDeliveryBeforeStartingNextOrder();
        }
    }

    private void ResetTicket()
    {
        orderTicketBackground.gameObject.SetActive(false);
        orderTicket.gameObject.SetActive(false);
        
        isOnTimer = false;
        if (isLate)
        {            
            GameManager.Instance.isLateTableCount -= 1;
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

