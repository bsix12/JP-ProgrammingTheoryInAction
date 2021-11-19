using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeFood : MonoBehaviour
{
    private Rigidbody _otherRb;
    private GameObject _player;
    private float _offsetDistance = .5f;
    private Vector3 _offset;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            _otherRb = other.GetComponent<Rigidbody>();
            MoveFoodToPlate();
        }

        // review and post results, allow new order to be generated
        // isDoneServing to prevent player triggering duplicate report
        if (other.CompareTag("Player") && GameManager.Instance.isDoneServing == false)
        {
            GameManager.Instance.Invoke("CountFoodDelivered", 1f);
            GameManager.Instance.Invoke("PostToKitchenReportCard", 2f);
            GameManager.Instance.Invoke("CalculateScore", 1f);
            GameManager.Instance.isDoneServing = true;
            GameManager.Instance.isReadyForNewOrder = true;
        }
    }

    void MoveFoodToPlate()
    {
        // remove player movement transfering with food
        OnPlatePosition();
        _otherRb.transform.position = transform.position + _offset;
        _otherRb.velocity = Vector3.zero;
        _otherRb.angularVelocity = Vector3.zero;
    }

    void OnPlatePosition()
    {
        // generate random locations on customer plate to deposit food
        _offset.x = Random.Range(-_offsetDistance, _offsetDistance);
        _offset.y = 0f;
        _offset.z = Random.Range(-_offsetDistance, _offsetDistance);
    }
}
