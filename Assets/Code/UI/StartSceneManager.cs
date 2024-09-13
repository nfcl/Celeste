using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartSceneManager : MonoBehaviour
{

    /// <summary>
    /// ����
    /// </summary>
    public static StartSceneManager instance;
    /// <summary>
    /// �����
    /// </summary>
    public MainCameraControl MainCameraControl;
    /// <summary>
    /// UI���
    /// </summary>
    [SerializeField] private Camera UICamera;
    /// <summary>
    /// ������UI����
    /// </summary>
    [SerializeField] private StartPannel startPannel;
    /// <summary>
    /// ������UI����
    /// </summary>
    [SerializeField] private MainPannel mainPannel;
    /// <summary>
    /// ѡ�����
    /// </summary>
    [SerializeField] private OptionPannel optionPannel;
    /// <summary>
    /// �浵����
    /// </summary>
    [SerializeField] private SavePannel savePannel;
    /// <summary>
    /// �ؿ�ѡ�����
    /// </summary>
    [SerializeField] private MissionPannel missionPannel;
    /// <summary>
    /// ȷ���ͷ��ذ�ť��
    /// </summary>
    [SerializeField] private Transform[] ProcessButtons;
    /// <summary>
    /// ����
    /// </summary>
    [SerializeField] private Animation BackGround;
    /// <summary>
    /// ����ģ��
    /// </summary>
    public MountainController mountainManager;
    /// <summary>
    /// �����л�
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

        Metric.Debug.Log("����������ʼ�����");

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
