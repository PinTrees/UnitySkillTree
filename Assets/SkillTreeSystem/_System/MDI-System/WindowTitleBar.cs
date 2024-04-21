using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class WindowTitleBar : RowUI, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public RectTransform dragableRectTransform { get; private set; }
    public MdiWindow parentWindow { get; private set; }

    public Image backgroundImage;
    public ButtonUI closeButton;

    public float titleBarHeight { get; private set; }


    // Runtime Create Window Title Bar
    public static WindowTitleBar _(MdiWindow baseWindow)
    {
        GameObject go = new GameObject("WindowTitleBar");
        var titleRect = go.AddComponent<RectTransform>();
        var windowTitlebar = go.AddComponent<WindowTitleBar>();

        windowTitlebar.style = new RowStyle();
        windowTitlebar.style.fitWidth = false;

        windowTitlebar.parentWindow = baseWindow;
        windowTitlebar.dragableRectTransform = titleRect;
        baseWindow.AddChild(windowTitlebar);

        windowTitlebar.InitializeUI();
        windowTitlebar.SetUp();

        return windowTitlebar;
    }

    protected override void SetUp()
    {
        base.SetUp();

        backgroundImage = gameObject.AddComponent<Image>();
        backgroundImage.color = new Color(0.3f, 0.3f, 0.3f);

        closeButton = ButtonUI.Create();
        closeButton.SetWidth(64);
        closeButton.SetHeight(64);
        closeButton.SetColor(Color.red);
        closeButton.SetOnClick(() =>
        {
            MdiController.Instance.RemoveMdiWindow(parentWindow);
        });

        AddChild(closeButton);

        // 앵커와 피봇을 최상단 중앙으로 설정합니다.
        rectTransform.anchorMin = new Vector2(0.5f, 1f);    // 앵커 최소를 상단 중앙으로
        rectTransform.anchorMax = new Vector2(0.5f, 1f);    // 앵커 최대도 상단 중앙으로
        rectTransform.pivot = new Vector2(0.5f, 1f);        // 피봇을 상단으로

        backgroundImage.raycastTarget = true;

        SetHeight(64);
    }

    public void SetHeight(float height)
    {
        titleBarHeight = height; // 높이 값을 설정합니다.
    }

    private void Update()
    {
        if (parentWindow == null)
            return;

        float parentWidth = parentWindow.rectTransform.rect.width;
        backgroundImage.rectTransform.sizeDelta = new Vector2(parentWidth, titleBarHeight);
        dragableRectTransform.sizeDelta = new Vector2(parentWidth, titleBarHeight);

        // dragableRectTransform의 위치를 부모 윈도우의 상단에 맞추어 조정합니다.
        dragableRectTransform.anchoredPosition = new Vector2(0, 0);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (parentWindow == null)
            return;
        if (parentWindow.rectTransform == null)
            return;

        parentWindow.rectTransform.anchoredPosition += eventData.delta / MdiController.Instance.canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (parentWindow == null)
            return;
        if (parentWindow.rectTransform == null)
            return;

        parentWindow.rectTransform.SetAsLastSibling();
    }
}
