using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class MissionBar : MonoBehaviour
{

    [SerializeField] private Mission[] missions;

    [SerializeField] private Mission MissionPrefab; 

    [SerializeField] private Transform MissionContainer; 

    [SerializeField] private int currentIndex;

    [SerializeField] private bool isMoving;

    [SerializeField] private Transform FlagBackground;

    [SerializeField] private bool IsSelect;

    private void Start()
    {

        currentIndex = 0;

        isMoving = false;

    }

    public void QuickOpen()
    {

        if (missions == null || missions.Length == 0)
        {

            Metric.Debug.LogWarning("关卡选择条尚未初始化");

        }

        GenerateIcons();

        for (int i = 0; i < missions.Length; ++i)
        {

            if (missions[i].map == Metric.SceneOnloadVarible.GameScene.CurrentMap)
            {

                currentIndex = i;

                ForceToRefresh(i);

                break;

            }

        }

    }

    public void GenerateIcons()
    {

        while (MissionContainer.childCount > 0)
        {

            DestroyImmediate(MissionContainer.GetChild(0).gameObject);

        }

        missions = new Mission[Metric.SceneOnloadVarible.GameScene.CurrentSave.mapRecords.Length];

        float[] DestX = GetIconDstPotion(missions.Length, 0);

        for(int i = 0; i < missions.Length; ++i)
        {

            missions[i] = GameObject.Instantiate(MissionPrefab, MissionContainer);

            missions[i].map = Metric.MapsInfo.infos[Metric.SceneOnloadVarible.GameScene.CurrentSave.mapRecords[i].MapIndex];

            missions[i].transform.localPosition = new Vector3(DestX[i], 800, 0);

            missions[i].transform.localScale = new Vector3(0.55f, 0.55f, 1);

        }

        missions[0].transform.localScale = new Vector3(0.8f, 0.8f, 1);

    }

    public bool SelectPrev()
    {

        if (currentIndex == 0 || isMoving == true)
        {

            return false;

        }

        isMoving = true;

        currentIndex -= 1;

        MainSceneMusicManager.instance.RollIcon(true);

        RefreshMove(true);

        return true;

    }

    public bool SelectNext()
    {

        if (currentIndex == missions.Length - 1 || isMoving == true)
        {

            return false;

        }

        isMoving = true;

        currentIndex += 1;

        MainSceneMusicManager.instance.RollIcon(false);

        RefreshMove(false);

        return true;

    }

    public void SetVisible(bool visible, bool isSelect = false)
    {

        StartCoroutine(setVisible(visible, isSelect));

    }

    public Mission GetCurrentMission()
    {

        return missions[currentIndex];

    }

    private IEnumerator setVisible(bool visible,bool isSelect = false)
    {
     
        WaitForSeconds wait = new WaitForSeconds(0.01f);

        FlagBackground.DOLocalMoveY(visible ? 387.5f : 787.5f, 0.2f);

        if (visible)
        {

            CameraTransformRefresh();

            for (int i = missions.Length - 1; i >= 0; --i)
            {

                if (isSelect && i == currentIndex)
                {

                    continue;

                }

                missions[i].transform.DOLocalMoveY(400, 0.2f);

                yield return wait;

            }

        }
        else
        {

            for (int i = 0; i < missions.Length; ++i)
            {

                if (isSelect && i == currentIndex)
                {

                    continue;

                }

                missions[i].transform.DOLocalMoveY(800, 0.2f);

                yield return wait;

            }

        }

    }

    private void CameraTransformRefresh()
    {

        StartSceneManager.instance.MainCameraControl.CurrentState = missions[currentIndex].map.MoveOnCameraState;

        StartSceneManager.instance.mountainManager.TargetMode = missions[currentIndex].map.MountainMaterialState;

        StartSceneManager.instance.MountainHeartCoverVisible = missions[currentIndex].map.MapName != "核心";

        StartSceneManager.instance.mountainManager.IsMountain = missions[currentIndex].map.MapName != "再见";

    }

    private float[] GetIconDstPotion(int Num, int Curr)
    {

        float[] Result = new float[Num];

        Result[Curr] = 0f;

        if (Curr != 0)
        {

            Result[Curr - 1] = -210;

            for (int i = Curr - 2; i >= 0; --i)
            {

                Result[i] = Result[i + 1] - 130;

            }

        }

        if (Curr != Num - 1)
        {

            Result[Curr + 1] = 210;

            for (int i = Curr + 2; i < Num; ++i)
            {

                Result[i] = Result[i - 1] + 130;

            }

        }

        return Result;

    }

    private void ForceToRefresh(int newIndex)
    {

        if (newIndex < 0 || newIndex >= missions.Length)
        {

            Metric.Debug.LogWarning("非法的新位置");

        }

        float[] destX = GetIconDstPotion(missions.Length,newIndex);

        for(int i = 0; i < destX.Length; ++i)
        {

            missions[i].transform.localPosition = new Vector3(destX[i], missions[i].transform.localPosition.y, 0);

            missions[i].transform.localScale = new Vector3(0.55f, 0.55f, 1);

        }

        missions[newIndex].transform.localScale = new Vector3(0.8f, 0.8f, 1);

    }

    private void RefreshMove(bool isLeft)
    {

        CameraTransformRefresh();

        float animeTime = 0.2f;

        float[] destX = GetIconDstPotion(missions.Length, currentIndex);

        destX[currentIndex] = 0;

        for(int i= 0; i < missions.Length; ++i)
        {

            if(i == currentIndex)
            {

                continue;

            }

            missions[i].transform.DOLocalMoveX(destX[i], animeTime);

            missions[i].transform.DOScale(0.55f, animeTime);

        }

        missions[currentIndex].transform.DOLocalRotate(new Vector3(0, 0, (isLeft ? -1 : 1) * 50), animeTime / 4).OnComplete(
            () =>
            {
                missions[currentIndex].transform.DOLocalRotate(new Vector3(0, 0, (isLeft ? -1 : 1) * -20), animeTime / 3).OnComplete(
                    () =>
                    {
                        missions[currentIndex].transform.DOLocalRotate(new Vector3(0, 0, 0), animeTime / 4);
                    }
                );
            }
        );

        missions[currentIndex].transform.DOLocalMoveX(0, animeTime);

        missions[currentIndex].transform.DOScale(0.8f, animeTime).OnComplete(() => isMoving = false);

    }

}
