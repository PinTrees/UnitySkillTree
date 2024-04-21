using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WindowBody : UIObjectBase
{
    public MdiWindow parentWindow { get; private set; }
    

    public static WindowBody CreateBody(MdiWindow baseWindow)
    {
        var go = new GameObject("WindowBody");
        go.AddComponent<RectTransform>();

        var windowBody = go.AddComponent<WindowBody>();
        baseWindow.AddChild(windowBody);

        windowBody.parentWindow = baseWindow;

        windowBody.InitializeUI();
        windowBody.SetUp();

        return windowBody;
    }

    protected override void SetUp()
    {
        base.SetUp();
        // 앵커와 피봇 설정
        rectTransform.anchorMin = new Vector2(0, 0); // 왼쪽 하단
        rectTransform.anchorMax = new Vector2(1, 1); // 오른쪽 상단
        rectTransform.pivot = new Vector2(0.5f, 0.5f); // 중앙 기준
    }

    void Update()
    {
        if (parentWindow == null)
            return;

        // 타이틀 바의 높이 가져오기
        float titleBarHeight = parentWindow.GetTitleBarHeight();

        // 부모 창의 사이즈를 가져오기
        Vector2 parentSize = parentWindow.rectTransform.rect.size;

        // bodyRectTransform 사이즈와 위치 조정
        // offsetMin과 offsetMax를 사용해 상단을 타이틀 바 높이만큼 비우고, 나머지는 부모 사이즈에 맞춤
        rectTransform.offsetMin = new Vector2(0, 0); // 하단과 왼쪽 경계
        rectTransform.offsetMax = new Vector2(0, -titleBarHeight); // 상단 경계를 타이틀 바 높이만큼 설정
    }
}
