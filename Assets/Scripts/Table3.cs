using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table3 : Table
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

            else if (_timeRemainingUntilNextOrder <= 0 && !isGeneratingOrderTable1 && !isGeneratingOrderTable2)
            {
                GameManager.Instance.isReadyForNewOrderTable3 = false;
                isGeneratingOrderTable3 = true;
                GenerateOrderDetailsForTable();
            }
        }
    }
}
