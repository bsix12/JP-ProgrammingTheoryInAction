using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSmashed : MonoBehaviour
{
    private Renderer _myRenderer;
    public Color32 myColor; // provided by the food object that this new object replaces
    private bool _isMaxSize;

    private void Awake()
    {
        _myRenderer = GetComponent<Renderer>();       
    }

    private void Start()
    {
        Invoke("SetColor", .1f); // required delay; color not received from Base fast enough?
        StartCoroutine("GrowToMaximumSize");
    }

    private void SetColor()
    {
        _myRenderer.material.color = myColor; 
        gameObject.GetComponent<MeshRenderer>().enabled = true; // renderer disabled until color is set, otherwise starts default white
    }

    IEnumerator GrowToMaximumSize()
    {
        for(int i = 0; i < 10; i++)
        {
            transform.localScale = Vector3.one * i/10;
            yield return new WaitForSeconds(.1f);
        }        
    }
}
