using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserStation : MonoBehaviour
{
    ////////////////////////////
    /// Assign In Inspector
    ////////////////////////////

    public GameObject stationMaterial;
    public Transform spawnLoc;
    public AudioSource audioSource;
    public AudioClip dispenserSound;

    ////////////////////////////

    [SerializeField] private bool _canDispense; // _canDispense is player in trigger.  GameManager.Instance.canDispense is disabled if needs to clean
   


    private void Update()
    {
        if (GameManager.Instance.canDispense && _canDispense && Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance.isFirstTimeTriggeringDispenser)
            {
                GameManager.Instance.isFirstTimeTriggeringDispenser = false;

                if (GameManager.Instance.isFirstTimeSpinningPlatter)
                {
                    GameManager.Instance.isMoveWithQEUnlocked = true;
                    GameManager.Instance.messageText.text = "Press <u>Q</u> or <u>E</u> to rotate platter.";
                }
                else
                {
                    GameManager.Instance.messageText.text = "";
                }                
            }
            
            Instantiate(stationMaterial, spawnLoc);
            audioSource.PlayOneShot(dispenserSound, .3f);
            StatsTracker.Instance.foodDispensedSession += 1;

            if(stationMaterial.name == "Beef") // these strings must match prefab.name
            {
                StatsTracker.Instance.beefDispensedSession += 1;
            }

            else if(stationMaterial.name == "Chicken")
            {
                StatsTracker.Instance.chickenDispensedSession += 1;
            }

            else if (stationMaterial.name == "Broccoli")
            {
                StatsTracker.Instance.broccoliDispensedSession += 1;
            }

            else if (stationMaterial.name == "Carrot")
            {
                StatsTracker.Instance.carrotsDispensedSession += 1;
            }

            else if (stationMaterial.name == "Salad")
            {
                StatsTracker.Instance.saladsDispensedSession += 1;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TopPlate"))
        {
            _canDispense = true;

            if (GameManager.Instance.isFirstTimeTriggeringDispenser)
            {
                GameManager.Instance.messageText.text = "Press <u>spacebar</u> to dispense food.";
            }

            if (GameManager.Instance.isMoveWithQEUnlocked && GameManager.Instance.isFirstTimeSpinningPlatter)
            {
                GameManager.Instance.messageText.text = "Press <u>Q</u> or <u>E</u> to rotate platter.";
            }            
        }  
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TopPlate"))
        {
            _canDispense = false;

            if (GameManager.Instance.isFirstTimeTriggeringDispenser)
            {
                GameManager.Instance.messageText.text = "";
            }
        }
    }
}
