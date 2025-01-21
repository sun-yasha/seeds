using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    private int minuts = 0;
    private int seconds = 0;
    private Text textClock;
    private float delta_time;
    private bool stop_clock = false;
    public static Clock instance;

    private void Awake()
    {
        if (instance) Destroy(instance);
        instance = this;
        textClock = GetComponent<Text>();
        if(GameSettings.Instance.GetContinueGame()) delta_time = Config.ReadGameTime();
        else delta_time = 0;
    }
    void Start()
    {
        stop_clock = false;
    }

    void Update()
    {
        if (GameSettings.Instance.GetPaused() == false && stop_clock == false)
        {
            delta_time += Time.deltaTime;
            TimeSpan span = TimeSpan.FromSeconds(delta_time);
            string minuts_ = LeadingZero(span.Minutes);
            string seconds_ = LeadingZero(span.Seconds);
            textClock.text = minuts_ + ":" + seconds_;
        }
    }

    string LeadingZero(int c)
    {
        return c.ToString().PadLeft(2, '0');
    }

    public void OnGameOver()
    {
        stop_clock = true;
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += OnGameOver;
    }
    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;
    }
    public Text GetCurrentTimeText()
    {
        return textClock;
    }

    public static string GetCurrentTime()
    {
        return instance.delta_time.ToString();
    }
}
