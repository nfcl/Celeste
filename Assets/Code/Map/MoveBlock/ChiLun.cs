using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiLun : MonoBehaviour
{

    private static Sprite[] Clips;

    [SerializeField] private int CurrentClipIndex;

    [SerializeField] private SpriteRenderer sr;

    public bool IsBackGround
    {

        set
        {

            sr.color = value ? new Color32(65, 65, 65, 255) : new Color32(255, 255, 255, 255);

            sr.sortingOrder = value ? 0 : 1;

        }

    }

    public void Init()
    {

        CurrentClipIndex = Random.Range(0, Clips.Length);

        sr.sprite = Clips[CurrentClipIndex];

    }

    public void Award()
    {

        if (CurrentClipIndex == Clips.Length - 1)
        {

            CurrentClipIndex = 0;

        }
        else
        {

            CurrentClipIndex += 1;

        }

        sr.sprite = Clips[CurrentClipIndex];

    }

    public void Back()
    {

        if (CurrentClipIndex == 0)
        {

            CurrentClipIndex = Clips.Length - 1;

        }
        else
        {

            CurrentClipIndex -= 1;

        }

        sr.sprite = Clips[CurrentClipIndex];

    }

    private void Awake()
    {
        
        if(Clips == null)
        {

            Clips = new Sprite[3];

            for(int i= 0; i < 3; ++i)
            {

                Clips[i] = Resources.Load<Sprite>($"Map/MoveBlock/BlockChiLun/sprite_{i}");

            }

        }

    }
}
