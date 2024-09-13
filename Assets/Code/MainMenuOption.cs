using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuOption : MonoBehaviour
{

    [SerializeField] private Text label;

    [SerializeField] private Animation anime;

    [SerializeField] private MainMenuOptionToggle toggle;

    [SerializeField] private UnityEvent OnOptionSelected;

    [SerializeField] private MainSceneMusicManager.MenuEffectKind ClickAudio;

    public bool IsMoveOn
    {

        set
        {

            if (toggle != null)
            {

                toggle.IsMoveOn = value;

            }

            if (value)
            {

                label.material = ResourcesManager.TextMat.Shinning;


                anime.Play("OptionMoveOn");

            }
            else
            {

                label.material = ResourcesManager.TextMat.Normal;

                anime.Play("OptionMoveOff");

            }

        }

    }

    public void RollUp()
    {

        if (toggle != null)
        {

            toggle.RollRight();

        }

    }

    public void RollDown()
    {

        if (toggle != null)
        {

            toggle.RollLeft();

        }

    }

    public void OnSelected()
    {

        if (OnOptionSelected.GetPersistentEventCount() != 0)
        {

            MainSceneMusicManager.instance.PlayMenuEffect(ClickAudio);

            OnOptionSelected.Invoke();

        }

    }

}
