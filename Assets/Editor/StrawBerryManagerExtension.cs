#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(StrawberryManager))]
public class StrawBerryManagerExtension : Editor
{

    private StrawberryManager m_target;
    /// <summary>
    /// �������Ծʱ����Inspector����ʾʱ����unity�Զ����ô˺���
    /// </summary>
    private void OnEnable()
    {
        m_target = target as StrawberryManager; //��target��target�ٷ����ͣ� The object being inspected
    }
    /// <summary>
    /// ��дOnInspectorGUI��֮�����е�GUI���ƶ��ڴ˷����С�
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); //���ø��෽������һ��GUI��TutorialMono��ԭ���Ŀ����л����ݵȻ����������һ�Ρ�
        //��������ø��෽���������Mono��InspectorȫȨ�����������ơ�

        if (GUILayout.Button("����"))   //�Զ��尴ť
        {

            m_target.Generate();

        }
    }

}

#endif