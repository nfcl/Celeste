using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartPannel : MonoBehaviour, IUIInputEvent
{

    [SerializeField] private Image ConfirmButton;

    [SerializeField] private Animation anime;

    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {

        ConfirmButton.sprite = ResourcesManager.Button.GetButtonSprite(Metric.MenuControl.ConfirmButton);

    }

    public void Init(bool isCompleteInit)
    {

        if (isCompleteInit)
        {

            anime.Play("ShowStartPannel");

            StartCoroutine(open());

        }
        else
        {

            anime["HideStartPannel"].time = anime["HideStartPannel"].length;

            anime.Play("HideStartPannel");

        }

    }

    public void Open()
    {

        anime.Play("ShowStartPannel");

        StartSceneManager.instance.SetBackGroundVisible(true);

        StartCoroutine(open());

    }

    private IEnumerator open()
    {

        StartSceneManager.instance.MainCameraControl.CurrentState = Metric.Camera.CameraTargetState.Main_0;

        yield return new WaitForSecondsRealtime(anime["ShowStartPannel"].length);

        UIInputListener.eventInstance = this;

    }

    public void Close()
    {

        anime.Play("HideStartPannel");

        StartSceneManager.instance.SetBackGroundVisible(false);

        UIInputListener.eventInstance = null;

    }

    public void OnConfirmClick()
    {

        GetComponent<AudioSource>().Play();

        StartSceneManager.instance.Start2Main();

    }

    public void OnCancelClick()
    {



    }

    public void OnLeftKeepDown()
    {



    }

    public void OnRightKeepDown()
    {



    }

    public void OnUpClick()
    {



    }

    public void OnDownClick()
    {



    }
}
