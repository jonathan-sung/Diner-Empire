using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{

    int hour;
    int minute;
    //int second;

    //public Text secondText;
    public Text hourText;
    public Text minuteText;
    public Text am_pmText;

    public Text rushText;
    public Image rushImage;

    public void UpdateRush(float time, bool rushOn)
    {
        if (rushOn)
        {
            float scaleValue = (0.125f * Mathf.Sin((time * 4 * Mathf.PI) + (Mathf.PI / 2))) + 0.875f;
            rushText.transform.localScale = new Vector3(scaleValue, scaleValue, 0);
            rushImage.sprite = Items.panel_5;
        }
        else
        {
            rushText.transform.localScale = new Vector3(1, 1, 0);
            rushImage.sprite = Items.panel_4;
        }

    }
    public void UpdateClock(float time)
    {
        //43200 is 12 hours
        //second = Mathf.FloorToInt(time % 60);
        minute = Mathf.FloorToInt((time / 60) % 60);
        hour = Mathf.FloorToInt((time / 3600) % 24);
        if ((Mathf.FloorToInt(hour / 12) % 2) == 1)
        {
            am_pmText.text = "pm";
        }
        else
        {
            am_pmText.text = "am";
        }
        if (hour == 0) hour = 12;
        if (hour > 12) hour -= 12;
        string h = hour.ToString().PadLeft(2, '0');
        string m = minute.ToString().PadLeft(2, '0');
        //string s = second.ToString().PadLeft(2, '0');
        hourText.text = h;
        minuteText.text = m;

        //secondText.text = s;
    }
}
