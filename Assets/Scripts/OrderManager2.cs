using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager2 : MonoBehaviour
{

    public static OrderManager2 Instance;

    public bool isReadyForNewOrderTable1 = true;
    public bool isReadyForNewOrderTable2 = true;
    public bool isReadyForNewOrderTable3 = true;

    public bool isDoneServingTable1 = true;
    public bool isDoneServingTable2 = true;
    public bool isDoneServingTable3 = true;

    //public bool isLateTable1 = false;


    public bool isGenerateNewOrderBusy = false;


    private void Start()
    {
        Instance = this;
    }
}
