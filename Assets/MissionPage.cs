using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MissionPage : MonoBehaviour
{

    [SerializeField] private Image BackGround_Top;
    [SerializeField] private Image BackGround_Bottom;
    [SerializeField] private Image Bar;
    [SerializeField] private Image Bar_Cross;
    [SerializeField] private Text MissionIndex;
    [SerializeField] private Text MissionTitle;

    [SerializeField] private Mission CurrentMission;

    [SerializeField] private MissionPannel missionPannel;

    [SerializeField] private SidePage sidePage;

    [SerializeField] private CheckPointPage checkPointPage;

    private void SetBackGround_BottomChange(float Bottom)
    {

        BackGround_Bottom.transform.DOLocalMoveY(-Bottom, 0.5f);

        BackGround_Bottom.DOFillAmount(Bottom / BackGround_Bottom.GetComponent<RectTransform>().rect.height, 0.5f);

    }

    private void RefreshMission()
    {

        if (CurrentMission == null)
        {

            return;

        }

        Bar.color = CurrentMission.map.MainColor;
        Bar_Cross.color = CurrentMission.map.SecondColor;

        MissionIndex.text = CurrentMission.map.MapIndex;
        
        if(MissionIndex.text.Length == 0)
        {

            MissionTitle.transform.localPosition = new Vector3(
                MissionTitle.transform.localPosition.x,
                0,
                MissionTitle.transform.localPosition.z
            );

        }
        else
        {

            MissionIndex.color = CurrentMission.map.SecondColor;

            MissionTitle.transform.localPosition = new Vector3(
                MissionTitle.transform.localPosition.x,
                -19,
                MissionTitle.transform.localPosition.z
            );

        }
        
        MissionTitle.text = CurrentMission.map.MapName;

    }

    public void Open(Mission newMission)
    {

        CurrentMission = newMission;

        RefreshMission();

        const float animeTime = 0.8f;

        transform.DOLocalMoveX(0, animeTime, false);

        MainCameraControl.instance.CurrentState = newMission.map.SelectCameraState;

        IconAnime(animeTime, true);

        ToSidePage();

    }

    public void Close()
    {

        StartCoroutine(close());

    }

    private IEnumerator close()
    {

        const float animeTime = 0.8f;

        transform.DOLocalMoveX(1920, animeTime, false);

        IconAnime(animeTime, false);

        UIInputListener.eventInstance = null;

        MainCameraControl.instance.CurrentState = CurrentMission.map.MoveOnCameraState;

        missionPannel.OnPageClose();

        yield return new WaitForSecondsRealtime(animeTime);

        UIInputListener.eventInstance = missionPannel;

    }

    public void QuickOpen(Mission newMission)
    {

        CurrentMission = newMission;

        MainCameraControl.instance.ForceToNewState(CurrentMission.map.ReturnCameraState);

        MainCameraControl.instance.CurrentState = CurrentMission.map.SelectCameraState;

        RefreshMission();

        sidePage.QuickOpen(CurrentMission);

        checkPointPage.QuickOpen(sidePage.GetCurrentSide());

        transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);

        CurrentMission.transform.localPosition = new Vector3(800, 356, 0);

        CurrentMission.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

    }

    public void ToSidePage()
    {

        sidePage.Open(CurrentMission);

        checkPointPage.Close();

    }

    public void ToCheckPointPage()
    {
        
        UIInputListener.eventInstance = null;

        sidePage.Close();

        checkPointPage.Open(sidePage.GetCurrentSide());

    }

    private void IconAnime(float animeTime,bool isShow)
    {

        CurrentMission.transform.SetAsLastSibling();

        if (isShow)
        {

            MainSceneMusicManager.instance.SelectIcon();

            CurrentMission.transform.DOLocalMove(new Vector3(800,356,0), animeTime);

            CurrentMission.transform.DOLocalRotate(new Vector3(0, 90, 0), animeTime / 4, RotateMode.FastBeyond360).OnComplete(
                () =>
                {
                    CurrentMission.GetComponent<Image>().sprite = CurrentMission.map.IconBack;
                    CurrentMission.transform.DOLocalRotate(new Vector3(0, 270, 0), animeTime / 2, RotateMode.FastBeyond360).OnComplete(
                        () =>
                        {
                            CurrentMission.GetComponent<Image>().sprite = CurrentMission.map.IconFace;
                            CurrentMission.transform.DOLocalRotate(new Vector3(0, 360, 0), animeTime / 4, RotateMode.FastBeyond360);
                        }
                    );
                }
            );

            CurrentMission.transform.DOScale(2, animeTime / 2).OnComplete(
                () =>
                {
                    CurrentMission.transform.DOScale(0.8f, animeTime / 2);
                }
            );

        }
        else
        {

            MainSceneMusicManager.instance.UnSelectIcon();

            CurrentMission.transform.DOLocalMove(new Vector3(0, 387.5f, 0), animeTime);

            CurrentMission.transform.DOLocalRotate(new Vector3(0, 90, 0), animeTime / 4, RotateMode.FastBeyond360).OnComplete(
                () =>
                {
                    CurrentMission.GetComponent<Image>().sprite = CurrentMission.map.IconBack;
                    CurrentMission.transform.DOLocalRotate(new Vector3(0, 270, 0), animeTime / 2, RotateMode.FastBeyond360).OnComplete(
                        () =>
                        {
                            CurrentMission.GetComponent<Image>().sprite = CurrentMission.map.IconFace;
                            CurrentMission.transform.DOLocalRotate(new Vector3(0, 360, 0), animeTime / 4, RotateMode.FastBeyond360);
                        }
                    );
                }
            );

            CurrentMission.transform.DOScale(2, animeTime / 2).OnComplete(
                () =>
                {
                    CurrentMission.transform.DOScale(0.8f, animeTime / 2);
                }
            );

        }

    }

#if UNITY_EDITOR

    [SerializeField] private float BackGround_BottomValue;

    public void OnValidate()
    {

        BackGround_Bottom.transform.localPosition = new Vector3(BackGround_Bottom.transform.localPosition.x, -BackGround_BottomValue, BackGround_Bottom.transform.localPosition.z);
        BackGround_Bottom.fillAmount = BackGround_BottomValue / BackGround_Bottom.GetComponent<RectTransform>().rect.height;

        //RefreshMission();

    }

#endif

}
