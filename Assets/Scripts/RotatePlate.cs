using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlate : MonoBehaviour
{
    private float _turnSpeed = 150f;



    void Update()
    {
        RotatePlateTop();
    }

    private void RotatePlateTop()
    {
        if (Input.GetKey(KeyCode.Q) && GameManager.Instance.moveWithQEUnlocked)
        {
            if (GameManager.Instance.isFirstTimeSpinningPlatter)
            {
                GameManager.Instance.messageText.text = "";
                GameManager.Instance.isFirstTimeSpinningPlatter = false;
                GameManager.Instance.isDoingMovementTutorial = false;
            }
            transform.Rotate(Time.deltaTime * _turnSpeed * Vector3.down);
        }

        if (Input.GetKey(KeyCode.E) && GameManager.Instance.moveWithQEUnlocked)
        {
            if (GameManager.Instance.isFirstTimeSpinningPlatter)
            {
                GameManager.Instance.messageText.text = "";
                GameManager.Instance.isFirstTimeSpinningPlatter = false;
                GameManager.Instance.isDoingMovementTutorial = false;
            }

            transform.Rotate(Time.deltaTime * _turnSpeed * Vector3.up);
        }
    }
}
