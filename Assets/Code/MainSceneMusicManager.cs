using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using static Metric.Settings;

public class MainSceneMusicManager : MonoBehaviour
{

    public static MainSceneMusicManager instance;

    [SerializeField] private MenuEffect menuEffect;

    [SerializeField] private WorldEffect worldEffect;

    public SavePannel savePannel;

    [Serializable]
    private class MenuEffect
    {

        public AudioSource Select;
        public AudioSource Back;
        public AudioSource ToggleOn;
        public AudioSource ToggleOff;
        public AudioSource RollUp;
        public AudioSource RollDown;
        public AudioSource Climb;

    }

    public enum MenuEffectKind
    {

        None,
        Select,
        Back,
        ToggleOn,
        ToggleOff,
        RollUp,
        RollDown,
        Climb,

    }

    public void PlayMenuEffect(MenuEffectKind kind)
    {

        switch (kind)
        {

            case MenuEffectKind.Select:
                {

                    menuEffect.Select.Play();

                    break;
                }
            case MenuEffectKind.Back:
                {

                    menuEffect.Back.Play();

                    break;
                }
            case MenuEffectKind.ToggleOn:
                {

                    menuEffect.ToggleOn.Play();

                    break;
                }
            case MenuEffectKind.ToggleOff:
                {

                    menuEffect.ToggleOff.Play();

                    break;
                }
            case MenuEffectKind.RollUp:
                {

                    menuEffect.RollUp.Play();

                    break;
                }
            case MenuEffectKind.RollDown:
                {

                    menuEffect.RollDown.Play();

                    break;
                }
            case MenuEffectKind.Climb:
                {

                    menuEffect.Climb.Play();

                    break;
                }
        }

    }

    [Serializable]
    private class WorldEffect
    {

        public Mission mission;

        [Serializable]
        public class Mission
        {

            public Icon icon;
            public Chapter chapter;
            public Side side;
            public CheckPoint checkPoint;

            [Serializable]
            public class Icon
            {

                public Flip flip;
                public Roll roll;
                public AudioSource Select;

                [Serializable]
                public class Flip
                {

                    public AudioSource Left;
                    public AudioSource Right;

                }

                [Serializable]
                public class Roll
                {

                    public AudioSource Left;
                    public AudioSource Right;

                }

            }

            [Serializable]
            public class Chapter
            {

                public Roll roll;

                [Serializable]
                public class Roll
                {

                    public AudioSource Left;
                    public AudioSource Right;

                }

            }

            [Serializable]
            public class Side
            {

                public AudioSource Select;
                public AudioSource Back;

            }

            [Serializable]
            public class CheckPoint
            {

                public AudioSource[] Add;
                public AudioSource[] Remove;
                public AudioSource Select;
                public AudioSource Back;

            }

        }

    }

    public void RollIcon(bool isLeft)
    {

        if (isLeft)
        {

            worldEffect.mission.icon.roll.Left.Play();

        }
        else
        {

            worldEffect.mission.icon.roll.Right.Play();

        }

    }

    public void SelectIcon()
    {

        worldEffect.mission.icon.flip.Right.Play();

        worldEffect.mission.icon.Select.Play();

    }

    public void UnSelectIcon()
    {

        worldEffect.mission.icon.flip.Left.Play();

        worldEffect.mission.side.Back.Play();

    }

    public void Enter_ASide()
    {

        worldEffect.mission.checkPoint.Select.Play();

    }

    public void UnSelectCheckPoint()
    {

        worldEffect.mission.checkPoint.Back.Play();

    }

    public void RollCheckPoint(bool isLeft)
    {

        if (isLeft)
        {

            worldEffect.mission.chapter.roll.Left.Play();

            worldEffect.mission.checkPoint.Add[UnityEngine.Random.Range(0, worldEffect.mission.checkPoint.Add.Length)].Play();

        }
        else
        {

            worldEffect.mission.chapter.roll.Right.Play();

            worldEffect.mission.checkPoint.Remove[UnityEngine.Random.Range(0, worldEffect.mission.checkPoint.Remove.Length)].Play();

        }

    }

    public void RollSide(bool isLeft)
    {

        if (isLeft)
        {

            worldEffect.mission.chapter.roll.Left.Play();

        }
        else
        {

            worldEffect.mission.chapter.roll.Right.Play();

        }

    }

    public void SelectSide()
    {

        worldEffect.mission.side.Select.Play();

    }

    public void UnSelectSide()
    {

        worldEffect.mission.side.Back.Play();

    }

    [Serializable]
    public class SavePannel
    {

        public AudioSource Enter;

        public AudioSource[] Roll;

        public void OnEnter()
        {

            Enter.Play();

        }

        public void OnRoll()
        {

            Roll[UnityEngine.Random.Range(0, Roll.Length)].Play();

        }

    }

    public enum GroupKind
    {

        Master,
        Music,
        SoundEffect,
        Environment

    }

    [SerializeField] private AudioMixer mixer;

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
            case GroupKind.Environment:
                {

                    mixer.SetFloat("EnvironmentVolume", volumn);

                    Audio.EnvironmentStrength = value;

                    break;
                }
        }

    }

    public void EnvironmentFadeChange(bool isFadeIn)
    {

        StartCoroutine(Fade(isFadeIn));

    }

    private IEnumerator Fade(bool isFadeIn)
    {

        float startValue = isFadeIn ? -80f : 0f;

        float endValue = isFadeIn ? 0f : -80f;

        float lerpValue = 0;

        WaitForFixedUpdate wait = new WaitForFixedUpdate();

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

            mixer.SetFloat("EnvironmentVolume", Mathf.Lerp(startValue   , endValue, lerpValue));

        }

    }

    public void Init()
    {

        ChangeStrength(GroupKind.Master, Audio.MasterStrength);
        ChangeStrength(GroupKind.Music, Audio.MusicStrength);
        ChangeStrength(GroupKind.SoundEffect, Audio.EffectStrength);
        ChangeStrength(GroupKind.Environment, Audio.EnvironmentStrength);

        Metric.Debug.Log(
            $"当前音量 : \n" +
            $"|->主音量 {Audio.MasterStrength}\n" +
            $"   |->音乐音量 {Audio.MusicStrength}\n" +
            $"   |->音效音量 {Audio.EffectStrength}\n" +
            $"      |->环境音量 {Audio.EnvironmentStrength}"
        );

    }

    private void Awake()
    {

        if(instance == null)
        {

            instance = this;

        }

    }

}