using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    ////////////////////////////
    /// Assign In Inspector
    ////////////////////////////
    
    public TextMeshProUGUI scoreText;
    
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI credits;
    //public TextMeshProUGUI notes;
    public TextMeshProUGUI buttonTable1Text;
    public TextMeshProUGUI buttonTable2Text;
    public TextMeshProUGUI buttonTable3Text;
    public TextMeshProUGUI scoreFlyUpToCorner;
    public TextMeshProUGUI perfectFlyUp;
    public TextMeshProUGUI penaltyFlyUp;
    public TextMeshPro highScoreText;
    public TextMeshPro reportCardText;
    public TextMeshPro guestChefName;

    public Button foodGuideButton;
    public Button notesButton;

    public Image foodGuideBackground;
    public Image creditsBackground;
    public Image notesBackground;
    public Image notesImage;
 
    public GameObject kitchenDoor;
    public GameObject reservedTable1;
    public GameObject reservedTable2;
    public GameObject reservedTable3;
    public GameObject orderBoardClosedTable1;
    public GameObject orderBoardClosedTable2;
    public GameObject orderBoardClosedTable3;
    public GameObject smashedFoodContainer;
    public GameObject TablesUICanvas;
    public GameObject underlineTable1;
    public GameObject foodGuide;
    public GameObject statsButton;
    public GameObject statsCanvas;
    public GameObject notesLeftArrow;
    public GameObject notesRightArrow;


    public AudioSource playerAudioSource;
    public AudioClip perfectDeliveryAudio;
    public AudioClip positiveDeliveryAudio;
    public AudioClip negativeDeliveryAudio;
    public AudioClip buttonClick;

    ////////////////////////////

    public List<GameObject> readyToServeGameObjects = new List<GameObject>();
    public List<GameObject> notesPagesListObjects = new List<GameObject>();
    public List<string> onlyFoodOrderedNames = new List<string>();
    public List<string> foodServedNames = new List<string>();
    public List<string> reportCardToPost = new List<string>();
    
    public Vector3 serveTableLocation;
        
    public string atTableName; // reference to which table player is at
    public string lastTableServedName;
    
    public float secondsOfPrepAllowedPerMain = 90;
    public float secondsToWaitBeforeClearMessage;

    public int isOpenNowTableCount = 0;
    public int isLateNowTableCount = 0;
    public int numberOfNowSeatedGuests = 0;
    public int maximumOrderScorePossible;
    public int chickenOrdered;
    public int beefRareOrdered;
    public int beefMediumOrdered;
    public int beefWellDoneOrdered;
    public int carrotsSteamedOrdered;
    public int broccoliSteamedOrdered;
    public int saladsOrdered;
    public int smashedFoodPenalty = 100; // play test value, should vary for mains versus sides
    public int wastedFoodPenalty = 50; // play test value, should vary for mains versus sides
    public int wasOnFloorAndServedPenalty = 150;
    


    public bool isGameStarted;
    public bool isActiveTable1 = false;
    public bool isActiveTable2 = false;
    public bool isActiveTable3 = false;
    public bool isGeneratingOrderTable1 = false; // order generator can only be used by one table at a time
    public bool isGeneratingOrderTable2 = false;
    public bool isGeneratingOrderTable3 = false;
    public bool isReadyForNewOrderTable1 = true;
    public bool isReadyForNewOrderTable2 = true;
    public bool isReadyForNewOrderTable3 = true;
    public bool isDoneServingTable1 = true;
    public bool isDoneServingTable2 = true;
    public bool isDoneServingTable3 = true;
    public bool isWaitingToCloseTable1 = false;
    public bool isWaitingToCloseTable2 = false;
    public bool isWaitingToCloseTable3 = false;

    public bool isMovementTutorialActive = true;
    public bool isMoveWithWSUnlocked = false;
    public bool isMoveWithADUnlocked = false;
    public bool isMoveWithQEUnlocked = false;
    public bool isFirstTimeTriggeringDispenser = true;
    public bool isFirstTimeSpinningPlatter = true;
    public bool isFirstTimeAtCookStation = true;
    public bool isFirstTimeServingCustomers = true;
 
    public bool isCalculatingScore = false;   
    public bool isInServiceArea = false;
    public bool playerInDiningRoom = false;
    public bool canDispense = true;
    public bool isUsingScrubbi = false;

    ////////////////////////////
 
    private TextMeshProUGUI _buttonFoodGuideText;

    private List<string> _menuMainsNames = new List<string>();
    private List<string> _menuSidesNames = new List<string>();
    private List<string> _includesNoneFoodOrderedNames = new List<string>();

    private bool _isFirstTimeOpeningDiningRoom = true;
    private bool _isFirstTimeClosingTableWithGuestsSeated = true;
    private bool _isActiveStats = false;
    private bool _isActiveCredits = false;
    private bool _isActiveNotes = false;
    private bool _isActiveFoodGuide = false;
    private bool _mustClean = false;
    private bool _wasPerfectDelivery = false;
    private bool _isNotesImageActive = false;
   
    private int _page;
    private int _perSecondPenaltyIfLate = 5; // play test value
    
    private float _oncePerSecond = 1f;

    [SerializeField] private int _chickenRawServed;
    [SerializeField] private int _chickenCookedServed;
    [SerializeField] private int _chickenRuinedServed;
    [SerializeField] private int _beefRawServed;
    [SerializeField] private int _beefRareServed;
    [SerializeField] private int _beefMediumServed;
    [SerializeField] private int _beefWellDoneServed;
    [SerializeField] private int _beefRuinedServed;
    [SerializeField] private int _carrotsRawServed;
    [SerializeField] private int _carrotsSteamedServed;
    [SerializeField] private int _carrotsRuinedServed;
    [SerializeField] private int _broccoliRawServed;
    [SerializeField] private int _broccoliSteamedServed;
    [SerializeField] private int _broccoliRuinedServed;
    [SerializeField] private int _saladsGoodServed;
    [SerializeField] private int _saladsRuinedServed;
    private int _rawFoodServedCount = 0;
    private int _ruinedFoodServedCount = 0;
    private int _wasOnFloorAndServedCount = 0;
    [SerializeField] private int _orderScore;
    [SerializeField] private int _totalScore;

    private TableTracker _table1;
    private TableTracker _table2;
    private TableTracker _table3;


    private void Start()
    {
        Instance = this;
        
        _table1 = GameObject.Find("TablesManager").GetComponent<Table1>();
        _table2 = GameObject.Find("TablesManager").GetComponent<Table2>();
        _table3 = GameObject.Find("TablesManager").GetComponent<Table3>();
        guestChefName.text = DataStorage.Instance.playerNameInputData;
        highScoreText.text = DataStorage.Instance.bestPlayerData + "\n\n with score of:\n" + DataStorage.Instance.highScoreData;
        _orderScore = 0;
        isGameStarted = true;
        canDispense = true;
        
        LoadFoodMenu();
        KitchenDoorControl();
        StartCoroutine(BeginTutorial());
    }


    private void Update()
    {
        ApplyPerSecondPenaltyIfLate();
        MonitorSmashedFoodContainer();
    }

    IEnumerator BeginTutorial()
    {
        messageText.text = "Welcome to our kitchen!";
        yield return new WaitForSeconds(1.5f);
        isMoveWithADUnlocked = true;
        messageText.text = "Use <u>A</u> or <u>D</u> to rotate around.";
    }

    public void EnableDiningTablesUI()
    {
        //diningTablesUIBackground.gameObject.SetActive(true);
        TablesUICanvas.gameObject.SetActive(true);
        
        buttonTable1Text = GameObject.Find("OpenTable1ButtonText").GetComponent<TextMeshProUGUI>();
        buttonTable2Text = GameObject.Find("OpenTable2ButtonText").GetComponent<TextMeshProUGUI>();
        buttonTable3Text = GameObject.Find("OpenTable3ButtonText").GetComponent<TextMeshProUGUI>();
        buttonTable1Text.color = Color.red;
        buttonTable2Text.color = Color.red;
        buttonTable3Text.color = Color.red;
    }

    public void EnableFoodGuide()
    {
        messageText.text = "A food guide is available for reference.";
        StartCoroutine(ClearMessage(5));
        foodGuideButton.gameObject.SetActive(true);
        _buttonFoodGuideText = GameObject.Find("FoodGuideButtonText").GetComponent<TextMeshProUGUI>();
    }

    
    public void ApplySmashedFoodPenalty()
    {
        _totalScore -= smashedFoodPenalty;
        penaltyFlyUp.text = smashedFoodPenalty.ToString() + ", Mess";
        StartCoroutine(PenaltyFlyUp());
        UpdateScore();
    }

    public void ApplyWastedFoodPenalty()
    {
        _totalScore -= wastedFoodPenalty;
        penaltyFlyUp.text = wastedFoodPenalty.ToString() + ", Waste";
        StartCoroutine(PenaltyFlyUp());
        UpdateScore();
    }

    private void LoadFoodMenu()
    {
        _menuMainsNames.Add("Chicken");
        _menuMainsNames.Add("Beef: Rare");
        _menuMainsNames.Add("Beef: Medium");
        _menuMainsNames.Add("Beef: Well-Done");

        _menuSidesNames.Add("Steamed Carrots");
        _menuSidesNames.Add("Steamed Broccoli");
        //_menuSides.Add("Garden Salad");
    }

    //////////////////////////////////////////////////////////////////////////
    /// Generate New Order Details
    ///     this method is called whenever any table needs a new order
    //////////////////////////////////////////////////////////////////////////

    public void GenerateOrderForTable()
    {
        if (isActiveTable1 && isDoneServingTable1 && isGeneratingOrderTable1)
        {
            _table1.GenerateAndPublishOrderDetails();
            isDoneServingTable1 = false;
            isGeneratingOrderTable1 = false;
        }

        if (isActiveTable2 && isDoneServingTable2 && isGeneratingOrderTable2)
        {
            _table2.GenerateAndPublishOrderDetails();
            isDoneServingTable2 = false;
            isGeneratingOrderTable2 = false;
        }

        if (isActiveTable3 && isDoneServingTable3 && isGeneratingOrderTable3)
        {
            _table3.GenerateAndPublishOrderDetails();
            isDoneServingTable3 = false;
            isGeneratingOrderTable3 = false;
        }
    }


    public void GenerateOrderDetails()
    {
        PickMenuItemsAndQuantities();
        SummarizeOnlyFoodOrdered();
        CalculateMaximumOrderScorePossible();
        //UpdateScore();
    }
    

    public void PickMenuItemsAndQuantities()
    {
        int mains = 0;
        int sides = 0;
        int salads = 0;
        int minMains = 2;
        int maxMains = 8;   // the number of diners at each table is modeled for up to 8 people
        int sideSelectedIndex;
        int mainSelectedIndex;
        
        
        if(StatsTracker.Instance.ordersReceivedSession < 5)
        {
            mains = StatsTracker.Instance.ordersReceivedSession + 1;
            sides = StatsTracker.Instance.ordersReceivedSession + 1;
            salads = StatsTracker.Instance.ordersReceivedSession + 0;
        }

        else
        {
            mains = Random.Range(minMains, maxMains);
        }

        if (mains <= 4 && StatsTracker.Instance.ordersReceivedSession >= 5)
        {
            sides = Random.Range(mains - 1, mains + 2);
            salads = Random.Range(mains - 2, mains);
        }
        else if (mains > 4 && StatsTracker.Instance.ordersReceivedSession >= 5)
        {
            sides = Random.Range(mains - 2, mains + 4);
            salads = Random.Range(mains - 4, mains);
        }
        
        for (int i = 0; i < sides; i++)
        {
            sideSelectedIndex = Random.Range(0, _menuSidesNames.Count);
            _includesNoneFoodOrderedNames.Add(_menuSidesNames[sideSelectedIndex]);
        }

        for (int i = 0; i < mains; i++)
        {
            mainSelectedIndex = Random.Range(0, _menuMainsNames.Count);
            _includesNoneFoodOrderedNames.Add(_menuMainsNames[mainSelectedIndex]);
        }

        for (int i = 0; i < _includesNoneFoodOrderedNames.Count; i++)
        {
            if (_includesNoneFoodOrderedNames[i] == "Chicken")
            {
                chickenOrdered += 1;
                StatsTracker.Instance.chickenCookedOrderedSession += 1;
            }

            if (_includesNoneFoodOrderedNames[i] == "Beef: Rare")
            {
                beefRareOrdered += 1;
                StatsTracker.Instance.beefRareOrderedSession += 1;
            }

            if (_includesNoneFoodOrderedNames[i] == "Beef: Medium")
            {
                beefMediumOrdered += 1;
                StatsTracker.Instance.beefMediumOrderedSession += 1;
            }

            if (_includesNoneFoodOrderedNames[i] == "Beef: Well-Done")
            {
                beefWellDoneOrdered += 1;
                StatsTracker.Instance.beefWellDoneOrderedSession += 1;
            }

            if (_includesNoneFoodOrderedNames[i] == "Steamed Carrots")
            {
                carrotsSteamedOrdered += 1;
                StatsTracker.Instance.carrotsSteamedOrderedSession += 1;
            }

            if (_includesNoneFoodOrderedNames[i] == "Steamed Broccoli")
            {
                broccoliSteamedOrdered += 1;
                StatsTracker.Instance.broccoliSteamedOrderedSession += 1;
            }
        }
        
        saladsOrdered = salads; // salads are not randomly selected; base on quantity of Mains ordered
        StatsTracker.Instance.saladsOrderedSession += saladsOrdered;
    }


    private void SummarizeOnlyFoodOrdered()
    {        
        onlyFoodOrderedNames.Add("<b><u>Main Dishes</b></u>\n");

        if (chickenOrdered > 0)
        {
            onlyFoodOrderedNames.Add(chickenOrdered + " - Chicken\n");
        }

        if (beefRareOrdered > 0)
        {
            onlyFoodOrderedNames.Add(beefRareOrdered + " - Beef: Rare\n");
        }

        if (beefMediumOrdered > 0)
        {
            onlyFoodOrderedNames.Add(beefMediumOrdered + " - Beef: Medium\n");
        }

        if (beefWellDoneOrdered > 0)
        {
            onlyFoodOrderedNames.Add(beefWellDoneOrdered + " - Beef: Well-Done\n");
        }

        onlyFoodOrderedNames.Add("\n<b><u>Sides</b></u>\n");

        if (carrotsSteamedOrdered > 0)
        {
            onlyFoodOrderedNames.Add(carrotsSteamedOrdered + " - Steamed Carrots\n");
        }

        if (broccoliSteamedOrdered > 0)
        {
            onlyFoodOrderedNames.Add(broccoliSteamedOrdered + " - Steamed Broccoli\n");
        }

        if (saladsOrdered > 0)
        {
            onlyFoodOrderedNames.Add(saladsOrdered + " - Garden Salad\n");
        }
    }


    private void CalculateMaximumOrderScorePossible() // utilize the same methods called to score delivery
    {
        _chickenCookedServed = chickenOrdered; // set input for perfect delivery
        _beefRareServed = beefRareOrdered;
        _beefMediumServed = beefMediumOrdered;
        _beefWellDoneServed = beefWellDoneOrdered;
        _carrotsSteamedServed = carrotsSteamedOrdered;
        _broccoliSteamedServed = broccoliSteamedOrdered;
        _saladsGoodServed = saladsOrdered;

        _orderScore = 0; // reset score
        CalculateScore(); // call method to calculate score
        
        maximumOrderScorePossible = _orderScore; // store the max score
        _orderScore = 0; // reset score after storing max
    }


    //////////////////////////////////////////////////////////////////////////
    /// Reset 
    ///     note to future self: doing .Clear() on lists appeared to not work
    ///     new orders were added to previous orders and never got cleared
    ///     took awhile to realize these lists are generated from stored data
    ///     needed to reset all the input data to generate lists based only on current data
    //////////////////////////////////////////////////////////////////////////


    public void ResetAfterOrderManagerStoresDetailsForTable()
    {
        ResetStoredLists(); 
        ResetStoredInts();
        //ResetOrderReportText();
    }
   

    private void ResetStoredLists()
    {
        _includesNoneFoodOrderedNames.Clear(); // list of ints
    }


    private void ResetStoredInts()
    {
        maximumOrderScorePossible = 0;

        chickenOrdered = 0;
        beefRareOrdered = 0;
        beefMediumOrdered = 0;
        beefWellDoneOrdered = 0;
        carrotsSteamedOrdered = 0;
        broccoliSteamedOrdered = 0;
        saladsOrdered = 0;

        _chickenRawServed = 0;
        _chickenCookedServed = 0;
        _chickenRuinedServed = 0;
        _beefRawServed = 0;
        _beefRareServed = 0;
        _beefMediumServed = 0;
        _beefWellDoneServed = 0;
        _beefRuinedServed = 0;
        _carrotsRawServed = 0;
        _carrotsSteamedServed = 0;
        _carrotsRuinedServed = 0;
        _broccoliRawServed = 0;
        _broccoliSteamedServed = 0;
        _broccoliRuinedServed = 0;
        _saladsGoodServed = 0;
        _saladsRuinedServed = 0;

        _rawFoodServedCount = 0;
        _ruinedFoodServedCount = 0;
        _wasOnFloorAndServedCount = 0;
    }


    private void ResetOrderReportText() // this will get moved to OrderManager when reports for each table are implemented
    {
        
        reportCardText.text = "";
    }


    //////////////////////////////////////////////////////////////////////////
    /// Monitor and Tracking Actions
    //////////////////////////////////////////////////////////////////////////

    public void KitchenDoorControl()
    {


        if (isOpenNowTableCount > 0)
        {
            kitchenDoor.gameObject.SetActive(false); // this should be in more generic section
            
            if (_isFirstTimeOpeningDiningRoom)
            {
                messageText.text = "Guests will arrive soon and begin placing orders.\n\nOpen and close tables as needed, it's your kitchen today.";
                StartCoroutine(ClearMessage(5f));
                _isFirstTimeOpeningDiningRoom = false;
            }
      /*      else
            {
                messageText.text = "";
            }
      */
        }

        else if (isOpenNowTableCount == 0 && !playerInDiningRoom)
        {
            kitchenDoor.gameObject.SetActive(true);

            if (!isMovementTutorialActive)
            {
                messageText.text = "The dining room is now closed.\nTo begin seating guests, select tables to open.";
            }
        }
    }


    public void CloseTableThatWasWaitingToClose()
    {
        if (!isActiveTable1 && isWaitingToCloseTable1 && lastTableServedName == "Table1")
        {
            reservedTable1.gameObject.SetActive(true);
            orderBoardClosedTable1.gameObject.SetActive(true);
            isWaitingToCloseTable1 = false;
            isOpenNowTableCount -= 1;
            buttonTable1Text.text = "Closed";
        }

        else if (!isActiveTable2 && isWaitingToCloseTable2 && lastTableServedName == "Table2")
        {
            reservedTable2.gameObject.SetActive(true);
            orderBoardClosedTable2.gameObject.SetActive(true);
            isWaitingToCloseTable2 = false;
            isOpenNowTableCount -= 1;
            buttonTable2Text.text = "Closed";
        }

        else if (!isActiveTable3 && isWaitingToCloseTable3 && lastTableServedName == "Table3")
        {
            reservedTable3.gameObject.SetActive(true);
            orderBoardClosedTable3.gameObject.SetActive(true);
            isWaitingToCloseTable3 = false;
            isOpenNowTableCount -= 1;
            buttonTable3Text.text = "Closed";
        }
    }


    public IEnumerator ClearMessage(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        messageText.text = "";
    }

    
    private void ApplyPerSecondPenaltyIfLate() //////////// ADD up tabels late
    {
        if (isLateNowTableCount > 0 && _oncePerSecond > 0f) // 1 second timer
        {
            _oncePerSecond -= Time.deltaTime;
        }

        if (isLateNowTableCount > 0 && _oncePerSecond <= 0f)
        {
            _orderScore -= _perSecondPenaltyIfLate * isLateNowTableCount; // for each second late, apply a penalty, per table late
            scoreText.text = "Score: " + _orderScore.ToString();
            _oncePerSecond = 1f; // reset 1 second timer
        }
    }
       

    private void MonitorSmashedFoodContainer()
    {
        if (smashedFoodContainer.transform.childCount > 20 && !_mustClean)
        {
            canDispense = false;
            messageText.text = "This place is a mess...you must clean up!";
            _mustClean = true;
        }

        if (smashedFoodContainer.transform.childCount < 5 && _mustClean)
        {
            canDispense = true;
            messageText.text = "";
            _mustClean = false;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    /// After Player Makes Delivery
    //////////////////////////////////////////////////////////////////////////


    public void AfterFoodIsServedActions()
    {
        //Debug.Log("after food is served has been called");
        //DetermineWhichTableWasServed();
        CountFoodServed();
        CheckIfPerfectDelivery();
        CalculateScore();
        _orderScore -= _wasOnFloorAndServedCount* wasOnFloorAndServedPenalty;    // tacked on, probably should have a category like 'ruined' for food names.
                                                                                // needs to be outside of calculate score.  
        UpdateScore();
        StartCoroutine(ScoreFlyUp());       
        ResetOrderReportText();
        PostToKitchenReportCard();
        PostCustomerComments();  // future

        foodServedNames.Clear();
        ResetStoredInts();
        _wasPerfectDelivery = false;
        isCalculatingScore = false;
    }


    public void DetermineWhichTableWasServed()
    {
        if (atTableName == "Table1") // string must match Inspector, DiningRoom/DiningTableX/inServiceAreaTrigger/TableIAm.cs
        {
            isDoneServingTable1 = true;
            isReadyForNewOrderTable1 = true;            
        }

        else if (atTableName == "Table2") // string must match Inspector, DiningRoom/DiningTableX/inServiceAreaTrigger/TableIAm.cs
        {
            isDoneServingTable2 = true;
            isReadyForNewOrderTable2 = true;           
        }

        else if (atTableName == "Table3") // string must match Inspector, DiningRoom/DiningTableX/inServiceAreaTrigger/TableIAm.cs
        {
            isDoneServingTable3 = true;
            isReadyForNewOrderTable3 = true;            
        }
    }


    private void CountFoodServed()
    {

        StatsTracker.Instance.foodServedSession += foodServedNames.Count;

        //Debug.Log("CountFoodServed() has been called");
        for (int i = 0; i < foodServedNames.Count; i++)
        {
            if (foodServedNames[i] == "Raw Chicken")
            {
                _chickenRawServed += 1;
                StatsTracker.Instance.chickenRawServedSession += 1;
            }

            if (foodServedNames[i] == "Cooked Chicken")
            {
                _chickenCookedServed += 1;
                StatsTracker.Instance.chickenCookedServedSession += 1;
            }

            if (foodServedNames[i] == "Ruined Chicken")
            {
                _chickenRuinedServed += 1;
                StatsTracker.Instance.chickenRuinedServedSession += 1;
            }

            if (foodServedNames[i] == "Raw Beef")
            {
                _beefRawServed += 1;
                StatsTracker.Instance.beefRawServedSession += 1;
            }

            if (foodServedNames[i] == "Beef: Rare")
            {
                _beefRareServed += 1;
                StatsTracker.Instance.beefRareServedSession += 1;
            }

            if (foodServedNames[i] == "Beef: Medium")
            {
                _beefMediumServed += 1;
                StatsTracker.Instance.beefMediumServedSession += 1;
            }

            if (foodServedNames[i] == "Beef: Well-Done")
            {
                _beefWellDoneServed += 1;
                StatsTracker.Instance.beefWellDoneServedSession += 1;
            }

            if (foodServedNames[i] == "Ruined Beef")
            {
                _beefRuinedServed += 1;
                StatsTracker.Instance.beefRuinedServedSession += 1;
            }

            if (foodServedNames[i] == "Raw Carrots")
            {
                _carrotsRawServed += 1;
                StatsTracker.Instance.carrotsRawServedSession += 1;
            }

            if (foodServedNames[i] == "Steamed Carrots")
            {
                _carrotsSteamedServed += 1;
                StatsTracker.Instance.carrotsSteamedServedSession += 1;
            }

            if (foodServedNames[i] == "Ruined Carrots")
            {
                _carrotsRuinedServed += 1;
                StatsTracker.Instance.carrotsRuinedServedSession += 1;
            }

            if (foodServedNames[i] == "Raw Broccoli")
            {
                _broccoliRawServed += 1;
                StatsTracker.Instance.broccoliRawServedSession += 1;
            }

            if (foodServedNames[i] == "Steamed Broccoli")
            {
                _broccoliSteamedServed += 1;
                StatsTracker.Instance.broccoliSteamedServedSession += 1;
            }

            if (foodServedNames[i] == "Ruined Broccoli")
            {
                _broccoliRuinedServed += 1;
                StatsTracker.Instance.broccoliRuinedServedSession += 1;
            }

            if (foodServedNames[i] == "Salad")
            {
                _saladsGoodServed += 1;
                StatsTracker.Instance.saladsGoodServedSession += 1;
            }

            if (foodServedNames[i] == "Ruined Salad")
            {
                _saladsRuinedServed += 1;
                StatsTracker.Instance.saladsRuinedServedSession += 1;
            }
        }
        _rawFoodServedCount = _beefRawServed + _chickenRawServed + _broccoliRawServed + _carrotsRawServed;
        _ruinedFoodServedCount = _beefRuinedServed + _chickenRuinedServed + _broccoliRuinedServed + _carrotsRuinedServed + _saladsRuinedServed;
    }

    private void CheckIfPerfectDelivery()
    {
        if(_chickenCookedServed == chickenOrdered
            && _beefRareServed == beefRareOrdered
            && _beefMediumServed == beefMediumOrdered
            && _beefWellDoneServed == beefWellDoneOrdered
            && _carrotsSteamedServed == carrotsSteamedOrdered
            && _broccoliSteamedServed == broccoliSteamedOrdered 
            && _saladsGoodServed == saladsOrdered
            && _rawFoodServedCount == 0
            && _ruinedFoodServedCount == 0
            && _wasOnFloorAndServedCount == 0) // can't be perfect if served food off the floor
        {
            StatsTracker.Instance.perfectDeliveriesMadeSession += 1;
            _wasPerfectDelivery = true;
            StartCoroutine(PerfectFlyUp());
        }
    }


    public void CheckIfAnyHasBeenOnFloor()
    {
        for (int i = 0; i < readyToServeGameObjects.Count; i++)
        {
            if (readyToServeGameObjects[i].GetComponent<Food>().hasBeenOnFloor == true)
            {
                StatsTracker.Instance.wasOnFloorAndServedSession += 1;
                _wasOnFloorAndServedCount += 1;
            }
        }
    }


    private void PostToKitchenReportCard()
    {
        //Debug.Log("post to kitchen report card has been called");
        reportCardToPost.Add("<b><u>Kitchen Report Card</b></u>\n\n");

        if (carrotsSteamedOrdered > 0 && _carrotsSteamedServed == carrotsSteamedOrdered)
        {
            reportCardToPost.Add("Steamed Carrots - Pass\n");
        }

        if (carrotsSteamedOrdered > 0 && _carrotsSteamedServed != carrotsSteamedOrdered)
        {
            reportCardToPost.Add("Steamed Carrots - Fail\n");
        }

        if (broccoliSteamedOrdered > 0 && _broccoliSteamedServed == broccoliSteamedOrdered)
        {
            reportCardToPost.Add("Steamed Broccoli - Pass\n");
        }

        if (broccoliSteamedOrdered > 0 && _broccoliSteamedServed != broccoliSteamedOrdered)
        {
            reportCardToPost.Add("Steamed Broccoli - Fail\n");
        }

        if (saladsOrdered > 0 && _saladsGoodServed == saladsOrdered)
        {
            reportCardToPost.Add("Salads - Pass\n");
        }

        if (saladsOrdered > 0 && _saladsGoodServed != saladsOrdered)
        {
            reportCardToPost.Add("Salads - Fail\n");
        }

        if (chickenOrdered > 0 && _chickenCookedServed == chickenOrdered)
        {
            reportCardToPost.Add("Chicken - Pass\n");
        }

        if (chickenOrdered > 0 && _chickenCookedServed != chickenOrdered)
        {
            reportCardToPost.Add("Chicken - Fail\n");
        }

        if (beefRareOrdered > 0 && _beefRareServed == beefRareOrdered)
        {
            reportCardToPost.Add("Beef: Rare - Pass\n");
        }

        if (beefRareOrdered > 0 && _beefRareServed != beefRareOrdered)
        {
            reportCardToPost.Add("Beef: Rare - Fail\n");
        }

        if (beefMediumOrdered > 0 && _beefMediumServed == beefMediumOrdered)
        {
            reportCardToPost.Add("Beef: Medium - Pass\n");
        }

        if (beefMediumOrdered > 0 && _beefMediumServed != beefMediumOrdered)
        {
            reportCardToPost.Add("Beef: Medium - Fail\n");
        }

        if (beefWellDoneOrdered > 0 && _beefWellDoneServed == beefWellDoneOrdered)
        {
            reportCardToPost.Add("Beef: Well-Done - Pass\n");
        }

        if (beefWellDoneOrdered > 0 && _beefWellDoneServed != beefWellDoneOrdered)
        {
            reportCardToPost.Add("Beef: Well-Done - Fail\n");
        }

        if (_wasOnFloorAndServedCount > 0)
        {
            reportCardToPost.Add("You just served food that touched the floor!\n");
        }

        for (int i = 0; i < reportCardToPost.Count; i++)
        {
            reportCardText.text += reportCardToPost[i] + "\n";
        }
    }


    private void CalculateScore()
    {
        //Debug.Log("calculate score has been called");
        CalculateScoreForBeefServed();       //
        CalculateScoreForChickenServed();    // 
        CalculateScoreForCarrotsServed();    // ABSTRACTION - method names indicate actions, details in separate methods
        CalculateScoreForBroccoliServed();   // 
        CalculateScoreForSaladsServed();     //
    }


    private void CalculateScoreForBeefServed()
    {
        int _allPortionsServedAnyCondition = _beefRawServed + _beefRareServed + _beefMediumServed + _beefWellDoneServed + _beefRuinedServed;

        int _allPortionsOrdered = beefRareOrdered + beefMediumOrdered + beefWellDoneOrdered;
        int _perPortionCorrectScore = 100;
        int _largeOrder = 3;
        int _allPortionsCorrectBonus = 250;
        int _allPortionsCorrectBonusLarge = 500;

        int _extraPortionPenalty = -40;
        int _missingPortionPenalty = -125;
        int _rawPortionPenalty = -250;
        int _ruinedPortionPenalty = -75;
        bool _receivedAllCorrectBonus = false;

        // points for each correct portion
        if (_beefRareServed >= beefRareOrdered) // no credit for extra portions
        {
            _orderScore += beefRareOrdered * _perPortionCorrectScore;
        }

        if (_beefRareServed < beefRareOrdered) // even if not all portions served, credit for those that are correct    
        {
            _orderScore += _beefRareServed * _perPortionCorrectScore;
        }

        if (_beefMediumServed >= beefMediumOrdered) // no credit for extra portions
        {
            _orderScore += beefMediumOrdered * _perPortionCorrectScore;
        }

        if (_beefMediumServed < beefMediumOrdered) // even if not all portions served, credit for those that are correct    
        {
            _orderScore += _beefMediumServed * _perPortionCorrectScore;
        }

        if (_beefWellDoneServed >= beefWellDoneOrdered) // no credit for extra portions
        {
            _orderScore += beefWellDoneOrdered * _perPortionCorrectScore;
        }

        if (_beefWellDoneServed < beefWellDoneOrdered) // even if not all portions served, credit for those that are correct    
        {
            _orderScore += _beefWellDoneServed * _perPortionCorrectScore;
        }

        if (_beefRareServed == beefRareOrdered
            && _beefMediumServed == beefMediumOrdered
            && _beefWellDoneServed == beefWellDoneOrdered
            && _allPortionsOrdered !=0) // bonus not available if no portions ordered
        {
            if (_allPortionsOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _orderScore += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if(_allPortionsOrdered > 1)
            {
                _orderScore += _allPortionsCorrectBonus; // small order bonus
            }
            _receivedAllCorrectBonus = true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///  NEED TO SOLVE in CheckBeefPartials()
        ///   - correct total edible portions with none correct
        ///   - correct total edible portions with at least one correct
        ///
        /// if( ordered A+B+C == servedEdible A+B+C ) // this ensures only reviewing cases where the correct number of portions were served.  examine mix of what was served.
        /// {
        ///     if( ordered A > 0 && served A > 0) 
        ///     { 
        ///         return  // at least one portion of A was correct
        ///         // score for including some correct portions
        ///     }
        ///     
        ///     if(ordered B > 0 && served B > 0)
        ///     {
        ///         return  // at least one order of B was correct
        ///         // score for including some correct portions
        ///     }
        ///     
        ///     if(ordered C > 0 && served C > 0)
        ///     {
        ///         return // at least one order of C was correct
        ///         // score for including some correct portions
        ///     }
        ///     
        ///     // no portions were correct, good try  
        /// }   
        ///
        
        CheckBeefPartials(_allPortionsOrdered, _largeOrder, _receivedAllCorrectBonus); // ABSTRACTION.  Details in separate method.
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        if (_allPortionsOrdered > _allPortionsServedAnyCondition)
        {
            _orderScore += (_allPortionsOrdered - _allPortionsServedAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (_allPortionsOrdered < _allPortionsServedAnyCondition)
        {
            _orderScore += (_allPortionsServedAnyCondition - _allPortionsOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _orderScore += _beefRawServed * _rawPortionPenalty; // penalty for every raw portion served
        _orderScore += _beefRuinedServed * _ruinedPortionPenalty; // penalty for every Ruined portion served
    }


    private void CheckBeefPartials(int _allPortionsOrdered, int _largeOrder, bool _receivedAllCorrectBonus)
    {
        int _allPortionsServedEdible = _beefRareServed + _beefMediumServed + _beefWellDoneServed;
        int _goodTryMitigation = 25; // correct number of edible portions served, but none met order requirements  ex: 3 rare when 2 medium and 1 wellDone were ordered
        int _goodEdibleCountPartialCorrect = 50;
        int _goodEdibleCountPartialCorrectLarge = 100;

        if (_allPortionsServedEdible == _allPortionsOrdered && _allPortionsOrdered > 1 && !_receivedAllCorrectBonus)
        {
            if (beefRareOrdered > 0 && _beefRareServed > 0)
            {
                Debug.Log("correct number portions, partial correct, rare");
                if (_allPortionsOrdered >= _largeOrder)
                {
                    _orderScore += _goodEdibleCountPartialCorrectLarge;
                    return;
                }
                _orderScore += _goodEdibleCountPartialCorrect;
                return;
            }

            if (beefMediumOrdered > 0 && _beefMediumServed > 0)
            {
                Debug.Log("correct number portions, partial correct, medium");
                if (_allPortionsOrdered >= _largeOrder)
                {
                    _orderScore += _goodEdibleCountPartialCorrectLarge;
                    return;
                }
                _orderScore += _goodEdibleCountPartialCorrect;
                return;
            }

            if (beefWellDoneOrdered > 0 && _beefWellDoneServed > 0)
            {
                Debug.Log("correct number portions, partial correct, medium");
                if (_allPortionsOrdered >= _largeOrder)
                {
                    _orderScore += _goodEdibleCountPartialCorrectLarge;
                    return;
                }
                _orderScore += _goodEdibleCountPartialCorrect;
                return;
            }

            _orderScore += _goodTryMitigation;
            Debug.Log("correct number portions, all wrong");
        }
    }


    private void CalculateScoreForChickenServed()
    {
        int _allPortionsServedAnyCondition = _chickenCookedServed + _chickenRawServed + _chickenRuinedServed;
        int _perPortionCorrectScore = 100;
        int _largeOrder = 3;
        int _allPortionsCorrectBonus = 100;
        int _allPortionsCorrectBonusLarge = 300;
        int _extraPortionPenalty = -40;
        int _missingPortionPenalty = -125;
        int _rawPortionPenalty = -250;
        int _ruinedPortionPenalty = -75;

        // points for each correct portion
        if (_chickenCookedServed >= chickenOrdered) // no credit for extra portions
        {
            _orderScore += chickenOrdered * _perPortionCorrectScore;
        }

        if (_chickenCookedServed < chickenOrdered) // even if not all portions served, credit for those that are correct    
        {
            _orderScore += _chickenCookedServed * _perPortionCorrectScore;
        }

        if (_chickenCookedServed == chickenOrdered && chickenOrdered != 0) // bonus not available if no portions ordered
        {
            if(chickenOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _orderScore += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if (chickenOrdered > 1)
            {
                _orderScore += _allPortionsCorrectBonus; // small order bonus
            }                            
        }

        if (chickenOrdered > _allPortionsServedAnyCondition)
        {
            _orderScore += (chickenOrdered - _allPortionsServedAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (chickenOrdered < _allPortionsServedAnyCondition)
        {
            _orderScore += (_allPortionsServedAnyCondition - chickenOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _orderScore += _chickenRawServed * _rawPortionPenalty; // penalty for every raw portion served
        _orderScore += _chickenRuinedServed * _ruinedPortionPenalty; // penalty for every Ruined portion served
    }


    private void CalculateScoreForCarrotsServed()
    {
        int _allPortionsServedAnyCondition = _carrotsSteamedServed + _carrotsRawServed + _carrotsRuinedServed;
        int _perPortionCorrectScore = 25;
        int _largeOrder = 3;
        int _allPortionsCorrectBonus = 25;
        int _allPortionsCorrectBonusLarge = 50;
        int _extraPortionPenalty = -10;
        int _missingPortionPenalty = -25;
        int _rawPortionPenalty = -15;
        int _ruinedPortionPenalty = -30;

        // points for each correct portion
        if (_carrotsSteamedServed >= carrotsSteamedOrdered) // no credit for extra portions
        {
            _orderScore += carrotsSteamedOrdered * _perPortionCorrectScore;
        }

        if (_carrotsSteamedServed < carrotsSteamedOrdered) // even if not all portions served, credit for those that are correct    
        {
            _orderScore += _carrotsSteamedServed * _perPortionCorrectScore;
        }

        if (_carrotsSteamedServed == carrotsSteamedOrdered && carrotsSteamedOrdered != 0)
        {
            if (carrotsSteamedOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _orderScore += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if (carrotsSteamedOrdered > 1)
            {
                _orderScore += _allPortionsCorrectBonus; // small order bonus
            }
        }

        if (carrotsSteamedOrdered > _allPortionsServedAnyCondition)
        {
            _orderScore += (carrotsSteamedOrdered - _allPortionsServedAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (carrotsSteamedOrdered < _allPortionsServedAnyCondition)
        {
            _orderScore += (_allPortionsServedAnyCondition - carrotsSteamedOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _orderScore += _carrotsRawServed * _rawPortionPenalty; // penalty for every raw portion served
        _orderScore += _carrotsRuinedServed * _ruinedPortionPenalty; // penalty for every Ruined portion served
    }


    private void CalculateScoreForBroccoliServed()
    {
        int _allPortionsServedAnyCondition = _broccoliSteamedServed + _broccoliRawServed + _broccoliRuinedServed;
        int _perPortionCorrectScore = 25;
        int _largeOrder = 3;
        int _allPortionsCorrectBonus = 25;
        int _allPortionsCorrectBonusLarge = 50;
        int _extraPortionPenalty = -10;
        int _missingPortionPenalty = -25;
        int _rawPortionPenalty = -15;
        int _ruinedPortionPenalty = -30;

        // points for each correct portion
        if (_broccoliSteamedServed >= broccoliSteamedOrdered) // no credit for extra portions
        {
            _orderScore += broccoliSteamedOrdered * _perPortionCorrectScore;
        }

        if (_broccoliSteamedServed < broccoliSteamedOrdered) // even if not all portions served, credit for those that are correct    
        {
            _orderScore += _broccoliSteamedServed * _perPortionCorrectScore;
        }

        if (_broccoliSteamedServed == broccoliSteamedOrdered && broccoliSteamedOrdered != 0)
        {
            if (broccoliSteamedOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _orderScore += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if (broccoliSteamedOrdered > 1)
            {
                _orderScore += _allPortionsCorrectBonus; // small order bonus
            }
        }

        if (broccoliSteamedOrdered > _allPortionsServedAnyCondition)
        {
            _orderScore += (broccoliSteamedOrdered - _allPortionsServedAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (broccoliSteamedOrdered < _allPortionsServedAnyCondition)
        {
            _orderScore += (_allPortionsServedAnyCondition - broccoliSteamedOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _orderScore += _broccoliRawServed * _rawPortionPenalty; // penalty for every raw portion served
        _orderScore += _broccoliRuinedServed * _ruinedPortionPenalty; // penalty for every Ruined portion served
    }


    private void CalculateScoreForSaladsServed()
    {
        int _allPortionsServedAnyCondition = _saladsGoodServed + _saladsRuinedServed;
        int _perPortionCorrectScore = 35;
        int _largeOrder = 3;
        int _allPortionsCorrectBonus = 25;
        int _allPortionsCorrectBonusLarge = 50;
        int _extraPortionPenalty = -10;
        int _missingPortionPenalty = -40;
        int _ruinedPortionPenalty = -45;

        // points for each correct portion
        if (_saladsGoodServed >= saladsOrdered) // no credit for extra portions
        {
            _orderScore += saladsOrdered * _perPortionCorrectScore;
        }

        if (_saladsGoodServed < saladsOrdered) // even if not all portions served, credit for those that are correct    
        {
            _orderScore += _saladsGoodServed * _perPortionCorrectScore;
        }

        if (_saladsGoodServed == saladsOrdered && saladsOrdered != 0)
        {
            if (saladsOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _orderScore += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if (saladsOrdered > 1)
            {
                _orderScore += _allPortionsCorrectBonus; // small order bonus
            }
        }

        if (saladsOrdered > _allPortionsServedAnyCondition)
        {
            _orderScore += (saladsOrdered - _allPortionsServedAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (saladsOrdered < _allPortionsServedAnyCondition)
        {
            _orderScore += (_allPortionsServedAnyCondition - saladsOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _orderScore += _saladsRuinedServed * _ruinedPortionPenalty; // penalty for every ruined portion served
       
    }

    private void UpdateScore()
    {
        _totalScore += _orderScore; // after any score, hold a copy of the total score
        scoreText.text = "Score: " + _totalScore.ToString();
        CheckIfHighScore();
    }


    IEnumerator ScoreFlyUp()
    {
        if(_orderScore < 0)
        {
            scoreFlyUpToCorner.color = Color.red;
            playerAudioSource.PlayOneShot(negativeDeliveryAudio, .2f);
        }
        else if (_orderScore > 0)
        {
            scoreFlyUpToCorner.color = new Color32(75, 200, 50, 255);
            if (!_wasPerfectDelivery)
            {
                playerAudioSource.PlayOneShot(positiveDeliveryAudio, .5f);
            }
            
        }
        scoreFlyUpToCorner.text = _orderScore.ToString();
        scoreFlyUpToCorner.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        scoreFlyUpToCorner.gameObject.SetActive(false);
        scoreFlyUpToCorner.GetComponent<Animator>().Rebind(); 
    }


    IEnumerator PerfectFlyUp()
    {
        perfectFlyUp.gameObject.SetActive(true);
        playerAudioSource.PlayOneShot(perfectDeliveryAudio);
        yield return new WaitForSeconds(2);
        perfectFlyUp.gameObject.SetActive(false);
        perfectFlyUp.GetComponent<Animator>().Rebind();
    }

    IEnumerator PenaltyFlyUp()
    {
        penaltyFlyUp.gameObject.SetActive(true);
        playerAudioSource.PlayOneShot(negativeDeliveryAudio, .1f);
        yield return new WaitForSeconds(1.5f);
        penaltyFlyUp.gameObject.SetActive(false);
        penaltyFlyUp.GetComponent<Animator>().Rebind();
    }


    //////////////////////////////////////////////////////////////////////////
    /// WIP Customer Comments
    ///     add feedback to kitchen based on performance, many potential things to evaluate
    //////////////////////////////////////////////////////////////////////////

    private void PostCustomerComments()
    {
    /*
        int _deltaSteamedCarrots =  _carrotsSteamedServed - _carrotsSteamedOrdered;
        int _deltaSteamedBroccoli = _broccoliSteamedServed - _broccoliSteamedOrdered;
        int _deltaSalads = _saladsGoodServed - _saladsOrdered;
        int _deltaChicken = _chickenCookedServed - _chickenOrdered;
        int _deltaBeefRare = _beefRareServed - _beefRareOrdered;
        int _deltaBeefMedium = _beefMediumServed - _beefMediumOrdered;
        int _deltaBeefWellDone = _beefWellDoneServed - _beefWellDoneOrdered;

        int _totalBeefOrdered = _beefRareOrdered + _beefMediumOrdered + _beefWellDoneOrdered;

        int _totalCarrotsServed = _carrotsRawServed + _carrotsSteamedServed + _carrotsRuinedServed;
        int _totalBroccoliServed = _broccoliRawServed + _broccoliSteamedServed + _broccoliRuinedServed;
        int _totalSaladsServed = _saladsGoodServed + _saladsRuinedServed;
        int _totalChickenServed = _chickenCookedServed + _chickenRuinedServed;
        int _totalBeefServed = _beefRawServed + _beefRareServed + _beefMediumServed + _beefWellDoneServed + _beefRuinedServed;
               
        if (_totalBeefOrdered + _chickenOrdered > 0 && _beefRawServed + _chickenRawServed > 0)
        {
            _resultsToPost.Add("You actually served us raw meat!\n");
        }

        if (_totalBeefOrdered + _chickenOrdered > 0 && _beefRuinedServed + _chickenRuinedServed > 0)
        {
            _resultsToPost.Add("The meat on my plate was Ruined to a crisp!\n");
        }

        if (_carrotsSteamedOrdered + _broccoliSteamedOrdered > 0 && _carrotsRuinedServed + _broccoliRuinedServed > 0)
        {
            _resultsToPost.Add("Vegetables were Ruined, and not just a little!\n");
        }

        if (_saladsRuinedServed > 0)
        {
            _resultsToPost.Add("How hard is a Garden Salad!, this one was ruined\n");
        }
        
        if (_deltaRawCarrots == 1)
        {
            _resultsToPost.Add ("Received 1 extra order of raw carrots\n");
        }
        
        if (_deltaRawCarrots == -1)
        {
            _resultsToPost.Add("1 order of raw carrots is missing!\n");
        }

        if (_deltaRawCarrots > 1)
        {
            _resultsToPost.Add("Received " + _deltaRawCarrots + " extra orders of raw carrots\n");
        }
        
        if (_deltaRawCarrots < -1)
        {
            _resultsToPost.Add(Mathf.Abs(_deltaRawCarrots) + " orders of raw carrots were missing!\n");
        }

        if (_carrotsRuinedServed != 0)
        {
            _resultsToPost.Add("These carrots are Ruined!  This is disgraceful!\n");
        }
    */
    }


    //////////////////////////////////////////////////////////////////////////
    /// UI Buttons
    //////////////////////////////////////////////////////////////////////////

    public void ActivateTable1()
    {
        if (!isActiveTable1 && isDoneServingTable1)
        {            
            isActiveTable1 = true;
            isOpenNowTableCount += 1;
            buttonTable1Text.color = new Color32(75, 200, 50, 255);
            buttonTable1Text.text = "Open";
            reservedTable1.gameObject.SetActive(false);
            orderBoardClosedTable1.gameObject.SetActive(false);
            isReadyForNewOrderTable1 = true;
            messageText.text = "";
            GameObject.Find("TablesManager").GetComponent<Table1>().BeginNewOrder();            
            underlineTable1.gameObject.SetActive(false);
        }

        else if (isActiveTable1) // note to future self, need to do 'else if' for these 'one or the other' otherwise cycles
        {
            isActiveTable1 = false;
            buttonTable1Text.color = Color.red;
            
            if (_table1.isEmptyWithNoGuests)
            {
                reservedTable1.gameObject.SetActive(true); 
                orderBoardClosedTable1.gameObject.SetActive(true);
                isOpenNowTableCount -= 1;
                buttonTable1Text.text = "Closed";
            }

            else if (!_table1.isEmptyWithNoGuests)
            {
                CheckIfFirstTimeClosingTableWhileGuestsNotServed();
                isWaitingToCloseTable1 = true;
                buttonTable1Text.text = "To Close";
            }            
        }

        playerAudioSource.PlayOneShot(buttonClick, .05f);
        KitchenDoorControl();
    }


    public void ActivateTable2()
    {
        if (!isActiveTable2 && isDoneServingTable2)
        {
            isActiveTable2 = true;
            isOpenNowTableCount += 1;
            buttonTable2Text.color = new Color32(75, 200, 50, 255);
            buttonTable2Text.text = "Open";
            reservedTable2.gameObject.SetActive(false);
            orderBoardClosedTable2.gameObject.SetActive(false);
            isReadyForNewOrderTable2 = true;
            messageText.text = "";
            GameObject.Find("TablesManager").GetComponent<Table2>().BeginNewOrder();
        }

        else if (isActiveTable2) // note to future self, need to do 'else if' for these 'one or the other' otherwise cycles
        {
            isActiveTable2 = false;
            buttonTable2Text.color = Color.red;
            
            if (_table2.isEmptyWithNoGuests)
            {
                reservedTable2.gameObject.SetActive(true);
                orderBoardClosedTable2.gameObject.SetActive(true);
                isOpenNowTableCount -= 1;
                buttonTable2Text.text = "Closed";
            }

            else if (!_table2.isEmptyWithNoGuests)
            {
                CheckIfFirstTimeClosingTableWhileGuestsNotServed();
                isWaitingToCloseTable2 = true;
                buttonTable2Text.text = "To Close";
            }
        }

        playerAudioSource.PlayOneShot(buttonClick, .05f);
        KitchenDoorControl();
    }


    public void ActivateTable3()
    { 
        if (!isActiveTable3 && isDoneServingTable3)
        {
            isActiveTable3 = true;
            isOpenNowTableCount += 1;
            buttonTable3Text.color = new Color32(75, 200, 50, 255);
            buttonTable3Text.text = "Open";
            reservedTable3.gameObject.SetActive(false);
            orderBoardClosedTable3.gameObject.SetActive(false);
            isReadyForNewOrderTable3 = true;
            messageText.text = "";
            GameObject.Find("TablesManager").GetComponent<Table3>().BeginNewOrder();            
        }

        else if (isActiveTable3) // note to future self, need to do 'else if' for these 'one or the other' otherwise cycles
        {
            isActiveTable3 = false;
            buttonTable3Text.color = Color.red;
            
            if (_table3.isEmptyWithNoGuests)
            {
                reservedTable3.gameObject.SetActive(true);
                orderBoardClosedTable3.gameObject.SetActive(true);
                isOpenNowTableCount -= 1;
                buttonTable3Text.text = "Closed";
            }

            else if (!_table3.isEmptyWithNoGuests)
            {
                CheckIfFirstTimeClosingTableWhileGuestsNotServed();
                isWaitingToCloseTable3 = true;
                buttonTable3Text.text = "To Close";
            }
        }
        
        playerAudioSource.PlayOneShot(buttonClick, .05f);
        KitchenDoorControl();
    }


    private void CheckIfFirstTimeClosingTableWhileGuestsNotServed()
    {
        if (_isFirstTimeClosingTableWithGuestsSeated && (!isDoneServingTable1 || !isDoneServingTable2 || !isDoneServingTable3))
        {
            messageText.text = "Seated guests must be served.\n\nTable is now closed to new guests.\nYou may reopen table after current guests leave.";
            StartCoroutine(ClearMessage(8));
            _isFirstTimeClosingTableWithGuestsSeated = false;
        }
    }

    
    public void FoodGuide()
    {
        if(_isActiveFoodGuide)
        {
            foodGuideBackground.gameObject.SetActive(false);
            foodGuide.gameObject.SetActive(false);
            _buttonFoodGuideText.text = "Food Guide";
            _isActiveFoodGuide = false;
        }

        else if(!_isActiveFoodGuide)
        {
            foodGuideBackground.gameObject.SetActive(true);
            foodGuide.gameObject.SetActive(true);
            _buttonFoodGuideText.text = "Hide Guide";
            _isActiveFoodGuide = true;
        }

        playerAudioSource.PlayOneShot(buttonClick, .05f);
    }
    

    public void Stats()
    {
        if (_isActiveStats)
        {
            HideStats();
        }

        else if (!_isActiveStats)
        {
            StatsTracker.Instance.FillStatsText();
            statsCanvas.gameObject.SetActive(true);
            _isActiveStats = true;
            GamePause();
        }

        playerAudioSource.PlayOneShot(buttonClick, .05f);
    }

    private void HideStats()
    {
        statsCanvas.gameObject.SetActive(false);
        _isActiveStats = false;
        GameUnpause();
    }


    private void CheckIfHighScore()
    {
        if(_totalScore > DataStorage.Instance.highScoreData)
        {
            highScoreText.text = DataStorage.Instance.playerNameInputData + "\n\n with score of:\n" + _totalScore;
            DataStorage.Instance.bestPlayerData = DataStorage.Instance.playerNameInputData;
            DataStorage.Instance.highScoreData = _totalScore;
        }
    }



    public void Credits()
    {
        if (_isActiveNotes)
        {
            HideNotes();
        }

        if (_isActiveStats)
        {
            HideStats();
        }

        if (_isActiveCredits)
        {
            HideCredits();
        }

        else if (!_isActiveCredits) // needs to be 'else if' of does not work
        {
            credits.gameObject.SetActive(true);
            creditsBackground.gameObject.SetActive(true);
            _isActiveCredits = true;
            GamePause();
        }

        playerAudioSource.PlayOneShot(buttonClick, .05f);
    }


    private void HideCredits()
    {
        credits.gameObject.SetActive(false);
        creditsBackground.gameObject.SetActive(false);
        _isActiveCredits = false;
        GameUnpause();
    }


    public void Notes()
    {
        if (_isActiveCredits)
        {
            HideCredits();
        }

        if (_isActiveStats)
        {
            HideStats();
        }

        if (_isActiveNotes)
        {
            HideNotes();
        }

        else if(!_isActiveNotes) // needs to be 'else if' of does not work
        {
            _page = 0;
            notesRightArrow.gameObject.SetActive(true);
            Debug.Log(_page);
            notesPagesListObjects[0].gameObject.SetActive(true);
            notesBackground.gameObject.SetActive(true);
            _isActiveNotes = true;
            GamePause();
        }

        playerAudioSource.PlayOneShot(buttonClick, .05f);
    }


    private void HideNotes()
    {
        notesPagesListObjects[_page].gameObject.SetActive(false);
        notesBackground.gameObject.SetActive(false);
        notesLeftArrow.gameObject.SetActive(false);
        notesRightArrow.gameObject.SetActive(false);
        HideNotesImage();
        _isActiveNotes = false;
        GameUnpause();
    }


    public void NotesRightArrow()
    {
        if (_isNotesImageActive)
        {
            HideNotesImage();
        }

        if (_page < notesPagesListObjects.Count)
        {
            notesPagesListObjects[_page].gameObject.SetActive(false);
            _page += 1;
            notesPagesListObjects[_page].gameObject.SetActive(true);

            if (_page > 0)
            {
                notesLeftArrow.gameObject.SetActive(true);
            }

            if (_page == notesPagesListObjects.Count-1)
            {
                notesRightArrow.gameObject.SetActive(false);
            }
        }
    }


    public void NotesLeftArrow()
    {
        if (_isNotesImageActive)
        {
            HideNotesImage();
        }

        if (_page > 0)
        {
            notesPagesListObjects[_page].gameObject.SetActive(false);
            _page -= 1;
            notesPagesListObjects[_page].gameObject.SetActive(true);
            
            if (_page < notesPagesListObjects.Count)
            {
                notesRightArrow.gameObject.SetActive(true);
            }      
            
            if(_page == 0)
            {
                notesLeftArrow.gameObject.SetActive(false);
            }
        }
    }


    public void ShowNotesImage()
    {
        if (_isNotesImageActive)
        {
            HideNotesImage();
        }

        else if (!_isNotesImageActive)
        {
            notesImage.gameObject.SetActive(true);
            _isNotesImageActive = true;
        }
    }


    public void HideNotesImage()
    {
        notesImage.gameObject.SetActive(false);
        _isNotesImageActive = false;
    }


    private void GamePause()
    {
        Time.timeScale = 0; // pause
    }


    private void GameUnpause()
    {
        Time.timeScale = 1; // unpause
    }


    public void QuitGame()
    {
        StatsTracker.Instance.AddLastStoredAndSessionValuesSendToDataStorage();
        DataStorage.Instance.SaveDataToDisk();
        playerAudioSource.PlayOneShot(buttonClick, .05f);

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}