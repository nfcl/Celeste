using DG.Tweening;
using Metric;
using System.Collections;
using UnityEngine;

public class MountainController : MonoBehaviour
{

    public static MountainController instance;

    [SerializeField] private Renderer Mountain;
    [SerializeField] private Renderer MountainHeart;
    [SerializeField] private Renderer Building;

    [SerializeField] private GameObject Model_Mountain;
    [SerializeField] private GameObject Model_Space;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private UnityEngine.Camera MainCamera;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private ParticleSystem MountainSnowParticle;
    [SerializeField] private ParticleSystem.MainModule MountainSnowMainModule;
    [SerializeField] private ParticleSystem.VelocityOverLifetimeModule MountainSnowVelocityOverLifetimeModule;

    private float[] lerpValues;

    public float[] TargetMode;

    private void Start()
    {

        TargetMode = new float[3] { 1, 0, 0 };
        lerpValues = new float[3] { 1, 0, 0 };

    }

    public void SetHeartCoverVisible(bool visible)
    {

        MountainHeart.transform.DOLocalMove(visible ? new Vector3(0, 0, 0) : new Vector3(-1.94f, -1, -0.37f), 0.5f);

    }

    private bool CurrentIsMountain = true;

    public bool IsMountain
    {

        set
        {

            if (CurrentIsMountain != value)
            {

                CurrentIsMountain = value;

                Model_Mountain.SetActive(CurrentIsMountain);

                Model_Space.SetActive(!CurrentIsMountain);

                MountainSnowMainModule = MountainSnowParticle.main;
                MountainSnowVelocityOverLifetimeModule = MountainSnowParticle.velocityOverLifetime;

                if (!CurrentIsMountain)
                {

                    MainCamera.clearFlags = CameraClearFlags.SolidColor;

                    MountainSnowMainModule.startSpeed = new ParticleSystem.MinMaxCurve(0, 0);

                    MountainSnowVelocityOverLifetimeModule.x = new ParticleSystem.MinMaxCurve(-1f, 1f);
                    MountainSnowVelocityOverLifetimeModule.z = new ParticleSystem.MinMaxCurve(-1f, 1f);

                }
                else
                {

                    MainCamera.clearFlags = CameraClearFlags.Skybox;

                    MountainSnowMainModule.startSpeed = new ParticleSystem.MinMaxCurve(2, 4);

                    MountainSnowVelocityOverLifetimeModule.x = new ParticleSystem.MinMaxCurve(-2f, 2f);
                    MountainSnowVelocityOverLifetimeModule.z = new ParticleSystem.MinMaxCurve(-2f, 2f);

                }

            }

        }

    }

    private void FixedUpdate()
    {

        lerpValues[0] = Mathf.Lerp(lerpValues[0], TargetMode[0], 2 * Time.fixedDeltaTime);
        lerpValues[1] = Mathf.Lerp(lerpValues[1], TargetMode[1], 2 * Time.fixedDeltaTime);
        lerpValues[2] = Mathf.Lerp(lerpValues[2], TargetMode[2], 2 * Time.fixedDeltaTime);

        Mountain.material.SetFloat("_MainScale1", lerpValues[0]);
        Mountain.material.SetFloat("_MainScale2", lerpValues[1]);
        Mountain.material.SetFloat("_MainScale3", lerpValues[2]);
        MountainHeart.material.SetFloat("_MainScale1", lerpValues[0]);
        MountainHeart.material.SetFloat("_MainScale2", lerpValues[1]);
        MountainHeart.material.SetFloat("_MainScale3", lerpValues[2]);
        Building.material.SetFloat("_MainScale1", lerpValues[0]);
        Building.material.SetFloat("_MainScale2", lerpValues[1]);
        Building.material.SetFloat("_MainScale3", lerpValues[2]);
        RenderSettings.skybox.SetFloat("_Scale1", lerpValues[0]);
        RenderSettings.skybox.SetFloat("_Scale2", lerpValues[1]);
        RenderSettings.skybox.SetFloat("_Scale3", lerpValues[2]);

    }

    private void Awake()
    {

        if(instance == null)
        {

            instance = this;

        }

    }

}
