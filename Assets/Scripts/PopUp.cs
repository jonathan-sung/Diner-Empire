using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUp : MonoBehaviour, IPointerClickHandler
{

    public float showTime;
    float currentTime;
    public int type; //1 rating, 2 message/rush hour
    public float rating;
    public float money;
    public Text text;
    public Image textImage;

    public void StartPopUp()
    {
        if (type == 1)
        {
            text.text = (rating * 100).ToString("F0") + "%";
            if (rating >= 0.5f)
            {
                text.color = new Color32(115, 227, 38, 255);
            }
            else if (rating >= 0.25f)
            {
                text.color = new Color32(227, 222, 38, 255);
            }
            else
            {
                text.color = new Color32(255, 0, 0, 255);
            }
        }
    }

    public void StartPopUp(string message)
    {
        if (type == 2)
        {
            text.text = message;
        }
    }

    void Update()
    {
        //showTime -= Time.deltaTime;
        currentTime += Time.deltaTime;
        if (currentTime >= showTime)
        {
            Destroy(gameObject);
        }
        if (type == 1)
        {
            if (currentTime < 0.2f)
            {
                transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);
            }
            else if (currentTime >= 0.2f && currentTime < 1)
            {
                transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, 0);
                if (transform.localScale.x < 1) transform.localScale = new Vector3(1, 1, 1);
            }
            if (showTime >= 0 && showTime < 1)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, showTime);
                transform.localPosition -= new Vector3(0, Time.deltaTime, 0);
            }
        }
        else if (type == 2)
        {
            float scaleValue = (0.125f * Mathf.Sin((currentTime * 4 * Mathf.PI) + (Mathf.PI / 2))) + 0.875f;
            textImage.transform.localScale = new Vector3(scaleValue, scaleValue, 0);
        }
        else if (type == 3)
        {
            float scaleValue = (-4 / (showTime * showTime)) * currentTime * (currentTime - showTime);
            textImage.transform.localScale = new Vector3(scaleValue, scaleValue, 0);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}
