using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornPoint : MonoBehaviour
{

    public Vector2 GetBornPosition()
    {

        return transform.position;

    }

    public Vector2 GetBornScale()
    {

        return transform.localScale;

    }

}
