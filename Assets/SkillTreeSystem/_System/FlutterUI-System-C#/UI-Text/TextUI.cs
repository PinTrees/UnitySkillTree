using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class xTextStyle
{
    public int fontSize;
    public Font font;
    public Color color;
    public bool overflow;
    public bool bold;

    public xTextStyle(
        int fontSize=24,
        Font font=null,
        Color? color=null,
        bool overflow=false,
        bool bold=false
    )
    {
        this.fontSize = fontSize;
        this.font = font;
        this.overflow = overflow;
        this.bold = bold;

        if(color == null)
        {
            this.color = Color.black;
        }
        else
        {
            this.color = color.Value;
        }
    }
}


public class TextUI : UIObjectBase
{
    Text text;


    public static TextUI Create(TextStyle style, GameObject parent=null, string text="")
    {
        var go = new GameObject("TextUI");

        if(parent != null)
        {
            go.transform.SetParent(parent.transform, true);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
        }

        var ui = go.AddComponent<TextUI>();
        ui.text = ui.AddComponent<Text>();
        ui.text.font = style.font;
        ui.text.fontSize = style.fontSize;
        ui.text.color = style.textColor;
        ui.text.raycastTarget = false;
        
        if(ui.text.font == null)
        {
            ui.text.font = UIStyleHub.Instance.font;
        }

        if(style.overflow)
        {
            ui.text.horizontalOverflow = HorizontalWrapMode.Overflow;
            ui.text.verticalOverflow = VerticalWrapMode.Overflow;
        }

        ui.InitializeUI();
        ui.SetText(text);
        return ui;
    }
    public static TextUI Create(xTextStyle style=null, string text="")
    {
        var go = new GameObject("TextUI");

        var ui = go.AddComponent<TextUI>();
        ui.text = ui.AddComponent<Text>();
        ui.text.font = style.font;
        ui.text.fontSize = style.fontSize;
        ui.text.color = style.color;
        
        if(style.bold)
        {
            ui.text.fontStyle = FontStyle.Bold;
        }

        if (ui.text.font == null)
        {
            ui.text.font = UIStyleHub.Instance.font;
        }

        if (style.overflow)
        {
            ui.text.horizontalOverflow = HorizontalWrapMode.Overflow;
            ui.text.verticalOverflow = VerticalWrapMode.Overflow;
        }
        else
        {
            ui.text.horizontalOverflow = HorizontalWrapMode.Wrap;
            ui.text.verticalOverflow = VerticalWrapMode.Truncate;
        }

        ui.InitializeUI();
        ui.SetText(text);
        return ui;
    }


    public override void InitializeUI()
    {
        base.InitializeUI();
    }

    public void SetText(string str)
    {
        if (text == null)
            return;

        text.text = str;
    }

    private void LateUpdate()
    {
        // 텍스트의 실제 렌더링 크기를 얻기 위해 cachedTextGenerator를 사용
        TextGenerationSettings settings = text.GetGenerationSettings(text.rectTransform.rect.size);
        float width = text.cachedTextGeneratorForLayout.GetPreferredWidth(text.text, settings) / text.pixelsPerUnit;
        float height = text.cachedTextGeneratorForLayout.GetPreferredHeight(text.text, settings) / text.pixelsPerUnit;

        // RectTransform의 크기를 텍스트의 실제 크기로 설정
        text.rectTransform.sizeDelta = new Vector2(width, height);
    }
}
