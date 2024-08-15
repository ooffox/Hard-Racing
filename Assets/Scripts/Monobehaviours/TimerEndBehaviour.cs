using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class TimerEndBehaviour : MonoBehaviour
{
    GameManager _gameManager;
    void Start()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.parent != null && col.transform.parent.CompareTag("Player") && !_gameManager.HasFinishedRace)
        {
            _gameManager.HasFinishedRace = true;
            string timerText = GameObject.FindWithTag("Time Text").GetComponent<TMP_Text>().text;
            int secondsSep = timerText.IndexOf(".");
            int textSep = timerText.IndexOf(":") + 2;
            float time = float.Parse(timerText.Substring(textSep, timerText.Length - textSep));
            int seconds = Mathf.FloorToInt(time);
            int miliseconds = (int)((time % 1) * 1000);
            Timer timer = new Timer(0, seconds, miliseconds);

            UIManager uiManager = GameObject.FindObjectOfType<UIManager>();
            uiManager.WritePostraceTime(timer);
            uiManager.EnablePostraceOverlay();
        }
    }


}
