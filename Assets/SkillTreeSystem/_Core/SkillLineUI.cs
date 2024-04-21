using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillLineUI : UIObjectBase
{
    public ImageBezior beziorLine;
    public SkillLineStyle style;
    [field: SerializeField] public SkillSlotUIBase m_ParentSlot { get; private set; }
    [field: SerializeField] public SkillSlotUIBase m_ChildSlot { get; private set; }

    public static SkillLineUI _(SkillLineStyle style=null)
    {
        var go = new GameObject("SkillLineUI");
        go.AddComponent<RectTransform>();

        var ui = go.AddComponent<SkillLineUI>();
        ui.style = style ?? new();
        ui.SetUp();

        return ui;
    }

    protected override void SetUp()
    {
        base.SetUp();

        if(beziorLine == null)
        {
            beziorLine = gameObject.AddComponent<ImageBezior>();
            beziorLine.Initailize();
        }
    }

    public void SetLineColor(Color color) 
    {
        beziorLine.color = color;
    } 
    public void SetParentSlot(SkillSlotUIBase parent)
    {
        m_ParentSlot = parent;
        var pos = parent.transform.position;
        beziorLine.startPoint = pos - transform.position;
    }
    public void SetChildSlot(SkillSlotUIBase child)
    {
        m_ChildSlot = child;
        var pos = child.transform.position;
        beziorLine.endPoint = pos - transform.position;
    }
    public void SetLinierControlPosition()
    {
        beziorLine.controlPoint = (beziorLine.startPoint + beziorLine.endPoint) / 2;
    }

#if UNITY_EDITOR
    public override void _Editor_SelectedUpdate()
    {
        base._Editor_SelectedUpdate();

        if(rectTransform)
        {
            rectTransform.sizeDelta = Vector3.zero;
        }

        beziorLine.sprite = style.sprite;
        beziorLine.color = style.color;
        beziorLine.SetThickness(style.width);
        beziorLine.SetSegment(style.segment);
    }
#endif
}
