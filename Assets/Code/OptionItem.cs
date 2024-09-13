using ObjectPool;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OptionItem : MonoBehaviour, ISetActivable
{

    [SerializeField] private Text NameText;

    private Coroutine ShinningAnime;

    public float OriginY;

    public bool IsSelected
    {

        set
        {

            if (value)
            {

                StartCoroutine(Dithering());

                ShinningAnime = StartCoroutine(TextShinning());

            }
            else
            {

                if (ShinningAnime != null)
                {

                    StopCoroutine(ShinningAnime);

                }
                NameText.color = Color.white;

            }

        }

    }

    public string Name
    {

        get
        {

            return NameText.text;

        }
        set
        {

            NameText.text = value;

        }

    }

    private IEnumerator Dithering()
    {

        float Add = 0;

        while (Add <= 3.14f * 2)
        {

            yield return new WaitForSecondsRealtime(0.01f);

            transform.localPosition = new Vector2(0, OriginY - 8 * Mathf.Sin(Add));

            Add += 0.5f;

        }

    }

    private IEnumerator TextShinning()
    {

        bool isReserve = false;

        float Add = 0;

        while (true)
        {

            yield return new WaitForSecondsRealtime(0.01f);

            if (isReserve)
            {

                NameText.color = Color.Lerp(Color.yellow, Color.green, Add);

            }
            else
            {

                NameText.color = Color.Lerp(Color.green, Color.yellow, Add);

            }

            Add += 0.1f;

            if (Add > 1) 
            {

                isReserve = !isReserve;

                Add = 0;
            
            }

        }

    }

    public void SetActive(bool active)
    {

        gameObject.SetActive(active);

    }
}
