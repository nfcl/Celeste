using UnityEngine;

public class MainPannelOptionEvent : MonoBehaviour
{

    public void Option_Climb()
    {

        StartSceneManager.instance.Main2Save();

    }

    public void Option_Option()
    {

        StartSceneManager.instance.Main2Option();

    }

    public void Option_Exit()
    {

        Metric.Tools.Exit();

    }

}
