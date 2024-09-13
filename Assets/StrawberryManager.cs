using Map;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrawberryManager : MonoBehaviour
{

    [SerializeField] private Image StrawberryPrefab;

    [SerializeField] private Image DividePrefab;

    [SerializeField] private Transform Container;

    [SerializeField] private float StrawberryPaddingX = 32;

    [SerializeField] private float DividePaddingX = 32;

    [SerializeField] private Sprite StrawBerryBall;

    [SerializeField] private Sprite EmptyStrawBerry;

    public enum State
    {

        None,
        Had,
        Have

    }

    [Serializable]
    public struct CheckPoint
    {

        public Strawberry[] StrawBerriesState;

    }

    [SerializeField] private CheckPoint[] CheckPoints;

    public void Generate()
    {

        while (Container.childCount > 0)
        {

            DestroyImmediate(Container.GetChild(0).gameObject, true);

        }

        float Length = DividePaddingX * (CheckPoints.Length - 1);

        for(int i = 0; i < CheckPoints.Length; ++i)
        {

            Length += CheckPoints[i].StrawBerriesState.Length * StrawberryPaddingX;

        }

        float StartX = -Length / 2;

        Image clone;

        for(int i = 0; i < CheckPoints.Length; ++i)
        {

            for(int j = 0; j < CheckPoints[i].StrawBerriesState.Length; ++j)
            {

                clone = GameObject.Instantiate(StrawberryPrefab, Container);

                switch (CheckPoints[i].StrawBerriesState[j].strawberryState)
                {

                    case State.None:
                        {

                            clone.transform.localScale = new Vector3(0.25f, 0.125f, 1);

                            clone.sprite = EmptyStrawBerry;

                            clone.color = new Color(0.6627451f, 0.6627451f, 0.6627451f);

                            break;
                        }
                    case State.Had:
                        {

                            clone.transform.localScale = new Vector3(0.36f, 0.36f, 0.36f);

                            clone.sprite = StrawBerryBall;

                            clone.color = new Color(0.254902f, 0.5764706f, 1);

                            break;
                        }
                    case State.Have:
                        {

                            clone.transform.localScale = new Vector3(0.36f, 0.36f, 0.36f);

                            clone.sprite = StrawBerryBall;

                            clone.color = new Color(1, 0.1882353f, 0.2509804f);

                            break;
                        }

                }

                StartX += StrawberryPaddingX / 2;

                clone.transform.localPosition = new Vector3(StartX, 0, 0);

                StartX += StrawberryPaddingX / 2;

            }

            if (i != CheckPoints.Length - 1)
            {

                clone = GameObject.Instantiate(DividePrefab, Container);

                StartX += DividePaddingX / 2;

                clone.transform.localPosition = new Vector3(StartX, 0, 0);

                StartX += DividePaddingX / 2;

            }

        }

    }

    public void Init()
    {

        if (Metric.SceneOnloadVarible.GameScene.CurrentSave != null)
        {

            CheckPoints = new CheckPoint[Metric.SceneOnloadVarible.GameScene.CurrentSide.CheckPoints.Length];

            for(int i= 0; i < CheckPoints.Length; ++i)
            {

                CheckPoints[i] = new CheckPoint
                {
                    StrawBerriesState = new Strawberry[Metric.SceneOnloadVarible.GameScene.CurrentSide.CheckPoints[i].StrawBerriesName.Length]
                };

                for (int j = 0; j < CheckPoints[i].StrawBerriesState.Length; ++j)
                {

                    CheckPoints[i].StrawBerriesState[j] = GameObject.Find(Metric.SceneOnloadVarible.GameScene.CurrentSide.CheckPoints[i].StrawBerriesName[j]).GetComponent<Strawberry>();

                    CheckPoints[i].StrawBerriesState[j].strawberryState = State.None;

                }

            }

            foreach(Metric.Archive.SaveFile.MapRecord.SideRecord.CheckPointRecord checkpoint in Metric.SceneOnloadVarible.GameScene.CurrentSide.SideRecord.checkPointRecords)
            {

                foreach (int strawberryIndex in checkpoint.Strawberries)
                {

                    CheckPoints[checkpoint.CheckPointIndex].StrawBerriesState[strawberryIndex].strawberryState = State.Had;

                }

            }

        }
        else
        {

            Metric.Debug.LogWarning("草莓以默认值初始化因为当前以无存档运行");

        }

    }

    public void Save()
    {

        for (int i = 0; i < CheckPoints.Length; ++i)
        {

            if(Metric.SceneOnloadVarible.GameScene.CurrentSide.CheckPoints[i].CheckPointRecord == null)
            {

                continue;

            }

            Metric.SceneOnloadVarible.GameScene.CurrentSide.CheckPoints[i].CheckPointRecord.Strawberries = new List<int>();

            for(int j = 0; j < CheckPoints[i].StrawBerriesState.Length; ++j)
            {

                if (CheckPoints[i].StrawBerriesState[j].strawberryState != State.None)
                {

                    Metric.SceneOnloadVarible.GameScene.CurrentSide.CheckPoints[i].CheckPointRecord.Strawberries.Add(j);

                }

            }

        }

    }

}