using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * basic button ui
 * text - option
 */
public class TextButtonUI : UIObjectBase
{
    [CreateScriptable(create_func: "_Editor_TextButtonStyle")]
    public TextButtonStyle style;

    public Button button;
    public ImageUI backgorundImage;
    public ElevationImageUI elevationImage;
    public TextUI text;

    [Button("SetUp")]
    public string __editor_setup;


    public static TextButtonUI _(TextButtonStyle style=null, string str="")
    {
        var go = new GameObject("TextButtonUI");
        go.AddComponent<RectTransform>();

        var ui = go.AddComponent<TextButtonUI>();
        ui.style = style ?? new TextButtonStyle();
        ui.SetUp();

        return ui;
    }

    public override void InitializeUI()
    {
        base.InitializeUI();
    }

    private void LateUpdate()
    {
        rectTransform.sizeDelta = new Vector2(style.width, style.height);
    }

    public void SetHeight(float h)
    {
        style.height = h;
        rectTransform.sizeDelta = new Vector2(style.width, style.height);
        backgorundImage.SetHeight(h);
    }
    public void SetWidth(float w)
    {
        style.width = w;
        rectTransform.sizeDelta = new Vector2(style.width, style.height);
        backgorundImage.SetWidth(w);
    }
    public void SetBackgroundImageColor(Color color)
    {
        backgorundImage.SetColor(color);
    }
    public void SetTargetGraphic(Image target)
    {
        button.targetGraphic = target;
    }

    protected override void SetUp()
    {
        InitializeUI();

        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }

        if (backgorundImage == null)
        {
            var imageStyle = new ImageStyle();
            imageStyle.width = style.width;
            imageStyle.height = style.height;

            backgorundImage = ImageUI._(imageStyle);
            AddChild(backgorundImage);
        }

        button.targetGraphic = backgorundImage.GetImage();
    }

#if UNITY_EDITOR
    private TextButtonStyle _Editor_TextButtonStyle() => new();
#endif
}
