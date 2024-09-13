using System.Collections;
using UnityEngine;

public class OptionPannel : MonoBehaviour, IUIInputEvent
{

    [SerializeField] private Animation anime;
    /// <summary>
    /// 选项列表
    /// </summary>
    [SerializeField] private MainMenuOption[] options;
    /// <summary>
    /// 当前的焦点选项的下标
    /// </summary>
    [SerializeField] private int CurrentOptionIndex;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private CanvasGroup canvasGroup;
    /// <summary>
    /// 打开选项页面
    /// </summary>
    public void Open()
    {

        StartCoroutine(open());

    }
    private IEnumerator open()
    {

        canvasGroup.alpha = 1;

        FocusOnNewOption(0);

        anime.Play("ShowOptionPannel");

        yield return new WaitForSeconds(anime["ShowOptionPannel"].length);

        UIInputListener.eventInstance = this;

    }
    /// <summary>
    /// 关闭选项页面
    /// </summary>
    public void Close()
    {

        UIInputListener.eventInstance = null;

        Metric.Settings.Save();

        StartCoroutine(close());

    }
    private IEnumerator close()
    {

        anime.Play("HideOptionPannel");

        yield return new WaitForSeconds(anime["HideOptionPannel"].length);

        canvasGroup.alpha = 0;

    }
    /// <summary>
    /// 焦点移动到新的选项上
    /// </summary>
    private void FocusOnNewOption(int newIndex)
    {

        options[CurrentOptionIndex].IsMoveOn = false;

        CurrentOptionIndex = newIndex;

        options[CurrentOptionIndex].IsMoveOn = true;

    }

    #region IUIInputEvent

    /// <summary>
    /// 确认
    /// </summary>
    public void OnConfirmClick()
    {

        options[CurrentOptionIndex].OnSelected();

    }
    /// <summary>
    /// 退回
    /// </summary>
    public void OnCancelClick()
    {

        MainSceneMusicManager.instance.PlayMenuEffect(MainSceneMusicManager.MenuEffectKind.Back);

        StartSceneManager.instance.Option2Main();

        StartSceneManager.instance.Back();

    }
    /// <summary>
    /// 向左
    /// </summary>
    public void OnLeftKeepDown()
    {

        options[CurrentOptionIndex].RollDown();

    }
    /// <summary>
    /// 向右
    /// </summary>
    public void OnRightKeepDown()
    {

        options[CurrentOptionIndex].RollUp();

    }
    /// <summary>
    /// 向上
    /// </summary>
    public void OnUpClick()
    {

        if(CurrentOptionIndex != 0)
        {

            MainSceneMusicManager.instance.PlayMenuEffect(MainSceneMusicManager.MenuEffectKind.RollUp);

            FocusOnNewOption(CurrentOptionIndex - 1);

        }

    }
    /// <summary>
    /// 向下
    /// </summary>
    public void OnDownClick()
    {

        if (CurrentOptionIndex != options.Length - 1)
        {

            MainSceneMusicManager.instance.PlayMenuEffect(MainSceneMusicManager.MenuEffectKind.RollDown);

            FocusOnNewOption(CurrentOptionIndex + 1);

        }

    }

    #endregion

}
