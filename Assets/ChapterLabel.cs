using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChapterLabel : MonoBehaviour
{

    public Image BackGround;

    public Image Icon;

    private Color OriginBackGroundColor;

    private Color OriginIconColor;

    public void SetMoveOn(bool isMoveOn)
    {

        float animeTime = 0.2f;

        transform.DOBlendableLocalMoveBy(new Vector3(0, -80, 0), animeTime / 2).OnComplete(
            () =>
            {
                if (isMoveOn == false)
                {
                    transform.SetAsFirstSibling();
                }
                else
                {
                    transform.SetAsLastSibling();
                }
                transform.DOBlendableLocalMoveBy(new Vector3(0, 80, 0), animeTime / 2);
            }
        );

        transform.DOScaleX(0.9f, animeTime / 2).OnComplete(() =>
        {
            transform.DOScaleX(1, animeTime / 2);
        });

        if (!isMoveOn)
        {

            BackGround.DOColor(new Color(OriginBackGroundColor.r * 0.5f, OriginBackGroundColor.g * 0.5f, OriginBackGroundColor.b * 0.5f), animeTime);

            Icon.DOColor(new Color(OriginIconColor.r * 0.5f, OriginIconColor.g * 0.5f, OriginIconColor.b * 0.5f), animeTime);

        }
        else
        {

            BackGround.DOColor(OriginBackGroundColor, animeTime);

            Icon.DOColor(OriginIconColor, animeTime);

        }

    }

    public void Init(Color BackGroundColor, Color IconColor, bool isMoveOn)
    {

        OriginBackGroundColor = BackGroundColor;

        OriginIconColor = IconColor;

        if (isMoveOn)
        {

            transform.SetAsLastSibling();
            BackGround.color = OriginBackGroundColor;
            Icon.color = OriginIconColor;

        }
        else
        {

            transform.SetAsFirstSibling();
            BackGround.color = new Color(OriginBackGroundColor.r * 0.5f, OriginBackGroundColor.g * 0.5f, OriginBackGroundColor.b * 0.5f);
            Icon.color = new Color(OriginIconColor.r * 0.5f, OriginIconColor.g * 0.5f, OriginIconColor.b * 0.5f);

        }

    }

}
