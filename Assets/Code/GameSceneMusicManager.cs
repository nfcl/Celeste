using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using static Metric.Settings;

public class GameSceneMusicManager : MonoBehaviour
{

    /// <summary>
    /// 单例
    /// </summary>
    public static GameSceneMusicManager instance;
    /// <summary>
    /// 当前场景的声音混合器
    /// </summary>
    [SerializeField] private AudioMixer mixer;
    /// <summary>
    /// 背景音乐
    /// </summary>
    public BGM bgm;
    [Serializable]
    public class BGM
    {

        [SerializeField] private AudioSource[] sources;

        public void Play()
        {

            for (int i = 0; i < sources.Length; ++i)
            {

                sources[i].Play();

            }

        }

    }
    /// <summary>
    /// 地图元素的音效
    /// </summary>
    public MapItem mapItem;
    [Serializable]
    public class MapItem
    {

        public DashCrystal dashCrystal;

        public JumpBoard jumpBoard;

        public Strawberry strawberry;

        public ZipMove zipMove;

        public FallingBlock fallingBlock;

        /// <summary>
        /// 充能水晶
        /// </summary>
        [Serializable]
        public class DashCrystal
        {

            [SerializeField] private AudioSource[] Return;

            [SerializeField] private AudioSource[] Touch;

            public void Appear()
            {

                Return[UnityEngine.Random.Range(0, Return.Length)].Play();

            }

            public void Fade()
            {

                Touch[UnityEngine.Random.Range(0, Touch.Length)].Play();

            }

        }
        /// <summary>
        /// 弹簧跳板
        /// </summary>
        [Serializable]
        public class JumpBoard
        {

            [SerializeField] private AudioSource[] Touch;

            public void Jump()
            {

                Touch[UnityEngine.Random.Range(0, Touch.Length)].Play();

            }

        }
        [Serializable]
        public class Strawberry
        {

            public Get get;
            public Touch touch;
            public Fly fly;

            [Serializable]
            public class Get
            {

                public AudioSource[] Red;
                public AudioSource[] Blue;
                public AudioSource[] Moon;

            }
            [Serializable]
            public class Touch
            {

                public AudioSource Red;
                public AudioSource Blue;

            }
            [Serializable]
            public class Fly
            {

                public AudioSource FlyAway;
                public AudioSource Laugh;

            }

            public void GetStrawberry_Red(int Level)
            {

                get.Red[Level].Play();

            }

            public void TouchStrawberry_Red()
            {

                touch.Red.Play();

            }

            public void FlyAway()
            {

                fly.FlyAway.Play();

            }

            public void Laugh()
            {

                fly.Laugh.Play();

            }

        }
        [Serializable]
        public class ZipMove
        {

            public AudioSource[] Touch;
            public AudioSource[] Impact;
            public AudioSource[] Return;
            public AudioSource[] Reset;

            public void OnTouch()
            {

                Touch[UnityEngine.Random.Range(0, Touch.Length)].Play();

            }

            public void OnImpact()
            {

                Impact[UnityEngine.Random.Range(0, Impact.Length)].Play();

            }

            public void OnReturn()
            {

                Return[UnityEngine.Random.Range(0, Return.Length)].Play();

            }

            public void OnReset()
            {

                Reset[UnityEngine.Random.Range(0, Reset.Length)].Play();

            }

        }
        [Serializable]
        public class FallingBlock
        {

            public AudioSource[] Shake;
            public AudioSource[] Impact;

            public void OnShake()
            {

                Shake[UnityEngine.Random.Range(0, Shake.Length)].Play();

            }

            public void OnImpact()
            {

                Impact[UnityEngine.Random.Range(0, Impact.Length)].Play();

            }

        }

    }

    public void FadeOut()
    {

        StartCoroutine(fadeOut());

    }

    private IEnumerator fadeOut()
    {

        mixer.GetFloat("MusicVolume", out float currentValue);

        float endValue = -80f;

        float lerpValue = 0;

        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(Time.fixedDeltaTime);

        bool animeContinue = true;

        while (animeContinue)
        {

            yield return wait;

            lerpValue += Time.fixedDeltaTime;

            if (lerpValue > 1)
            {

                lerpValue = 1;

                animeContinue = false;

            }

            mixer.SetFloat("MusicVolume", Mathf.Lerp(currentValue, endValue, lerpValue));

        }

    }

    [SerializeField] private AudioSource gamePause;

    public void GameContinue()
    {

        mixer.SetFloat("MusicVolume", Mathf.Lerp(0, -80, (1.0f / Mathf.Clamp(Metric.Settings.Audio.MusicStrength, 0, 10) - 0.1f) / 0.9f));

    }

    public void GamePause()
    {

        gamePause.Play();

        mixer.SetFloat("MusicVolume", Mathf.Lerp(0, -80, (1.0f / Mathf.Clamp(Metric.Settings.Audio.MusicStrength / 2, 0, 10) - 0.1f) / 0.9f));

    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {

        ChangeStrength(GroupKind.Master, Audio.MasterStrength);
        ChangeStrength(GroupKind.Music, Audio.MusicStrength);
        ChangeStrength(GroupKind.SoundEffect, Audio.EffectStrength);

    }
    /// <summary>
    /// 声音组类型
    /// </summary>
    public enum GroupKind
    {

        Master,
        Music,
        SoundEffect

    }
    /// <summary>
    /// 更改声音组的响度
    /// </summary>
    /// <param name="kind"></param>
    /// <param name="value"></param>
    public void ChangeStrength(GroupKind kind, int value)
    {

        value = Mathf.Clamp(value, 0, 10);

        float volumn = Mathf.Lerp(0, -80, (1.0f / value - 0.1f) / 0.9f);

        switch (kind)
        {

            case GroupKind.Master:
                {

                    mixer.SetFloat("MasterVolume", volumn);

                    Audio.MasterStrength = value;

                    break;
                }
            case GroupKind.Music:
                {

                    mixer.SetFloat("MusicVolume", volumn);

                    Audio.MusicStrength = value;

                    break;
                }
            case GroupKind.SoundEffect:
                {

                    mixer.SetFloat("EffectVolume", volumn);

                    Audio.EffectStrength = value;

                    break;
                }
        }

    }

    private void Awake()
    {

        instance = this;

    }

}