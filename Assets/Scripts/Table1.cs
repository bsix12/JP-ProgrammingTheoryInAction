using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table1 : Table
{
    protected override void Update()
    {
        TimeRemainingUntilNextOrder();
        base.Update();
    }

    private void TimeRemainingUntilNextOrder()
    {
        if (GameManager.Instance.isActiveTable1 && GameManager.Instance.isReadyForNewOrderTable1)
        {
            if (_timeRemainingUntilNextOrder > 0)
            {
                _timeRemainingUntilNextOrder -= Time.deltaTime;
            }

            else if (_timeRemainingUntilNextOrder <= 0 && !isGeneratingOrderTable2 && !isGeneratingOrderTable3)
            {
                GameManager.Instance.isReadyForNewOrderTable1 = false;
                isGeneratingOrderTable1 = true;
                GenerateOrderDetailsForTable();
            }
        }
    }
}
