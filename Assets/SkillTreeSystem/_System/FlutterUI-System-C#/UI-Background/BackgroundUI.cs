using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BackgroundUI : UIObjectBase
{
    public Image backgroundImage;
    public Button button;

    BackgroundStyle style;


    public static BackgroundUI Create(BackgroundStyle style)
    {
        var canvas = GameObject.FindAnyObjectByType<Canvas>();
        var go = new GameObject("BackgroundUI");
        go.transform.SetParent(canvas.transform, true);
        go.transform.SetAsLastSibling();
        go.transform.localPosition = Vector3.zero;

        var ui = go.AddComponent<BackgroundUI>();
        ui.style = style;
        ui.backgroundImage = UICreator.CreateImageComponent(go);
        ui.backgroundImage.color = style.foregroundColor;

        ui.InitializeUI();
        ui.rectTransform.sizeDelta = canvas.pixelRect.size;

        return ui;  
    }


    public override void InitializeUI()
    {
        base.InitializeUI();
    }
}
