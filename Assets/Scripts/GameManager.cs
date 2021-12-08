using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    ////////////////////////////
    /// Assign In Inspector
    ////////////////////////////
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI credits;
    public TextMeshProUGUI notes;
    public TextMeshPro reportCardText;

    public Button foodGuideButton;
    public Button notesButton;

    public Image diningTablesUIBackground;
    public Image foodGuidBackground;
    public Image creditsNotesBackground;

    public GameObject kitchenDoor;
    public GameObject reservedTable1;
    public GameObject reservedTable2;
    public GameObject reservedTable3;
    public GameObject smashedFoodContainer;
    public GameObject diningTablesUI;
    public GameObject foodGuide;

    ////////////////////////////

    public List<GameObject> readyToServeGameObjects = new List<GameObject>();
    public List<string> onlyFoodOrderedNames = new List<string>();
    public List<string> foodDeliveredNames = new List<string>();
    public List<string> reportCardToPost = new List<string>();
    
    public Vector3 serveTableLocation;
        
    public string atTableName; // reference to which table player is at
    
    public float secondsOfPrepAllowedPerMain = 90;
    public float secondsToWaitBeforeClearMessage;

    public int isLateTableCount;
    public int numberOfSeatedGuests = 0;
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

    public bool isActiveTable1 = false;
    public bool isActiveTable2 = false;
    public bool isActiveTable3 = false;
    public bool isReadyForNewOrderTable1 = true;
    public bool isReadyForNewOrderTable2 = true;
    public bool isReadyForNewOrderTable3 = true;
    public bool isGeneratingOrderTable1; // order generator can only be used by one table at a time          ////////////might be able to move this back to Table
    public bool isGeneratingOrderTable2;
    public bool isGeneratingOrderTable3;
    public bool isDoneServingTable1 = true;
    public bool isDoneServingTable2 = true;
    public bool isDoneServingTable3 = true;
    
    public bool isDoingMovementTutorial = true;
    public bool moveWithWSUnlocked = false;
    public bool moveWithADUnlocked = false;
    public bool moveWithQEUnlocked = false;
    public bool isFirstTimeTriggeringDispenser = true;
    public bool isFirstTimeSpinningPlatter = true;
    public bool isFirstTimeAtCookStation = true;
    public bool isFirstTimeServingCustomers = true;
 
    public bool isCalculatingScore;   
    public bool isInServiceArea; // manages if spacebar delivers food to table or allows for a new order
    public bool playerInDiningRoom;
    public bool canDispense;

    ////////////////////////////

    private TextMeshProUGUI _buttonTable1Text;
    private TextMeshProUGUI _buttonTable2Text;
    private TextMeshProUGUI _buttonTable3Text;
    private TextMeshProUGUI _buttonFoodGuideText;

    private List<string> _menuMainsNames = new List<string>();
    private List<string> _menuSidesNames = new List<string>();
    private List<string> _includesNoneFoodOrderedNames = new List<string>();

    private bool _isFirstTimeOpeningDiningRoom = true;
    private bool _isFirstTimeClosingTableWithGuestsSeated = true;
    private bool _isActiveCredits = false;
    private bool _isActiveNotes = false;
    private bool _isActiveFoodGuide = false;
    private bool _mustClean = false;

    private int _perSecondPenaltyIfLate = 5; // play test value
    private float _oncePerSecond = 1f;

    [SerializeField] private int _chickenRawDelivered;
    [SerializeField] private int _chickenCookedDelivered;
    [SerializeField] private int _chickenBurnedDelivered;
    [SerializeField] private int _beefRawDelivered;
    [SerializeField] private int _beefRareDelivered;
    [SerializeField] private int _beefMediumDelivered;
    [SerializeField] private int _beefWellDoneDelivered;
    [SerializeField] private int _beefBurnedDelivered;
    [SerializeField] private int _carrotsRawDelivered;
    [SerializeField] private int _carrotsSteamedDelivered;
    [SerializeField] private int _carrotsBurnedDelivered;
    [SerializeField] private int _broccoliRawDelivered;
    [SerializeField] private int _broccoliSteamedDelivered;
    [SerializeField] private int _broccoliBurnedDelivered;
    [SerializeField] private int _saladsGoodDelivered;
    [SerializeField] private int _saladsRuinedDelivered;
    [SerializeField] private int _score;
    [SerializeField] private int _holdTotalScore;



    private void Start()
    {
        Instance = this;
    
        _score = 0;
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
        messageText.text = "Welcome to your kitchen!";
        yield return new WaitForSeconds(2);
        moveWithADUnlocked = true;
        messageText.text = "Use <u>A</u> or <u>D</u> to rotate around.";
    }

    public void EnableDiningTablesUI()
    {
        diningTablesUIBackground.gameObject.SetActive(true);
        Instance.diningTablesUI.gameObject.SetActive(true);
        
        _buttonTable1Text = GameObject.Find("OpenTable1ButtonText").GetComponent<TextMeshProUGUI>();
        _buttonTable2Text = GameObject.Find("OpenTable2ButtonText").GetComponent<TextMeshProUGUI>();
        _buttonTable3Text = GameObject.Find("OpenTable3ButtonText").GetComponent<TextMeshProUGUI>();
        _buttonTable1Text.color = Color.red;
        _buttonTable2Text.color = Color.red;
        _buttonTable3Text.color = Color.red;
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
        _score -= smashedFoodPenalty;
        UpdateScore();
    }

    public void ApplyWastedFoodPenalty()
    {
        _score -= wastedFoodPenalty;
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

    public void GenerateOrderDetails()
    {
        PickMenuItemsAndQuantities();
        SummarizeOnlyFoodOrdered();
        CalculateMaximumOrderScorePossible();
        UpdateScore();
    }
    
    public void PickMenuItemsAndQuantities()
    {
        int minMains = 2;
        int maxMains = 8;   // the number of diners at each table is modeled for up to 8 people
        int sides = 0;
        int salads = 0;
        int sideSelectedIndex;
        int mainSelectedIndex;

        int mains = Random.Range(minMains, maxMains);

        if (mains <= 4)
        {
            sides = Random.Range(mains - 1, mains + 2);
            salads = Random.Range(mains - 2, mains);
        }
        else if (mains > 4)
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
            }

            if (_includesNoneFoodOrderedNames[i] == "Beef: Rare")
            {
                beefRareOrdered += 1;
            }

            if (_includesNoneFoodOrderedNames[i] == "Beef: Medium")
            {
                beefMediumOrdered += 1;
            }

            if (_includesNoneFoodOrderedNames[i] == "Beef: Well-Done")
            {
                beefWellDoneOrdered += 1;
            }

            if (_includesNoneFoodOrderedNames[i] == "Steamed Carrots")
            {
                carrotsSteamedOrdered += 1;
            }

            if (_includesNoneFoodOrderedNames[i] == "Steamed Broccoli")
            {
                broccoliSteamedOrdered += 1;
            }

            saladsOrdered = salads; // salads are not randomly selected; base on quantity of Mains ordered
        }        
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
        _chickenCookedDelivered = chickenOrdered; // set input for perfect delivery
        _beefRareDelivered = beefRareOrdered;
        _beefMediumDelivered = beefMediumOrdered;
        _beefWellDoneDelivered = beefWellDoneOrdered;
        _carrotsSteamedDelivered = carrotsSteamedOrdered;
        _broccoliSteamedDelivered = broccoliSteamedOrdered;
        _saladsGoodDelivered = saladsOrdered;

        _score = 0; // reset score
        CalculateScore(); // call method to calculate score
        
        maximumOrderScorePossible = _score; // store the max score
        _score = _holdTotalScore; // reset score to the stored value
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

        _chickenRawDelivered = 0;
        _chickenCookedDelivered = 0;
        _chickenBurnedDelivered = 0;
        _beefRawDelivered = 0;
        _beefRareDelivered = 0;
        _beefMediumDelivered = 0;
        _beefWellDoneDelivered = 0;
        _beefBurnedDelivered = 0;
        _carrotsRawDelivered = 0;
        _carrotsSteamedDelivered = 0;
        _carrotsBurnedDelivered = 0;
        _broccoliRawDelivered = 0;
        _broccoliSteamedDelivered = 0;
        _broccoliBurnedDelivered = 0;
        _saladsGoodDelivered = 0;
        _saladsRuinedDelivered = 0;
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


        if (isActiveTable1 || isActiveTable2 || isActiveTable3 || !isDoneServingTable1 || !isDoneServingTable2 || !isDoneServingTable3)
        {
            kitchenDoor.gameObject.SetActive(false); // this should be in more generic section
            
            if (_isFirstTimeOpeningDiningRoom)
            {
                messageText.text = "Guest will arrive soon and begin placing orders.";
                StartCoroutine(ClearMessage(5));
                _isFirstTimeOpeningDiningRoom = false;
            }
      /*      else
            {
                messageText.text = "";
            }
      */
        }

        else if (!isActiveTable1 && !isActiveTable2 && !isActiveTable3 && isDoneServingTable1 && isDoneServingTable2 && isDoneServingTable3 && !playerInDiningRoom && !isDoingMovementTutorial)
        {
            kitchenDoor.gameObject.SetActive(true);
            messageText.text = "The dining room is now closed.\nTo begin seating guests, select tables to open.";
            //StartCoroutine(ClearMessage(5));
        }
    }

    public IEnumerator ClearMessage(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        messageText.text = "";
    }
    
    private void ApplyPerSecondPenaltyIfLate() //////////// ADD up tabels late
    {
        if (isLateTableCount > 0 && _oncePerSecond > 0f) // 1 second timer
        {
            _oncePerSecond -= Time.deltaTime;
        }

        if (isLateTableCount > 0 && _oncePerSecond <= 0f)
        {
            _score -= _perSecondPenaltyIfLate * isLateTableCount; // for each second late, apply a penalty, per table late
            scoreText.text = "Score: " + _score.ToString();
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
        CountFoodDelivered();
        CalculateScore();
        UpdateScore();
        ResetOrderReportText();
        PostToKitchenReportCard();
        PostCustomerComments();  // future

        foodDeliveredNames.Clear();
        ResetStoredInts();
        isCalculatingScore = false;
    }
    

    private void CountFoodDelivered()
    {
        //Debug.Log("Count food delivered has been called");
        for (int i = 0; i < foodDeliveredNames.Count; i++)
        {
            if (foodDeliveredNames[i] == "Raw Chicken")
            {
                _chickenRawDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Cooked Chicken")
            {
                _chickenCookedDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Burned Chicken")
            {
                _chickenBurnedDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Raw Beef")
            {
                _beefRawDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Beef: Rare")
            {
                _beefRareDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Beef: Medium")
            {
                _beefMediumDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Beef: Well-Done")
            {
                _beefWellDoneDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Burned Beef")
            {
                _beefBurnedDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Raw Carrots")
            {
                _carrotsRawDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Steamed Carrots")
            {
                _carrotsSteamedDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Burned Carrots")
            {
                _carrotsBurnedDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Raw Broccoli")
            {
                _broccoliRawDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Steamed Broccoli")
            {
                _broccoliSteamedDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Burned Broccoli")
            {
                _broccoliBurnedDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Salad")
            {
                _saladsGoodDelivered += 1;
            }

            if (foodDeliveredNames[i] == "Ruined Salad")
            {
                _saladsRuinedDelivered += 1;
            }
        }
    }


    private void PostToKitchenReportCard()
    {
        //Debug.Log("post to kitchen report card has been called");
        reportCardToPost.Add("<b><u>Kitchen Report Card</b></u>\n\n");

        if (carrotsSteamedOrdered > 0 && _carrotsSteamedDelivered == carrotsSteamedOrdered)
        {
            reportCardToPost.Add("Steamed Carrots - Pass\n");
        }

        if (carrotsSteamedOrdered > 0 && _carrotsSteamedDelivered != carrotsSteamedOrdered)
        {
            reportCardToPost.Add("Steamed Carrots - Fail\n");
        }

        if (broccoliSteamedOrdered > 0 && _broccoliSteamedDelivered == broccoliSteamedOrdered)
        {
            reportCardToPost.Add("Steamed Broccoli - Pass\n");
        }

        if (broccoliSteamedOrdered > 0 && _broccoliSteamedDelivered != broccoliSteamedOrdered)
        {
            reportCardToPost.Add("Steamed Broccoli - Fail\n");
        }

        if (saladsOrdered > 0 && _saladsGoodDelivered == saladsOrdered)
        {
            reportCardToPost.Add("Salads - Pass\n");
        }

        if (saladsOrdered > 0 && _saladsGoodDelivered != saladsOrdered)
        {
            reportCardToPost.Add("Salads - Fail\n");
        }

        if (chickenOrdered > 0 && _chickenCookedDelivered == chickenOrdered)
        {
            reportCardToPost.Add("Chicken - Pass\n");
        }

        if (chickenOrdered > 0 && _chickenCookedDelivered != chickenOrdered)
        {
            reportCardToPost.Add("Chicken - Fail\n");
        }

        if (beefRareOrdered > 0 && _beefRareDelivered == beefRareOrdered)
        {
            reportCardToPost.Add("Beef: Rare - Pass\n");
        }

        if (beefRareOrdered > 0 && _beefRareDelivered != beefRareOrdered)
        {
            reportCardToPost.Add("Beef: Rare - Fail\n");
        }

        if (beefMediumOrdered > 0 && _beefMediumDelivered == beefMediumOrdered)
        {
            reportCardToPost.Add("Beef: Medium - Pass\n");
        }

        if (beefMediumOrdered > 0 && _beefMediumDelivered != beefMediumOrdered)
        {
            reportCardToPost.Add("Beef: Medium - Fail\n");
        }

        if (beefWellDoneOrdered > 0 && _beefWellDoneDelivered == beefWellDoneOrdered)
        {
            reportCardToPost.Add("Beef: Well-Done - Pass\n");
        }

        if (beefWellDoneOrdered > 0 && _beefWellDoneDelivered != beefWellDoneOrdered)
        {
            reportCardToPost.Add("Beef: Well-Done - Fail\n");
        }

        for (int i = 0; i < reportCardToPost.Count; i++)
        {
            reportCardText.text += reportCardToPost[i] + "\n";
        }
    }


    private void CalculateScore()
    {
        //Debug.Log("calculate score has been called");
        CalculateScoreForBeefDelivered();       //
        CalculateScoreForChickenDelivered();    // 
        CalculateScoreForCarrotsDelivered();    // ABSTRACTION - method names indicate actions, details in separate methods
        CalculateScoreForBroccoliDelivered();   // 
        CalculateScoreForSaladsDelivered();     // 
    }


    private void CalculateScoreForBeefDelivered()
    {
        int _allPortionsDeliveredAnyCondition = _beefRawDelivered + _beefRareDelivered + _beefMediumDelivered + _beefWellDoneDelivered + _beefBurnedDelivered;

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
        if (_beefRareDelivered >= beefRareOrdered) // no credit for extra portions
        {
            _score += beefRareOrdered * _perPortionCorrectScore;
        }

        if (_beefRareDelivered < beefRareOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _beefRareDelivered * _perPortionCorrectScore;
        }

        if (_beefMediumDelivered >= beefMediumOrdered) // no credit for extra portions
        {
            _score += beefMediumOrdered * _perPortionCorrectScore;
        }

        if (_beefMediumDelivered < beefMediumOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _beefMediumDelivered * _perPortionCorrectScore;
        }

        if (_beefWellDoneDelivered >= beefWellDoneOrdered) // no credit for extra portions
        {
            _score += beefWellDoneOrdered * _perPortionCorrectScore;
        }

        if (_beefWellDoneDelivered < beefWellDoneOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _beefWellDoneDelivered * _perPortionCorrectScore;
        }

        if (_beefRareDelivered == beefRareOrdered
            && _beefMediumDelivered == beefMediumOrdered
            && _beefWellDoneDelivered == beefWellDoneOrdered
            && _allPortionsOrdered !=0) // bonus not available if no portions ordered
        {
            if (_allPortionsOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _score += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if(_allPortionsOrdered > 1)
            {
                _score += _allPortionsCorrectBonus; // small order bonus
            }
            _receivedAllCorrectBonus = true;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///  NEED TO SOLVE in CheckBeefPartials()
        ///   - correct total edible portions with none correct
        ///   - correct total edible portions with at least one correct
        ///
        /// if( ordered A+B+C == deliveredEdible A+B+C ) // this ensures only reviewing cases where the correct number of portions were delivered.  examine mix of what was delivered.
        /// {
        ///     if( ordered A > 0 && delivered A > 0) 
        ///     { 
        ///         return  // at least one portion of A was correct
        ///         // score for including some correct portions
        ///     }
        ///     
        ///     if(ordered B > 0 && delivered B > 0)
        ///     {
        ///         return  // at least one order of B was correct
        ///         // score for including some correct portions
        ///     }
        ///     
        ///     if(ordered C > 0 && delivered C > 0)
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
        
        if (_allPortionsOrdered > _allPortionsDeliveredAnyCondition)
        {
            _score += (_allPortionsOrdered - _allPortionsDeliveredAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (_allPortionsOrdered < _allPortionsDeliveredAnyCondition)
        {
            _score += (_allPortionsDeliveredAnyCondition - _allPortionsOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _score += _beefRawDelivered * _rawPortionPenalty; // penalty for every raw portion delivered
        _score += _beefBurnedDelivered * _ruinedPortionPenalty; // penalty for every burned portion delivered
    }


    private void CheckBeefPartials(int _allPortionsOrdered, int _largeOrder, bool _receivedAllCorrectBonus)
    {
        int _allPortionsDeliveredEdible = _beefRareDelivered + _beefMediumDelivered + _beefWellDoneDelivered;
        int _goodTryMitigation = 25; // correct number of edible portions delivered, but none met order requirements  ex: 3 rare when 2 medium and 1 wellDone were ordered
        int _goodEdibleCountPartialCorrect = 50;
        int _goodEdibleCountPartialCorrectLarge = 100;

        if (_allPortionsDeliveredEdible == _allPortionsOrdered && _allPortionsOrdered > 1 && !_receivedAllCorrectBonus)
        {
            if (beefRareOrdered > 0 && _beefRareDelivered > 0)
            {
                Debug.Log("correct number portions, partial correct, rare");
                if (_allPortionsOrdered >= _largeOrder)
                {
                    _score += _goodEdibleCountPartialCorrectLarge;
                    return;
                }
                _score += _goodEdibleCountPartialCorrect;
                return;
            }

            if (beefMediumOrdered > 0 && _beefMediumDelivered > 0)
            {
                Debug.Log("correct number portions, partial correct, medium");
                if (_allPortionsOrdered >= _largeOrder)
                {
                    _score += _goodEdibleCountPartialCorrectLarge;
                    return;
                }
                _score += _goodEdibleCountPartialCorrect;
                return;
            }

            if (beefWellDoneOrdered > 0 && _beefWellDoneDelivered > 0)
            {
                Debug.Log("correct number portions, partial correct, medium");
                if (_allPortionsOrdered >= _largeOrder)
                {
                    _score += _goodEdibleCountPartialCorrectLarge;
                    return;
                }
                _score += _goodEdibleCountPartialCorrect;
                return;
            }

            _score += _goodTryMitigation;
            Debug.Log("correct number portions, all wrong");
        }
    }


    private void CalculateScoreForChickenDelivered()
    {
        int _allPortionsDeliveredAnyCondition = _chickenCookedDelivered + _chickenRawDelivered + _chickenBurnedDelivered;
        int _perPortionCorrectScore = 100;
        int _largeOrder = 3;
        int _allPortionsCorrectBonus = 100;
        int _allPortionsCorrectBonusLarge = 300;
        int _extraPortionPenalty = -40;
        int _missingPortionPenalty = -125;
        int _rawPortionPenalty = -250;
        int _ruinedPortionPenalty = -75;

        // points for each correct portion
        if (_chickenCookedDelivered >= chickenOrdered) // no credit for extra portions
        {
            _score += chickenOrdered * _perPortionCorrectScore;
        }

        if (_chickenCookedDelivered < chickenOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _chickenCookedDelivered * _perPortionCorrectScore;
        }

        if (_chickenCookedDelivered == chickenOrdered && chickenOrdered != 0) // bonus not available if no portions ordered
        {
            if(chickenOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _score += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if (chickenOrdered > 1)
            {
                _score += _allPortionsCorrectBonus; // small order bonus
            }                            
        }

        if (chickenOrdered > _allPortionsDeliveredAnyCondition)
        {
            _score += (chickenOrdered - _allPortionsDeliveredAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (chickenOrdered < _allPortionsDeliveredAnyCondition)
        {
            _score += (_allPortionsDeliveredAnyCondition - chickenOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _score += _chickenRawDelivered * _rawPortionPenalty; // penalty for every raw portion delivered
        _score += _chickenBurnedDelivered * _ruinedPortionPenalty; // penalty for every burned portion delivered
    }


    private void CalculateScoreForCarrotsDelivered()
    {
        int _allPortionsDeliveredAnyCondition = _carrotsSteamedDelivered + _carrotsRawDelivered + _carrotsBurnedDelivered;
        int _perPortionCorrectScore = 25;
        int _largeOrder = 3;
        int _allPortionsCorrectBonus = 25;
        int _allPortionsCorrectBonusLarge = 50;
        int _extraPortionPenalty = -10;
        int _missingPortionPenalty = -25;
        int _rawPortionPenalty = -15;
        int _ruinedPortionPenalty = -30;

        // points for each correct portion
        if (_carrotsSteamedDelivered >= carrotsSteamedOrdered) // no credit for extra portions
        {
            _score += carrotsSteamedOrdered * _perPortionCorrectScore;
        }

        if (_carrotsSteamedDelivered < carrotsSteamedOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _carrotsSteamedDelivered * _perPortionCorrectScore;
        }

        if (_carrotsSteamedDelivered == carrotsSteamedOrdered && carrotsSteamedOrdered != 0)
        {
            if (carrotsSteamedOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _score += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if (carrotsSteamedOrdered > 1)
            {
                _score += _allPortionsCorrectBonus; // small order bonus
            }
        }

        if (carrotsSteamedOrdered > _allPortionsDeliveredAnyCondition)
        {
            _score += (carrotsSteamedOrdered - _allPortionsDeliveredAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (carrotsSteamedOrdered < _allPortionsDeliveredAnyCondition)
        {
            _score += (_allPortionsDeliveredAnyCondition - carrotsSteamedOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _score += _carrotsRawDelivered * _rawPortionPenalty; // penalty for every raw portion delivered
        _score += _carrotsBurnedDelivered * _ruinedPortionPenalty; // penalty for every burned portion delivered
    }


    private void CalculateScoreForBroccoliDelivered()
    {
        int _allPortionsDeliveredAnyCondition = _broccoliSteamedDelivered + _broccoliRawDelivered + _broccoliBurnedDelivered;
        int _perPortionCorrectScore = 25;
        int _largeOrder = 3;
        int _allPortionsCorrectBonus = 25;
        int _allPortionsCorrectBonusLarge = 50;
        int _extraPortionPenalty = -10;
        int _missingPortionPenalty = -25;
        int _rawPortionPenalty = -15;
        int _ruinedPortionPenalty = -30;

        // points for each correct portion
        if (_broccoliSteamedDelivered >= broccoliSteamedOrdered) // no credit for extra portions
        {
            _score += broccoliSteamedOrdered * _perPortionCorrectScore;
        }

        if (_broccoliSteamedDelivered < broccoliSteamedOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _broccoliSteamedDelivered * _perPortionCorrectScore;
        }

        if (_broccoliSteamedDelivered == broccoliSteamedOrdered && broccoliSteamedOrdered != 0)
        {
            if (broccoliSteamedOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _score += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if (broccoliSteamedOrdered > 1)
            {
                _score += _allPortionsCorrectBonus; // small order bonus
            }
        }

        if (broccoliSteamedOrdered > _allPortionsDeliveredAnyCondition)
        {
            _score += (broccoliSteamedOrdered - _allPortionsDeliveredAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (broccoliSteamedOrdered < _allPortionsDeliveredAnyCondition)
        {
            _score += (_allPortionsDeliveredAnyCondition - broccoliSteamedOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _score += _broccoliRawDelivered * _rawPortionPenalty; // penalty for every raw portion delivered
        _score += _broccoliBurnedDelivered * _ruinedPortionPenalty; // penalty for every burned portion delivered
    }


    private void CalculateScoreForSaladsDelivered()
    {
        int _allPortionsDeliveredAnyCondition = _saladsGoodDelivered + _saladsRuinedDelivered;
        int _perPortionCorrectScore = 35;
        int _largeOrder = 3;
        int _allPortionsCorrectBonus = 25;
        int _allPortionsCorrectBonusLarge = 50;
        int _extraPortionPenalty = -10;
        int _missingPortionPenalty = -40;
        int _ruinedPortionPenalty = -45;

        // points for each correct portion
        if (_saladsGoodDelivered >= saladsOrdered) // no credit for extra portions
        {
            _score += saladsOrdered * _perPortionCorrectScore;
        }

        if (_saladsGoodDelivered < saladsOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _saladsGoodDelivered * _perPortionCorrectScore;
        }

        if (_saladsGoodDelivered == saladsOrdered && saladsOrdered != 0)
        {
            if (saladsOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _score += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if (saladsOrdered > 1)
            {
                _score += _allPortionsCorrectBonus; // small order bonus
            }
        }

        if (saladsOrdered > _allPortionsDeliveredAnyCondition)
        {
            _score += (saladsOrdered - _allPortionsDeliveredAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (saladsOrdered < _allPortionsDeliveredAnyCondition)
        {
            _score += (_allPortionsDeliveredAnyCondition - saladsOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _score += _saladsRuinedDelivered * _ruinedPortionPenalty; // penalty for every ruined portion delivered
       
    }

    private void UpdateScore()
    {
        _holdTotalScore = _score; // after any score, hold a copy of the total score
        scoreText.text = "Score: " + _score.ToString();
    }


    //////////////////////////////////////////////////////////////////////////
    /// WIP Customer Comments
    ///     add feedback to kitchen based on performance, many potential things to evaluate
    //////////////////////////////////////////////////////////////////////////

    private void PostCustomerComments()
    {
        Debug.Log("WIP, post customer comments");
        /*
        int _deltaSteamedCarrots =  _carrotsSteamedDelivered - _carrotsSteamedOrdered;
        int _deltaSteamedBroccoli = _broccoliSteamedDelivered - _broccoliSteamedOrdered;
        int _deltaSalads = _saladsGoodDelivered - _saladsOrdered;
        int _deltaChicken = _chickenCookedDelivered - _chickenOrdered;
        int _deltaBeefRare = _beefRareDelivered - _beefRareOrdered;
        int _deltaBeefMedium = _beefMediumDelivered - _beefMediumOrdered;
        int _deltaBeefWellDone = _beefWellDoneDelivered - _beefWellDoneOrdered;

        int _totalBeefOrdered = _beefRareOrdered + _beefMediumOrdered + _beefWellDoneOrdered;

        int _totalCarrotsDelivered = _carrotsRawDelivered + _carrotsSteamedDelivered + _carrotsBurnedDelivered;
        int _totalBroccoliDelivered = _broccoliRawDelivered + _broccoliSteamedDelivered + _broccoliBurnedDelivered;
        int _totalSaladsDelivered = _saladsGoodDelivered + _saladsRuinedDelivered;
        int _totalChickenDelivered = _chickenCookedDelivered + _chickenBurnedDelivered;
        int _totalBeefDelivered = _beefRawDelivered + _beefRareDelivered + _beefMediumDelivered + _beefWellDoneDelivered + _beefBurnedDelivered;
               
        if (_totalBeefOrdered + _chickenOrdered > 0 && _beefRawDelivered + _chickenRawDelivered > 0)
        {
            _resultsToPost.Add("You actually served us raw meat!\n");
        }

        if (_totalBeefOrdered + _chickenOrdered > 0 && _beefBurnedDelivered + _chickenBurnedDelivered > 0)
        {
            _resultsToPost.Add("The meat on my plate was burned to a crisp!\n");
        }

        if (_carrotsSteamedOrdered + _broccoliSteamedOrdered > 0 && _carrotsBurnedDelivered + _broccoliBurnedDelivered > 0)
        {
            _resultsToPost.Add("Vegetables were burned, and not just a little!\n");
        }

        if (_saladsRuinedDelivered > 0)
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

        if (_carrotsBurnedDelivered != 0)
        {
            _resultsToPost.Add("These carrots are burned!  This is disgraceful!\n");
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
            _buttonTable1Text.color = new Color32(75, 200, 50, 255);
            _buttonTable1Text.text = "Open";
            reservedTable1.gameObject.SetActive(false);
            isReadyForNewOrderTable1 = true;
            //isDoneServingTable1 = true;
            GameObject.Find("TablesManager").GetComponent<Table1>().BeginNewOrder();
        }

        else if (isActiveTable1) // note to future self, need to do 'else if' for these 'one or the other' otherwise cycles
        {
            if (isDoneServingTable1)
            {
                reservedTable1.gameObject.SetActive(true);
            }

            isActiveTable1 = false;
            _buttonTable1Text.color = Color.red;
            _buttonTable1Text.text = "Closed";
            CheckIfFirstTimeClosingTableWhileGuestsSeated();
        }

        KitchenDoorControl();
    }

    public void ActivateTable2()
    {
        if (!isActiveTable2 && isDoneServingTable2)
        {
            isActiveTable2 = true;
            _buttonTable2Text.color = new Color32(75, 200, 50, 255);
            _buttonTable2Text.text = "Open";
            reservedTable2.gameObject.SetActive(false);
            isReadyForNewOrderTable2 = true;
            //isDoneServingTable2 = true;
            GameObject.Find("TablesManager").GetComponent<Table2>().BeginNewOrder();
        }

        else if (isActiveTable2) // note to future self, need to do 'else if' for these 'one or the other' otherwise cycles
        {
            if (isDoneServingTable2)
            {
                reservedTable2.gameObject.SetActive(true);
            }

            isActiveTable2 = false;
            _buttonTable2Text.color = Color.red;
            _buttonTable2Text.text = "Closed";
            CheckIfFirstTimeClosingTableWhileGuestsSeated();
        }

        KitchenDoorControl();
    }

    public void ActivateTable3()
    {
        if (!isActiveTable3 && isDoneServingTable3)
        {
            isActiveTable3 = true;
            _buttonTable3Text.color = new Color32(75, 200, 50, 255);
            _buttonTable3Text.text = "Open";
            reservedTable3.gameObject.SetActive(false);
            isReadyForNewOrderTable3 = true;
            //isDoneServingTable3 = true;
            GameObject.Find("TablesManager").GetComponent<Table3>().BeginNewOrder();            
        }

        else if (isActiveTable3) // note to future self, need to do 'else if' for these 'one or the other' otherwise cycles
        {
            if (isDoneServingTable3)
            {
                reservedTable3.gameObject.SetActive(true);
            }

            isActiveTable3 = false;
            _buttonTable3Text.color = Color.red;
            _buttonTable3Text.text = "Closed";
            CheckIfFirstTimeClosingTableWhileGuestsSeated();
        }

        KitchenDoorControl();
    }

    private void CheckIfFirstTimeClosingTableWhileGuestsSeated()
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
        if(_isActiveFoodGuide == true)
        {
            foodGuidBackground.gameObject.SetActive(false);
            foodGuide.gameObject.SetActive(false);
            _buttonFoodGuideText.text = "Food Guide";
            _isActiveFoodGuide = false;
        }

        else if(_isActiveFoodGuide == false)
        {
            foodGuidBackground.gameObject.SetActive(true);
            foodGuide.gameObject.SetActive(true);
            _buttonFoodGuideText.text = "Hide Guide";
            _isActiveFoodGuide = true;
        }
    }
    

    public void Credits()
    {
        if(_isActiveCredits == true)
        {
            credits.gameObject.SetActive(false);
            creditsNotesBackground.gameObject.SetActive(false);
            _isActiveCredits = false;
            notesButton.gameObject.SetActive(false);
            notes.gameObject.SetActive(false);
            _isActiveNotes = false;
        }
        
        else if(_isActiveCredits == false) // needs to be 'else if' or does not work
        {
            creditsNotesBackground.gameObject.SetActive(true);
            credits.gameObject.SetActive(true);
            _isActiveCredits = true;
            notesButton.gameObject.SetActive(true);
        }
    }


    public void Notes()
    {
        if(_isActiveNotes == true)
        {
            notes.gameObject.SetActive(false);            
            _isActiveNotes = false;
        }

        else if(_isActiveNotes == false) // needs to be 'else if' of does not work
        {
            notes.gameObject.SetActive(true);            
            _isActiveNotes = true;
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}