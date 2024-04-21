using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Action = System.Action;

/*
 * custom virtul this class
 */
public class SkillSlotUIBase : UIObjectBase
{
    /*
     * data
     */
    public SkillTreeViewer ownerSkillTree;
    public List<SkillSlotUIBase> parentSkills = new();
    public List<SkillSlotUIBase> childrenSkills = new();

    /*
     * skill data
     */
    [Header("Skill Data")]
    [field: SerializeField]
    public SkillData data { get; private set; }
    public string uid;

    /*
     * ui style data
     */
    [Header("Style")]
    public SkillSlotStyle style;

    /*
     * frame image
     * skill icon image
     * skill slot button
     * background image
     * elevation
     */
    [Header("UI Component - debug value")]
    public TextButtonUI button;
    public ImageUI icon;
    public ImageUI frame;

    /*
     * event
     */
    public Action onClick;


    /* named constructor */
    public static SkillSlotUIBase _(SkillSlotStyle style=null)
    {
        var go = new GameObject("SkillSlotUI");
        go.AddComponent<RectTransform>();

        var ui = go.AddComponent<SkillSlotUIBase>();
        ui.style = style ?? new();
        ui.InitializeUI();
        ui.SetUp();

        return ui;
    }

    protected override void SetUp()
    {
        base.SetUp();

        if (button == null)
        {
            button = TextButtonUI._();
            AddChild(button);
        }

        if(icon == null)
        {
            icon = ImageUI._(expand: true);
            button.AddChild(icon);
        }

        button.SetTargetGraphic(icon.GetImage());

        childrenSkills.ForEach(e =>
        {
            var skillLine = SkillLineUI._();
            AddChild(skillLine);
        });
    }

    public void SetData(SkillDataContainer container)
    {
        transform.localPosition = new Vector3(container.nodePosition.x * 0.5f, container.nodePosition.y, 0);
        uid = container.uid;
        this.data = container.data;
    }
   
    public void SetActivate()
    {
        icon.SetColor(Color.white);
    }
    public void SetHilight()
    {
        icon.SetColor(Color.blue);
    }
    public void SetDisable()
    {
        icon.SetColor(new Color(0.5f, 0.5f, 0.5f));
    }
    public void SetOnClick(Action action)
    {
        if(button == null)
        {
            return;
        }

        button.button.onClick.RemoveAllListeners();
        button.button.onClick.AddListener(() =>
        {
            action();
        });
    }

    public void Init()
    {

    }

#if UNITY_EDITOR
    
    public SkillData _Editor_SkillData() => new();
    public SkillSlotStyle _Editor_SkillSlotStyle() => new();

    public override void _Editor_SelectedUpdate()
    {
        if (data == null)
        {
            return;
        }

        if (rectTransform)
        {
            rectTransform.sizeDelta = style.size;
        }

        if(button)
        {
            button.SetWidth(style.size.x);
            button.SetHeight(style.size.y);
            button.SetBackgroundImageColor(style.backgroundColor);
        }

        if(icon)
        {
            icon.SetWidth(style.size.x);
            icon.SetHeight(style.size.y);
            icon.SetSprite(data.skillIcon);
        }
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(SkillSlotUIBase))]
public class SkillSlotUIEditor : Editor
{
    SkillSlotUIBase owner;

    void OnEnable()
    {
        owner = (SkillSlotUIBase)target;
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
