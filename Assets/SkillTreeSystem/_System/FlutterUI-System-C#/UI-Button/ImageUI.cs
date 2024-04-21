using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ImageUI : UIObjectBase
{
    public Image image;

    /*
     * style
     */
    [Header("Style")]
    public bool expand;

    ImageStyle style;


    public static ImageUI _(ImageStyle style = null, bool expand=false)
    {
        var go = new GameObject("ImageUI");
        go.AddComponent<RectTransform>();

        var ui = go.AddComponent<ImageUI>();

        ui.expand = expand;

        ui.SetUp();

        return ui;
    }


    public override void InitializeUI()
    {
        base.InitializeUI();
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
        image.color = color;
    }
    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public Image GetImage() { return image; }


    protected override void SetUp()
    {
        base.SetUp();

        if (image == null)
        {
            image = gameObject.AddComponent<Image>();
        }

        //SetWidth(style.width);
        //SetHeight(style.height);
        //SetColor(style.color);
    }


#if UNITY_EDITOR
    public override void _Editor_SelectedUpdate()
    {
        if (expand)
        {
            rectTransform.sizeDelta = parent.rectTransform.sizeDelta;
        }
    }
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(ImageUI))]
public class ImageUIEditor : Editor
{
    ImageUI owner;

    void OnEnable()
    {
        owner = (ImageUI)target;
        EditorApplication.update += OnEditorUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    void OnEditorUpdate()
    {
        if (EditorApplication.isPlaying)
        {
            return;
        }

        owner._Editor_SelectedUpdate();
    }
}
#endif
