using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorktopHover : MonoBehaviour
{

    private Item item;
    public InfoDisplay infoDisplay;
    private Worktop worktop;
    public bool over;
    bool clicked;

    // Use this for initialization
    void Start()
    {
        worktop = GetComponent<Worktop>();
        infoDisplay = GameObject.Find("Player").GetComponent<InfoDisplay>();
        CheckItem();
    }

    // Update is called once per frame
    void Update()
    {
        if (over)
        {
            CheckItem();
            if (item != null) infoDisplay.Display(item);
        }
    }

    private void CheckItem()
    {
        item = worktop.slot;
    }
}
