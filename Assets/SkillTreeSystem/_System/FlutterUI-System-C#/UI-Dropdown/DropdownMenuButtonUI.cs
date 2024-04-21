using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class DropdownMenuButtonStyle
{
    public Sprite backgroundImage;
    public Color backgroundColor;
    public float height;
    public int textFontSize;
}
public class DropdownMenuButtonParam
{
    public Sprite iconImage;
    public string text;
    public Action onClick;

    public DropdownMenuButtonParam(string text, Action onClick)
    {
        this.text = text;
        this.onClick = onClick;
    }
}


public class DropdownMenuButtonUI : RowUI
{
    public Image backgroundImage;
    public Button button;
    public Text text;
    public float height;

    public DropdownMenuUI owner;

    public static DropdownMenuButtonUI Create(DropdownMenuUI owner, DropdownMenuButtonStyle style, DropdownMenuButtonParam param)
    {
        var go = new GameObject("DropdownMenuButtonUI");
        go.transform.SetParent(owner.transform, true);
        go.transform.localScale = Vector3.one;

        var ui = go.AddComponent<DropdownMenuButtonUI>();

        ui.owner = owner;
        ui.backgroundImage = UICreator.CreateImageComponent(go);
        ui.backgroundImage.sprite = style.backgroundImage;
        ui.backgroundImage.color = style.backgroundColor;
        ui.button = UICreator.CreateButtonComponent(go);
        ui.button.targetGraphic = ui.backgroundImage;

        ui.text = UICreator.CreateTextObject(go);
        ui.text.font = UIStyleHub.Instance.font;
        ui.text.text = param.text;
        ui.text.alignment = TextAnchor.MiddleCenter;
        ui.text.horizontalOverflow = HorizontalWrapMode.Overflow;
        ui.text.verticalOverflow = VerticalWrapMode.Overflow;
        ui.text.fontSize = style.textFontSize;

        ui.InitializeUI();
        ui.SetHeight(style.height);
        ui.SetOnClick(param.onClick);
        ui.SetAligmentMiddleCenter();

        return ui;
    }


    public override void InitializeUI()
    {
        base.InitializeUI();
    }

    public void SetHeight(float height)
    {
        this.height = height;
        rectTransform.sizeDelta = new Vector2(0, height);
    }
    public void SetOnClick(Action action)
    {
        if (button == null)
            return;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        { 
            action();
            dropdownmenu_manager.Instance.Clear();
        });
    }
}
