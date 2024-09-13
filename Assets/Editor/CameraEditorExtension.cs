using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Camera))] //ָ���Զ���Editor��Ҫ�󶨵�Mono���ͣ��������typeof(TutorialMono)
public class CameraEditorExtension : Editor //�̳�Editor
{
    private Camera m_target; //��Inspector����ʾ��ʵ��Ŀ��

    /// <summary>
    /// �������Ծʱ����Inspector����ʾʱ����unity�Զ����ô˺���
    /// </summary>
    private void OnEnable()
    {
        m_target = target as Camera; //��target��target�ٷ����ͣ� The object being inspected
    }
    /// <summary>
    /// ��дOnInspectorGUI��֮�����е�GUI���ƶ��ڴ˷����С�
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); //���ø��෽������һ��GUI��TutorialMono��ԭ���Ŀ����л����ݵȻ����������һ�Ρ�
        //��������ø��෽���������Mono��InspectorȫȨ�����������ơ�

        if (GUILayout.Button("����һ����ť"))   //�Զ��尴ť
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