using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackConnect : MonoBehaviour
{

    [SerializeField] private Transform LeftTransform;
    [SerializeField] private Transform RightTransform;

    [SerializeField] private SpriteRenderer LeftSpriteRenderer;
    [SerializeField] private SpriteRenderer RightSpriteRenderer;

    [SerializeField] private float lerpValue;

    public bool Flip
    {

        set
        {

            LeftSpriteRenderer.flipX = value;

            RightSpriteRenderer.flipX = !value;

        }

    }

    public float LerpValue
    {

        set
        {

            lerpValue = value;

            LeftTransform.localPosition = new Vector2(0, length / 2);

            RightTransform.localPosition = new Vector2(0, length * -(lerpValue - 0.5f));

            LeftSpriteRenderer.size = new Vector2(0.08f, length * lerpValue);

            RightSpriteRenderer.size = new Vector2(0.08f, length * (1 - lerpValue));

            LeftSpriteRenderer.transform.localScale = Vector3.one;

            RightSpriteRenderer.transform.localScale = Vector3.one;

        }

    }

    public float length;

//#if UNITY_EDITOR

//    private void OnValidate()
//    {

//        LeftSpriteRenderer.transform.localScale = Vector3.one;

//        RightSpriteRenderer.transform.localScale = Vector3.one;

//    }

//#endif

}
