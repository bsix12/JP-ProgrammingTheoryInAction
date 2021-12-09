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
        if (Input.GetKey(KeyCode.Q) && GameManager.Instance.isMoveWithQEUnlocked)
        {
            if (GameManager.Instance.isFirstTimeSpinningPlatter)
            {
                GameManager.Instance.messageText.text = "";
                GameManager.Instance.isFirstTimeSpinningPlatter = false;
                GameManager.Instance.isMovementTutorialActive = false;
            }
            transform.Rotate(Time.deltaTime * _turnSpeed * Vector3.down);
        }

        if (Input.GetKey(KeyCode.E) && GameManager.Instance.isMoveWithQEUnlocked)
        {
            if (GameManager.Instance.isFirstTimeSpinningPlatter)
            {
                GameManager.Instance.messageText.text = "";
                GameManager.Instance.isFirstTimeSpinningPlatter = false;
                GameManager.Instance.isMovementTutorialActive = false;
            }

            transform.Rotate(Time.deltaTime * _turnSpeed * Vector3.up);
        }
    }
}
