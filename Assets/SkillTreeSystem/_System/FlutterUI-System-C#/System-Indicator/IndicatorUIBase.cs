// Designed by YM, 2024

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// IndicatorUIBase class is responsible for displaying and updating an indicator UI element.
/// This class manages the indicator's position and appearance based on the target object's position,
/// both when it is within the camera's view and when it is off-screen.
/// 
/// IndicatorUIBase 클래스는 UI 요소를 표시하고 업데이트하는 역할을 담당합니다.
/// 이 클래스는 타겟 오브젝트의 위치에 따라 UI의 위치와 회전을 관리합니다.
/// </summary>
public class IndicatorUIBase : UIObjectBase
{
    [SerializeField]
    //private float outOfSightOffset = 20f;   // 화면 밖 대상을 표시할 때 사용되는 오프셋 값
    private RectTransform canvasRect;       // UI 캔버스의 RectTransform
    public void SetCanvasRectTransform(RectTransform canvasRectTransform) => canvasRect = canvasRectTransform;

    [Header("runtime debug field")]
    [SerializeField]
    private GameObject target;              // 추적할 대상
    private bool isOutOfScreen = false;     // 대상이 화면 밖에 있는지 여부

    // 대상의 화면 상태에 따라 호출될 이벤트
    public Action onScreenInActive;                 // 화면 내에 진입했을 경우 호출
    public Action onScreenInUpdate;                 // 화면 내에 있을 경우 매 업데이트 프레임마다 호출
    public Action onScreenOutActive;                // 화면 밖으로 진입했을 경우 호출
    public Action<Quaternion> onScreenOutUpdate;    // 화면 밖에 있을 경우 매 업데이트 프레임마다 호출
    public Action onScreenInSelect;                 // -----


    /*
     * override initialize
     */
    public override void InitializeUI()
    {
        base.InitializeUI();
    }


    /*
     * set indicator target
     */
    public void SetTargetObject(GameObject targetObject)
    {
        this.target = targetObject;
    }

    /* Show, CLose - ui object base methode */
    #region UI Base Override
    public override void ShowUI()
    {
        base.ShowUI();

        IndicatorUIManager.Instance.AddIndicator(this);
    }

    public override void CloseUI()
    {
        base.CloseUI();

        IndicatorUIManager.Instance.RemoveIndicator(this);
    }
    #endregion



    /*
     * update indicator position
     */
    public void UpdateIndicator()
    {
        if (target == null) return;

        Vector3 indicatorPosition = CalculateIndicatorPosition();
        rectTransform.position = indicatorPosition;

        UpdateIndicatorEvent(indicatorPosition);
    }

    /* 
     * 주어진 화면 위치를 바탕으로 타겟이 카메라 화면 안에 있는지 여부를 판단합니다.
     */
    private bool IsTargetVisibleOnScreen(Vector3 screenPosition)
    {
        return screenPosition.z >= 0f &&
               screenPosition.x <= canvasRect.rect.width * canvasRect.localScale.x &&
               screenPosition.y <= canvasRect.rect.height * canvasRect.localScale.y &&
               screenPosition.x >= 0f &&
               screenPosition.y >= 0f;
    }




    #region Calculate UI Poisition
    /*
     * 타겟의 위치에 따라 UI의 화면 위치를 계산합니다.
     * 타겟이 화면 안에 있을 경우, 그 위치를 반환합니다.
     * 화면 밖에 있을 경우, 화면 가장자리에 위치한 지표의 위치를 계산합니다.
     */
    private Vector3 CalculateIndicatorPosition()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);

        if (IsTargetVisibleOnScreen(screenPosition))
        {
            screenPosition.z = 0f; 
            return screenPosition;
        }
        else
        {
            screenPosition.z = -1f;
            return CalculateOffScreenPosition(screenPosition);
        }
    }

    /*
     * Calculates the position on screen for an off-screen target. If the target is off-screen,
     * places an indicator at the nearest screen edge towards the target.
     */
    private Vector3 CalculateOffScreenPosition(Vector3 screenPosition)
    {
        // Ensure Z position is at 0
        screenPosition.z = 0f;

        // Convert screen position to be relative to canvas center
        Vector3 canvasCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 relativePosition = screenPosition - canvasCenter;

        // Calculate the multiplier to determine how far to place the UI element from the center
        float maxX = Screen.width / 2f;
        float maxY = Screen.height / 2f;
        float screenRatio = Screen.width / Screen.height;
        float targetRatio = Mathf.Abs(relativePosition.x) / Mathf.Abs(relativePosition.y);

        Vector3 edgePosition;
        if (targetRatio > screenRatio)
        {
            // Place on left or right
            float x = Mathf.Sign(relativePosition.x) * maxX;
            float y = relativePosition.y * maxX / Mathf.Abs(relativePosition.x);
            edgePosition = new Vector3(x, y, 0f) + canvasCenter;
        }
        else
        {
            // Place on top or bottom
            float x = relativePosition.x * maxY / Mathf.Abs(relativePosition.y);
            float y = Mathf.Sign(relativePosition.y) * maxY;
            edgePosition = new Vector3(x, y, 0f) + canvasCenter;
        }

        return RotateIndicatorTowardsTarget(edgePosition, relativePosition);
    }

    /*
     * Rotates the UI indicator to face towards the off-screen target.
     */
    private Vector3 RotateIndicatorTowardsTarget(Vector3 indicatorPosition, Vector3 targetDirection)
    {
        // Calculate the angle to rotate the indicator
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        // Apply rotation to the indicator
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        return indicatorPosition;
    }
    #endregion




    /// <summary>
    /// 화면 상의 지표 위치에 따라 상태를 업데이트합니다.
    /// 지표가 화면 내에 있는지 또는 화면 밖에 있는지에 따라 다른 이벤트를 호출합니다.
    /// </summary>
    /// <param name="indicatorPosition"></param>
    private void UpdateIndicatorEvent(Vector3 indicatorPosition)
    {
        if (IsTargetVisibleOnScreen(indicatorPosition))
        {
            if (!isOutOfScreen)
            {
                onScreenInUpdate?.Invoke();
            }
            else
            {
                onScreenInActive?.Invoke();
                isOutOfScreen = false;
            }
        }
        else
        {
            if (isOutOfScreen)
            {
                Vector3 direction = (indicatorPosition - new Vector3(canvasRect.rect.width / 2f, canvasRect.rect.height / 2f, 0f)).normalized;
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
                onScreenOutUpdate?.Invoke(rotation);
            }
            else
            {
                onScreenOutActive?.Invoke();
                isOutOfScreen = true;
            }
        }
    }
}
