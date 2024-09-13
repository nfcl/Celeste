using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointPage : MonoBehaviour, IUIInputEvent
{

    [SerializeField] private Text Title;

    [SerializeField] private Transform ChapterLabelContainer;

    [SerializeField] private ChapterLabel[] ChapterLabels;

    [SerializeField] private ChapterLabel ChapterLabelPrefab;

    [SerializeField] private Metric.MapsInfo.Side CurrentSide;

    [SerializeField] private int CurrentCheckPointIndex;

    [SerializeField] private MissionPage missionPage;

    [SerializeField] private Sprite[] CheckPointsIcon;

    [SerializeField] private Transform CheckPointPicContainer;

    [SerializeField] private CheckPointPic CheckPointPicPrefab;

    [SerializeField] private CheckPointPic[] CheckPointPics;

    public void Open(Metric.MapsInfo.Side newSide)
    {

        CurrentSide = newSide;

        Metric.SceneOnloadVarible.GameScene.CurrentSide = newSide;

        CurrentCheckPointIndex = 0;

        transform.DOLocalMoveX(0, 0.5f).OnComplete(() => UIInputListener.eventInstance = this);

        GenerateSideLabels();

        GennerateCheckPointPic();

        ChapterLabelMove(0.5f, true);

        RefreshMoveOnSide(0);

    }

    public void QuickOpen(Metric.MapsInfo.Side newSide)
    {

        CurrentSide = newSide;

        transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);

        CurrentCheckPointIndex = -1;

        for (int i = 0; i < CurrentSide.CheckPoints.Length; ++i)
        {

            if (CurrentSide.CheckPoints[i] == Metric.SceneOnloadVarible.GameScene.CurrentCheckPoint)
            {

                CurrentCheckPointIndex = i;

                break;

            }

        }

        if(CurrentCheckPointIndex == -1)
        {

            throw new System.Exception("无法找到对应的存档点");

        }

        GenerateSideLabels();

        GennerateCheckPointPic();

        ForceToRefreshPics();

        ForceToRefreshLabel(true);

        UIInputListener.eventInstance = this;

    }

    private void ChapterLabelMove(float animeTime, bool isLeft)
    {

        float[] DestX = GetChapterLabelPostion(ChapterLabels.Length);

        for (int i = 0; i < ChapterLabels.Length; ++i)
        {

            ChapterLabels[i].transform.DOLocalMoveX(DestX[i] + (isLeft ? 0 : 1920), animeTime);

        }

    }

    private void ForceToRefreshLabel(bool isLeft)
    {

        float[] DestX = GetChapterLabelPostion(ChapterLabels.Length);

        for (int i = 0; i < ChapterLabels.Length; ++i)
        {

            ChapterLabels[i].transform.localPosition = new Vector3(
                DestX[i] + (isLeft ? 0 : 1920), 
                ChapterLabels[i].transform.localPosition.y, 
                ChapterLabels[i].transform.localPosition.z
            );

        }

    }

    private float[] GetChapterLabelPostion(int num)
    {

        float[] DestX = new float[num];

        float BigDistance = 200;

        float X = -(num - 1) * BigDistance / 2;

        for (int i = 0; i < DestX.Length; ++i)
        {

            DestX[i] = X;

            X += BigDistance;

        }

        return DestX;

    }

    private void GenerateSideLabels()
    {

        for (int i = 0; i < ChapterLabels.Length; ++i)
        {

            DestroyImmediate(ChapterLabels[i].gameObject);

        }

        float[] Xs = GetChapterLabelPostion(CurrentSide.SideRecord.checkPointRecords.Count);

        ChapterLabels = new ChapterLabel[Xs.Length];

        for (int i = 0; i < ChapterLabels.Length; ++i)
        {

            ChapterLabels[i] = GameObject.Instantiate(ChapterLabelPrefab, ChapterLabelContainer);

            ChapterLabels[i].transform.localPosition = new Vector3(Xs[i] + transform.localPosition.x, -720, 0);

            ChapterLabels[i].Icon.sprite = CheckPointsIcon[CurrentSide.SideRecord.checkPointRecords[i].CheckPointIndex == 0 ? 0 : 1];

            ChapterLabels[i].Icon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ChapterLabels[i].Icon.sprite.rect.width);
            ChapterLabels[i].Icon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ChapterLabels[i].Icon.sprite.rect.height);

            ChapterLabels[i].Icon.transform.localScale = Vector3.one;

            if (i != 0)
            {

                ChapterLabels[i].Init(new Color(0.2509804f, 0.4980392f, 0.686274f), Color.white, i == CurrentCheckPointIndex);

            }
            else
            {

                ChapterLabels[i].Init(new Color(1.0000000f, 0.8117648f, 0.3058824f), Color.white, i == CurrentCheckPointIndex);

            }

        }

    }

    private void GennerateCheckPointPic()
    {

        while (CheckPointPicContainer.childCount > 0)
        {

            DestroyImmediate(CheckPointPicContainer.GetChild(0).gameObject);

        }

        CheckPointPics = new CheckPointPic[CurrentSide.SideRecord.checkPointRecords.Count];

        for (int i = 0; i < CheckPointPics.Length; ++i)
        {

            CheckPointPics[i] = GameObject.Instantiate(CheckPointPicPrefab, CheckPointPicContainer);

            CheckPointPics[i].Init(CurrentSide.CheckPoints[CurrentSide.SideRecord.checkPointRecords[i].CheckPointIndex]);

            CheckPointPics[i].transform.localEulerAngles = new Vector3(0, 0, (Random.Range(0, 2) == 0 ? -1 : 1) * Random.Range(2f, 8f));

            CheckPointPics[i].transform.SetAsFirstSibling();

        }

    }

    public void Close()
    {

        transform.DOLocalMoveX(1920, 0.5f);

        ChapterLabelMove(0.5f, false);

        UIInputListener.eventInstance = null;

    }

    private void RefreshMoveOnSide(int NewSideIndex)
    {

        ChapterLabels[CurrentCheckPointIndex].SetMoveOn(false);

        CurrentCheckPointIndex = NewSideIndex;

        ChapterLabels[CurrentCheckPointIndex].SetMoveOn(true);

        Title.text = CurrentSide.CheckPoints[CurrentCheckPointIndex].CheckPointName;

    }

    public void OnConfirmClick()
    {

        if (CurrentSide.SceneName.Length > 0)
        {

            Metric.SceneOnloadVarible.GameScene.CurrentCheckPoint = CurrentSide.CheckPoints[CurrentSide.SideRecord.checkPointRecords[CurrentCheckPointIndex].CheckPointIndex];

            try
            {

                if (Metric.SceneOnloadVarible.GameScene.CurrentMap.Sides.Contains(Metric.SceneOnloadVarible.GameScene.CurrentSide))
                {

                    if (Metric.SceneOnloadVarible.GameScene.CurrentSide.CheckPoints.Contains(Metric.SceneOnloadVarible.GameScene.CurrentCheckPoint))
                    {

                        MainCameraControl.instance.CurrentState = Metric.SceneOnloadVarible.GameScene.CurrentMap.ReturnCameraState;

                        missionPage.transform.DOLocalMoveX(1920, 0.5f);

                        MainSceneMusicManager.instance.EnvironmentFadeChange(false);

                        StartSceneManager.instance.EnterMission(CurrentSide.SceneName);
                    }
                    else
                    {

                        throw new System.Exception("");

                    }

                }
                else
                {

                    throw new System.Exception("");

                }

            }
            catch
            {

                Metric.Debug.LogWarning("当前选择的地图与关卡和存档点无法匹配上");

            }
            
        }

    }

    public void OnCancelClick()
    {

        missionPage.ToSidePage();

        MainSceneMusicManager.instance.UnSelectCheckPoint();

    }

    private float LastToggleTime;

    private readonly float MinToggleChangeTime = 0.2f;

    public void ForceToRefreshPics()
    {

        for(int i = 0; i < CurrentCheckPointIndex; ++i)
        {

            CheckPointPics[i].transform.localPosition = new Vector3(
                1920, 
                CheckPointPics[i].transform.localPosition.y, 
                CheckPointPics[i].transform.localPosition.z
            );

        }

        for(int i = CurrentCheckPointIndex; i < CheckPointPics.Length; ++i)
        {

            CheckPointPics[i].transform.localPosition = new Vector3(
                0, 
                CheckPointPics[i].transform.localPosition.y, 
                CheckPointPics[i].transform.localPosition.z
            );

        }

    }

    public void OnLeftKeepDown()
    {

        if (Time.realtimeSinceStartup - LastToggleTime > MinToggleChangeTime && CurrentCheckPointIndex != 0)
        {

            MainSceneMusicManager.instance.RollCheckPoint(true);

            LastToggleTime = Time.realtimeSinceStartup;

            CheckPointPics[CurrentCheckPointIndex - 1].transform.DOLocalMoveX(0, 0.5f);

            RefreshMoveOnSide(CurrentCheckPointIndex - 1);

        }

    }

    public void OnRightKeepDown()
    {

        if (Time.realtimeSinceStartup - LastToggleTime > MinToggleChangeTime && CurrentCheckPointIndex != CurrentSide.SideRecord.checkPointRecords.Count - 1)
        {

            MainSceneMusicManager.instance.RollCheckPoint(false);

            LastToggleTime = Time.realtimeSinceStartup;

            CheckPointPics[CurrentCheckPointIndex].transform.DOLocalMoveX(1920, 0.5f);

            RefreshMoveOnSide(CurrentCheckPointIndex + 1);

        }

    }

    public void OnUpClick()
    {

    }

    public void OnDownClick()
    {

    }

}
