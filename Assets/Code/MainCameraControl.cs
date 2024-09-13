using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using UnityEngine;

public class MainCameraControl : MonoBehaviour
{
    public static MainCameraControl instance;
    /// <summary>
    /// 旋转模式极坐标角速度(°)
    /// </summary>
    [SerializeField] private float RotationSpeed = 10;
    /// <summary>
    /// 旋转模式极坐标半径
    /// </summary>
    [SerializeField] private float RotationRaid = 15;
    /// <summary>
    /// 相机二维极坐标
    /// </summary>
    [SerializeField] private PolarCoordinates polarCoordinates;
    /// <summary>
    /// 相机高度
    /// </summary>
    [SerializeField] private float height;

    public Metric.Camera.CameraTargetState CurrentState;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="newState"></param>
    public void ForceToNewState(Metric.Camera.CameraTargetState newState)
    {

        CurrentState = newState;

        polarCoordinates = CurrentState.polarCoordinates;

        height = CurrentState.height;

        RefreshPosition();

        transform.localEulerAngles = CurrentState.EulerAngle;

    }
    /// <summary>
    /// 目标模式
    /// </summary>
    private IEnumerator TargetMode()
    {

        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        float TargetLerpScale = 3;

        while (true)
        {

            yield return wait;

            if (CurrentState.IsRotation)
            {

                polarCoordinates.Radius = Mathf.Lerp(polarCoordinates.Radius, RotationRaid, 3 * Time.fixedDeltaTime);

                polarCoordinates.Angle += RotationSpeed * Time.fixedDeltaTime;

                height = Mathf.Lerp(height, CurrentState.height, 3 * Time.fixedDeltaTime);

                RefreshPosition();

                transform.localEulerAngles = new Vector3(
                    Mathf.LerpAngle(transform.localEulerAngles.x, CurrentState.EulerAngle.x, 3 * Time.fixedDeltaTime),
                    Mathf.LerpAngle(transform.localEulerAngles.y, -polarCoordinates.Angle - 90, TargetLerpScale * Time.fixedDeltaTime),
                    transform.localEulerAngles.z
                );

            }
            else
            {

                polarCoordinates = PolarCoordinates.Lerp(polarCoordinates, CurrentState.polarCoordinates, TargetLerpScale * Time.fixedDeltaTime);

                height = Mathf.Lerp(height, CurrentState.height, TargetLerpScale * Time.fixedDeltaTime);

                RefreshPosition();

                transform.localEulerAngles = new Vector3(
                    Mathf.LerpAngle(transform.localEulerAngles.x, CurrentState.EulerAngle.x, TargetLerpScale * Time.fixedDeltaTime),
                    Mathf.LerpAngle(transform.localEulerAngles.y, CurrentState.EulerAngle.y, TargetLerpScale * Time.fixedDeltaTime),
                    Mathf.LerpAngle(transform.localEulerAngles.z, CurrentState.EulerAngle.z, TargetLerpScale * Time.fixedDeltaTime)
                );

            }

        }

    }
    /// <summary>
    /// 使用极坐标刷新相机位置
    /// </summary>
    private void RefreshPosition()
    {

        transform.position = polarCoordinates.GetPosition3(height);

    }
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {

        polarCoordinates = transform.position.ToPolarCoordinates();

        height = transform.position.y;

        StartCoroutine(TargetMode());

    }
    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        
        if(instance == null)
        {

            instance = this;

        }

    }

#if UNITY_EDITOR

    [SerializeField] private bool __IsUserChange;

    [SerializeField] private Vector3 __position;

    [SerializeField] private Vector3 __target;

    private void OnValidate()
    {

        if (__IsUserChange)
        {

            transform.position = __position;

            transform.LookAt(__target);

            Debug.Log($"{transform.localEulerAngles.x}f,{transform.localEulerAngles.y}f,{transform.localEulerAngles.z}f");

        }

    }

#endif

}

//main_0  pos : 0.000f,12.000f,24.000f tar : 0.482f,11.736f,22.077f rot :  7.585f, 165.929f,0f
//main_1  pos :-2.856f, 3.000f,14.711f tar :-2.532f, 3.516f,12.806f rot :-14.951f, 170.348f,0f (旋转)
//start_0 pos : 1.374f, 1.224f, 7.971f tar : 0.440f, 0.499f, 6.358f rot : 21.254f,-149.927f,0f
//start_1 pos : 1.390f, 0.784f, 7.593f tar : 0.052f, 0.545f, 6.125f rot :  6.861f,-137.653f,0f
//start_2 pos : 1.155f, 0.683f, 7.348f tar : 0.264f, 0.561f, 5.561f rot :  3.496f,-153.499f,0f
//lvl1_0  pos : 0.952f, 4.218f, 9.744f tar : 0.111f, 3.393f, 8.128f rot : 24.364f,-152.507f,0f
//lvl1_1  pos : 0.052f, 1.659f, 9.902f tar :-1.110f, 1.526f, 8.280f rot :  3.814f,-144.382f,0f
//lvl1_2  pos :-1.387f, 1.884f, 7.215f tar :-1.386f, 0.910f, 5.468f rot : 29.141f, 179.967f,0f
//lvl2_0  pos : 3.399f, 5.614f, 3.870f tar : 2.240f, 4.436f, 2.743f rot : 36.080f,-134.198f,0f
//lvl2_1  pos : 3.890f, 3.903f, 3.702f tar : 2.037f, 3.815f, 2.955f rot :  2.522f,-111.956f,0f
//lvl2_2  pos : 3.362f, 4.315f, 3.335f tar : 1.928f, 3.733f, 2.069f rot : 16.923f, -131.44f,0f
//lvl3_0  pos :-5.961f, 8.823f, 5.058f tar :-5.061f, 7.757f, 3.625f rot : 32.209f, 147.869f,0f
//lvl3_1  pos :-4.294f, 6.633f, 5.193f tar :-5.027f, 6.828f, 3.342f rot : -5.594f,-158.396f,0f
//lvl3_2  pos :-5.156f, 6.648f, 2.934f tar :-5.086f, 6.447f, 0.945f rot :  5.767f, 177.984f,0f
//lvl4_0  pos :-9.626f, 8.824f,-4.140f tar :-7.924f, 8.240f,-3.267f rot : 16.978f,  62.846f,0f
//lvl4_1  pos :-8.429f, 5.837f,-5.086f tar :-6.662f, 6.019f,-4.167f rot : -5.221f,  62.521f,0f
//lvl4_2  pos :-7.389f, 5.461f,-3.631f tar :-5.809f, 5.686f,-2.426f rot :  -6.46f,  52.669f,0f
//lvl5_0  pos : 0.963f,10.542f,-5.314f tar : 0.178f, 9.588f,-3.741f rot : 28.487f, -26.521f,0f
//lvl5_1  pos :-1.786f, 8.760f,-5.080f tar :-0.494f, 8.810f,-3.554f rot : -1.432f,  40.253f,0f
//lvl5_2  pos : 0.079f, 9.274f,-4.312f tar : 0.668f, 9.085f,-2.411f rot :  5.425f,  17.215f,0f
//lvl6_0  pos :-1.113f,12.154f, 6.334f tar : 0.086f,11.118f, 5.115f rot : 31.212f, 135.474f,0f
//lvl6_1  pos :-1.113f,12.154f, 6.334f tar :-0.945f,11.175f, 4.598f rot : 29.306f, 174.472f,0f
//lvl6_2  pos : 0.084f,10.304f, 3.209f tar : 0.585f, 9.319f, 1.542f rot : 29.505f, 163.272f,0f
//lvl7_0  pos :14.620f, 3.606f,19.135f tar :13.134f, 4.115f,17.897f rot :-14.744f,-129.798f,0f
//lvl7_1  pos :13.453f, 5.141f,18.179f tar :11.907f, 5.751f,17.067f rot :-17.761f,-125.727f,0f
//lvl7_2  pos : 9.604f, 6.691f,13.034f tar : 8.414f, 7.332f,11.559f rot :-18.687f,-141.104f,0f
//end_0   pos : 1.234f, 0.677f, 7.598f tar : 0.221f, 0.734f, 5.875f rot : -1.634f,-149.548f,0f
//end_1   pos : 1.234f, 0.677f, 7.598f tar :-0.010f, 0.694f, 6.032f rot : -0.487f,-141.537f,0f
//end_2   pos : 1.121f, 0.663f, 7.332f tar : 0.276f, 0.582f, 5.521f rot :  2.321f,-154.987f,0f
//lvl8_0  pos : 4.473f, 7.158f, 5.463f tar : 3.630f, 6.660f, 3.719f rot : 14.418f,-154.202f,0f
//lvl8_1  pos : 3.404f, 6.677f, 3.846f tar : 2.093f, 6.202f, 2.413f rot : 13.743f,-137.546f,0f
//lvl8_2  pos : 3.614f, 5.999f, 0.422f tar : 1.676f, 5.630f, 0.098f rot : 10.636f, -99.491f,0f
//lvl9_0  pos : 6.032f,33.050f, 7.698f tar : 4.774f,32.938f, 6.147f rot :   3.21f,-140.955f,0f (旋转)
//lvl9_1  pos :-5.881f,31.525f, 2.871f tar :-4.394f,31.481f, 1.535f rot :  1.261f, 131.938f,0f
//lvl9_2  pos :-2.505f,31.069f, 1.248f tar :-0.748f,31.063f, 0.292f rot :  0.172f, 118.551f,0f