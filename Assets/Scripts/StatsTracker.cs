using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsTracker : MonoBehaviour
{
    public static StatsTracker Instance;

    public TextMeshProUGUI statSummaryLabels1;
    public TextMeshProUGUI statSummaryLabels2;
    public TextMeshProUGUI sessionStatSummaryValues1;
    public TextMeshProUGUI sessionStatSummaryValues2;
    public TextMeshProUGUI allTimeStatSummaryValues1;
    public TextMeshProUGUI allTimeStatSummaryValues2;
    public Image statsBackground;



    ////////////////////////////
    /// THIS SESSION
    ////////////////////////////

    public float elapsedGameTimeSession = 0;
    public float diningRoomOpenTimeSession = 0; // StatsTracker/TrackDiningRoomServiceTimeTotal()
    public float tableOpenTimeSession = 0; // StatsTracker/TrackDiningRoomServiceTimeTotal().  1 table open for one minute = 1; 3 tables open for one minute = 3.
    public float isUsingScrubbiTimeSession = 0; // StatsTracker/TrackScrubbiTimeTotal()
    public float isLateTimeSession = 0; // StatsTracker/TrackLateTimeTotal()
    public float elapsedWaitTimeTableSession = 0; // TableTracker/AfterFoodIsServedActions()
    public float averageWaitTimePerGuestSession = 0; // (elapsedTimeToServeOrder/guestsSeatedTotal)
    public float averageWaitTimePerTableSession = 0; // (elapsedTimeToServeOrder/deliveriesMadeTotal)

    ////////////////////////////

    public int ordersReceivedSession = 0; // TableTracker/PublishOrder()
    public int guestsSeatedSession = 0; // TableTracker/SeatGuests()
    public int deliveriesMadeSession = 0; // TableTracker/AfterFoodIsServedActions()
    public int perfectDeliveriesMadeSession = 0; // GameManager/CheckIfPerfectDelivery()
    public int wasLateTableCountSession = 0; // TableTracker/OrderPrepElapsedTime()

    ////////////////////////////

    public int foodDispensedSession = 0; // DispenserStation/Update()/GetKeyDown
    public int beefDispensedSession = 0;
    public int chickenDispensedSession = 0;
    public int carrotsDispensedSession = 0;
    public int broccoliDispensedSession = 0;
    public int saladsDispensedSession = 0;

    public int foodServedSession = 0;
    public int beefServedSession = 0;
    public int beefRawServedSession = 0; // GameManager/CountFoodDelivered()
    public int beefRareServedSession = 0;
    public int beefMediumServedSession = 0;
    public int beefWellDoneServedSession = 0;
    public int beefRuinedServedSession = 0;
    public int chickenServedSession = 0;
    public int chickenRawServedSession = 0;
    public int chickenCookedServedSession = 0;
    public int chickenRuinedServedSession = 0;
    public int carrotsServedSession = 0;
    public int carrotsRawServedSession = 0;
    public int carrotsSteamedServedSession = 0;
    public int carrotsRuinedServedSession = 0;
    public int broccoliServedSession = 0;
    public int broccoliRawServedSession = 0;
    public int broccoliSteamedServedSession = 0;
    public int broccoliRuinedServedSession = 0;
    public int saladsServedSession = 0;
    public int saladsGoodServedSession = 0;
    public int saladsRuinedServedSession = 0;

    public int foodOrderedSession = 0;
    public int beefOrderedSession = 0;
    public int beefRareOrderedSession = 0; // GameManager/PickMenuItemsAndQuantities()
    public int beefMediumOrderedSession = 0;
    public int beefWellDoneOrderedSession = 0;
    public int chickenCookedOrderedSession = 0;
    public int carrotsSteamedOrderedSession = 0;
    public int broccoliSteamedOrderedSession = 0;
    public int saladsOrderedSession = 0;

    ////////////////////////////

    public int foodSmashedSession = 0; // Food/MonitorTimeOnFloor()
    public int foodWasteBinSession = 0; // WasteStation/OnTriggerEnter()/Destroy
    public int wasOnFloorAndServedSession = 0;

    ////////////////////////////

    private string _roundElapsedGameTimeSession;
    private string _roundDiningRoomOpenTimeSession;
    private string _roundTableOpenTimeSession;
    private string _roundIsUsingScrubbiTimeSession;
    private string _roundIsLateTimeSession;
    private string _roundElapsedWaitTimeTableSession;
    private string _roundAverageWaitTimePerGuestSession;
    private string _roundAverageWaitTimePerTableSession;




    ////////////////////////////
    /// ALL TIME, LAST STORED
    ////////////////////////////

    private string _roundElapsedGameTimeAllTime;
    private string _roundDiningRoomOpenTimeAllTime;
    private string _roundTableOpenTimeAllTime;
    private string _roundIsUsingScrubbiTimeAllTime;
    private string _roundIsLateTimeAllTime;
    private string _roundElapsedWaitTimeTableAllTime;
    private string _roundAverageWaitTimePerGuestAllTime;
    private string _roundAverageWaitTimePerTableAllTime;

    
    private bool isStatsActive = false;



  
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        statSummaryLabels1.text = "";
        statSummaryLabels2.text = "";
    }

    
    private void Update()
    {
        TrackElapsedGameTimeSession();
        TrackDiningRoomServiceTimeSession();
        TrackScrubbiTimeSession();
        TrackLateTimeSession();
        averageWaitTimePerGuestSession = elapsedWaitTimeTableSession / guestsSeatedSession;
        averageWaitTimePerTableSession = elapsedWaitTimeTableSession / deliveriesMadeSession;
        foodOrderedSession = beefRareOrderedSession + beefMediumOrderedSession + beefWellDoneOrderedSession + chickenCookedOrderedSession + carrotsSteamedOrderedSession + broccoliSteamedOrderedSession + saladsOrderedSession;
        beefOrderedSession = beefRareOrderedSession + beefMediumOrderedSession + beefWellDoneOrderedSession;
        beefServedSession = beefRawServedSession + beefRareServedSession + beefMediumServedSession + beefWellDoneServedSession + beefRuinedServedSession;
        chickenServedSession = chickenRawServedSession + chickenCookedServedSession + chickenRuinedServedSession;
        carrotsServedSession = carrotsRawServedSession + carrotsSteamedServedSession + carrotsRuinedServedSession;
        broccoliServedSession = broccoliRawServedSession + broccoliSteamedServedSession + broccoliRuinedServedSession;
        saladsServedSession = saladsGoodServedSession + saladsRuinedServedSession;
    }


    private void TrackElapsedGameTimeSession()
    {
        if (GameManager.Instance.isGameStarted)
        {
            elapsedGameTimeSession += Time.deltaTime;
        }
    }


    private void TrackDiningRoomServiceTimeSession()
    {
        if(GameManager.Instance.isOpenNowTableCount > 0)
        {
            diningRoomOpenTimeSession += Time.deltaTime;
            tableOpenTimeSession += Time.deltaTime * GameManager.Instance.isOpenNowTableCount;
        }
    }

    private void TrackScrubbiTimeSession()
    {
        if (GameManager.Instance.isUsingScrubbi)
        {
            isUsingScrubbiTimeSession += Time.deltaTime;
        }
    }

    private void TrackLateTimeSession()
    {
        if(GameManager.Instance.isLateNowTableCount > 0)
        {
            isLateTimeSession += Time.deltaTime * GameManager.Instance.isLateNowTableCount;
        }
    }


    private void FormatTimesForDisplay()
    {
        _roundElapsedGameTimeSession = Mathf.Floor(elapsedGameTimeSession / 60).ToString("0") + ":" + 
            Mathf.FloorToInt(elapsedGameTimeSession % 60).ToString("00");
        _roundDiningRoomOpenTimeSession = Mathf.Floor(diningRoomOpenTimeSession / 60).ToString("0") + ":" + 
            Mathf.FloorToInt(diningRoomOpenTimeSession % 60).ToString("00");
        _roundTableOpenTimeSession = Mathf.Floor(tableOpenTimeSession / 60).ToString("0") + ":" + 
            Mathf.FloorToInt(tableOpenTimeSession % 60).ToString("00");
        _roundIsUsingScrubbiTimeSession = Mathf.Floor(isUsingScrubbiTimeSession / 60).ToString("0") + ":" + 
            Mathf.FloorToInt(isUsingScrubbiTimeSession % 60).ToString("00");
        _roundIsLateTimeSession = Mathf.Floor(isLateTimeSession / 60).ToString("0") + ":" + 
            Mathf.FloorToInt(isLateTimeSession % 60).ToString("00");
        _roundElapsedWaitTimeTableSession = Mathf.Floor(elapsedWaitTimeTableSession / 60).ToString("0") + ":" + 
            Mathf.FloorToInt(elapsedWaitTimeTableSession % 60).ToString("00");        

        if(deliveriesMadeSession < 1)
        {
            _roundAverageWaitTimePerGuestSession = "n/a";
            _roundAverageWaitTimePerTableSession = "n/a";
        }

        else
        {
            _roundAverageWaitTimePerGuestSession = Mathf.Floor(averageWaitTimePerGuestSession / 60).ToString("0") + ":" + 
                Mathf.FloorToInt(averageWaitTimePerGuestSession % 60).ToString("00");
            _roundAverageWaitTimePerTableSession = Mathf.Floor(averageWaitTimePerTableSession / 60).ToString("0") + ":" + 
                Mathf.FloorToInt(averageWaitTimePerTableSession % 60).ToString("00");
        }
        
        _roundElapsedGameTimeAllTime = Mathf.Floor((DataStorage.Instance.elapsedGameTimeAllTimeLastStored + elapsedGameTimeSession) / 60).ToString("0") + ":" + 
            Mathf.FloorToInt((DataStorage.Instance.elapsedGameTimeAllTimeLastStored + elapsedGameTimeSession) % 60).ToString("00");
        _roundDiningRoomOpenTimeAllTime = Mathf.Floor((DataStorage.Instance.diningRoomOpenTimeAllTimeLastStored + diningRoomOpenTimeSession) / 60).ToString("0") + ":" + 
            Mathf.FloorToInt((DataStorage.Instance.diningRoomOpenTimeAllTimeLastStored + diningRoomOpenTimeSession) % 60).ToString("00");
        _roundTableOpenTimeAllTime = Mathf.Floor((DataStorage.Instance.tableOpenTimeAllTimeLastStored + tableOpenTimeSession) / 60).ToString("0") + ":" + 
            Mathf.FloorToInt((DataStorage.Instance.tableOpenTimeAllTimeLastStored + tableOpenTimeSession) % 60).ToString("00");
        _roundIsUsingScrubbiTimeAllTime = Mathf.Floor((DataStorage.Instance.isUsingScrubbiTimeAllTimeLastStored + isUsingScrubbiTimeSession) / 60).ToString("0") + ":" + 
            Mathf.FloorToInt((DataStorage.Instance.isUsingScrubbiTimeAllTimeLastStored + isUsingScrubbiTimeSession) % 60).ToString("00");
        _roundIsLateTimeAllTime = Mathf.Floor((DataStorage.Instance.isLateTimeAllTimeLastStored + isLateTimeSession) / 60).ToString("0") + ":" + 
            Mathf.FloorToInt((DataStorage.Instance.isLateTimeAllTimeLastStored + isLateTimeSession) % 60).ToString("00");
        _roundElapsedWaitTimeTableAllTime = Mathf.Floor((DataStorage.Instance.elapsedWaitTimeTableAllTimeLastStored + elapsedWaitTimeTableSession) / 60).ToString("0") + ":" + 
            Mathf.FloorToInt((DataStorage.Instance.elapsedWaitTimeTableAllTimeLastStored + elapsedWaitTimeTableSession) % 60).ToString("00");
 
        if(DataStorage.Instance.deliveriesMadeAllTimeLastStored == 0)
        {
            _roundAverageWaitTimePerGuestAllTime = _roundAverageWaitTimePerGuestSession;
            _roundAverageWaitTimePerTableAllTime = _roundAverageWaitTimePerTableSession;            
        }

        else
        {
            _roundAverageWaitTimePerGuestAllTime = Mathf.Floor((DataStorage.Instance.averageWaitTimePerGuestAllTimeLastStored + averageWaitTimePerGuestSession) / 60).ToString("0") + ":" + 
                Mathf.FloorToInt((DataStorage.Instance.averageWaitTimePerGuestAllTimeLastStored + averageWaitTimePerGuestSession) % 60).ToString("00");
            _roundAverageWaitTimePerTableAllTime = Mathf.Floor((DataStorage.Instance.averageWaitTimePerTableAllTimeLastStored + averageWaitTimePerTableSession) / 60).ToString("0") + ":" + 
                Mathf.FloorToInt((DataStorage.Instance.averageWaitTimePerTableAllTimeLastStored + averageWaitTimePerTableSession) % 60).ToString("00");
        } 
    }


    public void FillStatsText()
    {
        if (!isStatsActive)
        {
            FormatTimesForDisplay();
            StatsSummaryNames();
            StatsSummaryValuesSession();
            StatsSummaryValuesAllTime();
            statSummaryLabels1.gameObject.SetActive(true);
            statSummaryLabels2.gameObject.SetActive(true);
            sessionStatSummaryValues1.gameObject.SetActive(true);
            sessionStatSummaryValues2.gameObject.SetActive(true);
            allTimeStatSummaryValues1.gameObject.SetActive(true);
            allTimeStatSummaryValues2.gameObject.SetActive(true);
            statsBackground.gameObject.SetActive(true); // need to move this to closer canvas, cant set active false food guide button
            
            Time.timeScale = 0; // pause
            isStatsActive = true;
        }

        else if (isStatsActive)
        {
            statSummaryLabels1.gameObject.SetActive(false);
            statSummaryLabels2.gameObject.SetActive(false);
            sessionStatSummaryValues1.gameObject.SetActive(false);
            sessionStatSummaryValues2.gameObject.SetActive(false);
            allTimeStatSummaryValues1.gameObject.SetActive(false);
            allTimeStatSummaryValues2.gameObject.SetActive(false);
            statsBackground.gameObject.SetActive(false);
            
            Time.timeScale = 1; // unpause
            isStatsActive = false;
        }
    }


    private void StatsSummaryNames()
    {
        statSummaryLabels1.text =
            "Elapsed Time: \n" +
            "Dining Room Open Time: \n" +
            "Table 'Open' Time: \n" +
            "Scrubbi Time: \n\n" +

            "Average Wait Time per Table: \n" +
            "Average Wait Time per Guest: \n\n" +

            "Total Wait Time: \n" +
            "Total 'Late' Time: \n\n" +

            "Total Guests Seated: \n\n" +

            "Total Orders Received: \n" +
            "Total Orders Delivered: \n\n" +

            "Number of Perfect Deliveries: \n" +
            "Number of Late Deliveries: \n\n\n\n\n" +


            "<u><b>Total Items Dispensed: </u></b>\n" +
            "Beef: \n" +
            "Chicken: \n" +
            "Carrots: \n" +
            "Broccoli: \n" +
            "Salads: \n\n\n" +


            "Served after touching floor: \n" +
            "Tossed in waste bin: \n" +
            "Smashed on floor: \n";

        statSummaryLabels2.text =

            "<u><b>Total Items Ordered: </u></b>\n\n" +
            "Beef Total: \n" +
            "<indent=10%>Rare: \n" +
            "Medium: \n" +
            "Well-Done: </indent>\n\n" +

            "Chicken: \n" +
            "Carrots: \n" +
            "Broccoli: \n" +
            "Salads: \n\n\n" +


            "<u><b>Total Items Served: </u></b>\n\n" +

            "<b>Beef</b>\n" +
            "Rare: \n" +
            "Medium: \n" +
            "Well-Done: \n" +
            "Raw: \n" +
            "Ruined: \n\n" +

            "<b>Chicken</b>\n" +
            "Cooked: \n" +
            "Raw: \n" +
            "Ruined: \n\n" +

            "<b>Carrots</b>\n" +
            "Cooked: \n" +
            "Raw: \n" +
            "Ruined: \n\n" +

            "<b>Broccoli</b>\n" +
            "Cooked: \n" +
            "Raw: \n" +
            "Ruined: \n\n" +

            "<b>Salads</b>\n" +
            "Fresh: \n" +
            "Ruined: \n\n\n";
    }
     
    private void StatsSummaryValuesSession()
        {

            sessionStatSummaryValues1.text =
            _roundElapsedGameTimeSession + "\n" +
            _roundDiningRoomOpenTimeSession + "\n" +
            _roundTableOpenTimeSession + "\n" +
            _roundIsUsingScrubbiTimeSession + "\n\n" +

            _roundAverageWaitTimePerTableSession + "\n" +
            _roundAverageWaitTimePerGuestSession + "\n\n" +

            _roundElapsedWaitTimeTableSession + "\n" +
            _roundIsLateTimeSession + "\n\n" +

            guestsSeatedSession + "\n\n" +

            ordersReceivedSession + "\n" +
            deliveriesMadeSession + "\n\n" +

            perfectDeliveriesMadeSession + "\n" +
            wasLateTableCountSession + "\n\n\n\n\n" +
            
            foodDispensedSession + "\n" +
            beefDispensedSession + "\n" +
            chickenDispensedSession + "\n" +
            carrotsDispensedSession + "\n" +
            broccoliDispensedSession + "\n" +
            saladsDispensedSession + "\n\n\n" +


            wasOnFloorAndServedSession + "\n" +
            foodWasteBinSession + "\n" +
            foodSmashedSession + "\n";

        sessionStatSummaryValues2.text =
            foodOrderedSession + "\n\n" +
            beefOrderedSession + "\n" +
            beefRareOrderedSession + "\n" +
            beefMediumOrderedSession + "\n" +
            beefWellDoneOrderedSession + "\n\n" +

            chickenCookedOrderedSession + "\n" +
            carrotsSteamedOrderedSession + "\n" +
            broccoliSteamedOrderedSession + "\n" +
            saladsOrderedSession + "\n\n\n" +


            foodServedSession + "\n\n" +
            beefServedSession + "\n" +
            beefRareServedSession + "\n" +
            beefMediumServedSession + "\n" +
            beefWellDoneServedSession + "\n" +
            beefRawServedSession + "\n" +
            beefRuinedServedSession + "\n\n" +

            chickenServedSession +"\n" +
            chickenCookedServedSession + "\n" +
            chickenRawServedSession + "\n" +
            chickenRuinedServedSession + "\n\n" +

            carrotsServedSession + "\n" +
            carrotsSteamedServedSession + "\n" +
            carrotsRawServedSession + "\n" +
            carrotsRuinedServedSession + "\n\n" +

            broccoliServedSession + "\n" +
            broccoliSteamedServedSession + "\n" +
            broccoliRawServedSession + "\n" +
            broccoliRuinedServedSession + "\n\n" +

            saladsServedSession + "\n" +
            saladsGoodServedSession + "\n" +
            saladsRuinedServedSession;
    }

    private void StatsSummaryValuesAllTime() // AllTime = AllTimeLastStored + Session
    {
        allTimeStatSummaryValues1.text =
            _roundElapsedGameTimeAllTime + "\n" +
            _roundDiningRoomOpenTimeAllTime + "\n" +
            _roundTableOpenTimeAllTime + "\n" +
            _roundIsUsingScrubbiTimeAllTime + "\n\n" +

            _roundAverageWaitTimePerTableAllTime + "\n" +
            _roundAverageWaitTimePerGuestAllTime + "\n\n" +

            _roundElapsedWaitTimeTableAllTime + "\n" +
            _roundIsLateTimeAllTime + "\n\n" +

            (DataStorage.Instance.guestsSeatedAllTimeLastStored + guestsSeatedSession) + "\n\n" +

            (DataStorage.Instance.ordersReceivedAllTimeLastStored + ordersReceivedSession) + "\n" +
            (DataStorage.Instance.deliveriesMadeAllTimeLastStored + deliveriesMadeSession) + "\n\n" +

            (DataStorage.Instance.perfectDeliveriesMadeAllTimeLastStored + perfectDeliveriesMadeSession) + "\n" +
            (DataStorage.Instance.wasLateTableCountAllTimeLastStored + wasLateTableCountSession) + "\n\n\n\n\n" +

            (DataStorage.Instance.foodDispensedAllTimeLastStored + foodDispensedSession) + "\n" +
            (DataStorage.Instance.beefDispensedAllTimeLastStored + beefDispensedSession) + "\n" +
            (DataStorage.Instance.chickenDispensedAllTimeLastStored + chickenDispensedSession) + "\n" +
            (DataStorage.Instance.carrotsDispensedAllTimeLastStored + carrotsDispensedSession) + "\n" +
            (DataStorage.Instance.broccoliDispensedAllTimeLastStored + broccoliDispensedSession) + "\n" +
            (DataStorage.Instance.saladsDispensedAllTimeLastStored + saladsDispensedSession) + "\n\n\n" +


            (DataStorage.Instance.wasOnFloorAndServedAllTimeLastStored + wasOnFloorAndServedSession) + "\n" +
            (DataStorage.Instance.foodWasteBinAllTimeLastStored + foodWasteBinSession) + "\n" +
            (DataStorage.Instance.foodSmashedAllTimeLastStored + foodSmashedSession) + "\n";

        allTimeStatSummaryValues2.text =
            (DataStorage.Instance.foodOrderedAllTimeLastStored + foodOrderedSession) + "\n\n" +
            (DataStorage.Instance.beefOrderedAllTimeLastStored + beefOrderedSession) + "\n" +
            (DataStorage.Instance.beefRareOrderedAllTimeLastStored + beefRareOrderedSession) + "\n" +
            (DataStorage.Instance.beefMediumOrderedAllTimeLastStored + beefMediumOrderedSession) + "\n" +
            (DataStorage.Instance.beefWellDoneOrderedAllTimeLastStored + beefWellDoneOrderedSession) + "\n\n" +

            (DataStorage.Instance.chickenCookedOrderedAllTimeLastStored + chickenCookedOrderedSession) + "\n" +
            (DataStorage.Instance.carrotsSteamedOrderedAllTimeLastStored + carrotsSteamedOrderedSession) + "\n" +
            (DataStorage.Instance.broccoliSteamedOrderedAllTimeLastStored + broccoliSteamedOrderedSession) + "\n" +
            (DataStorage.Instance.saladsOrderedAllTimeLastStored + saladsOrderedSession) + "\n\n\n" +


            (DataStorage.Instance.foodServedAllTimeLastStored + foodServedSession) + "\n\n" +
            (DataStorage.Instance.beefServedAllTimeLastStored + beefServedSession) + "\n" +
            (DataStorage.Instance.beefRareServedAllTimeLastStored + beefRareServedSession) + "\n" +
            (DataStorage.Instance.beefMediumServedAllTimeLastStored + beefMediumServedSession) + "\n" +
            (DataStorage.Instance.beefWellDoneServedAllTimeLastStored + beefWellDoneServedSession) + "\n" +
            (DataStorage.Instance.beefRawServedAllTimeLastStored + beefRawServedSession) + "\n" +
            (DataStorage.Instance.beefRuinedServedAllTimeLastStored + beefRuinedServedSession) + "\n\n" +

            (DataStorage.Instance.chickenServedAllTimeLastStored + chickenServedSession) + "\n" +
            (DataStorage.Instance.chickenCookedServedAllTimeLastStored + chickenCookedServedSession) + "\n" +
            (DataStorage.Instance.chickenRawServedAllTimeLastStored + chickenRawServedSession) + "\n" +
            (DataStorage.Instance.chickenRuinedServedAllTimeLastStored + chickenRuinedServedSession) + "\n\n" +

            (DataStorage.Instance.carrotsServedAllTimeLastStored + carrotsServedSession) + "\n" +
            (DataStorage.Instance.carrotsSteamedServedAllTimeLastStored + carrotsSteamedServedSession) + "\n" +
            (DataStorage.Instance.carrotsRawServedAllTimeLastStored + carrotsRawServedSession) + "\n" +
            (DataStorage.Instance.carrotsRuinedServedAllTimeLastStored + carrotsRuinedServedSession) + "\n\n" +

            (DataStorage.Instance.broccoliServedAllTimeLastStored + broccoliServedSession) + "\n" +
            (DataStorage.Instance.broccoliSteamedServedAllTimeLastStored + broccoliSteamedServedSession) + "\n" +
            (DataStorage.Instance.broccoliRawServedAllTimeLastStored + broccoliRawServedSession) + "\n" +
            (DataStorage.Instance.broccoliRuinedServedAllTimeLastStored + broccoliRuinedServedSession) + "\n\n" +

            (DataStorage.Instance.saladsServedAllTimeLastStored + saladsServedSession) + "\n" +
            (DataStorage.Instance.saladsGoodServedAllTimeLastStored + saladsGoodServedSession) + "\n" +
            (DataStorage.Instance.saladsRuinedServedAllTimeLastStored + saladsRuinedServedSession);
    }

    private void AddLastStoredAndSessionValuesSendToDataStorage()
    {
        DataStorage.Instance.elapsedGameTimeAllTimeLastStored += elapsedGameTimeSession;
        DataStorage.Instance.diningRoomOpenTimeAllTimeLastStored += diningRoomOpenTimeSession;
        DataStorage.Instance.tableOpenTimeAllTimeLastStored += tableOpenTimeSession;
        DataStorage.Instance.isUsingScrubbiTimeAllTimeLastStored += isUsingScrubbiTimeSession;

        DataStorage.Instance.averageWaitTimePerTableAllTimeLastStored += averageWaitTimePerTableSession;
        DataStorage.Instance.averageWaitTimePerGuestAllTimeLastStored += averageWaitTimePerGuestSession;

        DataStorage.Instance.elapsedWaitTimeTableAllTimeLastStored += elapsedWaitTimeTableSession;
        DataStorage.Instance.isLateTimeAllTimeLastStored += isLateTimeSession;

        DataStorage.Instance.guestsSeatedAllTimeLastStored += guestsSeatedSession;

        DataStorage.Instance.ordersReceivedAllTimeLastStored += ordersReceivedSession;
        DataStorage.Instance.deliveriesMadeAllTimeLastStored += deliveriesMadeSession;

        DataStorage.Instance.perfectDeliveriesMadeAllTimeLastStored += perfectDeliveriesMadeSession;
        DataStorage.Instance.wasLateTableCountAllTimeLastStored += wasLateTableCountSession;

        DataStorage.Instance.foodDispensedAllTimeLastStored += foodDispensedSession;
        DataStorage.Instance.beefDispensedAllTimeLastStored += beefDispensedSession;
        DataStorage.Instance.chickenDispensedAllTimeLastStored += chickenDispensedSession;
        DataStorage.Instance.carrotsDispensedAllTimeLastStored += carrotsDispensedSession;
        DataStorage.Instance.broccoliDispensedAllTimeLastStored += broccoliDispensedSession;
        DataStorage.Instance.saladsDispensedAllTimeLastStored += saladsDispensedSession;

        DataStorage.Instance.wasOnFloorAndServedAllTimeLastStored += wasOnFloorAndServedSession;
        DataStorage.Instance.foodWasteBinAllTimeLastStored += foodWasteBinSession;
        DataStorage.Instance.foodSmashedAllTimeLastStored += foodSmashedSession;

        DataStorage.Instance.foodOrderedAllTimeLastStored += foodOrderedSession;
        DataStorage.Instance.beefOrderedAllTimeLastStored += beefOrderedSession;
        DataStorage.Instance.beefRareOrderedAllTimeLastStored += beefRareOrderedSession;
        DataStorage.Instance.beefMediumOrderedAllTimeLastStored += beefMediumOrderedSession;
        DataStorage.Instance.beefWellDoneOrderedAllTimeLastStored += beefWellDoneOrderedSession;

        DataStorage.Instance.chickenCookedOrderedAllTimeLastStored += chickenCookedOrderedSession;
        DataStorage.Instance.carrotsSteamedOrderedAllTimeLastStored += carrotsSteamedOrderedSession;
        DataStorage.Instance.broccoliSteamedOrderedAllTimeLastStored += broccoliSteamedOrderedSession;
        DataStorage.Instance.saladsOrderedAllTimeLastStored += saladsOrderedSession;

        DataStorage.Instance.foodServedAllTimeLastStored += foodServedSession;
        DataStorage.Instance.beefServedAllTimeLastStored += beefServedSession;
        DataStorage.Instance.beefRareServedAllTimeLastStored += beefRareServedSession;
        DataStorage.Instance.beefMediumServedAllTimeLastStored += beefMediumServedSession;
        DataStorage.Instance.beefWellDoneServedAllTimeLastStored += beefWellDoneServedSession;
        DataStorage.Instance.beefRawServedAllTimeLastStored += beefRawServedSession;
        DataStorage.Instance.beefRuinedServedAllTimeLastStored += beefRuinedServedSession;

        DataStorage.Instance.chickenServedAllTimeLastStored += chickenServedSession;
        DataStorage.Instance.chickenCookedServedAllTimeLastStored += chickenCookedServedSession;
        DataStorage.Instance.chickenRawServedAllTimeLastStored += chickenRawServedSession;
        DataStorage.Instance.chickenRuinedServedAllTimeLastStored += chickenRuinedServedSession;

        DataStorage.Instance.carrotsServedAllTimeLastStored += carrotsServedSession;
        DataStorage.Instance.carrotsSteamedServedAllTimeLastStored += carrotsSteamedServedSession;
        DataStorage.Instance.carrotsRawServedAllTimeLastStored += carrotsRawServedSession;
        DataStorage.Instance.carrotsRuinedServedAllTimeLastStored += carrotsRuinedServedSession;

        DataStorage.Instance.broccoliServedAllTimeLastStored += broccoliServedSession;
        DataStorage.Instance.broccoliSteamedServedAllTimeLastStored += broccoliSteamedServedSession;
        DataStorage.Instance.broccoliRawServedAllTimeLastStored += broccoliRawServedSession;
        DataStorage.Instance.broccoliRuinedServedAllTimeLastStored += broccoliRuinedServedSession;

        DataStorage.Instance.saladsServedAllTimeLastStored += saladsServedSession;
        DataStorage.Instance.saladsGoodServedAllTimeLastStored += saladsGoodServedSession;
        DataStorage.Instance.saladsRuinedServedAllTimeLastStored += saladsRuinedServedSession;
    }
}

