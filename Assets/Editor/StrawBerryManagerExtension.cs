#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(StrawberryManager))]
public class StrawBerryManagerExtension : Editor
{

    private StrawberryManager m_target;
    /// <summary>
    /// 当对象活跃时（在Inspector中显示时），unity自动调用此函数
    /// </summary>
    private void OnEnable()
    {
        m_target = target as StrawberryManager; //绑定target，target官方解释： The object being inspected
    }
    /// <summary>
    /// 重写OnInspectorGUI，之后所有的GUI绘制都在此方法中。
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); //调用父类方法绘制一次GUI，TutorialMono中原本的可序列化数据等会在这里绘制一次。
        //如果不调用父类方法，则这个Mono的Inspector全权由下面代码绘制。

        if (GUILayout.Button("生成"))   //自定义按钮
        {

            m_target.Generate();

        }
    }

}

#endif