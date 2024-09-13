using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartSceneManager : MonoBehaviour
{

    /// <summary>
    /// 单例
    /// </summary>
    public static StartSceneManager instance;
    /// <summary>
    /// 主相机
    /// </summary>
    public MainCameraControl MainCameraControl;
    /// <summary>
    /// UI相机
    /// </summary>
    [SerializeField] private Camera UICamera;
    /// <summary>
    /// 主场景UI管理
    /// </summary>
    [SerializeField] private StartPannel startPannel;
    /// <summary>
    /// 主场景UI管理
    /// </summary>
    [SerializeField] private MainPannel mainPannel;
    /// <summary>
    /// 选项界面
    /// </summary>
    [SerializeField] private OptionPannel optionPannel;
    /// <summary>
    /// 存档界面
    /// </summary>
    [SerializeField] private SavePannel savePannel;
    /// <summary>
    /// 关卡选择界面
    /// </summary>
    [SerializeField] private MissionPannel missionPannel;
    /// <summary>
    /// 确定和返回按钮组
    /// </summary>
    [SerializeField] private Transform[] ProcessButtons;
    /// <summary>
    /// 背景
    /// </summary>
    [SerializeField] private Animation BackGround;
    /// <summary>
    /// 场地模型
    /// </summary>
    public MountainController mountainManager;
    /// <summary>
    /// 场景切换
    /// </summary>
    [SerializeField] private CutScene cutScene;

    private void Start()
    {

        if (Metric.SceneOnloadVarible.GameScene.CurrentCheckPoint != null)
        {

            cutScene.ForceTo(true, true);

        }

        Metric.Settings.Read();

        Metric.Archive.Read();

        MainSceneMusicManager.instance.Init();

        MainSceneMusicManager.instance.EnvironmentFadeChange(true);

        Metric.Debug.Log("主管理器初始化完成");

        ProcessButton.AllRefresh();

        if (Metric.SceneOnloadVarible.GameScene.CurrentCheckPoint != null)
        {

            cutScene.ToggleChange(
                false,
                CutScene.Kind.Fade,
                1,
                null,
                true
            );

            startPannel.Init(false);

            missionPannel.QuickOpen();

            SetBackGroundVisible(false);

        }
        else
        {

            startPannel.Init(true);

        }

    }

    public bool MountainHeartCoverVisible
    {

        set
        {

            mountainManager.SetHeartCoverVisible(value);

        }

    }

    public void SetBackGroundVisible(bool visible)
    {

        if (visible == true)
        {

            BackGround.Play("ShowBackGround");

        }
        else
        {

            BackGround.Play("HideBackGround");

        }

    }

    public void SetProcessButtonVisible(bool visible)
    {

        ProcessButtons[0].parent.DOLocalMoveX(visible ? 0 : 1920, 0.4f);

    }

    public void Confirm()
    {

        ProcessButtons[0].DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f, 5, 0.5f);

    }

    public void Back()
    {

        ProcessButtons[1].DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f, 5, 0.5f);

    }

    public void Start2Main()
    {

        startPannel.Close();

        mainPannel.Open();

        SetProcessButtonVisible(true);

    }

    public void Main2Start()
    {

        mainPannel.Close();

        startPannel.Open();

        SetProcessButtonVisible(false);

    }

    public void Main2Mission()
    {

        mainPannel.Close();

        missionPannel.Open();

    }

    public void Main2Save()
    {

        mainPannel.Close();

        savePannel.Open();

    }

    public void Save2Main()
    {

        savePannel.Close(false);

        mainPannel.Open();

    }

    public void Save2Mission()
    {

        savePannel.Close(true);

        missionPannel.Open();

    }

    public void Mission2Main()
    {

        mountainManager.TargetMode = Metric.Mountain.MainMountainState;

        mainPannel.Open();

        missionPannel.Close();

    }

    public void Main2Option()
    {

        mainPannel.Close();

        optionPannel.Open();

    }

    public void Option2Main()
    {

        optionPannel.Close();

        mainPannel.Open();

    }

    public void Awake()
    {

        if (instance == null)
        {

            instance = this;

        }

    }

    public void EnterMission(string MissionSceneName)
    {

        UIInputListener.eventInstance = null;

        MainSceneMusicManager.instance.Enter_ASide();

        cutScene.ToggleChange(
            true,
            CutScene.Kind.Lvl1,
            2,
            delegate
            {

                SceneManager.LoadScene(MissionSceneName);

            }, 
            false
        );

    }

}
