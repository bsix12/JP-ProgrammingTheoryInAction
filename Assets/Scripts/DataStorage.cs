using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataStorage : MonoBehaviour
{
    public static DataStorage Instance;

    public int highScoreData;
    public string playerNameInputData;
    public string bestPlayerData;

    public float elapsedGameTimeAllTimeLastStored;
    public float diningRoomOpenTimeAllTimeLastStored; // StatsTracker/TrackDiningRoomServiceTimeTotal()
    public float tableOpenTimeAllTimeLastStored; // StatsTracker/TrackDiningRoomServiceTimeTotal().  1 table open for one minute = 1; 3 tables open for one minute = 3.
    public float isUsingScrubbiTimeAllTimeLastStored; // StatsTracker/TrackScrubbiTimeTotal()
    public float isLateTimeAllTimeLastStored; // StatsTracker/TrackLateTimeTotal()
    public float elapsedWaitTimeTableAllTimeLastStored; // TableTracker/AfterFoodIsServedActions()
    public float averageWaitTimePerGuestAllTimeLastStored; // (elapsedTimeToServeOrder/guestsSeatedTotal)
    public float averageWaitTimePerTableAllTimeLastStored; // (elapsedTimeToServeOrder/deliveriesMadeTotal)

    ////////////////////////////

    public int ordersReceivedAllTimeLastStored; // TableTracker/PublishOrder()
    public int guestsSeatedAllTimeLastStored; // TableTracker/SeatGuests()
    public int deliveriesMadeAllTimeLastStored; // ServeFood/Update()/GetKeyDown
    public int perfectDeliveriesMadeAllTimeLastStored; // GameManager/CheckIfPerfectDelivery()
    public int wasLateTableCountAllTimeLastStored; // TableTracker/OrderPrepElapsedTime()

    ////////////////////////////

    public int foodDispensedAllTimeLastStored; // DispenserStation/Update()/GetKeyDown
    public int beefDispensedAllTimeLastStored;
    public int chickenDispensedAllTimeLastStored;
    public int carrotsDispensedAllTimeLastStored;
    public int broccoliDispensedAllTimeLastStored;
    public int saladsDispensedAllTimeLastStored;

    public int foodServedAllTimeLastStored;
    public int beefServedAllTimeLastStored;
    public int beefRawServedAllTimeLastStored; // GameManager/CountFoodDelivered()
    public int beefRareServedAllTimeLastStored;
    public int beefMediumServedAllTimeLastStored;
    public int beefWellDoneServedAllTimeLastStored;
    public int beefRuinedServedAllTimeLastStored;
    public int chickenServedAllTimeLastStored;
    public int chickenRawServedAllTimeLastStored;
    public int chickenCookedServedAllTimeLastStored;
    public int chickenRuinedServedAllTimeLastStored;
    public int carrotsServedAllTimeLastStored;
    public int carrotsRawServedAllTimeLastStored;
    public int carrotsSteamedServedAllTimeLastStored;
    public int carrotsRuinedServedAllTimeLastStored;
    public int broccoliServedAllTimeLastStored;
    public int broccoliRawServedAllTimeLastStored;
    public int broccoliSteamedServedAllTimeLastStored;
    public int broccoliRuinedServedAllTimeLastStored;
    public int saladsServedAllTimeLastStored;
    public int saladsGoodServedAllTimeLastStored;
    public int saladsRuinedServedAllTimeLastStored;

    public int foodOrderedAllTimeLastStored;
    public int beefOrderedAllTimeLastStored;
    public int beefRareOrderedAllTimeLastStored; // GameManager/PickMenuItemsAndQuantities()
    public int beefMediumOrderedAllTimeLastStored;
    public int beefWellDoneOrderedAllTimeLastStored;
    public int chickenCookedOrderedAllTimeLastStored;
    public int carrotsSteamedOrderedAllTimeLastStored;
    public int broccoliSteamedOrderedAllTimeLastStored;
    public int saladsOrderedAllTimeLastStored;

    ////////////////////////////

    public int foodSmashedAllTimeLastStored; // Food/MonitorTimeOnFloor()
    public int foodWasteBinAllTimeLastStored; // WasteStation/OnTriggerEnter()/Destroy
    public int wasOnFloorAndServedAllTimeLastStored;

    // Start is called before the first frame update


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadDataFromDisk();
    }

    [System.Serializable]
    class SaveData
    {
        public int highScoreData = 0;
        public string playerNameInputData;
        public string bestPlayerData;

        public float elapsedGameTimeAllTimeLastStored = 0;
        public float diningRoomOpenTimeAllTimeLastStored = 0; // StatsTracker/TrackDiningRoomServiceTimeTotal()
        public float tableOpenTimeAllTimeLastStored = 0; // StatsTracker/TrackDiningRoomServiceTimeTotal().  1 table open for one minute = 1 = 0; 3 tables open for one minute = 3.
        public float isUsingScrubbiTimeAllTimeLastStored = 0; // StatsTracker/TrackScrubbiTimeTotal()
        public float isLateTimeAllTimeLastStored = 0; // StatsTracker/TrackLateTimeTotal()
        public float elapsedWaitTimeTableAllTimeLastStored = 0; // TableTracker/AfterFoodIsServedActions()
        
        ////////////////////////////

        public int ordersReceivedAllTimeLastStored = 0; // TableTracker/PublishOrder()
        public int guestsSeatedAllTimeLastStored = 0; // TableTracker/SeatGuests()
        public int deliveriesMadeAllTimeLastStored = 0; // ServeFood/Update()/GetKeyDown
        public int perfectDeliveriesMadeAllTimeLastStored = 0; // GameManager/CheckIfPerfectDelivery()
        public int wasLateTableCountAllTimeLastStored = 0; // TableTracker/OrderPrepElapsedTime()

        ////////////////////////////

        public int foodDispensedAllTimeLastStored = 0; // DispenserStation/Update()/GetKeyDown
        public int beefDispensedAllTimeLastStored = 0;
        public int chickenDispensedAllTimeLastStored = 0;
        public int carrotsDispensedAllTimeLastStored = 0;
        public int broccoliDispensedAllTimeLastStored = 0;
        public int saladsDispensedAllTimeLastStored = 0;

        public int foodServedAllTimeLastStored = 0;
        public int beefServedAllTimeLastStored = 0;
        public int beefRawServedAllTimeLastStored = 0; // GameManager/CountFoodDelivered()
        public int beefRareServedAllTimeLastStored = 0;
        public int beefMediumServedAllTimeLastStored = 0;
        public int beefWellDoneServedAllTimeLastStored = 0;
        public int beefRuinedServedAllTimeLastStored = 0;
        public int chickenServedAllTimeLastStored = 0;
        public int chickenRawServedAllTimeLastStored = 0;
        public int chickenCookedServedAllTimeLastStored = 0;
        public int chickenRuinedServedAllTimeLastStored = 0;
        public int carrotsServedAllTimeLastStored = 0;
        public int carrotsRawServedAllTimeLastStored = 0;
        public int carrotsSteamedServedAllTimeLastStored = 0;
        public int carrotsRuinedServedAllTimeLastStored = 0;
        public int broccoliServedAllTimeLastStored = 0;
        public int broccoliRawServedAllTimeLastStored = 0;
        public int broccoliSteamedServedAllTimeLastStored = 0;
        public int broccoliRuinedServedAllTimeLastStored = 0;
        public int saladsServedAllTimeLastStored = 0;
        public int saladsGoodServedAllTimeLastStored = 0;
        public int saladsRuinedServedAllTimeLastStored = 0;

        public int foodOrderedAllTimeLastStored = 0;
        public int beefOrderedAllTimeLastStored = 0;
        public int beefRareOrderedAllTimeLastStored = 0; // GameManager/PickMenuItemsAndQuantities()
        public int beefMediumOrderedAllTimeLastStored = 0;
        public int beefWellDoneOrderedAllTimeLastStored = 0;
        public int chickenCookedOrderedAllTimeLastStored = 0;
        public int carrotsSteamedOrderedAllTimeLastStored = 0;
        public int broccoliSteamedOrderedAllTimeLastStored = 0;
        public int saladsOrderedAllTimeLastStored = 0;
    }

    public void SaveDataToDisk()
    {
        //Debug.Log(Application.persistentDataPath);
        SaveData data = new SaveData();
        data.highScoreData = highScoreData;
        data.bestPlayerData = bestPlayerData;
        data.elapsedGameTimeAllTimeLastStored = elapsedGameTimeAllTimeLastStored;
        data.diningRoomOpenTimeAllTimeLastStored = diningRoomOpenTimeAllTimeLastStored;
        data.tableOpenTimeAllTimeLastStored = tableOpenTimeAllTimeLastStored;
        data.isUsingScrubbiTimeAllTimeLastStored = isUsingScrubbiTimeAllTimeLastStored;
        data.isLateTimeAllTimeLastStored = isLateTimeAllTimeLastStored;
        data.elapsedWaitTimeTableAllTimeLastStored = elapsedWaitTimeTableAllTimeLastStored;
        
        ////////////////////////////

        data.ordersReceivedAllTimeLastStored = ordersReceivedAllTimeLastStored;
        data.guestsSeatedAllTimeLastStored = guestsSeatedAllTimeLastStored;
        data.deliveriesMadeAllTimeLastStored = deliveriesMadeAllTimeLastStored;
        data.perfectDeliveriesMadeAllTimeLastStored = perfectDeliveriesMadeAllTimeLastStored;
        data.wasLateTableCountAllTimeLastStored = wasLateTableCountAllTimeLastStored;

        ////////////////////////////

        data.foodDispensedAllTimeLastStored = foodDispensedAllTimeLastStored;
        data.beefDispensedAllTimeLastStored = beefDispensedAllTimeLastStored;
        data.chickenDispensedAllTimeLastStored = chickenDispensedAllTimeLastStored;
        data.carrotsDispensedAllTimeLastStored = carrotsDispensedAllTimeLastStored;
        data.broccoliDispensedAllTimeLastStored = broccoliDispensedAllTimeLastStored;
        data.saladsDispensedAllTimeLastStored = saladsDispensedAllTimeLastStored;

        data.foodServedAllTimeLastStored = foodServedAllTimeLastStored;
        data.beefServedAllTimeLastStored = beefServedAllTimeLastStored;
        data.beefRawServedAllTimeLastStored = beefRawServedAllTimeLastStored;
        data.beefRareServedAllTimeLastStored = beefRareServedAllTimeLastStored;
        data.beefMediumServedAllTimeLastStored = beefMediumServedAllTimeLastStored;
        data.beefWellDoneServedAllTimeLastStored = beefWellDoneServedAllTimeLastStored;
        data.beefRuinedServedAllTimeLastStored = beefRuinedServedAllTimeLastStored;
        data.chickenServedAllTimeLastStored = chickenServedAllTimeLastStored;
        data.chickenRawServedAllTimeLastStored = chickenRawServedAllTimeLastStored;
        data.chickenCookedServedAllTimeLastStored = chickenCookedServedAllTimeLastStored;
        data.chickenRuinedServedAllTimeLastStored = chickenRuinedServedAllTimeLastStored;
        data.carrotsServedAllTimeLastStored = carrotsServedAllTimeLastStored;
        data.carrotsRawServedAllTimeLastStored = carrotsRawServedAllTimeLastStored;
        data.carrotsSteamedServedAllTimeLastStored = carrotsSteamedServedAllTimeLastStored;
        data.carrotsRuinedServedAllTimeLastStored = carrotsRuinedServedAllTimeLastStored;
        data.broccoliServedAllTimeLastStored = broccoliServedAllTimeLastStored;
        data.broccoliRawServedAllTimeLastStored = broccoliRawServedAllTimeLastStored;
        data.broccoliSteamedServedAllTimeLastStored = broccoliSteamedServedAllTimeLastStored;
        data.broccoliRuinedServedAllTimeLastStored = broccoliRuinedServedAllTimeLastStored;
        data.saladsServedAllTimeLastStored = saladsServedAllTimeLastStored;
        data.saladsGoodServedAllTimeLastStored = saladsGoodServedAllTimeLastStored;
        data.saladsRuinedServedAllTimeLastStored = saladsRuinedServedAllTimeLastStored;

        data.foodOrderedAllTimeLastStored = foodOrderedAllTimeLastStored;
        data.beefOrderedAllTimeLastStored = beefOrderedAllTimeLastStored;
        data.beefRareOrderedAllTimeLastStored = beefRareOrderedAllTimeLastStored;
        data.beefMediumOrderedAllTimeLastStored = beefMediumOrderedAllTimeLastStored;
        data.beefWellDoneOrderedAllTimeLastStored = beefWellDoneOrderedAllTimeLastStored;
        data.chickenCookedOrderedAllTimeLastStored = chickenCookedOrderedAllTimeLastStored;
        data.carrotsSteamedOrderedAllTimeLastStored = carrotsSteamedOrderedAllTimeLastStored;
        data.broccoliSteamedOrderedAllTimeLastStored = broccoliSteamedOrderedAllTimeLastStored;
        data.saladsOrderedAllTimeLastStored = saladsOrderedAllTimeLastStored;


        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadDataFromDisk()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScoreData = data.highScoreData;
            bestPlayerData = data.bestPlayerData;
            elapsedGameTimeAllTimeLastStored = data.elapsedGameTimeAllTimeLastStored;
            diningRoomOpenTimeAllTimeLastStored = data.diningRoomOpenTimeAllTimeLastStored;
            tableOpenTimeAllTimeLastStored = data.tableOpenTimeAllTimeLastStored;
            isUsingScrubbiTimeAllTimeLastStored = data.isUsingScrubbiTimeAllTimeLastStored;
            isLateTimeAllTimeLastStored = data.isLateTimeAllTimeLastStored;
            elapsedWaitTimeTableAllTimeLastStored = data.elapsedWaitTimeTableAllTimeLastStored;
            
            ////////////////////////////

            ordersReceivedAllTimeLastStored = data.ordersReceivedAllTimeLastStored;
            guestsSeatedAllTimeLastStored = data.guestsSeatedAllTimeLastStored;
            deliveriesMadeAllTimeLastStored = data.deliveriesMadeAllTimeLastStored;
            perfectDeliveriesMadeAllTimeLastStored = data.perfectDeliveriesMadeAllTimeLastStored;
            wasLateTableCountAllTimeLastStored = data.wasLateTableCountAllTimeLastStored;

            ////////////////////////////

            foodDispensedAllTimeLastStored = data.foodDispensedAllTimeLastStored;
            beefDispensedAllTimeLastStored = data.beefDispensedAllTimeLastStored;
            chickenDispensedAllTimeLastStored = data.chickenDispensedAllTimeLastStored;
            carrotsDispensedAllTimeLastStored = data.carrotsDispensedAllTimeLastStored;
            broccoliDispensedAllTimeLastStored = data.broccoliDispensedAllTimeLastStored;
            saladsDispensedAllTimeLastStored = data.saladsDispensedAllTimeLastStored;

            foodServedAllTimeLastStored = data.foodServedAllTimeLastStored;
            beefServedAllTimeLastStored = data.beefServedAllTimeLastStored;
            beefRawServedAllTimeLastStored = data.beefRawServedAllTimeLastStored;
            beefRareServedAllTimeLastStored = data.beefRareServedAllTimeLastStored;
            beefMediumServedAllTimeLastStored = data.beefMediumServedAllTimeLastStored;
            beefWellDoneServedAllTimeLastStored = data.beefWellDoneServedAllTimeLastStored;
            beefRuinedServedAllTimeLastStored = data.beefRuinedServedAllTimeLastStored;
            chickenServedAllTimeLastStored = data.chickenServedAllTimeLastStored;
            chickenRawServedAllTimeLastStored = data.chickenRawServedAllTimeLastStored;
            chickenCookedServedAllTimeLastStored = data.chickenCookedServedAllTimeLastStored;
            chickenRuinedServedAllTimeLastStored = data.chickenRuinedServedAllTimeLastStored;
            carrotsServedAllTimeLastStored = data.carrotsServedAllTimeLastStored;
            carrotsRawServedAllTimeLastStored = data.carrotsRawServedAllTimeLastStored;
            carrotsSteamedServedAllTimeLastStored = data.carrotsSteamedServedAllTimeLastStored;
            carrotsRuinedServedAllTimeLastStored = data.carrotsRuinedServedAllTimeLastStored;
            broccoliServedAllTimeLastStored = data.broccoliServedAllTimeLastStored;
            broccoliRawServedAllTimeLastStored = data.broccoliRawServedAllTimeLastStored;
            broccoliSteamedServedAllTimeLastStored = data.broccoliSteamedServedAllTimeLastStored;
            broccoliRuinedServedAllTimeLastStored = data.broccoliRuinedServedAllTimeLastStored;
            saladsServedAllTimeLastStored = data.saladsServedAllTimeLastStored;
            saladsGoodServedAllTimeLastStored = data.saladsGoodServedAllTimeLastStored;
            saladsRuinedServedAllTimeLastStored = data.saladsRuinedServedAllTimeLastStored;

            foodOrderedAllTimeLastStored = data.foodOrderedAllTimeLastStored;
            beefOrderedAllTimeLastStored = data.beefOrderedAllTimeLastStored;
            beefRareOrderedAllTimeLastStored = data.beefRareOrderedAllTimeLastStored;
            beefMediumOrderedAllTimeLastStored = data.beefMediumOrderedAllTimeLastStored;
            beefWellDoneOrderedAllTimeLastStored = data.beefWellDoneOrderedAllTimeLastStored;
            chickenCookedOrderedAllTimeLastStored = data.chickenCookedOrderedAllTimeLastStored;
            carrotsSteamedOrderedAllTimeLastStored = data.carrotsSteamedOrderedAllTimeLastStored;
            broccoliSteamedOrderedAllTimeLastStored = data.broccoliSteamedOrderedAllTimeLastStored;
            saladsOrderedAllTimeLastStored = data.saladsOrderedAllTimeLastStored;

        }
    }
}
