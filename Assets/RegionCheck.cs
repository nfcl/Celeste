using UnityEngine;
using UnityEngine.Events;

public class RegionCheck : MonoBehaviour
{

    [SerializeField] private UnityEvent Enter; 
    [SerializeField] private UnityEvent Stay; 
    [SerializeField] private UnityEvent Exit;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Enter.Invoke();

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        Stay.Invoke();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        Exit.Invoke();

    }

}
