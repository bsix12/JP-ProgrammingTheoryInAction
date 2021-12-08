using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserStation : MonoBehaviour
{
    public GameObject stationMaterial;
    public Transform spawnLoc;
    public AudioSource audioSource;
    public AudioClip dispenserSound;
    [SerializeField] private bool _canDispense;
   

    private void Update()
    {
        if (GameManager.Instance.canDispense && _canDispense && Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.Instance.isFirstTimeTriggeringDispenser)
            {
                GameManager.Instance.isFirstTimeTriggeringDispenser = false;

                if (GameManager.Instance.isFirstTimeSpinningPlatter)
                {
                    GameManager.Instance.moveWithQEUnlocked = true;
                    GameManager.Instance.messageText.text = "Press <u>Q</u> or <u>E</u> to rotate platter.";
                }
                else
                {
                    GameManager.Instance.messageText.text = "";
                }
                
            }
            
            Instantiate(stationMaterial, spawnLoc);
            audioSource.PlayOneShot(dispenserSound, .3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TopPlate"))
        {
            _canDispense = true;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TopPlate"))
        {
            _canDispense = false;
        }
    }
}
