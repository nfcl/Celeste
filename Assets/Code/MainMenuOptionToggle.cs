using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class MainMenuOptionToggle : MonoBehaviour
{

    [SerializeField] private Text Content;
    [SerializeField] private Text Left;
    [SerializeField] private Text Right;
    private float CenterOriginX;
    private float LeftOriginX;
    private float RightOriginX;

    [SerializeField] private string[] Contents;
    [SerializeField] private int CurrIndex;

    [SerializeField] private UnityEvent<MainMenuOptionToggle> OnToggleInit;
    [SerializeField] private UnityEvent<string> OnToggleValueChanged;

    private bool isSelected;

    public bool IsMoveOn
    {

        set
        {

            isSelected = value;

            UpdateSelected();

        }

    }

    private void Start()
    {

        LeftOriginX = Left.transform.localPosition.x;

        RightOriginX = Right.transform.localPosition.x;

        CenterOriginX = Content.transform.localPosition.x;

        OnToggleInit?.Invoke(this);

    }

    private IEnumerator Shake(bool isLeft)
    {

        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        float animeTime = 0.2f;

        float Add = 0;

        bool animeContinue = true;

        while (animeContinue)
        {

            yield return wait;

            Add += Time.fixedDeltaTime;

            if (Add > animeTime)
            {

                Add = animeTime;

                animeContinue = false;

            }

            if (isLeft)
            {

                Left.transform.localPosition = new Vector3(
                    LeftOriginX - 5 * Mathf.Sin(2 * Mathf.PI * Mathf.Min(1, Add / animeTime)),
                    Left.transform.localPosition.y,
                    Left.transform.localPosition.z
                );

            }
            else
            {

                Right.transform.localPosition = new Vector3(
                    RightOriginX + 5 * Mathf.Sin(2 * Mathf.PI * Mathf.Min(1, Add / animeTime)),
                    Right.transform.localPosition.y,
                    Right.transform.localPosition.z
                );

            }

            Content.transform.localPosition = new Vector3(
                CenterOriginX + (isLeft ? -1 : 1) * 5 * Mathf.Sin(2 * Mathf.PI * Mathf.Min(1, Add / animeTime)),
                Content.transform.localPosition.y,
                Content.transform.localPosition.z
            );

        }

    }

    public void UpdateContentForce(string unCheckedValue)
    {

        CurrIndex = 0;

        for(int i = 0; i < Contents.Length; ++i)
        {

            if (Contents[i] == unCheckedValue)
            {

                CurrIndex = i;

                break;

            }

        }

        Content.text = Contents[CurrIndex];

    }

    private void UpdateContent(bool isLeft)
    {

        Content.text = Contents[CurrIndex];

        OnToggleValueChanged?.Invoke(Content.text);

        StartCoroutine(Shake(isLeft));

        UpdateSelected();

    }

    private void UpdateSelected()
    {

        if(CurrIndex == 0)
        {

            Left.material = ResourcesManager.TextMat.Disabled;

        }
        else if (isSelected)
        {

            Left.material = ResourcesManager.TextMat.Shinning;

        }
        else
        {

            Left.material = ResourcesManager.TextMat.Normal;

        }
        if(CurrIndex == Contents.Length - 1)
        {

            Right.material = ResourcesManager.TextMat.Disabled;

        }
        else if (isSelected)
        {

            Right.material = ResourcesManager.TextMat.Shinning;

        }
        else
        {

            Right.material = ResourcesManager.TextMat.Normal;

        }

        if (isSelected)
        {

            Content.material = ResourcesManager.TextMat.Shinning;

        }
        else
        {

            Content.material = ResourcesManager.TextMat.Normal;

        }

    }

    private float ValueChangeDistanceTime = 0.2f;

    private float LastValueChangeTime = 0;

    public bool RollRight()
    {

        if(Time.realtimeSinceStartup - LastValueChangeTime <= ValueChangeDistanceTime)
        {

            return false;

        }

        LastValueChangeTime = Time.realtimeSinceStartup;

        if (CurrIndex == Contents.Length - 1)
        {

            return false;

        }
        else
        {

            CurrIndex += 1;

            UpdateContent(false);

            MainSceneMusicManager.instance.PlayMenuEffect(MainSceneMusicManager.MenuEffectKind.ToggleOn);

            return true;

        }

    }

    public bool RollLeft()
    {

        if (Time.realtimeSinceStartup - LastValueChangeTime <= ValueChangeDistanceTime)
        {

            return false;

        }

        LastValueChangeTime = Time.realtimeSinceStartup;

        if (CurrIndex == 0)
        {

            return false;

        }
        else
        {

            CurrIndex -= 1;

            UpdateContent(true);

            MainSceneMusicManager.instance.PlayMenuEffect(MainSceneMusicManager.MenuEffectKind.ToggleOff);

            return true;

        }

    }

}
