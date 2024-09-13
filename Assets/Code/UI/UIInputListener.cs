using System.Collections;
using UnityEngine;

public interface IUIInputEvent
{

    public void OnConfirmClick();
    public void OnCancelClick();
    public void OnLeftKeepDown();
    public void OnRightKeepDown();
    public void OnUpClick();
    public void OnDownClick();

}

public class UIInputListener : MonoBehaviour
{

    private struct KeyState
    {

        public bool Last;
        public bool Curr;
        int KindIndex;

        public KeyState(Metric.Settings.KeyBoard.KeyKind kind)
        {

            Last = Curr = false;

            KindIndex = (int)kind;

        }

        public void Listen()
        {

            Curr = Input.GetKey(Metric.Settings.KeyBoard.mapping[KindIndex]);

        }

        public void Refresh()
        {

            Last = Curr;

        }

        public bool IsCurrentSingleDown()
        {

            return Last == false && Curr == true;

        }

    }

    private IEnumerator InputListener()
    {

        KeyState Confirm = new KeyState(Metric.Settings.KeyBoard.KeyKind.Menu_Confirm);
        KeyState Cancel = new KeyState(Metric.Settings.KeyBoard.KeyKind.Menu_Cancel);
        KeyState Left = new KeyState(Metric.Settings.KeyBoard.KeyKind.Menu_Left);
        KeyState Right = new KeyState(Metric.Settings.KeyBoard.KeyKind.Menu_Right);
        KeyState Up = new KeyState(Metric.Settings.KeyBoard.KeyKind.Menu_Up);
        KeyState Down = new KeyState(Metric.Settings.KeyBoard.KeyKind.Menu_Down);

        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (true)
        {
            yield return wait;

            Confirm.Listen();
            Cancel.Listen();
            Left.Listen();
            Right.Listen();
            Up.Listen();
            Down.Listen();

            if (Confirm.IsCurrentSingleDown())
            {
                eventInstance?.OnConfirmClick();
            }
            else if (Cancel.IsCurrentSingleDown())
            {
                eventInstance?.OnCancelClick();
            }
            else if (Up.IsCurrentSingleDown())
            {
                eventInstance?.OnUpClick();
            }
            else if (Down.IsCurrentSingleDown())
            {
                eventInstance?.OnDownClick();
            }
            else if (Left.Curr)
            {
                eventInstance?.OnLeftKeepDown();
            }
            else if (Right.Curr)
            {
                eventInstance?.OnRightKeepDown();
            }
            Confirm.Refresh();
            Cancel.Refresh();
            Left.Refresh();
            Right.Refresh();
            Up.Refresh();
            Down.Refresh();
        }

    }

    private void Start()
    {

        StartCoroutine(InputListener());

    }

    public static IUIInputEvent eventInstance;

}
