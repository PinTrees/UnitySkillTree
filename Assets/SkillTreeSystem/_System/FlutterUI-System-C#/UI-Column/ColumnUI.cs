using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColumnUI : UIObjectBase
{
    public VerticalLayoutGroup verticalLayoutGroup;

    ColumnStyle style;


    public static ColumnUI Create(ColumnStyle style=null, List<UIObjectBase> children = null)
    {
        var go = new GameObject("ColumnUI");
        go.AddComponent<RectTransform>();

        var ui = go.AddComponent<ColumnUI>();

        ui.style = style ?? new ColumnStyle();

        ui.InitializeUI();

        if (children != null)
        {
            ui.AddChildren(children);
        }

        if(style.spacing > 0)
        {
            ui.SetSpacing(style.spacing);
        }

        return ui;
    }

    public override void InitializeUI()
    {
        base.InitializeUI();

        if (verticalLayoutGroup == null)
        {
            verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
        }

        verticalLayoutGroup.childControlWidth = true;
        verticalLayoutGroup.childControlHeight = false;
        verticalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
    }

    public void SetSpacing(float spacing)
    {
        if (verticalLayoutGroup == null)
            return;

        verticalLayoutGroup.spacing = spacing;
    }

    private void LateUpdate()
    {
        float width = 0;
        float height = 0;

        foreach (var child in children)
        {
            height += child.rectTransform.sizeDelta.y + style.spacing; 
            if(child.rectTransform.sizeDelta.x > width)
            {
                width = child.rectTransform.sizeDelta.x;
            }
        }

        height -= style.spacing;

        rectTransform.sizeDelta = new Vector2(
            style.fitWidth ? width : rectTransform.sizeDelta.x,
            style.fitHeight ? height : rectTransform.sizeDelta.y
        );
    }
}
