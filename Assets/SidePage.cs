using DG.Tweening;
using Map;
using UnityEngine;
using UnityEngine.UI;

public class SidePage : MonoBehaviour, IUIInputEvent
{

    [SerializeField] private Text Title;

    [SerializeField] private Text DeathNum;

    [SerializeField] private Text CurrentStrawberryNum;

    [SerializeField] private Text MaxStrawberryNum;

    [SerializeField] private Transform ChapterLabelContainer;

    [SerializeField] private ChapterLabel[] ChapterLabels;

    [SerializeField] private MissionPage missionPage;

    [SerializeField] private Mission CurrentMission;

    [SerializeField] private int CurrentSideIndex;

    [SerializeField] private ChapterLabel ChapterLabelPrefab;

    [SerializeField] private Sprite[] SideIcons;

    public void Open(Mission newMission)
    {

        CurrentSideIndex = 0;

        CurrentMission = newMission;

        Metric.SceneOnloadVarible.GameScene.CurrentMap = newMission.map;

        transform.DOLocalMoveX(0, 0.5f).OnComplete(() => UIInputListener.eventInstance = this);

        GenerateSideLabels();

        ChapterLabelMove(0.5f, true);

        RefreshMoveOnSide(0);

    }

    public void QuickOpen(Mission newMission)
    {

        CurrentMission = newMission;

        CurrentSideIndex = -1;

        for (int i = 0; i < newMission.map.Sides.Length; ++i)
        {

            if (newMission.map.Sides[i] == Metric.SceneOnloadVarible.GameScene.CurrentSide)
            {

                CurrentSideIndex = i;

                break;

            }

        }

        if(CurrentSideIndex == -1)
        {

            throw new System.Exception("当前选择的关卡不在选择的地图中");

        }

        GenerateSideLabels();

        ChapterLabelMove(0.01f, false);

        ForceToRefreshLabel(false);

        transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);

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

        for(int i = 0; i < ChapterLabels.Length; ++i)
        {

            DestroyImmediate(ChapterLabels[i].gameObject);

        }

        float[] Xs = GetChapterLabelPostion(CurrentMission.map.MapRecord.sideRecords.Length);

        ChapterLabels = new ChapterLabel[Xs.Length];

        if (CurrentSideIndex < 0)
        {

            CurrentSideIndex = 0;

        }
        else if (CurrentSideIndex >= ChapterLabels.Length)
        {

            CurrentSideIndex = ChapterLabels.Length - 1;

        }

        for (int i = 0; i < ChapterLabels.Length; ++i)
        {

            ChapterLabels[i] = GameObject.Instantiate(ChapterLabelPrefab, ChapterLabelContainer);

            ChapterLabels[i].transform.localPosition = new Vector3(Xs[i] + transform.localPosition.x, -720, 0);

            switch (CurrentMission.map.Sides[CurrentMission.map.MapRecord.sideRecords[i].SideIndex].SideType)
            {

                case Metric.MapsInfo.SideType.Plot:
                case Metric.MapsInfo.SideType.A:
                    {
                        ChapterLabels[i].Icon.sprite = SideIcons[0];
                        break;
                    }
                case Metric.MapsInfo.SideType.B:
                    {
                        ChapterLabels[i].Icon.sprite = SideIcons[1];
                        ChapterLabels[i].Icon.transform.localScale = Vector3.one;
                        break;
                    }
                case Metric.MapsInfo.SideType.C:
                    {
                        ChapterLabels[i].Icon.sprite = SideIcons[2];
                        ChapterLabels[i].Icon.transform.localScale = Vector3.one;
                        break;
                    }

            }

            ChapterLabels[i].Icon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, ChapterLabels[i].Icon.sprite.rect.width);
            ChapterLabels[i].Icon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, ChapterLabels[i].Icon.sprite.rect.height);

            if (i == CurrentSideIndex)
            {

                ChapterLabels[i].Init(new Color(0.2509804f, 0.4980392f, 0.686274f), Color.white, true);

            }
            else
            {

                ChapterLabels[i].Init(new Color(0.2509804f, 0.4980392f, 0.686274f), Color.white, false);

            }

        }

    }

    private void RefreshMoveOnSide(int NewSideIndex)
    {

        ChapterLabels[CurrentSideIndex].SetMoveOn(false);

        CurrentSideIndex = NewSideIndex;

        ChapterLabels[CurrentSideIndex].SetMoveOn(true);

        DeathNum.text = CurrentMission.map.MapRecord.sideRecords[CurrentSideIndex].DeathNum.ToString();

        CurrentStrawberryNum.text = CurrentMission.map.Sides[CurrentSideIndex].SideRecord.GetStrawberryNum().ToString();

        MaxStrawberryNum.text = CurrentMission.map.Sides[CurrentSideIndex].GetStrawberryNum().ToString();

        switch (CurrentMission.map.Sides[CurrentSideIndex].SideType)
        {

            case Metric.MapsInfo.SideType.Plot:
            case Metric.MapsInfo.SideType.A:
                {
                    Title.text = "攀登";
                    break;
                }
            case Metric.MapsInfo.SideType.B:
                {
                    Title.text = "B面";
                    break;
                }
            case Metric.MapsInfo.SideType.C:
                {
                    Title.text = "C面";
                    break;
                }

        }

    }

    public void Close()
    {

        transform.DOLocalMoveX(1920, 0.5f);

        ChapterLabelMove(0.5f, false);

    }

    public Metric.MapsInfo.Side GetCurrentSide()
    {

        return CurrentMission.map.Sides[CurrentSideIndex];

    }

    public void OnConfirmClick()
    {

        if (
               CurrentMission.map.Sides[CurrentSideIndex].CheckPoints.Length == 0 
            || CurrentMission.map.Sides[CurrentSideIndex].SideType == Metric.MapsInfo.SideType.Plot
            || CurrentMission.map.Sides[CurrentSideIndex].SideType == Metric.MapsInfo.SideType.C
        )
        {

            return;

        }

        MainSceneMusicManager.instance.SelectSide();

        missionPage.ToCheckPointPage();

    }

    public void OnCancelClick()
    {

        missionPage.Close();

    }

    private float LastToggleTime;

    private float MinToggleChangeTime = 0.2f;

    public void OnLeftKeepDown()
    {

        if (Time.realtimeSinceStartup - LastToggleTime > MinToggleChangeTime && CurrentSideIndex != 0)
        {

            MainSceneMusicManager.instance.RollSide(true);

            LastToggleTime = Time.realtimeSinceStartup;

            RefreshMoveOnSide(CurrentSideIndex - 1);

        }

    }

    public void OnRightKeepDown()
    {

        if (
               Time.realtimeSinceStartup - LastToggleTime > MinToggleChangeTime 
            && CurrentSideIndex != ChapterLabels.Length - 1
        )
        {

            MainSceneMusicManager.instance.RollSide(false);

            LastToggleTime = Time.realtimeSinceStartup;

            RefreshMoveOnSide(CurrentSideIndex + 1);

        }

    }

    public void OnUpClick()
    {



    }

    public void OnDownClick()
    {



    }

}
