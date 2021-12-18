using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitlePage : MonoBehaviour
{
    public GameObject enterNameCanvas;
    public TMP_InputField enterNameInputField;
    public Button startGameButton;
    public TextMeshProUGUI subTitleAddChefName;
    //public TextMeshProUGUI highScoreText;


    private bool _isGameStarted;
    private bool _isGameOver;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && enterNameInputField.text != "")
        {
            ShowTitleScreen();
            DataStorage.Instance.playerNameInputData = enterNameInputField.text;
        }
    }

    private void ShowTitleScreen()
    {
        subTitleAddChefName.text = "with guest, Chef " + enterNameInputField.text;
        enterNameCanvas.gameObject.SetActive(false);
    }


    public void SavePlayerNameInput()
    {
        DataStorage.Instance.SaveDataToDisk();
    }

    public void EnterKitchen() // OnClick, assign in inspector
    {
        SceneManager.LoadScene(1);
    }
}
