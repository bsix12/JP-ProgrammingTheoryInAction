using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    ////////////////////////////
    /// Assign In Inspector
    ////////////////////////////
    
    public GameObject orderTicketTable1;
    public GameObject orderTicketTable2;
    public GameObject orderTicketTable3;

    public TextMeshProUGUI orderTicketTextUITable1; // UI display element
    public TextMeshProUGUI orderTicketTextUITable2;
    public TextMeshProUGUI orderTicketTextUITable3;

    public TextMeshProUGUI timeElapsedTextUITable1; // UI display element
    public TextMeshProUGUI timeElapsedTextUITable2;
    public TextMeshProUGUI timeElapsedTextUITable3;

    public TextMeshProUGUI maximumOrderScorePossibleTextUITable1; // UI display element
    public TextMeshProUGUI maximumOrderScorePossibleTextUITable2;
    public TextMeshProUGUI maximumOrderScorePossibleTextUITable3;
    
    public Image orderTicketBackgroundTable1; // UI display element on 2nd canvas, background image for order ticket
    public Image orderTicketBackgroundTable2;
    public Image orderTicketBackgroundTable3;

    public TextMeshPro orderTextTable1; // Scene text, orders billboard 
    public TextMeshPro orderTextTable2;
    public TextMeshPro orderTextTable3;

    public GameObject[] guestsTable1; // Scene objects
    public GameObject[] guestsTable2;
    public GameObject[] guestsTable3;

    ////////////////////////////
    
    public List<GameObject> onPlateGameObjectsTable1 = new List<GameObject>();
    public List<GameObject> onPlateGameObjectsTable2 = new List<GameObject>();
    public List<GameObject> onPlateGameObjectsTable3 = new List<GameObject>();

    public bool isReadyForNewOrderTable1 = false;
    public bool isReadyForNewOrderTable2 = false;
    public bool isReadyForNewOrderTable3 = false;

    public bool isDoneServingTable1 = false; // allows table to accept food served
    public bool isDoneServingTable2 = false;
    public bool isDoneServingTable3 = false;

    public bool isOnTimerTable1;
    public bool isOnTimerTable2;
    public bool isOnTimerTable3;

    public bool isLateTable1;
    public bool isLateTable2;
    public bool isLateTable3;

    public float timeElapsedTable1 = 0; // timer data
    public float timeElapsedTable2;
    public float timeElapsedTable3;

    private List<string> _onlyFoodOrderedNamesTable1 = new List<string>();
    private List<string> _onlyFoodOrderedNamesTable2 = new List<string>();
    private List<string> _onlyFoodOrderedNamesTable3 = new List<string>();

    private List<int> _guestsIndexList;
    
    private float _timeRemainingUntilNextOrderTable1;
    private float _timeRemainingUntilNextOrderTable2;
    private float _timeRemainingUntilNextOrderTable3;

    private float _timeGuestsStayAtTable1;
    private float _timeGuestsStayAtTable2;
    private float _timeGuestsStayAtTable3;
    
    private float _secondsOfPrepAllowedPerMain = 90;    // 90 seconds per main makes one table trivial, three tables difficult?
                                                        // testing required.  could make time allowed vary with total amount
                                                        // of mains and vary with number of tables active
                                                        // For now just trying a fixed number to try and set 'easy, medium, hard'

    private int _maximumOrderScorePossibleTable1;
    private int _maximumOrderScorePossibleTable2;
    private int _maximumOrderScorePossibleTable3;
    
    private int _chickenOrderedTable1;
    private int _beefRareOrderedTable1;
    private int _beefMediumOrderedTable1;
    private int _beefWellDoneOrderedTable1;
    private int _carrotsSteamedOrderedTable1;
    private int _broccoliSteamedOrderedTable1;
    private int _saladsOrderedTable1;
    private int _mainDishesOrderedTable1;
                                

    private void Start()
    {
        Instance = this;
        orderTicketTextUITable1.text = "";
    }

    private void Update()
    {
        TimeRemainingUntilNextOrder();
        OrderPrepElapsedTimeByTable();
    }


    //////////////////////////////////////////////////////////////////////////
    /// Generate and Publish New Order
    //////////////////////////////////////////////////////////////////////////
    
       
    public void GenerateOrderDetailsTable1()
    {
        if (isDoneServingTable1)
        {
            GameManager.Instance.GenerateOrderDetails(); // GameManagerGenerates the order              
            StoreOrderDetailsTable1();
            
            GameManager.Instance.ResetAfterOrderManagerStoresDetailsForTable();
            SeatGuestsTable1();
            PublishOrderTable1();
            
            _onlyFoodOrderedNamesTable1.Clear();
            
            isOnTimerTable1 = true;       
            isDoneServingTable1 = false;
        }

        else if (isReadyForNewOrderTable2)
        {
            Debug.Log("future stuff for table2");
        }
        else if (isReadyForNewOrderTable3)
        {
            Debug.Log("future stuff for table3");
        }
    }


    private void StoreOrderDetailsTable1()
    {
        _onlyFoodOrderedNamesTable1 = GameManager.Instance.onlyFoodOrderedNames; // store results for Table1
        _maximumOrderScorePossibleTable1 = GameManager.Instance.maximumOrderScorePossible; // store results for Table1

        _chickenOrderedTable1 = GameManager.Instance.chickenOrdered;
        _beefRareOrderedTable1 = GameManager.Instance.beefRareOrdered;
        _beefMediumOrderedTable1 = GameManager.Instance.beefMediumOrdered;
        _beefWellDoneOrderedTable1 = GameManager.Instance.beefWellDoneOrdered;
        _carrotsSteamedOrderedTable1 = GameManager.Instance.carrotsSteamedOrdered;
        _broccoliSteamedOrderedTable1 = GameManager.Instance.broccoliSteamedOrdered;
        _saladsOrderedTable1 = GameManager.Instance.saladsOrdered;
        _mainDishesOrderedTable1 = _chickenOrderedTable1 + _beefRareOrderedTable1 + _beefMediumOrderedTable1 + _beefWellDoneOrderedTable1;
    }
    
    
    // SetActive appropriate number of guests randomly from preset options.  Visually varied even with same number of guests.
    private void SeatGuestsTable1()
    {
        int _numberOfGuests = _mainDishesOrderedTable1;
        _guestsIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        Debug.Log("(4)  how many in guestIndexList: " + _guestsIndexList.Count);
        
        for (int i = 0; i < _numberOfGuests; i++)
        {
            int _random = Random.Range(0, _guestsIndexList.Count - 1); 
            Debug.Log("(5)  Loop Cycle i: " + i + ", IndexSelected: " + _random);

            guestsTable1[_guestsIndexList[_random]].gameObject.SetActive(true);
            _guestsIndexList.RemoveAt(_random); // needs to be non-repeating random index
        }
    }


    private void PublishOrderTable1()
    {
        orderTextTable1.text = ""; // clear the 'waiting for new order'
        orderTicketBackgroundTable1.gameObject.SetActive(true);
        orderTicketTable1.gameObject.SetActive(true);
        maximumOrderScorePossibleTextUITable1.text = _maximumOrderScorePossibleTable1.ToString() + " points possible";

        for (int i = 0; i < _onlyFoodOrderedNamesTable1.Count; i++)
        {
            orderTextTable1.text += _onlyFoodOrderedNamesTable1[i] + "\n";
            orderTicketTextUITable1.text += _onlyFoodOrderedNamesTable1[i] + "\n";
        }
    }


    //////////////////////////////////////////////////////////////////////////
    /// Monitor and Tracking Actions
    //////////////////////////////////////////////////////////////////////////

    private void OrderPrepElapsedTimeByTable()
    {
        timeElapsedTextUITable1.color = Color.black;

        if (isOnTimerTable1)
        {
            timeElapsedTable1 += Time.deltaTime;
                        
            string _roundTime = Mathf.Floor(timeElapsedTable1 / 60).ToString("0") + ":" + Mathf.FloorToInt(timeElapsedTable1 % 60).ToString("00");
            timeElapsedTextUITable1.text = _roundTime;
            
            if(timeElapsedTable1 > _mainDishesOrderedTable1 * _secondsOfPrepAllowedPerMain)
            {
                timeElapsedTextUITable1.color = Color.red;
                isLateTable1 = true;
            }
        }
    }


    //////////////////////////////////////////////////////////////////////////
    /// After Player Makes Delivery
    //////////////////////////////////////////////////////////////////////////

    public void AfterFoodIsServedActions()
    {
        if (GameManager.Instance.atTableName == "Table1") // this string needs to match Inspector, DiningRoom/DiningTableX/inServiceAreaTrigger/ServeFood.cs
        {
            //Debug.Log("OrderManager AfterFoodIsServedActions called");
            orderTicketBackgroundTable1.gameObject.SetActive(false);
            orderTicketTable1.gameObject.SetActive(false);
            isDoneServingTable1 = true;
            isReadyForNewOrderTable1 = true;
            isOnTimerTable1 = false;
            isLateTable1 = false;
            
            timeElapsedTable1 = 0;
            _maximumOrderScorePossibleTable1 = 0;

            orderTextTable1.text = "Next Order Soon";
            orderTicketTextUITable1.text = "";
            maximumOrderScorePossibleTextUITable1.text = "----";
            
            GameManager.Instance.chickenOrdered = _chickenOrderedTable1; // AfterFoodIsServedActions() will compare Dilvered versus OrderedTable1 
            GameManager.Instance.beefRareOrdered = _beefRareOrderedTable1;
            GameManager.Instance.beefMediumOrdered = _beefMediumOrderedTable1;
            GameManager.Instance.beefWellDoneOrdered = _beefWellDoneOrderedTable1;
            GameManager.Instance.carrotsSteamedOrdered = _carrotsSteamedOrderedTable1;
            GameManager.Instance.broccoliSteamedOrdered = _broccoliSteamedOrderedTable1;
            GameManager.Instance.saladsOrdered = _saladsOrderedTable1;

            _chickenOrderedTable1 = 0;
            _beefRareOrderedTable1 = 0;
            _beefMediumOrderedTable1 = 0;
            _beefWellDoneOrderedTable1 = 0;
            _carrotsSteamedOrderedTable1 = 0;
            _broccoliSteamedOrderedTable1 = 0;
            _saladsOrderedTable1 = 0;

            GameManager.Instance.AfterFoodIsServedActions();
            BeginNewOrderTable1();
            
        }

        if (GameManager.Instance.atTableName == "Table2")
        {
            Debug.Log("Table2 delivery actions to be inititated");
        }

        if (GameManager.Instance.atTableName == "Table3")
        {
            Debug.Log("Table3 delivery actions to be inititated");
        }
    }

    //////////////////////////////////////////////////////////////////////////
    /// Manage Next Order Timing
    //////////////////////////////////////////////////////////////////////////

    public void BeginNewOrderTable1()
    {
        _timeRemainingUntilNextOrderTable1 = Random.Range(5f, 15f); // debug timing ////////////////////////////////////////////////////////////////////
        _timeGuestsStayAtTable1 = _timeRemainingUntilNextOrderTable1 * .8f; // this is percent of delay that diners and food will remain at table before being cleared
        Invoke("GuestsLeaveAndClearTable1", _timeGuestsStayAtTable1);
    }

    private void TimeRemainingUntilNextOrder()
    {
        if (GameManager.Instance.isActiveTable1 && isReadyForNewOrderTable1)
        {
            if (_timeRemainingUntilNextOrderTable1 > 0)
            {
                _timeRemainingUntilNextOrderTable1 -= Time.deltaTime;
            }

            if (_timeRemainingUntilNextOrderTable1 <= 0)
            {
                isReadyForNewOrderTable1 = false; //////////////////////////////// CHANGED
                Debug.Log("(2)  GenerateOrderDetailsTable1");
                GenerateOrderDetailsTable1();
            }
        }
    }

    private void GuestsLeaveAndClearTable1()
    {
        for (int i = 0; i < onPlateGameObjectsTable1.Count; i++)
        {
            Destroy(onPlateGameObjectsTable1[i].gameObject); // added .gameObject at end like wasteStation
        }

        for (int i = 0; i < guestsTable1.Length; i++)
        {
            guestsTable1[i].gameObject.SetActive(false);
        }

        _mainDishesOrderedTable1 = 0;
        onPlateGameObjectsTable1.Clear();   // list of GameObjects
        GameManager.Instance.reportCardToPost.Clear();          // list of strings
    }
}
