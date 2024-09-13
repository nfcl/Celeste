using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Camera))] //指定自定义Editor所要绑定的Mono类型，这里就是typeof(TutorialMono)
public class CameraEditorExtension : Editor //继承Editor
{
    private Camera m_target; //在Inspector上显示的实例目标

    /// <summary>
    /// 当对象活跃时（在Inspector中显示时），unity自动调用此函数
    /// </summary>
    private void OnEnable()
    {
        m_target = target as Camera; //绑定target，target官方解释： The object being inspected
    }
    /// <summary>
    /// 重写OnInspectorGUI，之后所有的GUI绘制都在此方法中。
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); //调用父类方法绘制一次GUI，TutorialMono中原本的可序列化数据等会在这里绘制一次。
        //如果不调用父类方法，则这个Mono的Inspector全权由下面代码绘制。

        if (GUILayout.Button("这是一个按钮"))   //自定义按钮
        {

            m_target.transform.LookAt(new Vector3(0.482f, 11.736f, 22.077f));

        }
    }
}

//-0.241 12.698 -0.568
//0.249 12.698 -0.568

//5.339 8.633 -0.030

//pos : 0.952 4.218 9.744
//tar : 0.111 3.393 8.128

//pos : -5.652 3.000 13.879
//tar : -4.965 3.516 12.073