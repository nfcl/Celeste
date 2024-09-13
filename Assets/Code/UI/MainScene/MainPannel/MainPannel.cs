using System;
using System.Collections;
using UnityEngine;

public class MainPannel : MonoBehaviour, IUIInputEvent
{

    [SerializeField] private Animation anime;

    [SerializeField] private int CurrentOptionIndex;

    [SerializeField] private MainMenuOption[] options;

    [SerializeField] private CanvasGroup canvasGroup;

    public void Open()
    {

        canvasGroup.alpha = 1;

        StartCoroutine(open());

    }

    private IEnumerator open()
    {

        StartSceneManager.instance.MainCameraControl.CurrentState = Metric.Camera.CameraTargetState.Main_1;

        anime.Play("ShowMainPannel");

        MoveOnNewOption(0);

        yield return new WaitForSeconds(anime["ShowMainPannel"].length);

        UIInputListener.eventInstance = this;

    }

    public void Close()
    {

        UIInputListener.eventInstance = null;

        StartCoroutine(close());

    }

    private IEnumerator close()
    {

        anime.Play("HideMainPannel");

        yield return new WaitForSeconds(anime["HideMainPannel"].length);

        canvasGroup.alpha = 0;

    }

    private void MoveOnNewOption(int newIndex)
    {

        options[CurrentOptionIndex].IsMoveOn = false;

        CurrentOptionIndex = newIndex;

        options[CurrentOptionIndex].IsMoveOn = true;

    }

    public void OnConfirmClick()
    {

        options[CurrentOptionIndex].OnSelected();

        StartSceneManager.instance.Confirm();

    }

    public void OnCancelClick()
    {

        MainSceneMusicManager.instance.PlayMenuEffect(MainSceneMusicManager.MenuEffectKind.Back);

        StartSceneManager.instance.Back();

        StartSceneManager.instance.Main2Start();

    }

    public void OnLeftKeepDown()
    {



    }

    public void OnRightKeepDown()
    {



    }

    public void OnUpClick()
    {

        if (CurrentOptionIndex != 0)
        {

            MainSceneMusicManager.instance.PlayMenuEffect(MainSceneMusicManager.MenuEffectKind.RollUp);

            MoveOnNewOption(CurrentOptionIndex - 1);

        }


    }

    public void OnDownClick()
    {

        if (CurrentOptionIndex != options.Length - 1)
        {

            MainSceneMusicManager.instance.PlayMenuEffect(MainSceneMusicManager.MenuEffectKind.RollDown);

            MoveOnNewOption(CurrentOptionIndex + 1);

        }

    }

}
