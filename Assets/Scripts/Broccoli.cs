using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broccoli : Food
{
    private Renderer _myRenderer;

    private void Awake()
    {
        myTemp = 57f;
        myRawColor = new Color32(75, 200, 50, 255);
        myCurrentColor = myRawColor;

        _myRenderer = GetComponent<Renderer>();
        _myRenderer.material.color = myCurrentColor;
    }
}