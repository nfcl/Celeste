using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointPic : MonoBehaviour
{

    [SerializeField] private Image Icon;

    [SerializeField] private Transform StrawberryContainer;

    [SerializeField] private Image StrawberryPrefab;

    [SerializeField] private Image[] Strawberries;

    public void Init(Metric.MapsInfo.CheckPoint newCheckPoint)
    {

        Icon.sprite = newCheckPoint.CheckPointIcon;

        while (StrawberryContainer.childCount > 0)
        {

            DestroyImmediate(StrawberryContainer.GetChild(0).gameObject);

        }

        Strawberries = new Image[newCheckPoint.StrawBerriesName.Length];

        int StartX = 330 - 60 * (Strawberries.Length - 1);

        int OffestX = 60;

        for(int i = 0; i < Strawberries.Length; ++i)
        {

            Strawberries[i] = GameObject.Instantiate(StrawberryPrefab, StrawberryContainer);

            Strawberries[i].material = ResourcesManager.ImageMat.Strawberry.CheckPoint_StrawberrySlot;

            Strawberries[i].transform.localPosition = new Vector3(StartX + OffestX * i, 0, 0);

        }

        for (int i = 0; i < newCheckPoint.CheckPointRecord.Strawberries.Count; ++i)
        {

            Strawberries[newCheckPoint.CheckPointRecord.Strawberries[i]].material = ResourcesManager.ImageMat.Strawberry.CheckPoint_Strawberry;

        }

    }

}
