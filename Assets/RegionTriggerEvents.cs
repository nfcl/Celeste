using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class RegionTriggerEvents : MonoBehaviour
{

    /// <summary>
    /// 设置纪念碑文字是否显示
    /// </summary>
    /// <param name="visible"></param>
    public void SetMonumentTipsVisible(bool visible)
    {

        Text text = GameObject.Find("Map/Rooms/end/RegionTrigger/Monument/Canvas/Text").GetComponent<Text>();


        if (visible)
        {

            text.DOColor(new Color(1, 1, 1, 1), 0.5f);
            text.transform.DOLocalMoveY(240, 0.5f);

        }
        else
        {

            text.DOColor(new Color(1, 1, 1, 0), 0.5f);
            text.transform.DOLocalMoveY(200, 0.5f);

        }

    }
    /// <summary>
    /// 返回地图(主场景的选关界面)
    /// </summary>
    public void ReturnToMap()
    {

        GameSceneManager.instance.ReturnMap();

    }

}
