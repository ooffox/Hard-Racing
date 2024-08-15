using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public int Minutes;
    public int Seconds;
    public int Miliseconds;

    public Timer(int minutes, int seconds, int miliseconds)
    {
        int extraSeconds = (int)Mathf.Floor(miliseconds / 1000);
        miliseconds -= extraSeconds * 1000;
        seconds += extraSeconds;

        int extraMinutes = (int)Mathf.Floor(seconds / 60);
        seconds -= extraMinutes * 60;
        minutes += extraMinutes;

        Minutes = minutes;
        Seconds = seconds;
        Miliseconds = miliseconds;
    }

    public string ToDisplay()
    {
        return String.Format("{0}:{1}.{2}", Minutes.ToString("D2"), Seconds.ToString("D2"), Miliseconds.ToString("D3"));
    }

    public int ToMiliseconds()
    {
        Debug.Log(Miliseconds);
        Debug.Log(Seconds);
        Debug.Log(Minutes);
        return Miliseconds + (Seconds * 1000) + (Minutes * 60 * 1000);
    }

    public static bool IsSmaller(Timer t1, Timer t2)
    {
        if (t1.Minutes == t2.Minutes)
        {
            if (t1.Seconds == t2.Seconds)
            {
                return t1.Miliseconds < t2.Miliseconds;
            }
            return t1.Seconds < t2.Seconds;
        }
        return t1.Minutes < t2.Minutes;
    }
   
}
