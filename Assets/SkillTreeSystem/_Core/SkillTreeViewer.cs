using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SkillTreeViewer : UIObjectBase
{
    public ContentSizeFillterExtention contentRoot;
    public ContentSizeFillterExtention skillLineContainer;

    public SkillTreeData skillTreeData;

    [Header("Style - base")]
    [CreateScriptable(create_func: "_Editor_SkillTreeData")]
    public SkillSlotStyle baseSkillSlotStyle;
    [CreateScriptable(create_func: "_Editor_SkillLineStyle")]
    public SkillLineStyle baseSkillConnectorStyle;

    [Header("Nodes, Connectors")]
    public List<SkillSlotUIBase> skillSlotUIs = new();
    public List<SkillLineUI> skillLineUIs = new();

    public ImageViewer viewer;
    public Text m_SpText;
    public Button m_ClearButton;
    public Button m_SaveButton;

    private UserData m_UserDataCache;

    [Space]
    [Button("_Editor_SetUp")]
    public string _editor_setup;


    public override void ShowUI()
    {
        base.ShowUI();

        m_UserDataCache = GameManager.Instance.LoadUserData();

        // refresh
        Refresh();

        m_ClearButton.onClick.RemoveAllListeners();
        m_ClearButton.onClick.AddListener(() => Clear());

        m_SaveButton.onClick.RemoveAllListeners();
        m_SaveButton.onClick.AddListener(() => Save());
    }

    public void Clear()
    {
        // get user data
        m_UserDataCache.skilltree.skills.Clear();
        m_UserDataCache.sp = 1000;
        Save();
        Refresh();
    }

    public void Save()
    {
        GameManager.Instance.SaveUserData(m_UserDataCache);
    }

    public void Refresh()
    {
        // refresh sp text
        m_SpText.text = $"SP {m_UserDataCache.sp}";

        // refresh skill slot
        skillSlotUIs.ForEach(e =>
        {
            var result = m_UserDataCache.skilltree.skills.FirstOrDefault(s => s.uid == e.uid);

            if (result != null)
            {
                e.SetActivate();
                e.SetOnClick(() =>
                {
                    Debug.Log("이미 획득한 스킬입니다.");
                });
            }
            else
            {
                var parentSlots = FindParentSkillSlots(e.uid);

                bool connectAble = true;
                foreach (var slot in parentSlots)
                {
                    var r = m_UserDataCache.skilltree.skills.FirstOrDefault(s => s.uid == slot.uid);
                    if (r == null)
                    {
                        connectAble = false;
                        break;
                    }
                }

                if (connectAble)
                {
                    e.SetHilight();
                    e.SetOnClick(() =>
                    {
                        if (m_UserDataCache.sp >= e.data.sp)
                        {
                            Debug.Log("스킬을 획득했습니다.");
                            UserSkillData userSkillData = new();
                            userSkillData.uid = e.uid;
                            m_UserDataCache.sp -= e.data.sp;
                            m_UserDataCache.skilltree.skills.Add(userSkillData);
                            Refresh();
                        }
                        else
                        {
                            Debug.Log("SP가 부족합니다.");
                        }
                    });
                }
                else
                {
                    e.SetDisable();
                    e.SetOnClick(() =>
                    {
                        Debug.Log("현재 해당 스킬을 배울 수 없습니다. 선 스킬을 먼저 배운 후 시도해주세요.");
                    });
                }
            }
        });

        // init connectors
    }

    public List<SkillSlotUIBase> FindParentSkillSlots(string uid)
    {
        var connects = skillLineUIs.Where(e => e.m_ChildSlot.uid == uid).ToList();

        List<SkillSlotUIBase> slots = new();

        connects.ForEach(e =>
        {
            var slot = skillSlotUIs.FirstOrDefault(s => s.uid == e.m_ParentSlot.uid);
            if(slot != null)
            {
                slots.Add(slot);
            }
        });

        return slots;
    }

    void Update()
    {
    }


#if UNITY_EDITOR
    public SkillTreeData _Editor_SkillTreeData() => new();
    public SkillLineStyle _Editor_SkillLineStyle() => new();
    public void _Editor_SetUp()
    {
        if(skillTreeData == null)
        {
            return;
        }

        if(viewer == null)
        {
            viewer = gameObject.AddComponent<ImageViewer>();
        }

        ObjectEditorEx.DestroyMonoObject(contentRoot);
        contentRoot = ContentSizeFillterExtention._();
        AddChild(contentRoot);

        ObjectEditorEx.DestroyMonoObject(skillLineContainer);
        skillLineContainer = ContentSizeFillterExtention._();
        contentRoot.AddChild(skillLineContainer);

        viewer.SetupZoom(0.4f, 1.5f);

        skillSlotUIs.ForEach(e => ObjectEditorEx.DestroyMonoObject(e));
        skillSlotUIs.Clear();

        skillTreeData._Editor_GetSKillDatas().ForEach(e =>
        {
            if(baseSkillSlotStyle == null)
            {
                baseSkillSlotStyle = new SkillSlotStyle();
                baseSkillSlotStyle.size = new Vector2(100, 100);
                baseSkillSlotStyle.backgroundColor = Color.white;
            }
            var skillslot = SkillSlotUIBase._(style: baseSkillSlotStyle);
            skillSlotUIs.Add(skillslot);
            contentRoot.AddChild(skillslot);

            skillslot.SetData(e);
        });

        skillLineUIs.ForEach(e => ObjectEditorEx.DestroyMonoObject(e));
        skillLineUIs.Clear();

        skillLineContainer.UpdateContentSizeRect();
        skillLineContainer.rectTransform.localPosition = Vector3.zero;

        contentRoot.UpdateContentSizeRect();
        contentRoot.transform.localPosition = Vector3.zero;

        skillTreeData._Editor_GetSkillConnectDatas().ForEach(e =>
        {
            if(baseSkillConnectorStyle == null)
            {
                baseSkillConnectorStyle = new SkillLineStyle();
                baseSkillConnectorStyle.color = Color.white;
                baseSkillConnectorStyle.width = 8;
                baseSkillConnectorStyle.segment = 35;
            }

            var parentSkillSlot = skillSlotUIs.FirstOrDefault(s => s.uid == e.parentSkillUid);
            var childSkillSlot = skillSlotUIs.FirstOrDefault(s => s.uid == e.childSkillUid);

            var skillconnect = SkillLineUI._(style: baseSkillConnectorStyle);
            skillLineUIs.Add(skillconnect);
            skillLineContainer.AddChild(skillconnect);

            skillconnect.transform.position = contentRoot.transform.position;
            skillconnect.SetLineColor(Color.black);
            skillconnect.SetParentSlot(parentSkillSlot);
            skillconnect.SetChildSlot(childSkillSlot);
            skillconnect.SetLinierControlPosition();
        });
    }
    public override void _Editor_SelectedUpdate()
    {
        base._Editor_SelectedUpdate();

        for(int i = 0; i < skillSlotUIs.Count; ++i)
        {
            if (skillSlotUIs[i] == null)
            {
                skillSlotUIs.RemoveAt(i--);
                continue;
            }

            skillSlotUIs[i]._Editor_SelectedUpdate();
        }

        for (int i = 0; i < skillLineUIs.Count; ++i)
        {
            if (skillLineUIs[i] == null)
            {
                skillLineUIs.RemoveAt(i--);
                continue;
            }

            skillLineUIs[i]._Editor_SelectedUpdate();
        }
    }
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(SkillTreeViewer))]
public class SkillTreeViewerEditor : Editor
{
    SkillTreeViewer owner;

    void OnEnable()
    {
        owner = (SkillTreeViewer)target;
        EditorApplication.update += OnEditorUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    void OnEditorUpdate()
    {
        if (EditorApplication.isPlaying)
        {
            return;
        }

        owner._Editor_SelectedUpdate();
    }
}
#endif
