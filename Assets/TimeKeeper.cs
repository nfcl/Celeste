using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeKeeper : MonoBehaviour
{

    [SerializeField] private All all;

    [SerializeField] private Current current;

    [Serializable]
    public class All
    {

        [SerializeField] private Text HH;
        [SerializeField] private Text MM;
        [SerializeField] private Text SS;
        [SerializeField] private Text MS;

        public void SetTime(TimeSpan newSpan)
        {

            HH.text = (newSpan.Days * 24 + newSpan.Hours).ToString("00");
            MM.text = newSpan.Minutes.ToString("00");
            SS.text = newSpan.Seconds.ToString("00");
            MS.text = newSpan.Milliseconds.ToString("000");

        }

    }

    [Serializable]
    public class Current
    {

        [SerializeField] private Text HH;
        [SerializeField] private Text MM;
        [SerializeField] private Text SS;

        public void SetTime(TimeSpan newSpan)
        {

            HH.text = (newSpan.Days * 24 + newSpan.Hours).ToString("00");
            MM.text = newSpan.Minutes.ToString("00");
            SS.text = newSpan.Seconds.ToString("00");

        }

    }

    [SerializeField] private TimeSpan startAll;
    [SerializeField] private TimeSpan startCur;

    [SerializeField] private DateTime StartTime;
    
    public TimeSpan AddingTime
    {

        get
        {

            return DateTime.Now - StartTime;

        }

    }

    public void Init(TimeSpan StartAll,TimeSpan StartCur)
    {

        startAll = StartAll;

        startCur = StartCur;

        all.SetTime(startAll);

        current.SetTime(startCur);

    }

    public void Open()
    {

        StartTime = DateTime.Now;

        StartCoroutine(Timer());

    }

    private IEnumerator Timer()
    {

        var wait = new WaitForSecondsRealtime(Time.fixedDeltaTime);

        while (true)
        {

            yield return wait;

            all.SetTime(startAll.Add(AddingTime));

            current.SetTime(startCur.Add(AddingTime));

        }

    }

#if UNITY_EDITOR

    [SerializeField] private int __All_HH;
    [SerializeField] private int __All_MM;
    [SerializeField] private int __All_SS;
    [SerializeField] private int __All_MS;

    [SerializeField] private int __Cur_HH;
    [SerializeField] private int __Cur_MM;
    [SerializeField] private int __Cur_SS;
    [SerializeField] private int __Cur_MS;

    private void OnValidate()
    {

        all.SetTime(new TimeSpan(0, __All_HH, __All_MM, __All_SS, __All_MS));

        current.SetTime(new TimeSpan(0, __Cur_HH, __Cur_MM, __Cur_SS, __Cur_MS));

    }

#endif

}
