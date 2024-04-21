using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MdiWindow : UIObjectBase
{
    private MdiWindowData data;

    [Header("runtime value")]
    public WindowTitleBar titleBar { get; private set; }
    public WindowBody body { get; private set; }

    public Image backgroundImage;
    public Color backgroundColor;


    public static MdiWindow _(MdiWindowData data)
    {
        var go = new GameObject("Mdi-Window");
        var mdiWindow = go.AddComponent<MdiWindow>();
        mdiWindow.AddComponent<RectTransform>();
        mdiWindow.data = data;
        mdiWindow.Init();

        mdiWindow.SetTitleBarHeight(data.windowTitlebarHeight);
        mdiWindow.SetWindowScale(data.windowSize);
        mdiWindow.SetWindowPosition(data.basePosition);

        return mdiWindow;
    }

    protected override void Start()
    {
        base.Start();
        Init();
    }

    public void Init()
    {
        base.InitializeUI();
        MdiController.Instance.AddMdiWindow(this);

        if (body == null)
        {
            body = WindowBody.CreateBody(this);
        }

        if (titleBar == null)
        {
            titleBar = GetComponentInChildren<WindowTitleBar>();

            if (titleBar == null)
            {
                titleBar = WindowTitleBar._(this);
            }
        }

        if (backgroundImage == null)
        {
            backgroundImage = gameObject.AddComponent<Image>();
            backgroundImage.raycastTarget = true;

            SetBackgroundColor(Color.grey);
        }
    }


    public void SetWindowScale(Vector2 scale)
    {
        rectTransform.sizeDelta = scale;
    }
    public void SetWindowPosition(Vector2 position)
    {
        rectTransform.anchoredPosition = position;
    }
    public void SetTitleBarHeight(float height)
    {
        titleBar.SetHeight(height);
    }
    public float GetTitleBarHeight() 
    {
        return titleBar.titleBarHeight;
    }

    public void SetBackgroundColor(Color color)
    {
        if (backgroundImage == null)
            return;

        backgroundColor = color;
        backgroundImage.color = color;
    }


    public void LateUpdate()
    {
        if (data == null)
            return;

        if(data.fitChildWidth)
        {

        }
    }
}
