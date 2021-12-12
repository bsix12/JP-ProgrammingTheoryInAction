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
            "pleasing-bell\n" +
            "bell_02\n" +
            "cooking_without_cover_01\n" +
            "fire\n" +
            "metal+03\n" +
            "microwave_door_close\n" +
            "plop_01\n" +
            "qubodupFireLoop\n" +
            "Sizzling-sound-effect\n" +
            "wood01\n" +
            "\n" +
            "Thank you to my family for play testing and feedback!";
    }

    private void FillNotesText()
    {
        notesText.text =
            "\n<b><u>Notes</b></u>\n" +
            "Created for Unity Learn Junior Programming Theory In Action\n" +
            "Completed on xxxxxxxxxxxxxxxxxxxxx\n" +
            "\n" +
            "On my beginners journey to Unity and game development, I've heard the following message many times, 'just build something, <u>finish it</u> and publish it; that is the best way to learn.  For this last assignment, I decided to expand my effort andy try to finish a more complete game.\n" +
            "In this way, I would reinforce skills development in the UnityLearn Pathways while building a body of\n" +
            "work that could serve as reference material for future work.\n" +
            "\n" +
            "<u>Assignment Details</u>\n" +

            "ABSTRACTION - probably the easiest of the pillars to put into beginners practice.   \n" +
            "INHERITANCE -\n" +
            "POLYMORPHISM -\n" +
            "ENCAPSULATION -\n" +

            "While developing this game, code was refactored a number of times.  First time in an attempts to be\n" +
            "purposful/thoughtful about resetting variables after use.";

    }
}
