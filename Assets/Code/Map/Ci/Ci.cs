using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ci : MonoBehaviour
{

    [SerializeField] private BoxCollider2D bodyCollider;

    private void Start()
    {

        if(transform.localEulerAngles.z == 0)
        {

            bodyCollider.size = new Vector2(bodyCollider.size.x - 0.01f, bodyCollider.size.y);

        }

    }

}
