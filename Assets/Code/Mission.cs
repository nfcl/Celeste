using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Mission : MonoBehaviour
{

    public Metric.MapsInfo.Map map;

    public void Start()
    {

        Image icon = GetComponent<Image>();

        icon.sprite = map.IconFace;

        icon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, map.IconFace.rect.width);
        icon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, map.IconFace.rect.width);

    }

}
