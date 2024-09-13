using DG.Tweening;
using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoardSettingPannel : MonoBehaviour, IUIInputEvent
{

    [SerializeField] private Key[] Keys;

    [SerializeField] private int CurrentIndex;

    [SerializeField] private CanvasGroup CanvasGroup;

    [SerializeField] private OptionPannel optionPannel;

    [SerializeField] private RectTransform Container;

    [SerializeField] private SettingPage settingPage;

    [SerializeField] private float CenterY;

    [SerializeField] private float Speed;

    [Serializable] 
    private class SettingPage
    {

        public CanvasGroup canvasGroup;

        public Text KeyName;

    }

    [Serializable]
    private struct Key
    {

        public Image Icon;

        public Text Title;

        public RectTransform gameObject;

        public void SetMoveOn(bool isMoveOn, bool isUp = true)
        {

            if (isMoveOn)
            {

                Title.material = ResourcesManager.TextMat.Shinning;

                if (isUp)
                {

                    gameObject.transform.DOPunchPosition(Vector3.up * 10, 0.2f, 3, 0.5f, true);

                }
                else
                {

                    gameObject.transform.DOPunchPosition(Vector3.down * 10, 0.2f, 3, 0.5f, true);

                }

            }
            else
            {

                Title.material = ResourcesManager.TextMat.Normal;

            }

        }

        public void SetIcon(Sprite sprite)
        {

            Icon.sprite = sprite;

            Icon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Icon.sprite.bounds.size.x);
            Icon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Icon.sprite.bounds.size.y);

        }

    }

    public void Open()
    {

        UIInputListener.eventInstance = null;

        Init();

        ForceToNewKey(0);

        StartCoroutine(UpdateListPos());

        CanvasGroup.DOFade(1, 0.1f).OnComplete(() => UIInputListener.eventInstance = this);

    }

    public void Close()
    {

        StopAllCoroutines();

        CanvasGroup.DOFade(0, 0.1f).OnComplete(() => UIInputListener.eventInstance = optionPannel);

        ProcessButton.AllRefresh();

    }

    public void Init()
    {

        Keys = new Key[(int)Metric.Settings.KeyBoard.KeyKind.Menu_Log + 1];

        for (int i = 0; i < Keys.Length; ++i)
        {

            Keys[i].gameObject = transform.Find($"Content/{((Metric.Settings.KeyBoard.KeyKind)i).ToString().Replace('_', '/')}").GetComponent<RectTransform>();

            Keys[i].Icon = Keys[i].gameObject.GetComponentInChildren<Image>();

            Keys[i].Title = Keys[i].gameObject.GetComponentInChildren<Text>();

            Keys[i].SetIcon(ResourcesManager.Button.GetButtonSprite(Metric.Settings.KeyBoard.mapping[i]));

        }

    }

    public void ForceToNewKey(int newKey)
    {

        for(int i = 0; i < Keys.Length; ++i)
        {

            Keys[i].SetMoveOn(false);

        }

        CurrentIndex = newKey;

        Keys[CurrentIndex].SetMoveOn(true, false);

    }

    public void MoveOnNewKey(int newKey, bool isUp)
    {

        Keys[CurrentIndex].SetMoveOn(false);

        CurrentIndex = newKey;

        Keys[CurrentIndex].SetMoveOn(true, isUp);

    }

    private IEnumerator UpdateListPos()
    {

        var wait = new WaitForFixedUpdate();

        Vector3 TargetPos = new Vector3(Container.position.x, 0, Container.position.z);

        while (true)
        {

            yield return wait;

            float CenterPos = Keys[CurrentIndex].gameObject.position.y - Container.position.y;

            TargetPos.y = CenterY - CenterPos;

            Container.position = Vector3.Lerp(Container.position, TargetPos, Time.fixedDeltaTime * Speed);

        }

    }

    private IEnumerator OpenSettingPage(Metric.Settings.KeyBoard.KeyKind kind, string Title)
    {

        settingPage.canvasGroup.DOFade(1, 0.1f);

        Container.GetComponent<CanvasGroup>().DOFade(0.2f, 0.1f);

        settingPage.KeyName.text = Title;

        KeyCode newKey = Metric.Settings.KeyBoard.mapping[(int)kind];

        yield return new WaitForSeconds(0.2f);

        yield return new WaitUntil(
            ()=> 
            { 
                newKey = ResourcesManager.Button.GetAllowedKeyUp();
                return newKey != KeyCode.None; 
            }
        );

        Metric.Settings.KeyBoard.mapping[(int)kind] = newKey;

        Keys[(int)kind].SetIcon(ResourcesManager.Button.GetButtonSprite(Metric.Settings.KeyBoard.mapping[(int)kind]));

        Container.GetComponent<CanvasGroup>().DOFade(1, 0.1f);

        settingPage.canvasGroup.DOFade(0, 0.1f).OnComplete(() => UIInputListener.eventInstance = this);

    }

    public void OnConfirmClick()
    {

        UIInputListener.eventInstance = null;

        StartCoroutine(OpenSettingPage((Metric.Settings.KeyBoard.KeyKind)CurrentIndex, Keys[CurrentIndex].Title.text));

    }

    public void OnCancelClick()
    {

        Close();

    }

    public void OnLeftKeepDown()
    {


    }

    public void OnRightKeepDown()
    {

    }

    public void OnUpClick()
    {

        if (CurrentIndex != 0)
        {

            MoveOnNewKey(CurrentIndex - 1, true);

        }

    }

    public void OnDownClick()
    {

        if (CurrentIndex != Keys.Length - 1)
        {

            MoveOnNewKey(CurrentIndex + 1, false);

        }

    }

    //public void OnValidate()
    //{

    //    Init();

    //}

}
