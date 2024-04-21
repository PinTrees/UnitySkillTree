using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class ContainerUI : UIObjectBase
{
    Image backgroundImage;
    ContainerStyle style;
    Padding padding;


    public static ContainerUI Create(ContainerStyle style, GameObject parent=null, Padding padding=null, UIObjectBase child=null)
    {
        var go = new GameObject("ContainerUI");

        if(parent != null)
        {
            go.transform.SetParent(parent.transform, true);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
        }

        var ui = go.AddComponent<ContainerUI>();
        ui.style = style;
        ui.padding = padding;

        ui.backgroundImage = ui.AddComponent<Image>();
        ui.backgroundImage.color = style.backgroundImageColor;

        ui.InitializeUI();

        if(child != null)
        {
            ui.AddChild(child);
        }

        return ui;
    }


    public override void InitializeUI()
    {
        base.InitializeUI();
    }

    private void LateUpdate()
    {
        if (child != null)
        {
            rectTransform.sizeDelta = new Vector2(
                style.fitWidth ? child.rectTransform.sizeDelta.x : style.width,
                style.fitHeight ? child.rectTransform.sizeDelta.y : style.height
            );

            if (padding != null)
            {
                padding.SetRectTransformPadding(this);
            }
        }
    }
}
