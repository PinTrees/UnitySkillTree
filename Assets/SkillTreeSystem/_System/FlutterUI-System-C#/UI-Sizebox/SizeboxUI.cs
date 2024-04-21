using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SizeboxUI : UIObjectBase
{
    public float width;
    public float height;

    public bool expandWidth;
    public bool expandHeight;   

    public static SizeboxUI Create(float width=2, float height=2, bool expandWidth=false, bool expandHeight=false)
    {
        var go = new GameObject("SizeboxUI");

        var ui = go.AddComponent<SizeboxUI>();
        ui.width = width;
        ui.height = height;
        ui.expandWidth = expandWidth;
        ui.expandHeight = expandHeight;

        ui.InitializeUI();
        return ui;
    }


    public override void InitializeUI()
    {
        base.InitializeUI();
    }

    private void LateUpdate()
    {
        rectTransform.sizeDelta = new Vector2(width, height);

        if(parent != null)
        {
            rectTransform.sizeDelta = new Vector2(
                expandWidth ? parent.rectTransform.sizeDelta.x : width,
                expandHeight ? parent.rectTransform.sizeDelta.y : height
            );
        }
    }
}
