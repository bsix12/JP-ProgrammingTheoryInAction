using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsAndNotes : MonoBehaviour
{
    public TextMeshProUGUI creditsText;
    public TextMeshProUGUI notesText;

    private void Start()
    {
        FillCreditsText();
        FillNotesText();
    }

    private void FillCreditsText()
    {
        creditsText.text =
            "\n<b><u>Credits</b></u>\n" +
            "Music:\n" +
            "Hardmoon\n" +
            "\n" +
            "Sound Effects:\n" +
            "bell_02\n" +
            "cooking_without_cover_01\n" +
            "fire\n" +
            "metal+03\n" +
            "microwave_door_close\n" +
            "plop_01\n" +
            "qubodupFireLoop\n" +
            "Sizzling-sound-effect\n" +
            "\n" +
            "Thank you to my family for play testing and feedback!";
    }

    private void FillNotesText()
    {
        notesText.text =
            "\n<b><u>Notes</b></u>\n" +
            "Created for Unity Learn Junior Programming Theory In Action";

    }
}
