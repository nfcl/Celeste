using UnityEngine;

public class MissionPannel : MonoBehaviour, IUIInputEvent
{

    [SerializeField] private MissionBar missionBar;

    [SerializeField] private MissionPage missionPage;

    [SerializeField] private CanvasGroup canvasGroup;

    public void QuickOpen()
    {

        canvasGroup.alpha = 1;

        missionBar.QuickOpen();

        missionPage.QuickOpen(missionBar.GetCurrentMission());

    }

    public void Open()
    {

        canvasGroup.alpha = 1;

        missionBar.GenerateIcons();

        missionBar.SetVisible(true);

        UIInputListener.eventInstance = this;

    }

    public void Close()
    {

        missionBar.SetVisible(false);

        UIInputListener.eventInstance = null;

    }

    public void OnPageClose()
    {

        missionBar.SetVisible(true, true);

    }

    public void OnConfirmClick()
    {

        UIInputListener.eventInstance = null;

        MainSceneMusicManager.instance.SelectIcon();

        missionBar.SetVisible(false, true);

        missionPage.Open(missionBar.GetCurrentMission());

    }

    public void OnCancelClick()
    {

        MainSceneMusicManager.instance.PlayMenuEffect(MainSceneMusicManager.MenuEffectKind.Back);

        StartSceneManager.instance.Mission2Main();

    }

    public void OnLeftKeepDown()
    {

        missionBar.SelectPrev();

    }

    public void OnRightKeepDown()
    {

        missionBar.SelectNext();

    }

    public void OnUpClick()
    {

    }

    public void OnDownClick()
    {

    }
}
