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

            else if (_timeRemainingUntilNextOrder <= 0 && !GameManager.Instance.isGeneratingOrderTable2 && !GameManager.Instance.isGeneratingOrderTable3 && !GameManager.Instance.isCalculatingScore)
            {
                GameManager.Instance.isReadyForNewOrderTable1 = false;
                GameManager.Instance.isGeneratingOrderTable1 = true;
                GenerateOrderForTable();
            }
        }
    }
}
