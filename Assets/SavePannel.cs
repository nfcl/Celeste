using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavePannel : MonoBehaviour, IUIInputEvent
{

    [SerializeField] private float[] OriginY;

    [SerializeField] private SavePage[] pages;

    [SerializeField] private int CurrentIndex;

    [SerializeField] private SaveMenu saveMenu;

    [SerializeField] private CanvasGroup canvasGroup;

    public void Open()
    {

        canvasGroup.alpha = 1;

        CurrentIndex = -1;

        for (int i = 0; i < Metric.Archive.saveFiles.Length; ++i)
        {

            pages[i].Init(i);

            pages[i].transform.localPosition = new Vector3(1920, pages[i].transform.localPosition.y, pages[i].transform.localPosition.z);

        }

        StartCoroutine(open());

    }

    private IEnumerator open()
    {

        float DelayTime = 0.05f;

        float AnimeTime = 0.5f;

        WaitForSeconds wait = new WaitForSeconds(DelayTime);

        for(int i = 0; i < pages.Length; ++i)
        {

            pages[i].transform.localPosition = new Vector3(1920, OriginY[i], 0);

            pages[i].transform.DOLocalMoveX(0, AnimeTime);

            yield return wait;

        }

        yield return new WaitForSeconds(DelayTime * (pages.Length - 1) + AnimeTime - DelayTime);

        UIInputListener.eventInstance = this;

        CurrentIndex = 0;

        RefreshMoveOn();

    }

    public void Close(bool isLeft)
    {

        StartCoroutine(close(isLeft));

    }

    private IEnumerator close(bool isLeft)
    {

        float DelayTime = 0.05f;

        float AnimeTime = 0.5f;

        UIInputListener.eventInstance = null;

        WaitForSeconds wait = new WaitForSeconds(DelayTime);

        saveMenu.saveMenu.DOLocalMoveX((isLeft ? -1 : 1) * 1920, AnimeTime);

        for (int i = 0; i < pages.Length; ++i)
        {

            pages[i].transform.DOLocalMoveX((isLeft ? -1 : 1) * 1920, AnimeTime);

            yield return wait;

        }

        yield return new WaitForSeconds(DelayTime * (pages.Length - 1) + AnimeTime - DelayTime);

        canvasGroup.alpha = 0;

        CurrentIndex = -1;

        RefreshMoveOn();

    }

    public void RefreshMoveOn()
    {

        for (int i = 0; i < pages.Length; ++i)
        {

            if (i != CurrentIndex)
            {

                pages[i].IsMoveOn = false;

            }

        }

        if (CurrentIndex >= 0 && CurrentIndex < pages.Length)
        {

            pages[CurrentIndex].IsMoveOn = true;

        }

    }

    public void OnCancelClick()
    {

        StartSceneManager.instance.Save2Main();

    }

    public void OnConfirmClick()
    {

        for (int i = 0; i < CurrentIndex; ++i)
        {

            pages[i].transform.DOBlendableLocalMoveBy(new Vector3(0, 1080, 0), 0.2f);

        }

        for (int i = CurrentIndex + 1; i < pages.Length; ++i)
        {

            pages[i].transform.DOBlendableLocalMoveBy(new Vector3(0, -1080, 0), 0.2f);

        }

        pages[CurrentIndex].transform.DOLocalMove(new Vector3(0, 102, 0), 0.2f);

        pages[CurrentIndex].IsMoveOn = pages[CurrentIndex].SaveFile != null;

        UIInputListener.eventInstance = null;

        saveMenu.Open(
            CurrentIndex, 
            delegate 
            {

                saveMenu.saveMenu.DOLocalMoveY(OriginY[CurrentIndex], 0.1f); 
                saveMenu.saveMenu.GetComponent<CanvasGroup>().DOFade(0, 0.1f);

                for (int i = 0; i < CurrentIndex; ++i)
                {

                    pages[i].transform.DOBlendableLocalMoveBy(new Vector3(0, -1080, 0), 0.2f);

                }

                for (int i = CurrentIndex + 1; i < pages.Length; ++i)
                {

                    pages[i].transform.DOBlendableLocalMoveBy(new Vector3(0, 1080, 0), 0.2f);

                }

                pages[CurrentIndex].transform.DOLocalMoveY(OriginY[CurrentIndex], 0.2f).OnComplete(() => UIInputListener.eventInstance = this);

                pages[CurrentIndex].Init(CurrentIndex);

            }
        );

        saveMenu.saveMenu.localPosition = pages[CurrentIndex].transform.localPosition;

        saveMenu.saveMenu.DOLocalMoveY(-132, 0.2f);

        saveMenu.saveMenu.GetComponent<CanvasGroup>().DOFade(1, 0.2f);

    }

    public void OnLeftKeepDown()
    {

    }

    public void OnRightKeepDown()
    {

    }

    public void OnUpClick()
    {

        if (CurrentIndex == 0)
        {

            return;

        }

        MainSceneMusicManager.instance.savePannel.OnRoll();

        CurrentIndex -= 1;

        RefreshMoveOn();

    }

    public void OnDownClick()
    {

        if (CurrentIndex == pages.Length - 1)
        {

            return;

        }

        MainSceneMusicManager.instance.savePannel.OnRoll();

        CurrentIndex += 1;

        RefreshMoveOn();

    }

    [Serializable]
    private class SaveMenu : IUIInputEvent
    {

        public Transform saveMenu;

        private Action returnCallBack;

        [SerializeField] private Transform[] Options;

        [SerializeField] private int CurrentIndex;

        [SerializeField] private int ControlSaveIndex;

        public void Open(int Index,Action OnReturn)
        {

            returnCallBack = OnReturn;

            ControlSaveIndex = Index;

            CurrentIndex = 0;

            if (Metric.Archive.saveFiles[ControlSaveIndex] == null)
            {

                Options = new Transform[1]
                {
                    saveMenu.Find("Options/Start")
                };

                Options[0].Find("Title").GetComponent<Text>().text = "¿ªÊ¼";

                saveMenu.Find("Options/Delete").GetComponent<CanvasGroup>().alpha = 0;

            }
            else
            {

                Options = new Transform[2]
                {
                    saveMenu.Find("Options/Start"),
                    saveMenu.Find("Options/Delete"),
                };

                Options[0].Find("Title").GetComponent<Text>().text = "¼ÌÐø";

                Options[1].GetComponent<CanvasGroup>().alpha = 1;

            }

            Refresh();

            UIInputListener.eventInstance = this;

        }

        private void Refresh()
        {

            for(int i = 0; i < Options.Length; ++i)
            {

                if(i == CurrentIndex)
                {

                    Options[i].Find("Title").GetComponent<Text>().material = ResourcesManager.TextMat.Shinning;

                }
                else
                {

                    Options[i].Find("Title").GetComponent<Text>().material = ResourcesManager.TextMat.Normal;

                }

            }

        }

        public void OnCancelClick()
        {

            UIInputListener.eventInstance = null;

            returnCallBack();

        }

        public void OnConfirmClick()
        {

            if(CurrentIndex == 0)
            {

                if (Metric.Archive.saveFiles[ControlSaveIndex] == null)
                {

                    Metric.Archive.saveFiles[ControlSaveIndex] = new Metric.Archive.SaveFile
                    {

                        SaveName = "MadLine",
                        mapRecords = new Metric.Archive.SaveFile.MapRecord[]
                        {

                             new Metric.Archive.SaveFile.MapRecord
                             {
                                 MapIndex = 1,
                                 sideRecords = new Metric.Archive.SaveFile.MapRecord.SideRecord[]
                                 {
                                     new Metric.Archive.SaveFile.MapRecord.SideRecord
                                     {
                                         SideIndex = 0,
                                         DeathNum = 0,
                                         TimeSpent = new TimeSpan(0),
                                         MinimumTime = new TimeSpan(-1),
                                         checkPointRecords = new List<Metric.Archive.SaveFile.MapRecord.SideRecord.CheckPointRecord>
                                         {
                                             new Metric.Archive.SaveFile.MapRecord.SideRecord.CheckPointRecord
                                             {
                                                 CheckPointIndex = 0,
                                                 Strawberries = new List<int>{ }
                                             }
                                         }
                                     }
                                 }
                             }

                        }

                    };

                }

                Metric.SceneOnloadVarible.GameScene.CurrentSave = Metric.Archive.saveFiles[ControlSaveIndex];

                MainSceneMusicManager.instance.savePannel.OnEnter();

                StartSceneManager.instance.Save2Mission();

            }
            else
            {

                Metric.Archive.saveFiles[ControlSaveIndex] = null;

                UIInputListener.eventInstance = null;

                returnCallBack();

            }

        }

        public void OnLeftKeepDown()
        {

        }

        public void OnRightKeepDown()
        {

        }

        public void OnDownClick()
        {

            if (CurrentIndex != Options.Length - 1)
            {

                CurrentIndex += 1;

            }

            Refresh();

        }

        public void OnUpClick()
        {

            if (CurrentIndex != 0)
            {

                CurrentIndex -= 1;

            }

            Refresh();

        }
    }

}
