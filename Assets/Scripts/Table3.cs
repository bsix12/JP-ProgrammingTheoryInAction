using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table3 : TableTracker
{
    protected override void Update()
    {
        TimeRemainingUntilNextOrder();
        base.Update();
    }

    private void TimeRemainingUntilNextOrder()
    {
        if (GameManager.Instance.isActiveTable3 && GameManager.Instance.isReadyForNewOrderTable3)
        {
            if (_timeRemainingUntilNextOrder > 0)
            {
                _timeRemainingUntilNextOrder -= Time.deltaTime;
            }

            else if (_timeRemainingUntilNextOrder <= 0 && !GameManager.Instance.isGeneratingOrderTable1 && !GameManager.Instance.isGeneratingOrderTable2 && !GameManager.Instance.isCalculatingScore)
            {
                GameManager.Instance.isReadyForNewOrderTable3 = false;
                GameManager.Instance.isGeneratingOrderTable3 = true;
                GameManager.Instance.GenerateOrderForTable();
            }
        }
    }
}
