using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ElevationImageUI : UIObjectBase
{
    ElevationIStyle style;

    Image image;


    public static ElevationImageUI Create(ElevationIStyle style)
    {
        var go = new GameObject("ElevationUI");

        var ui = go.AddComponent<ElevationImageUI>();
        ui.image = go.AddComponent<Image>();
        ui.style = style;

        ui.InitializeUI();
        ui.set_color(style.color);
        return ui;
    }


    public override void InitializeUI()
    {
        base.InitializeUI();

        image.raycastTarget = false;
        image.sprite = UISystem.Instance.get_elevationSprite();

        image.type = Image.Type.Sliced;
    }

    public void set_color(Color color)
    {
        image.color = color;
    }

    private void LateUpdate()
    {
        if(parent)
        {
            rectTransform.sizeDelta = new Vector2(
                parent.rectTransform.sizeDelta.x + style.elevation * 2,
                parent.rectTransform.sizeDelta.y + style.elevation * 2
            );
        }
    }
}
