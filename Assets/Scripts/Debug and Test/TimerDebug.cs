using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerDebug : MonoBehaviour
{
    public TMP_Text timerText;
    private float timerFloat = 0;
    public TMP_Text inputTimer;


    void Start()
    {
        inputTimer.text = "0.00.00";
    }

    private void Update()
    {
        TimerDebugging();
    }

    private void TimerDebugging()
    {
        timerFloat += Time.deltaTime;


        int min = (int)timerFloat / 60;
        int sec = (int)(timerFloat % 60);
        int minsec = (int)((timerFloat - (int)timerFloat) * 100);

        timerText.text = min + "." + sec + "." + minsec;

        if (Input.GetKeyDown(KeyCode.E))
        {
            inputTimer.text = min + "." + sec + "." + minsec;
        }
    }
}
