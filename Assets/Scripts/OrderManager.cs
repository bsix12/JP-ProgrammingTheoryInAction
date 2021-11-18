using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    public bool isReadyForNewOrder;
    public bool isDoneServing;

    public TextMeshPro orderText;
    public TextMeshPro resultsText;
    [System.NonSerialized] public List<string> foodDelivered = new List<string>();

    private List<string> _menuMains = new List<string>();
    private List<string> _menuSides = new List<string>();
    private List<string> _foodOrdered = new List<string>();
    private List<string> _resultsToPost = new List<string>();

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
        _menuSides.Add("Garden Salad");               
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isReadyForNewOrder)
        {
            ResetStoredListsAndInts();
            GetNewOrder();
            isReadyForNewOrder = false;
            isDoneServing = false;
        }
    }

    void ResetStoredListsAndInts()
    {
        // note to future self: doing .Clear() only on lists appeared to not work
        // new orders were added to previous orders and never got cleared
        // took awhile to realize these lists are generated from stored data
        // needed to reset all the input data to generate lists based only on current data

        _foodOrdered.Clear();   
        _resultsToPost.Clear();

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

    void GetNewOrder()
    {
        int sides = 5; // placeholder
        int mains = 4; // placeholder
        int sideSelectedIndex;
        int mainSelectedIndex;

        for (int i = 0; i < sides; i++)
        {
            sideSelectedIndex = Random.Range(0, _menuSides.Count);
            _foodOrdered.Add(_menuSides[sideSelectedIndex]);
        }

        for (int i = 0; i < mains; i++)
        {
            mainSelectedIndex = Random.Range(0, _menuMains.Count);
            _foodOrdered.Add(_menuMains[mainSelectedIndex]);
        }

        for (int i = 0; i < _foodOrdered.Count; i++)
        {
            if (_foodOrdered[i] == "Chicken")
            {
                _chickenOrdered += 1;
            }

            if (_foodOrdered[i] == "Beef: Rare")
            {
                _beefRareOrdered += 1;
            }

            if (_foodOrdered[i] == "Beef: Medium")
            {
                _beefMediumOrdered += 1;
            }

            if (_foodOrdered[i] == "Beef: Well-Done")
            {
                _beefWellDoneOrdered += 1;
            }

            if (_foodOrdered[i] == "Steamed Carrots")
            {
                _carrotsSteamedOrdered += 1;
            }

            if (_foodOrdered[i] == "Steamed Broccoli")
            {
                _broccoliSteamedOrdered += 1;
            }

            if (_foodOrdered[i] == "Garden Salad")
            {
                _saladsOrdered += 1;
            }
        }
        PostToOrderBoard();
    }

    void PostToOrderBoard()
    {
       {
            orderText.text =
                "<b><u>Main Dishes</b></u>\n" +
                _chickenOrdered + " - Chicken\n" +
                _beefRareOrdered + " - Beef: Rare\n" +
                _beefMediumOrdered + " - Beef: Medium\n" +
                _beefWellDoneOrdered + " - Beef: Well-Done\n" +
                "\n" +
                "\n" +
                "<b><u>Sides</b></u>\n" +
                _carrotsSteamedOrdered + " - Steamed Carrots\n" +
                _broccoliSteamedOrdered + " - Steamed Broccoli\n" +
                _saladsOrdered + " - Garden Salad\n";

        }
    }
    public void CountFoodDelivered()
    {
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
    public void PostToResultsBoard()
    {
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


        _resultsToPost.Add("<b><u>Kitchen Report Card</b></u>\n\n");
        
        if(_carrotsSteamedOrdered > 0 && _carrotsSteamedDelivered == _carrotsSteamedOrdered)
        {
            //+score
            _resultsToPost.Add("Steamed Carrots - Pass\n");
        }

        if(_carrotsSteamedOrdered > 0 && _carrotsSteamedDelivered != _carrotsSteamedOrdered)
        {
            //-score
            _resultsToPost.Add("Steamed Carrots - Fail\n");
        }

        if(_broccoliSteamedOrdered > 0 && _broccoliSteamedDelivered == _broccoliSteamedOrdered)
        {
            //+score
            _resultsToPost.Add("Steamed Broccoli - Pass\n");
        }

        if(_broccoliSteamedOrdered > 0 && _broccoliSteamedDelivered != _broccoliSteamedOrdered)
        {
            //-score
            _resultsToPost.Add("Steamed Broccoli - Fail\n");
        }

        if (_saladsOrdered > 0 && _saladsGoodDelivered == _saladsOrdered)
        {
            //+score
            _resultsToPost.Add("Salads - Pass\n");
        }

        if (_saladsOrdered > 0 && _saladsGoodDelivered != _saladsOrdered)
        {
            //-score
            _resultsToPost.Add("Salads - Fail\n");
        }

        if (_chickenOrdered > 0 && _chickenCookedDelivered == _chickenOrdered)
        {
            //+score
            _resultsToPost.Add("Chicken - Pass\n");
        }

        if (_chickenOrdered > 0 && _chickenCookedDelivered != _chickenOrdered)
        {
            //-score
            _resultsToPost.Add("Chicken - Fail\n");
        }

        if (_beefRareOrdered > 0 && _beefRareDelivered == _beefRareOrdered)
        {
            //+score
            _resultsToPost.Add("Beef: Rare - Pass\n");
        }

        if (_beefRareOrdered > 0 && _beefRareDelivered != _beefRareOrdered)
        {
            //-score
            _resultsToPost.Add("Beef: Rare - Fail\n");
        }

        if (_beefMediumOrdered > 0 && _beefMediumDelivered == _beefMediumOrdered)
        {
            //+score
            _resultsToPost.Add("Beef: Medium - Pass\n");
        }

        if (_beefMediumOrdered > 0 && _beefMediumDelivered != _beefMediumOrdered)
        {
            //-score
            _resultsToPost.Add("Beef: Medium - Fail\n");
        }

        if (_beefWellDoneOrdered > 0 && _beefWellDoneDelivered == _beefWellDoneOrdered)
        {
            //+score
            _resultsToPost.Add("Beef: Well-Done - Pass\n");
        }

        if (_beefWellDoneOrdered > 0 && _beefWellDoneDelivered != _beefWellDoneOrdered)
        {
            //-score
            _resultsToPost.Add("Beef: Well-Done - Fail\n");
        }



        /*
         * 
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
        */

        /*        if (_deltaRawCarrots == 1)
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
        for (int i = 0; i < _resultsToPost.Count; i++)
        {
            resultsText.text += _resultsToPost[i]+"\n";  
        }

    }
}