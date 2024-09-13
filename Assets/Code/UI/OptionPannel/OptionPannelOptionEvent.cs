using UnityEngine;

public class OptionPannelOptionEvent : MonoBehaviour
{

    public void Option_KeyBoardSetting()
    {



    }

    public void Toggle_MusicStrengthChange(string value)
    {

        MainSceneMusicManager.instance.ChangeStrength(MainSceneMusicManager.GroupKind.Music, int.Parse(value));

    }

    public void Toggle_MusicInit(MainMenuOptionToggle toggle)
    {

        toggle.UpdateContentForce(Metric.Settings.Audio.MusicStrength.ToString());

    }

    public void Toggle_EffectStrengthChange(string value)
    {

        MainSceneMusicManager.instance.ChangeStrength(MainSceneMusicManager.GroupKind.SoundEffect, int.Parse(value));

    }

    public void Toggle_EffectInit(MainMenuOptionToggle toggle)
    {

        toggle.UpdateContentForce(Metric.Settings.Audio.EffectStrength.ToString());

    }

}
