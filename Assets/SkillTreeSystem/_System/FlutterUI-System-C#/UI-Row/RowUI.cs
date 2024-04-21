using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class RowUI : UIObjectBase
{
    public HorizontalLayoutGroup horizontalLayoutGroup;
    public RowStyle style;


    public static RowUI _(RowStyle style=null, List<UIObjectBase> children=null)
    {
        var go = new GameObject("RowUI");
        go.AddComponent<RectTransform>();

        var ui = go.AddComponent<RowUI>();
        ui.style = style ?? new();
      
        ui.InitializeUI();
        ui.SetUp();

        if (children != null)
        {
            ui.AddChildren(children);
        }

        return ui;
    }

    protected override void SetUp()
    {
        base.SetUp();

        horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
        horizontalLayoutGroup.childControlWidth = false;

        SetSpacing(style.spacing);

        if (style.fitHeight)
            horizontalLayoutGroup.childControlHeight = false;

        horizontalLayoutGroup.childAlignment = TextAnchor.MiddleRight;
    }

    public override void InitializeUI()
    {
        base.InitializeUI();
    }


    /*
     * set row children spacing
     */
    public void SetSpacing(float amount)
    {
        if (horizontalLayoutGroup == null)
            return;

        horizontalLayoutGroup.spacing = amount;
    }

    /*
     * set ancher position
     */
    public void SetAligmentMiddleCenter() => horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;


    private void LateUpdate()
    {
        float width = 0;
        float height = 0;

        foreach (var child in children)
        {
            width += child.rectTransform.sizeDelta.x + style.spacing;
            if (child.rectTransform.sizeDelta.y > height)
            {
                height = child.rectTransform.sizeDelta.y;
            }
        }

        width -= style.spacing;

        rectTransform.sizeDelta = new Vector2(
            style.fitWidth ? width : rectTransform.sizeDelta.x,
            style.fitHeight ? height : rectTransform.sizeDelta.y
        );
    }
}
