using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isReadyForNewOrder;
    public bool isDoneServing;
    public bool inServiceArea; // manages if spacebar delivers food to table or allows for a new order 

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI maxScorePossibleText;
    public TextMeshProUGUI orderTextUI;
    public TextMeshProUGUI timeElapsedTable1Text;
    public TextMeshProUGUI credits;
    public TextMeshProUGUI notes;
    public Image creditsNotesBackground;
    public Button notesButton;
    public TextMeshPro orderText;
    public TextMeshPro reportCardText;
    public Vector3 serveTableLocation;

    public List<string> foodDelivered = new List<string>();
    public List<GameObject> itemsToServe = new List<GameObject>();

    private List<string> _menuMains = new List<string>();
    private List<string> _menuSides = new List<string>();
    private List<string> _quantityOfEachFoodOrdered = new List<string>();
    private List<string> _onlyFoodOrdered = new List<string>();
    private List<string> _reportCardToPost = new List<string>();

    private float _timeElapsedTable1;
    private bool _timerOnTable1;
    private bool _creditsActive = false;
    private bool _notesActive = false;

    [SerializeField] private int _score;
    [SerializeField] private int _holdScore;
    [SerializeField] private int _maxScorePossible;
    [SerializeField] private int _chickenOrdered;
    [SerializeField] private int _beefRareOrdered;
    [SerializeField] private int _beefMediumOrdered;
    [SerializeField] private int _beefWellDoneOrdered;
    [SerializeField] private int _carrotsSteamedOrdered;
    [SerializeField] private int _broccoliSteamedOrdered;
    [SerializeField] private int _saladsOrdered;

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
    
    private void Start()
    {
        Instance = this;
        isReadyForNewOrder = true;

        _menuMains.Add("Chicken");
        _menuMains.Add("Beef: Rare");
        _menuMains.Add("Beef: Medium");
        _menuMains.Add("Beef: Well-Done");

        _menuSides.Add("Steamed Carrots");
        _menuSides.Add("Steamed Broccoli");
        //_menuSides.Add("Garden Salad");
    }

    private void Update()
    {
        NewOrderReset();
        OrderTimer();
    }

    private void NewOrderReset()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isReadyForNewOrder && !inServiceArea)
        {
            ClearCustomerTable();
            ResetStoredLists();
            ResetStoredInts();
            ResetOrderAndReportText();

            GetNewOrder();
            SummarizeOnlyFoodOrdered();
            PublishNewOrder();
            CalculateMaximumScorePossible();

            _timeElapsedTable1 = 0;
            _timerOnTable1 = true;
            isReadyForNewOrder = false; // prevent Update() actions including GetNewOrder until delivery against current order has been served 
            isDoneServing = false; // toggle - allows the now NewOrder to be delivered to table
        }
    }

    private void ClearCustomerTable()
    {
        for (int i = 0; i < itemsToServe.Count; i++)
        {
            Destroy(itemsToServe[i]);
        }
    }
    
    // note to future self: doing .Clear() only on lists appeared to not work
    // new orders were added to previous orders and never got cleared
    // took awhile to realize these lists are generated from stored data
    // needed to reset all the input data to generate lists based only on current data
    private void ResetStoredLists()
    {
        _quantityOfEachFoodOrdered.Clear();
        _onlyFoodOrdered.Clear();
        _reportCardToPost.Clear();
        foodDelivered.Clear();
        itemsToServe.Clear();
    }

    private void ResetStoredInts()
    {
        _chickenOrdered = 0;
        _beefRareOrdered = 0;
        _beefMediumOrdered = 0;
        _beefWellDoneOrdered = 0;
        _carrotsSteamedOrdered = 0;
        _broccoliSteamedOrdered = 0;
        _saladsOrdered = 0;

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

    private void ResetOrderAndReportText()
    {
        orderText.text = "";
        orderTextUI.text = "";
        reportCardText.text = "";
    }

    private void GetNewOrder()
    {
        int minMains = 2;
        int maxMains = 8;
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
            sideSelectedIndex = Random.Range(0, _menuSides.Count);
            _quantityOfEachFoodOrdered.Add(_menuSides[sideSelectedIndex]);
        }

        for (int i = 0; i < mains; i++)
        {
            mainSelectedIndex = Random.Range(0, _menuMains.Count);
            _quantityOfEachFoodOrdered.Add(_menuMains[mainSelectedIndex]);
        }

        for (int i = 0; i < _quantityOfEachFoodOrdered.Count; i++)
        {
            if (_quantityOfEachFoodOrdered[i] == "Chicken")
            {
                _chickenOrdered += 1;
            }

            if (_quantityOfEachFoodOrdered[i] == "Beef: Rare")
            {
                _beefRareOrdered += 1;
            }

            if (_quantityOfEachFoodOrdered[i] == "Beef: Medium")
            {
                _beefMediumOrdered += 1;
            }

            if (_quantityOfEachFoodOrdered[i] == "Beef: Well-Done")
            {
                _beefWellDoneOrdered += 1;
            }

            if (_quantityOfEachFoodOrdered[i] == "Steamed Carrots")
            {
                _carrotsSteamedOrdered += 1;
            }

            if (_quantityOfEachFoodOrdered[i] == "Steamed Broccoli")
            {
                _broccoliSteamedOrdered += 1;
            }

            _saladsOrdered = salads;
            /*
            if (_quantityOfEachFoodOrdered[i] == "Garden Salad")
            {
                _saladsOrdered += 1;
            }
            */
        }        
    }

    private void SummarizeOnlyFoodOrdered()
    {        
        _onlyFoodOrdered.Add("<b><u>Main Dishes</b></u>\n");

        if (_chickenOrdered > 0)
        {
            _onlyFoodOrdered.Add(_chickenOrdered + " - Chicken\n");
        }

        if (_beefRareOrdered > 0)
        {
            _onlyFoodOrdered.Add(_beefRareOrdered + " - Beef: Rare\n");
        }

        if (_beefMediumOrdered > 0)
        {
            _onlyFoodOrdered.Add(_beefMediumOrdered + " - Beef: Medium\n");
        }

        if (_beefWellDoneOrdered > 0)
        {
            _onlyFoodOrdered.Add(_beefWellDoneOrdered + " - Beef: Well-Done\n");
        }

        _onlyFoodOrdered.Add("\n<b><u>Sides</b></u>\n");

        if (_carrotsSteamedOrdered > 0)
        {
            _onlyFoodOrdered.Add(_carrotsSteamedOrdered + " - Steamed Carrots\n");
        }

        if (_broccoliSteamedOrdered > 0)
        {
            _onlyFoodOrdered.Add(_broccoliSteamedOrdered + " - Steamed Broccoli\n");
        }

        if (_saladsOrdered > 0)
        {
            _onlyFoodOrdered.Add(_saladsOrdered + " - Garden Salad\n");
        }
    }

    private void PublishNewOrder() 
    { 
        
        for (int i = 0; i < _onlyFoodOrdered.Count; i++)
        {
            orderText.text += _onlyFoodOrdered[i] + "\n";
            orderTextUI.text += _onlyFoodOrdered[i] + "\n";
        }
    }

    private void CalculateMaximumScorePossible()
    {
        _chickenCookedDelivered = _chickenOrdered;
        _beefRareDelivered = _beefRareOrdered;
        _beefMediumDelivered = _beefMediumOrdered;
        _beefWellDoneDelivered = _beefWellDoneOrdered;
        _carrotsSteamedDelivered = _carrotsSteamedOrdered;
        _broccoliSteamedDelivered = _broccoliSteamedOrdered;
        _saladsGoodDelivered = _saladsOrdered;

        _score = 0;
        CalculateScore();
        _maxScorePossible = _score;
        maxScorePossibleText.text = _maxScorePossible.ToString() + " points possible";

        _chickenCookedDelivered = 0;
        _beefRareDelivered = 0;
        _beefMediumDelivered = 0;
        _beefWellDoneDelivered = 0;
        _carrotsSteamedDelivered = 0;
        _broccoliSteamedDelivered = 0;
        _saladsGoodDelivered = 0;
        
        _score = _holdScore;
        scoreText.text = "Score: " + _score.ToString();
    }

    private void OrderTimer()
    {
        if (_timerOnTable1)
        {
            _timeElapsedTable1 += Time.deltaTime;
            string _roundTime = _timeElapsedTable1.ToString("#.0");
            timeElapsedTable1Text.text = _roundTime;
        }
    }

    public void AfterFoodIsServedActions()
    {
        //Debug.Log("after food is served has been called");
        CountFoodDelivered();
        PostToKitchenReportCard();
        CalculateScore();
        scoreText.text = "Score: " + _score.ToString();
        _holdScore = _score;

        isDoneServing = true;
        isReadyForNewOrder = true;
        _timerOnTable1 = false;
    }
    
    private void CountFoodDelivered()
    {
        //Debug.Log("Count food delivered has been called");
        for (int i = 0; i < foodDelivered.Count; i++)
        {
            if (foodDelivered[i] == "Raw Chicken")
            {
                _chickenRawDelivered += 1;
            }

            if (foodDelivered[i] == "Cooked Chicken")
            {
                _chickenCookedDelivered += 1;
            }

            if (foodDelivered[i] == "Burned Chicken")
            {
                _chickenBurnedDelivered += 1;
            }

            if (foodDelivered[i] == "Raw Beef")
            {
                _beefRawDelivered += 1;
            }

            if (foodDelivered[i] == "Beef: Rare")
            {
                _beefRareDelivered += 1;
            }

            if (foodDelivered[i] == "Beef: Medium")
            {
                _beefMediumDelivered += 1;
            }

            if (foodDelivered[i] == "Beef: Well-Done")
            {
                _beefWellDoneDelivered += 1;
            }

            if (foodDelivered[i] == "Burned Beef")
            {
                _beefBurnedDelivered += 1;
            }

            if (foodDelivered[i] == "Raw Carrots")
            {
                _carrotsRawDelivered += 1;
            }

            if (foodDelivered[i] == "Steamed Carrots")
            {
                _carrotsSteamedDelivered += 1;
            }

            if (foodDelivered[i] == "Burned Carrots")
            {
                _carrotsBurnedDelivered += 1;
            }

            if (foodDelivered[i] == "Raw Broccoli")
            {
                _broccoliRawDelivered += 1;
            }

            if (foodDelivered[i] == "Steamed Broccoli")
            {
                _broccoliSteamedDelivered += 1;
            }

            if (foodDelivered[i] == "Burned Broccoli")
            {
                _broccoliBurnedDelivered += 1;
            }

            if (foodDelivered[i] == "Salad")
            {
                _saladsGoodDelivered += 1;
            }

            if (foodDelivered[i] == "Ruined Salad")
            {
                _saladsRuinedDelivered += 1;
            }
        }
    }

    private void PostToKitchenReportCard()
    {
        //Debug.Log("post to kitchen report card has been called");
        _reportCardToPost.Add("<b><u>Kitchen Report Card</b></u>\n\n");

        if (_carrotsSteamedOrdered > 0 && _carrotsSteamedDelivered == _carrotsSteamedOrdered)
        {
            _reportCardToPost.Add("Steamed Carrots - Pass\n");
        }

        if (_carrotsSteamedOrdered > 0 && _carrotsSteamedDelivered != _carrotsSteamedOrdered)
        {
            _reportCardToPost.Add("Steamed Carrots - Fail\n");
        }

        if (_broccoliSteamedOrdered > 0 && _broccoliSteamedDelivered == _broccoliSteamedOrdered)
        {
            _reportCardToPost.Add("Steamed Broccoli - Pass\n");
        }

        if (_broccoliSteamedOrdered > 0 && _broccoliSteamedDelivered != _broccoliSteamedOrdered)
        {
            _reportCardToPost.Add("Steamed Broccoli - Fail\n");
        }

        if (_saladsOrdered > 0 && _saladsGoodDelivered == _saladsOrdered)
        {
            _reportCardToPost.Add("Salads - Pass\n");
        }

        if (_saladsOrdered > 0 && _saladsGoodDelivered != _saladsOrdered)
        {
            _reportCardToPost.Add("Salads - Fail\n");
        }

        if (_chickenOrdered > 0 && _chickenCookedDelivered == _chickenOrdered)
        {
            _reportCardToPost.Add("Chicken - Pass\n");
        }

        if (_chickenOrdered > 0 && _chickenCookedDelivered != _chickenOrdered)
        {
            _reportCardToPost.Add("Chicken - Fail\n");
        }

        if (_beefRareOrdered > 0 && _beefRareDelivered == _beefRareOrdered)
        {
            _reportCardToPost.Add("Beef: Rare - Pass\n");
        }

        if (_beefRareOrdered > 0 && _beefRareDelivered != _beefRareOrdered)
        {
            _reportCardToPost.Add("Beef: Rare - Fail\n");
        }

        if (_beefMediumOrdered > 0 && _beefMediumDelivered == _beefMediumOrdered)
        {
            _reportCardToPost.Add("Beef: Medium - Pass\n");
        }

        if (_beefMediumOrdered > 0 && _beefMediumDelivered != _beefMediumOrdered)
        {
            _reportCardToPost.Add("Beef: Medium - Fail\n");
        }

        if (_beefWellDoneOrdered > 0 && _beefWellDoneDelivered == _beefWellDoneOrdered)
        {
            _reportCardToPost.Add("Beef: Well-Done - Pass\n");
        }

        if (_beefWellDoneOrdered > 0 && _beefWellDoneDelivered != _beefWellDoneOrdered)
        {
            _reportCardToPost.Add("Beef: Well-Done - Fail\n");
        }

        for (int i = 0; i < _reportCardToPost.Count; i++)
        {
            reportCardText.text += _reportCardToPost[i] + "\n";
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

        int _allPortionsOrdered = _beefRareOrdered + _beefMediumOrdered + _beefWellDoneOrdered;
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
        if (_beefRareDelivered >= _beefRareOrdered) // no credit for extra portions
        {
            _score += _beefRareOrdered * _perPortionCorrectScore;
        }

        if (_beefRareDelivered < _beefRareOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _beefRareDelivered * _perPortionCorrectScore;
        }

        if (_beefMediumDelivered >= _beefMediumOrdered) // no credit for extra portions
        {
            _score += _beefMediumOrdered * _perPortionCorrectScore;
        }

        if (_beefMediumDelivered < _beefMediumOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _beefMediumDelivered * _perPortionCorrectScore;
        }

        if (_beefWellDoneDelivered >= _beefWellDoneOrdered) // no credit for extra portions
        {
            _score += _beefWellDoneOrdered * _perPortionCorrectScore;
        }

        if (_beefWellDoneDelivered < _beefWellDoneOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _beefWellDoneDelivered * _perPortionCorrectScore;
        }

        if (_beefRareDelivered == _beefRareOrdered
            && _beefMediumDelivered == _beefMediumOrdered
            && _beefWellDoneDelivered == _beefWellDoneOrdered
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
        
        scoreText.text = "Score: " + _score.ToString();
    }

    private void CheckBeefPartials(int _allPortionsOrdered, int _largeOrder, bool _receivedAllCorrectBonus)
    {
        int _allPortionsDeliveredEdible = _beefRareDelivered + _beefMediumDelivered + _beefWellDoneDelivered;
        int _goodTryMitigation = 25; // correct number of edible portions delivered, but none met order requirements  ex: 3 rare when 2 medium and 1 wellDone were ordered
        int _goodEdibleCountPartialCorrect = 50;
        int _goodEdibleCountPartialCorrectLarge = 100;

        if (_allPortionsDeliveredEdible == _allPortionsOrdered && _allPortionsOrdered > 1 && !_receivedAllCorrectBonus)
        {
            if (_beefRareOrdered > 0 && _beefRareDelivered > 0)
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

            if (_beefMediumOrdered > 0 && _beefMediumDelivered > 0)
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

            if (_beefWellDoneOrdered > 0 && _beefWellDoneDelivered > 0)
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
        if (_chickenCookedDelivered >= _chickenOrdered) // no credit for extra portions
        {
            _score += _chickenOrdered * _perPortionCorrectScore;
        }

        if (_chickenCookedDelivered < _chickenOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _chickenCookedDelivered * _perPortionCorrectScore;
        }

        if (_chickenCookedDelivered == _chickenOrdered && _chickenOrdered != 0) // bonus not available if no portions ordered
        {
            if(_chickenOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _score += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if (_chickenOrdered > 1)
            {
                _score += _allPortionsCorrectBonus; // small order bonus
            }                            
        }

        if (_chickenOrdered > _allPortionsDeliveredAnyCondition)
        {
            _score += (_chickenOrdered - _allPortionsDeliveredAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (_chickenOrdered < _allPortionsDeliveredAnyCondition)
        {
            _score += (_allPortionsDeliveredAnyCondition - _chickenOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _score += _chickenRawDelivered * _rawPortionPenalty; // penalty for every raw portion delivered
        _score += _chickenBurnedDelivered * _ruinedPortionPenalty; // penalty for every burned portion delivered
        scoreText.text = "Score: " + _score.ToString();    
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
        if (_carrotsSteamedDelivered >= _carrotsSteamedOrdered) // no credit for extra portions
        {
            _score += _carrotsSteamedOrdered * _perPortionCorrectScore;
        }

        if (_carrotsSteamedDelivered < _carrotsSteamedOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _carrotsSteamedDelivered * _perPortionCorrectScore;
        }

        if (_carrotsSteamedDelivered == _carrotsSteamedOrdered && _carrotsSteamedOrdered != 0)
        {
            if (_carrotsSteamedOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _score += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if (_carrotsSteamedOrdered > 1)
            {
                _score += _allPortionsCorrectBonus; // small order bonus
            }
        }

        if (_carrotsSteamedOrdered > _allPortionsDeliveredAnyCondition)
        {
            _score += (_carrotsSteamedOrdered - _allPortionsDeliveredAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (_carrotsSteamedOrdered < _allPortionsDeliveredAnyCondition)
        {
            _score += (_allPortionsDeliveredAnyCondition - _carrotsSteamedOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _score += _carrotsRawDelivered * _rawPortionPenalty; // penalty for every raw portion delivered
        _score += _carrotsBurnedDelivered * _ruinedPortionPenalty; // penalty for every burned portion delivered
        scoreText.text = "Score: " + _score.ToString();
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
        if (_broccoliSteamedDelivered >= _broccoliSteamedOrdered) // no credit for extra portions
        {
            _score += _broccoliSteamedOrdered * _perPortionCorrectScore;
        }

        if (_broccoliSteamedDelivered < _broccoliSteamedOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _broccoliSteamedDelivered * _perPortionCorrectScore;
        }

        if (_broccoliSteamedDelivered == _broccoliSteamedOrdered && _broccoliSteamedOrdered != 0)
        {
            if (_broccoliSteamedOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _score += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if (_broccoliSteamedOrdered > 1)
            {
                _score += _allPortionsCorrectBonus; // small order bonus
            }
        }

        if (_broccoliSteamedOrdered > _allPortionsDeliveredAnyCondition)
        {
            _score += (_broccoliSteamedOrdered - _allPortionsDeliveredAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (_broccoliSteamedOrdered < _allPortionsDeliveredAnyCondition)
        {
            _score += (_allPortionsDeliveredAnyCondition - _broccoliSteamedOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _score += _broccoliRawDelivered * _rawPortionPenalty; // penalty for every raw portion delivered
        _score += _broccoliBurnedDelivered * _ruinedPortionPenalty; // penalty for every burned portion delivered
        scoreText.text = "Score: " + _score.ToString();
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
        if (_saladsGoodDelivered >= _saladsOrdered) // no credit for extra portions
        {
            _score += _saladsOrdered * _perPortionCorrectScore;
        }

        if (_saladsGoodDelivered < _saladsOrdered) // even if not all portions delivered, credit for those that are correct    
        {
            _score += _saladsGoodDelivered * _perPortionCorrectScore;
        }

        if (_saladsGoodDelivered == _saladsOrdered && _saladsOrdered != 0)
        {
            if (_saladsOrdered >= _largeOrder) // bonus for delivering correct number of good portions
            {
                _score += _allPortionsCorrectBonusLarge; // large order bonus
            }
            else if (_saladsOrdered > 1)
            {
                _score += _allPortionsCorrectBonus; // small order bonus
            }
        }

        if (_saladsOrdered > _allPortionsDeliveredAnyCondition)
        {
            _score += (_saladsOrdered - _allPortionsDeliveredAnyCondition) * _missingPortionPenalty; // penalty for each missing portion
        }

        if (_saladsOrdered < _allPortionsDeliveredAnyCondition)
        {
            _score += (_allPortionsDeliveredAnyCondition - _saladsOrdered) * _extraPortionPenalty; // penalty for each extra portion
        }

        _score += _saladsRuinedDelivered * _ruinedPortionPenalty; // penalty for every ruined portion delivered
       
    }

    private void CustomerComments()
    {
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

    public void Credits()
    {
        if(_creditsActive == true)
        {
            credits.gameObject.SetActive(false);
            creditsNotesBackground.gameObject.SetActive(false);
            _creditsActive = false;
            notesButton.gameObject.SetActive(false);
            notes.gameObject.SetActive(false);
            _notesActive = false;
        }
        
        else if(_creditsActive == false) // needs to be 'else if' or does not work
        {
            creditsNotesBackground.gameObject.SetActive(true);
            credits.gameObject.SetActive(true);
            _creditsActive = true;
            notesButton.gameObject.SetActive(true);
        }
    }

    public void Notes()
    {
        if(_notesActive == true)
        {
            notes.gameObject.SetActive(false);            
            _notesActive = false;
        }

        else if(_notesActive == false) // needs to be 'else if' of does not work
        {
            notes.gameObject.SetActive(true);            
            _notesActive = true;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}