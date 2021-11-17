using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [SerializeField] private int _rawCarrotsOrdered;
    [SerializeField] private int _steamedCarrotsOrdered;
    [SerializeField] private int _rawBroccoliOrdered;
    [SerializeField] private int _steamedBroccoliOrdered;
    [SerializeField] private int _saladsOrdered;

    [SerializeField] private int _chickenOrdered;
    [SerializeField] private int _rareBeefOrdered;
    [SerializeField] private int _mediumBeefOrdered;
    [SerializeField] private int _wellDoneBeefOrdered;

    public TextMeshPro orderText;
    public TextMeshPro resultsText;

    public int _rawCarrotsDelivered;
    public int _steamedCarrotsDelivered;
    public int _burnedCarrotsDelivered;
    public int _rawBroccoliDelivered;
    public int _steamedBroccoliDelivered;
    public int _burnedBroccoliDelivered;
    public int _saladsDelivered;
    public int _ruinedSaladsDelivered;

    public int _rawChickenDelivered;
    public int _cookedChickenDelivered;
    public int _burnedChickenDelivered;
    public int _rawBeefDelivered;
    public int _rareBeefDelivered;
    public int _mediumBeefDelivered;
    public int _wellDoneBeefDelivered;
    public int _burnedBeefDelivered;

    public List<string> _foodDelivered = new List<string>();

    private List<string> _foodOrdered = new List<string>();
    private List<string> _foodChoicesMeat = new List<string>();
    private List<string> _foodChoicesVeg = new List<string>();
    private List<string> _resultsToPost = new List<string>();

    private void Start()
    {
        Instance = this;

        _foodChoicesVeg.Add("Raw Carrots");
        _foodChoicesVeg.Add("Steamed Carrots");
        _foodChoicesVeg.Add("Raw Broccoli");
        _foodChoicesVeg.Add("Steamed Broccoli");
        _foodChoicesVeg.Add("Garden Salad");

        _foodChoicesMeat.Add("Chicken");
        _foodChoicesMeat.Add("Beef: Rare");
        _foodChoicesMeat.Add("Beef: Medium");
        _foodChoicesMeat.Add("Beef: Well-Done");
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
            sideSelectedIndex = Random.Range(0, _foodChoicesVeg.Count);
            _foodOrdered.Add(_foodChoicesVeg[sideSelectedIndex]);
        }

        for (int i = 0; i < mains; i++)
        {
            mainSelectedIndex = Random.Range(0, _foodChoicesMeat.Count);
            _foodOrdered.Add(_foodChoicesMeat[mainSelectedIndex]);
        }

        for (int i = 0; i < _foodOrdered.Count; i++)
        {
            if (_foodOrdered[i] == "Raw Carrots")
            {
                _rawCarrotsOrdered += 1;
            }

            if (_foodOrdered[i] == "Steamed Carrots")
            {
                _steamedCarrotsOrdered += 1;
            }

            if (_foodOrdered[i] == "Raw Broccoli")
            {
                _rawBroccoliOrdered += 1;
            }

            if (_foodOrdered[i] == "Steamed Broccoli")
            {
                _steamedBroccoliOrdered += 1;
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
                _rareBeefOrdered += 1;
            }

            if (_foodOrdered[i] == "Beef: Medium")
            {
                _mediumBeefOrdered += 1;
            }

            if (_foodOrdered[i] == "Beef: Well-Done")
            {
                _wellDoneBeefOrdered += 1;
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
                _rareBeefOrdered + " - Beef: Rare\n" +
                _mediumBeefOrdered + " - Beef: Medium\n" +
                _wellDoneBeefOrdered + " - Beef: Well-Done\n" +
                "\n" +
                "\n" +
                "<b><u>Sides</b></u>\n" +
                _rawCarrotsOrdered + " - Raw Carrots\n" +
                _steamedCarrotsOrdered + " - Steamed Carrots\n" +
                _rawBroccoliOrdered + " - Raw Broccoli\n" +
                _steamedBroccoliOrdered + " - Steamed Broccoli\n" +
                _saladsOrdered + " - Garden Salad\n";

        }
    }
    public void ExamineFoodDelivered()
    {
        for (int i = 0; i < _foodDelivered.Count; i++)
        {
            if (_foodDelivered[i] == "Raw Carrots")
            {
                _rawCarrotsDelivered += 1;
            }

            if (_foodDelivered[i] == "Steamed Carrots")
            {
                _steamedCarrotsDelivered += 1;
            }

            if (_foodDelivered[i] == "Burned Carrots")
            {
                _burnedCarrotsDelivered += 1;
            }

            if (_foodDelivered[i] == "Raw Broccoli")
            {
                _rawBroccoliDelivered += 1;
            }

            if (_foodDelivered[i] == "Steamed Broccoli")
            {
                _steamedBroccoliDelivered += 1;
            }

            if (_foodDelivered[i] == "Burned Broccoli")
            {
                _burnedBroccoliDelivered += 1;
            }

            if (_foodDelivered[i] == "Salad")
            {
                _saladsDelivered += 1;
            }

            if (_foodDelivered[i] == "Ruined Salad")
            {
                _ruinedSaladsDelivered += 1;
            }

            if (_foodDelivered[i] == "Raw Chicken")
            {
                _rawChickenDelivered += 1;
            }

            if (_foodDelivered[i] == "Cooked Chicken")
            {
                _cookedChickenDelivered += 1;
            }

            if (_foodDelivered[i] == "Burned Chicken")
            {
                _burnedChickenDelivered += 1;
            }

            if (_foodDelivered[i] == "Raw Beef")
            {
                _rawBeefDelivered += 1;
            }

            if (_foodDelivered[i] == "Beef: Rare")
            {
                _rareBeefDelivered += 1;
            }

            if (_foodDelivered[i] == "Beef: Medium")
            {
                _mediumBeefDelivered += 1;
            }

            if (_foodDelivered[i] == "Beef: Well-Done")
            {
                _wellDoneBeefDelivered += 1;
            }

            if (_foodDelivered[i] == "Burned Beef")
            {
                _burnedBeefDelivered += 1;
            }

        }
    }
    public void PostToResultsBoard()
    {
        int _deltaRawCarrots = _rawCarrotsDelivered - _rawCarrotsOrdered;
        int _deltaSteamedCarrots = _steamedCarrotsOrdered - _steamedCarrotsDelivered;

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
        if (_burnedCarrotsDelivered != 0)
        {
            _resultsToPost.Add("These carrots are burned!  This is disgraceful!\n");
        }
                
        for (int i = 0; i < _resultsToPost.Count; i++)
        {
            resultsText.text += _resultsToPost[i]+"\n";  
        }

    }
}