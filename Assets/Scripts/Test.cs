using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    bool showInfo;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfoDisplay();
    }

    public void UpdateInfoDisplay()
    {
        if (showInfo)
        {
            Debug.Log("over");
        }
    }

    public void OnMouseExit()
    {
        showInfo = false;
    }
    public void OnMouseEnter()
    {
        showInfo = true;
    }

}
