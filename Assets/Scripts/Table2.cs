using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table2 : Table
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

            else if (_timeRemainingUntilNextOrder <= 0 && !isGeneratingOrderTable1 && !isGeneratingOrderTable3)
            {
                GameManager.Instance.isReadyForNewOrderTable2 = false;
                isGeneratingOrderTable2 = true;
                GenerateOrderDetailsForTable();
            }
        }
    }
}
