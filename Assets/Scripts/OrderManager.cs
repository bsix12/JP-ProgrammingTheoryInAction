using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    public bool isReadyForNewOrderTable1 = false;
    public bool isReadyForNewOrderTable2 = false;
    public bool isReadyForNewOrderTable3 = false;

    public bool isDoneServingTable1 = false; // allows table to accept food served
    public bool isDoneServingTable2 = false;
    public bool isDoneServingTable3 = false;

    public List<string> _onlyFoodOrderedTable1 = new List<string>();
    public List<string> _onlyFoodOrderedTable2 = new List<string>();
    public List<string> _onlyFoodOrderedTable3 = new List<string>();

    public float timeElapsedTable1; // timer data
    public float timeElapsedTable2;
    public float timeElapsedTable3;

    public bool isOnTimerTable1;
    public bool isOnTimerTable2;
    public bool isOnTimerTable3;

    public bool isLateTable1;
    public bool isLateTable2;
    public bool isLateTable3;


    public Image orderTicketBackgroundTable1;
    public Image orderTicketBackgroundTable2;
    public Image orderTicketBackgroundTable3;

    public TextMeshProUGUI orderTicketTextUITable1; // UI display element
    public TextMeshProUGUI orderTicketTextUITable2;
    public TextMeshProUGUI orderTicketTextUITable3;

    public TextMeshProUGUI timeElapsedTextUITable1; // UI display element
    public TextMeshProUGUI timeElapsedTextUITable2;
    public TextMeshProUGUI timeElapsedTextUITable3;

    public TextMeshProUGUI maxScorePossibleTextUITable1; // UI display element
    public TextMeshProUGUI maxScorePossibleTextUITable2;
    public TextMeshProUGUI maxScorePossibleTextUITable3;

    public TextMeshPro orderTextTable1;  // Scene text, orders billboard 
    public TextMeshPro orderTextTable2;
    public TextMeshPro orderTextTable3;

    public GameObject[] dinersTable1;
    public GameObject[] dinersTable2;
    public GameObject[] dinersTable3;

    private List<int> _dinersIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };

    public bool dinersClearedTable1 = false;
    private bool _dinersClearedTable2;
    private bool _dinersClearedTable3;

    [SerializeField] private float _delayUntilNextOrderTable1;
    private float _delayUntilNextOrderTable2;
    private float _delayUntilNextOrderTable3;

    [SerializeField] private float _nonEatTimeTable1;
    private float _nonEatTimeTable2;
    private float _nonEatTimeTable3;

    private int _maxScorePossibleTable1;
    private int _maxScorePossibleTable2;
    private int _maxScorePossibleTable3;
    
    private int _chickenOrderedTable1;
    private int _beefRareOrderedTable1;
    private int _beefMediumOrderedTable1;
    private int _beefWellDoneOrderedTable1;
    private int _carrotsSteamedOrderedTable1;
    private int _broccoliSteamedOrderedTable1;
    private int _saladsOrderedTable1;

    private int _mainDishesOrderedTable1;
    private float _secondsAllowedPerMain = 90;  // 90 seconds per main makes one table trivial, three tables difficult?
                                                // testing required.  could make time allowed vary with total amount
                                                // of mains and vary with number of tables active
                                                // For now just trying a fixed number to try and set 'easy, medium, hard'

    private void Start()
    {
        Instance = this;
        isReadyForNewOrderTable1 = true;
    }

    private void Update()
    {
        TimeRemainingUntilNextOrder();
        OrderPrepElapsedTimeByTable();
        ResetTable1();

        if (GameManager.Instance.isActiveTable1 && _delayUntilNextOrderTable1 <= 0)
        {
            GetNewOrderByTable();
        }
    }

    public void GetNewOrderByTable()
    {
        if (isDoneServingTable1)
        {
            isReadyForNewOrderTable1 = true;
        }

        if (isReadyForNewOrderTable1)
        {
            GameManager.Instance.GenerateNewOrder(); // GameManagerGenerates the order
            
            orderTextTable1.text = ""; // clear stored info from previous order
            orderTicketTextUITable1.text = "";
            timeElapsedTable1 = 0;
            
            _onlyFoodOrderedTable1 = GameManager.Instance.onlyFoodOrderedNames; // store results for Table1
            _maxScorePossibleTable1 = GameManager.Instance.maxScorePossible; // store results for Table1
            maxScorePossibleTextUITable1.text = _maxScorePossibleTable1.ToString() + " points possible";
            
            orderTicketBackgroundTable1.gameObject.SetActive(true);
            orderTicketTextUITable1.gameObject.SetActive(true);
            PublishOrderTable1();
                                    
            _chickenOrderedTable1 = GameManager.Instance.chickenOrdered; 
            _beefRareOrderedTable1 = GameManager.Instance.beefRareOrdered;
            _beefMediumOrderedTable1 = GameManager.Instance.beefMediumOrdered;
            _beefWellDoneOrderedTable1 = GameManager.Instance.beefWellDoneOrdered;
            _carrotsSteamedOrderedTable1 = GameManager.Instance.carrotsSteamedOrdered;
            _broccoliSteamedOrderedTable1 = GameManager.Instance.broccoliSteamedOrdered;
            _saladsOrderedTable1 = GameManager.Instance.saladsOrdered;
            _mainDishesOrderedTable1 = _chickenOrderedTable1 + _beefRareOrderedTable1 + _beefMediumOrderedTable1 + _beefWellDoneOrderedTable1;
            SeatDinersTable1();

            
            isOnTimerTable1 = true;
            isReadyForNewOrderTable1 = false; // prevent Update() actions including GetNewOrder until delivery against current order has been served 
            isDoneServingTable1 = false; // toggle - allows the now NewOrder to be delivered to table
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

    private void SeatDinersTable1()
    {
        int _diners = _mainDishesOrderedTable1;
        
        for (int i = 0; i<_diners; i++)
        {
            int _random = Random.Range(0, _dinersIndexList.Count - 1);

            dinersTable1[_dinersIndexList[_random]].gameObject.SetActive(true);
            _dinersIndexList.RemoveAt(_random);
        }

        ResetDinerIndexList();        
    }

    private void ResetDinerIndexList()
    {
        _dinersIndexList.Clear();

        for (int i = 0; i < 8; i++)
        {
            _dinersIndexList.Add(i);
        }
    }

    private void ResetTable1()
    {
        
        if(dinersClearedTable1 == false && _nonEatTimeTable1 > _delayUntilNextOrderTable1)
        {
            for (int i = 0; i < GameManager.Instance.onPlateGameObjectsTable1.Count; i++)
            {
                Destroy(GameManager.Instance.onPlateGameObjectsTable1[i].gameObject); // added .gameObject at end like wasteStation
            }
            
            for (int i = 0; i < dinersTable1.Length; i++)
            {
                dinersTable1[i].gameObject.SetActive(false);
            }            

            dinersClearedTable1= true;           
        }
    }

    private void PublishOrderTable1()
    {
        for (int i = 0; i < _onlyFoodOrderedTable1.Count; i++)
        {
            orderTextTable1.text += _onlyFoodOrderedTable1[i] + "\n";
            orderTicketTextUITable1.text += _onlyFoodOrderedTable1[i] + "\n";
        }
    }

    private void OrderPrepElapsedTimeByTable()
    {
        timeElapsedTextUITable1.color = Color.black;

        if (isOnTimerTable1)
        {
            timeElapsedTable1 += Time.deltaTime;
            string _roundTime = Mathf.Floor(timeElapsedTable1 / 60).ToString("0") + ":" + Mathf.FloorToInt(timeElapsedTable1 % 60).ToString("00");
            timeElapsedTextUITable1.text = _roundTime;

            if(timeElapsedTable1 > _mainDishesOrderedTable1 * _secondsAllowedPerMain)
            {
                timeElapsedTextUITable1.color = Color.red;
                isLateTable1 = true;
            }
        }
    }

    public void GenerateNextOrderDelay()
    {
        _delayUntilNextOrderTable1 = Random.Range(5f, 15f); // debug timing ////////////////////////////////////////////////////////////////////
        _nonEatTimeTable1 = _delayUntilNextOrderTable1 * .2f; // this is percent of delay that diners and food will remain at table before being cleared
    }

    private void TimeRemainingUntilNextOrder()
    {
        if (isReadyForNewOrderTable1 && _delayUntilNextOrderTable1 > 0)
        {
            _delayUntilNextOrderTable1 -= Time.deltaTime;
        }
    }

    public void AfterFoodIsServedActions()
    {
        if (GameManager.Instance.atTableName == "Table1") // this string needs to match Inspector, DiningRoom/DiningTableX/inServiceAreaTrigger/ServeFood.cs
        {
            //Debug.Log("OrderManager AfterFoodIsServedActions called");
            isDoneServingTable1 = true;
            isOnTimerTable1 = false;
            isReadyForNewOrderTable1 = true;
            isLateTable1 = false;
            orderTicketBackgroundTable1.gameObject.SetActive(false);
            orderTicketTextUITable1.gameObject.SetActive(false);
            GameManager.Instance.chickenOrdered = _chickenOrderedTable1; // AfterFoodIsServedActions() will compare Dilvered versus OrderedTable1 
            GameManager.Instance.beefRareOrdered = _beefRareOrderedTable1;
            GameManager.Instance.beefMediumOrdered = _beefMediumOrderedTable1;
            GameManager.Instance.beefWellDoneOrdered = _beefWellDoneOrderedTable1;
            GameManager.Instance.carrotsSteamedOrdered = _carrotsSteamedOrderedTable1;
            GameManager.Instance.broccoliSteamedOrdered = _broccoliSteamedOrderedTable1;
            GameManager.Instance.saladsOrdered = _saladsOrderedTable1;

            GameManager.Instance.AfterFoodIsServedActions();
            GenerateNextOrderDelay();
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
}
