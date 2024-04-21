using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
[CreateAssetMenu(menuName = "SkillTreeData")]
public class SkillTreeData : ScriptableObject
{
    /*
     * edit only graph view
     * don't edit inspector
     */
    [SerializeField] List<SkillDataContainer> m_SkillDatas = new();
    [SerializeField] List<SkillConnectData> m_SkillConnectDatas = new();
    [SerializeField] List<SkillUIPositionData> m_SkillUIPositionDatas = new();

    /*
     * runtime value
     */
    List<SkillData> skillTreeRoot = new();

    public void Init()
    {

    }

    /*
     * editor func
     */
#if UNITY_EDITOR
    public void _Editor_Clear()
    {
        m_SkillDatas.Clear();
        m_SkillConnectDatas.Clear();
    }
    public List<SkillUIPositionData> _Editor_GetSkillUIPositionData() => m_SkillUIPositionDatas;
    public List<SkillConnectData> _Editor_GetSkillConnectDatas() => m_SkillConnectDatas;
    public List<SkillDataContainer> _Editor_GetSKillDatas() => m_SkillDatas;
    public SkillDataContainer FindSkillDataContainer(string uid)
    {
        return m_SkillDatas.FirstOrDefault(e => e.uid == uid);
    }
    public void AddSkillDataContainer(SkillDataContainer data)
    {
        var origin = FindSkillDataContainer(data.uid);
        if (origin != null)
        {
            m_SkillDatas.Remove(origin);
        }

        m_SkillDatas.Add(data);
    }
#endif
}

[System.Serializable]
public class SkillDataContainer
{
    public string uid;
    public Rect nodePosition;
    public SkillData data;
}

[System.Serializable]
public class SkillConnectData
{
    public string parentSkillUid;
    public string childSkillUid;
}

[System.Serializable]
public class SkillUIPositionData
{
    public string uid;
    public Rect nodePosition;
}

#if UNITY_EDITOR
[CustomEditor(typeof(SkillTreeData))]
public class SkillTreeDataEditor : Editor
{
    SkillTreeData owner;

    private void OnEnable()
    {
        owner = (SkillTreeData)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(8);
        if (GUILayout.Button("Open Graph Editor"))
        {
            SkillTreeEditorWindow.Open(owner);
        }
    }
}
#endif
