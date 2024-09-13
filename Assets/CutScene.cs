using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class CutScene : MonoBehaviour
{

    [SerializeField] private Image CutSceneImage;

    [SerializeField] private Coroutine CurrentCoroutine;

    [SerializeField] private bool IsNeedCoverWhenLoadScene;

    [SerializeField] private Color BlackColor = new Color(0, 0, 0, 1);

    [SerializeField] private Color BackGroundColor = new Color(0.2f, 0.2f, 0.2f, 1);

    private void Start()
    {

        CutSceneImage.material.SetFloat("_LerpValue", IsNeedCoverWhenLoadScene ? 1 : 0);

        CurrentCoroutine = null;

    }

    public enum Kind
    {

        Fade,
        Lvl1

    }

    public void ForceTo(bool isCover, bool isBackGround)
    {

        CutSceneImage.material.SetFloat("_LerpValue", isCover ? 1 : 0);

        if (isBackGround)
        {

            CutSceneImage.material.SetColor("_Color", new Color(0.2f, 0.2f, 0.2f, 1));

        }
        else
        {

            CutSceneImage.material.SetColor("_Color", new Color(0, 0, 0, 1));

        }

    }

    public void ToggleChange(bool isCover,Kind kind = Kind.Lvl1, float speed = 2, Action OnCompleteEvent = null, bool isBackGround = false)
    {

        if (CurrentCoroutine != null)
        {

            Metric.Debug.Log("当前正在进行切换中");

            StopAllCoroutines();

        }

        if(kind == Kind.Fade)
        {

            CutSceneImage.material = ResourcesManager.ImageMat.CutScene.Fade;

        }
        else
        {

            CutSceneImage.material = ResourcesManager.ImageMat.CutScene.Lvl1;

        }

        if (isBackGround)
        {

            CutSceneImage.material.SetColor("_Color", new Color(0.2f, 0.2f, 0.2f, 1));

        }
        else
        {

            CutSceneImage.material.SetColor("_Color", new Color(0, 0, 0, 1));

        }

        CurrentCoroutine = StartCoroutine(Coroutine_ToggleChange(isCover, speed, isBackGround, OnCompleteEvent));

    }

    private IEnumerator Coroutine_ToggleChange(bool isCover, float speed, bool isBackGround, Action OnCompleteEvent)
    {

        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(Time.fixedDeltaTime);

        bool animeContinue = true;

        float LerpValue;

        CutSceneImage.material.SetColor("_Color", BlackColor);

        if (isCover)
        {

            transform.localScale = Vector3.one;

            LerpValue = 0;

            while (animeContinue)
            {

                yield return wait;

                LerpValue += Time.fixedDeltaTime * speed;

                if (LerpValue > 1)
                {

                    animeContinue = false;

                    LerpValue = 1;

                }

                CutSceneImage.material.SetFloat("_LerpValue", LerpValue);

                if (isBackGround)
                {

                    CutSceneImage.material.SetColor("_Color", Color.Lerp(BlackColor, BackGroundColor, LerpValue));

                }

            }

        }
        else
        {

            transform.localScale = new Vector3(-1, -1, 1);

            LerpValue = 1;

            while (animeContinue)
            {

                yield return wait;

                LerpValue -= Time.fixedDeltaTime * speed;

                if (LerpValue < 0)
                {

                    animeContinue = false;

                    LerpValue = 0;

                }

                CutSceneImage.material.SetFloat("_LerpValue", LerpValue);

                if (isBackGround)
                {

                    CutSceneImage.material.SetColor("_Color", Color.Lerp(BackGroundColor, BlackColor, LerpValue));

                }

            }

        }

        CurrentCoroutine = null;

        OnCompleteEvent?.Invoke();

    }

}
