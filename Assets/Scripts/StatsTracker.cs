using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsTracker : MonoBehaviour
{
    public static StatsTracker Instance;



 
    public float elapsedGameTimeTotal = 0;
    public float diningRoomOpenTimeTotal = 0; // StatsTracker/TrackDiningRoomServiceTimeTotal()
    public float tableOpenTimeTotal = 0; // StatsTracker/TrackDiningRoomServiceTimeTotal().  1 table open for one minute = 1; 3 tables open for one minute = 3.
    public float isUsingScrubbiTimeTotal = 0; // StatsTracker/TrackScrubbiTimeTotal()
    public float isLateTimeTotal = 0; // StatsTracker/TrackLateTimeTotal()
    public float elapsedTimeToServeOrder = 0; // TableTracker/AfterFoodIsServedActions()
    public float averageTimeToServePerGuest = 0; // (elapsedTimeToServeOrder/guestsSeatedTotal)
    public float averageTimeToServePerTable = 0; // (elapsedTimeToServeOrder/deliveriesMadeTotal)

    ////////////////////////////

    public int ordersReceivedTotal = 0; // TableTracker/PublishOrder()
    public int guestsSeatedTotal = 0; // TableTracker/SeatGuests()
    public int deliveriesMadeTotal = 0; // ServeFood/Update()/GetKeyDown
    public int perfectDeliveriesMadeTotal = 0; // GameManager/CheckIfPerfectDelivery()
    public int wasLateTableCountTotal = 0; // TableTracker/OrderPrepElapsedTime()

    ////////////////////////////

    public int foodDispensedTotal = 0; // DispenserStation/Update()/GetKeyDown
    public int beefDispensedTotal = 0;
    public int chickenDispensedTotal = 0;
    public int carrotsDispensedTotal = 0;
    public int broccoliDispensedTotal = 0;
    public int saladsDispensedTotal = 0;

    public int beefRawServedTotal = 0; // GameManager/CountFoodDelivered()
    public int beefRareServedTotal = 0;
    public int beefMediumServedTotal = 0;
    public int beefWellDoneServedTotal = 0;
    public int beefRuinedServedTotal = 0;    
    public int chickenRawServedTotal = 0;
    public int chickenCookedServedTotal = 0;
    public int chickenRuinedServedTotal = 0;    
    public int carrotsRawServedTotal = 0;
    public int carrotsSteamedServedTotal = 0;
    public int carrotsRuinedServedTotal = 0;
    public int broccoliRawServedTotal = 0;
    public int broccoliSteamedServedTotal = 0;
    public int broccoliRuinedServedTotal = 0;   
    public int saladsGoodServedTotal = 0;
    public int saladsRuinedServedTotal = 0;

    public int beefRareOrderedTotal = 0; // GameManager/PickMenuItemsAndQuantities()
    public int beefMediumOrderedTotal = 0;
    public int beefWellDoneOrderedTotal = 0;
    public int chickenCookedOrderedTotal = 0;
    public int carrotsSteamedOrderedTotal = 0;
    public int broccoliSteamedOrderedTotal = 0;
    public int saladsOrderedTotal = 0;

    ////////////////////////////

    public int foodSmashedTotal = 0; // Food/MonitorTimeOnFloor()
    public int foodWasteBinTotal = 0; // WasteStation/OnTriggerEnter()/Destroy
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
        averageTimeToServePerGuest = elapsedTimeToServeOrder / guestsSeatedTotal;
        averageTimeToServePerTable = elapsedTimeToServeOrder / deliveriesMadeTotal;

    }


    private void TrackElapsedGameTimeTotal()
    {
        if (GameManager.Instance.isGameStarted)
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

