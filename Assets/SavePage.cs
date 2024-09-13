using DG.Tweening;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SavePage : MonoBehaviour
{

    [SerializeField] private Transform Letter;
    [SerializeField] private Transform Envelop;

    [SerializeField] private GameObject[] Content;
    [SerializeField] private GameObject NewGame;

    [SerializeField] private Text SaveName;
    [SerializeField] private Text StrawberryNum;
    [SerializeField] private Text CurrentMapName;

    [SerializeField] private Text DeathNum;
    [SerializeField] private TimeText TimeSpent;

    [SerializeField] private int saveFileIndex;

    [Serializable]
    public class TimeText
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

    public Metric.Archive.SaveFile SaveFile
    {

        get
        {

            return Metric.Archive.saveFiles[saveFileIndex];

        }

    }

    public void Init(int index)
    {

        saveFileIndex = index;

        if(SaveFile == null)
        {

            foreach(var item in Content)
            {

                item.SetActive(false);

            }

            NewGame.SetActive(true);

            return;

        }
        else
        {

            foreach (var item in Content)
            {

                item.SetActive(true);

            }

            NewGame.SetActive(false);

        }

        SaveName.text = SaveFile.SaveName;

        if (SaveFile.mapRecords != null && SaveFile.mapRecords.Length > 0)
        {

            CurrentMapName.text = Metric.MapsInfo.infos[SaveFile.mapRecords[SaveFile.mapRecords.Length - 1].MapIndex].MapName;

        }
        else
        {

            CurrentMapName.text = "";

        }

        StrawberryNum.text = SaveFile.GetStrawberryNum().ToString();

        DeathNum.text = SaveFile.GetDeathNum().ToString();

        TimeSpent.SetTime(SaveFile.GetTimeSpent());

    }

    public bool IsMoveOn
    {

        set
        {

            if (value)
            {

                Letter.DOLocalMoveX(358, 0.2f);
                Envelop.DOLocalMoveX(-362f, 0.2f);

            }
            else
            {

                Letter.DOLocalMoveX(0, 0.2f);
                Envelop.DOLocalMoveX(0, 0.2f);

            }

        }

    }

#if UNITY_EDITOR

    [SerializeField] private bool __isMoveOn;

    [SerializeField] private bool __hasSave;

    [SerializeField] private string __SaveName;
    [SerializeField] private int __StrawberryNum;
    [SerializeField] private string __CurrentMapName;

    [SerializeField] private int __DeathNum;

    private void OnValidate()
    {

        if (__isMoveOn)
        {

            Letter.localPosition = new Vector3(358, Letter.localPosition.y, Letter.localPosition.z);
            Envelop.localPosition = new Vector3(-362f, Envelop.localPosition.y, Envelop.localPosition.z);

        }
        else
        {

            Letter.localPosition = Vector3.zero;
            Envelop.localPosition = Vector3.zero;

        }

        if (__hasSave)
        {

            foreach (var item in Content)
            {

                item.SetActive(true);

            }

            NewGame.SetActive(false);

        }
        else
        {

            foreach (var item in Content)
            {

                item.SetActive(false);

            }

            NewGame.SetActive(true);

            return;

        }

        SaveName.text = __SaveName;

        StrawberryNum.text = __StrawberryNum.ToString();

        CurrentMapName.text = __CurrentMapName;

        DeathNum.text = __DeathNum.ToString();

    }

#endif

}
