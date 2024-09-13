using UnityEngine;

public class HideBlock : MonoBehaviour, IRoomEvent
{

    [SerializeField] private Animation anime;

    [SerializeField] private SpriteRenderer sr;

    private bool isHide;

    public bool IsHide
    {

        get
        {

            return isHide;

        }

        set
        {

            if (isHide != value)
            {

                isHide = value;

                if (isHide == true)
                {

                    anime.Play("HideBlocksHide");

                }
                else
                {

                    sr.color = Color.white;

                }

            }

        }

    }

    public void EnterRoomInit()
    {

        //IsHide = false;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        IsHide = true;

    }

}
