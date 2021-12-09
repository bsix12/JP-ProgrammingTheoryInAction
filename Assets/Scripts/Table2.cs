using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table2 : TableTracker
{
    protected override void Update()
    {
        TimeRemainingUntilNextOrder();
        base.Update();
    }

    private void TimeRemainingUntilNextOrder()
    {
        if (GameManager.Instance.isActiveTable2 && GameManager.Instance.isReadyForNewOrderTable2)
        {
            if (_timeRemainingUntilNextOrder > 0)
            {
                _timeRemainingUntilNextOrder -= Time.deltaTime;
            }

            else if (_timeRemainingUntilNextOrder <= 0 && !GameManager.Instance.isGeneratingOrderTable1 && !GameManager.Instance.isGeneratingOrderTable3 && !GameManager.Instance.isCalculatingScore)
            {
                GameManager.Instance.isReadyForNewOrderTable2 = false;
                GameManager.Instance.isGeneratingOrderTable2 = true;
                GameManager.Instance.GenerateOrderForTable();
            }
        }
    }
}
