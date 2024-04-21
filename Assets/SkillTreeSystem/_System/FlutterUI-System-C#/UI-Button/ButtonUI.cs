using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonUI : UIObjectBase
{
    Image backgroundImage;
    Image iconImage;
    Button button;

    public static ButtonUI Create()
    {
        var go = new GameObject("Button");
        go.AddComponent<RectTransform>();
        go.AddComponent<CanvasRenderer>();

        var buttonUI = go.AddComponent<ButtonUI>();
        buttonUI.InitializeUI();
        buttonUI.SetUp();

        return buttonUI;
    }

    protected override void SetUp()
    {
        base.SetUp();

        backgroundImage = gameObject.AddComponent<Image>();
        iconImage = UICreator.CreateNewImageObject(gameObject);
       
        SetIconSize(new Vector2(32, 32));

        button = gameObject.AddComponent<Button>();
        button.targetGraphic = backgroundImage;
    }


    public void SetIconSize(Vector2 size)
    {
        if (iconImage == null)
            return;

        iconImage.rectTransform.sizeDelta = size;
    }
    public void SetWidth(float width)
    {
        rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
    }
    public void SetHeight(float height)
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
    }
    public void SetColor(Color color)
    {
        if (backgroundImage == null)
            return;

        backgroundImage.color = color;
    }
    public void SetOnClick(Action action)
    {
        if (button == null)
            return;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => { action(); });
    }
}

