using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProcessButton : MonoBehaviour
{

    [SerializeField] private Metric.Settings.KeyBoard.KeyKind kind;

    [SerializeField] private Image Icon;

    public static void AllRefresh()
    {

        ProcessButton[] buttons = GameObject.FindObjectsOfType<ProcessButton>();

        foreach (var button in buttons)
        {

            button.Refresh();

        }

    }

    public void Refresh()
    {

        Icon.sprite = ResourcesManager.Button.GetButtonSprite(Metric.Settings.KeyBoard.mapping[(int)kind]);

    }

}
