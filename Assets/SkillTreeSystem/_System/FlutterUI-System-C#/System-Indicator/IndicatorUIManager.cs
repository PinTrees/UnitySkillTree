using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// UI Indicator View 클래스는 인디케이터 UI 요소를 관리합니다.
/// </summary>
public class IndicatorUIManager : Singleton<IndicatorUIManager>
{
    public Canvas canvas;                                   // Canvas 객체 참조.
    private List<IndicatorUIBase> indicators = new();       // 활성화된 인디케이터의 목록.

    private readonly object indicatorsLock = new object();  // 인디케이터 목록에 대한 스레드 안전을 위한 잠금 객체.
    private volatile bool updateLock = false;               // 업데이트가 진행 중인지 여부를 나타내는 플래그.


    protected override void Awake()
    {
        if(null == canvas)
        {
            canvas = FindAnyObjectByType<Canvas>();
        }
    }

    public void Update()
    {
        if (updateLock) return;

        lock (indicatorsLock)
        {
            foreach (var indicator in indicators)
            {
                indicator?.UpdateIndicator();
            }
        }
    }


    /// <summary>
    /// 새 인디케이터를 추가합니다.
    /// </summary>
    /// <param name="indicator">추가할 인디케이터.</param>
    public void AddIndicator(IndicatorUIBase indicator)
    {
        lock (indicatorsLock)
        {
            updateLock = true;

            indicator.SetCanvasRectTransform(canvas.GetComponent<RectTransform>());
            indicator.transform.SetParent(canvas.transform, true);
            indicator.transform.localScale = Vector3.one;
            indicators.Add(indicator);

            updateLock = false;
        }
    }

    /// <summary>
    /// 인디케이터를 제거합니다.
    /// </summary>
    /// <param name="indicator">제거할 인디케이터.</param>
    public void RemoveIndicator(IndicatorUIBase indicator)
    {
        lock (indicatorsLock)
        {
            updateLock = true;

            indicators.Remove(indicator);

            updateLock = false;
        }
    }
}