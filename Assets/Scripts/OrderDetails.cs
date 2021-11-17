using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderDetails : MonoBehaviour
{
    [SerializeField] private int _rawCarrotsWanted;
    [SerializeField] private int _steamedCarrotsWanted;
    [SerializeField] private int _rawBroccoliWanted;
    [SerializeField] private int _steamedBroccoliWanted;
    [SerializeField] private int _saladsWanted;

    [SerializeField] private int _chickenWanted;
    [SerializeField] private int _rareBeefWanted;
    [SerializeField] private int _mediumBeefWanted;
    [SerializeField] private int _wellDoneBeefWanted;

    private List<string> _foodWanted = new List<string>();
    private List<string> _foodChoicesMeat = new List<string>();
    private List<string> _foodChoicesVeg = new List<string>();

    private void Start()
    {
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
        int sides = 6;
        int mains = 4;
        int sideSelectedIndex;
        int mainSelectedIndex;
        
        for (int i = 0; i < sides; i++)
        {
            sideSelectedIndex = Random.Range(0, _foodChoicesVeg.Count);
            _foodWanted.Add(_foodChoicesVeg[sideSelectedIndex]);
        }

        for (int i = 0; i < mains; i++)
        {
            mainSelectedIndex = Random.Range(0, _foodChoicesMeat.Count);
            _foodWanted.Add(_foodChoicesMeat[mainSelectedIndex]);
        }

       for (int i = 0; i < _foodWanted.Count; i++)
        {
            if(_foodWanted[i] == "Raw Carrots")
            {
                _rawCarrotsWanted += 1;
            }

            if (_foodWanted[i] == "Steamed Carrots")
            {
                _steamedCarrotsWanted += 1;
            }
            
            if (_foodWanted[i] == "Raw Broccoli")
            {
                _rawBroccoliWanted += 1;
            }

            if (_foodWanted[i] == "Steamed Broccoli")
            {
                _steamedBroccoliWanted += 1;
            }

            if (_foodWanted[i] == "Salad")
            {
                _saladsWanted += 1;
            }

            if (_foodWanted[i] == "Chicken")
            {
                _chickenWanted += 1;
            }

            if (_foodWanted[i] == "Beef: Rare")
            {
                _rareBeefWanted += 1;
            }

            if (_foodWanted[i] == "Beef: Medium")
            {
                _mediumBeefWanted += 1;
            }

            if (_foodWanted[i] == "Beef: Well-Done")
            {
                _wellDoneBeefWanted += 1;
            }
        }
    }
}
