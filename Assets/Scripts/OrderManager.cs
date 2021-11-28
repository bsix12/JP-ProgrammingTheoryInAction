using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private int _maxScorePossibleTable1;
    private int _maxScorePossibleTable2;
    private int _maxScorePossibleTable3;

    public TextMeshProUGUI maxScorePossibleTextUITable1; // UI display element
    public TextMeshProUGUI maxScorePossibleTextUITable2;
    public TextMeshProUGUI maxScorePossibleTextUITable3;

    public TextMeshProUGUI orderTextUITable1; // UI display element
    public TextMeshProUGUI orderTextUITable2;
    public TextMeshProUGUI orderTextUITable3;

    public TextMeshProUGUI timeElapsedTextUITable1; // UI display element
    public TextMeshProUGUI timeElapsedTextUITable2;
    public TextMeshProUGUI timeElapsedTextUITable3;

    public TextMeshPro orderTextTable1;  // Scene text, orders billboard 
    public TextMeshPro orderTextTable2;
    public TextMeshPro orderTextTable3;

    private void Start()
    {
        Instance = this;
        isReadyForNewOrderTable1 = true;
    }

    private void Update()
    {
        GetNewOrderByTable();
        OrderTimerByTable();
    }

    public void GetNewOrderByTable()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isReadyForNewOrderTable1)
        {
            GameManager.Instance.GenerateNewOrder();
            orderTextTable1.text = "";
            orderTextUITable1.text = "";
            _onlyFoodOrderedTable1 = GameManager.Instance.onlyFoodOrdered;
            _maxScorePossibleTable1 = GameManager.Instance.maxScorePossible;
            maxScorePossibleTextUITable1.text = _maxScorePossibleTable1.ToString() + " points possible";
            PublishOrderTable1();

            timeElapsedTable1 = 0;
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
    private void PublishOrderTable1()
    {
        for (int i = 0; i < _onlyFoodOrderedTable1.Count; i++)
        {
            orderTextTable1.text += _onlyFoodOrderedTable1[i] + "\n";
            orderTextUITable1.text += _onlyFoodOrderedTable1[i] + "\n";
        }
    }

    private void OrderTimerByTable()
    {
        if (isOnTimerTable1)
        {
            timeElapsedTable1 += Time.deltaTime;
            string _roundTime = timeElapsedTable1.ToString("#.0");
            timeElapsedTextUITable1.text = _roundTime;
        }
    }

    public void AfterFoodIsServedActions()
    {
        isDoneServingTable1 = true;
        isOnTimerTable1 = false;
        isReadyForNewOrderTable1 = true;
    }
}
