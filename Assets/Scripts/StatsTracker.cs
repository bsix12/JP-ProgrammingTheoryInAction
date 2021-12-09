using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsTracker : MonoBehaviour
{
    public static StatsTracker Instance;


    public bool isGameStarted;
    public bool isAnyTableOpen;


    public float elapsedGameTimeTotal = 0;
    public float diningRoomOpenTimeTotal = 0;
    public float tableOpenTimeTotal = 0; // 1 table open for one minute = 1; 3 tables open for one minute = 3.
    public float isUsingScrubbiTimeTotal = 0;
    public float isLateTimeTotal = 0;
    public float averageTimeToServePerGuest = 0;

    ////////////////////////////

    public int ordersReceivedTotal = 0;
    public int guestsSeatedTotal = 0;
    public int deliveriesMadeTotal = 0;
    public int perfectDeliveriesMadeTotal = 0;
    public int wasLateTableCountTotal = 0;

    ////////////////////////////

    public int itemsDispensedTotal = 0;

    public int beefDispensedTotal = 0;
    public int beefRawServedTotal = 0;
    public int beefRareServedTotal = 0;
    public int beefMediumServedTotal = 0;
    public int beefWellDoneServedTotal = 0;
    public int beefRuinedServedTotal = 0;

    public int chickenDispensedTotal = 0;
    public int chickenRawServedTotal = 0;
    public int chickenCookedServedTotal = 0;
    public int chickenRuinedServedTotal = 0;

    public int carrotsDispensedTotal = 0;
    public int carrotsRawServedTotal = 0;
    public int carrotsCookedServedTotal = 0;
    public int carrotsRuinedServedTotal = 0;

    public int broccoliDispensedTotal = 0;
    public int broccoliRawServedTotal = 0;
    public int broccoliCookedServedTotal = 0;
    public int broccoliRuinedServedTotal = 0;

    public int saladsDispensedTotal = 0;
    public int saladsGoodServedTotal = 0;
    public int saladsRuinedServedTotal = 0;

    public int beefRareOrderedTotal = 0;
    public int beefMediumOrderedTotal = 0;
    public int beefWellDoneOrderedTotal = 0;
    public int chickenCookedOrdered = 0;
    public int carrotsSteamedOrdered = 0;
    public int broccoliSteamedOrdered = 0;
    public int saladsOrdered = 0;

    ////////////////////////////

    public int foodSmashedTotal = 0;
    public int foodWasteBinTotal = 0;
    public int pickedUpOffFloorAndServedTotal = 0;

  
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    
    private void Update()
    {
        TrackElapsedGameTimeTotal();
        TrackDiningRoomServiceTimeTotal();
        TrackScrubbiTimeTotal();
        TrackLateTimeTotal();
    }


    private void TrackElapsedGameTimeTotal()
    {
        if (isGameStarted)
        {
            elapsedGameTimeTotal += Time.deltaTime;
        }
    }


    private void TrackDiningRoomServiceTimeTotal()
    {
        if(GameManager.Instance.isOpenNowTableCount > 0)
        {
            diningRoomOpenTimeTotal += Time.deltaTime;
            tableOpenTimeTotal += Time.deltaTime * GameManager.Instance.isOpenNowTableCount;
        }
    }

    private void TrackScrubbiTimeTotal()
    {
        if (GameManager.Instance.isUsingScrubbi)
        {
            isUsingScrubbiTimeTotal += Time.deltaTime;
        }
    }

    private void TrackLateTimeTotal()
    {
        if(GameManager.Instance.isLateNowTableCount > 0)
        {
            isLateTimeTotal += Time.deltaTime * GameManager.Instance.isLateNowTableCount;
        }
    }    
}

