using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    
    public TextMeshPro orderText;
    public TextMeshPro resultsText;
    public List<string> foodDelivered = new List<string>();

    private List<string> _menuMains = new List<string>();
    private List<string> _menuSides = new List<string>();
    private List<string> _foodOrdered = new List<string>();
    private List<string> _resultsToPost = new List<string>();

    [SerializeField] private int _chickenOrdered;
    [SerializeField] private int _beefRareOrdered;
    [SerializeField] private int _beefMediumOrdered;
    [SerializeField] private int _beefWellDoneOrdered;
    [SerializeField] private int _carrotsRawOrdered;
    [SerializeField] private int _carrotsSteamedOrdered;
    [SerializeField] private int _broccoliRawOrdered;
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
    [SerializeField] private int _saladsDelivered;
    [SerializeField] private int _saladsRuinedDelivered;

    private void Start()
    {
        Instance = this;

        _menuSides.Add("Raw Carrots");
        _menuSides.Add("Steamed Carrots");
        _menuSides.Add("Raw Broccoli");
        _menuSides.Add("Steamed Broccoli");
        _menuSides.Add("Garden Salad");

        _menuMains.Add("Chicken");
        _menuMains.Add("Beef: Rare");
        _menuMains.Add("Beef: Medium");
        _menuMains.Add("Beef: Well-Done");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetNewOrder();
        }
    }

    void GetNewOrder()
    {
        int sides = 6; // placeholder
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
            if (_foodOrdered[i] == "Raw Carrots")
            {
                _carrotsRawOrdered += 1;
            }

            if (_foodOrdered[i] == "Steamed Carrots")
            {
                _carrotsSteamedOrdered += 1;
            }

            if (_foodOrdered[i] == "Raw Broccoli")
            {
                _broccoliRawOrdered += 1;
            }

            if (_foodOrdered[i] == "Steamed Broccoli")
            {
                _broccoliSteamedOrdered += 1;
            }

            if (_foodOrdered[i] == "Salad")
            {
                _saladsOrdered += 1;
            }

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
                _carrotsRawOrdered + " - Raw Carrots\n" +
                _carrotsSteamedOrdered + " - Steamed Carrots\n" +
                _broccoliRawOrdered + " - Raw Broccoli\n" +
                _broccoliSteamedOrdered + " - Steamed Broccoli\n" +
                _saladsOrdered + " - Garden Salad\n";

        }
    }
    public void CountFoodDelivered()
    {
        for (int i = 0; i < foodDelivered.Count; i++)
        {
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
                _saladsDelivered += 1;
            }

            if (foodDelivered[i] == "Ruined Salad")
            {
                _saladsRuinedDelivered += 1;
            }

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

        }
    }
    public void PostToResultsBoard()
    {
        int _deltaRawCarrots = _carrotsRawDelivered - _carrotsRawOrdered;
        int _deltaSteamedCarrots =  _carrotsSteamedDelivered - _carrotsSteamedOrdered;
        int _deltaRawBroccoli = _broccoliRawDelivered - _broccoliRawOrdered;
        int _deltaSteamedBroccoli = _broccoliSteamedDelivered - _broccoliSteamedOrdered;
        int _deltaSalads = _saladsDelivered - _saladsOrdered;
        int _deltaChicken = _chickenCookedDelivered - _chickenOrdered;
        int _deltaBeefRare = _beefRareDelivered - _beefRareOrdered;
        int _deltaBeefMedium = _beefMediumDelivered - _beefMediumOrdered;
        int _deltaBeefWellDone = _beefWellDoneDelivered - _beefWellDoneOrdered;

        _resultsToPost.Add("<b><u>Customer Comments</b></u>\n\n");

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
                
        for (int i = 0; i < _resultsToPost.Count; i++)
        {
            resultsText.text += _resultsToPost[i]+"\n";  
        }

    }
}