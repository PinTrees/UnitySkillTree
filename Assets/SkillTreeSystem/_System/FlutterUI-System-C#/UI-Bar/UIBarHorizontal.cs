using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// UIBarHorizontal은 수평 바 UI 요소를 나타냅니다.
/// Setting 클래스를 SO로 분리하여 스타일 모듈화가 가능하도록 재구축
/// </summary>
public class UIBarHorizontal : UIObjectBase
{
    [SerializeField] private Image bar;

    /// <summary>
    /// 초기 설정을 합니다. raycastTarget을 비활성화하고 바의 채우기 방식을 설정합니다.
    /// </summary>
    public override void InitializeUI()
    {
        // 기반 클래스의 Init 메서드를 호출할 경우 아래 주석을 해제하세요.
        base.InitializeUI();

        if (bar == null)
        {
            Debug.LogError("Bar image is not assigned!");
            return;
        }

        bar.raycastTarget = false;
        bar.type = Image.Type.Filled;
        bar.fillMethod = Image.FillMethod.Horizontal;
        bar.fillOrigin = 1;
    }

    /// <summary>
    /// 바의 색상을 설정합니다.
    /// </summary>
    /// <param name="color">바에 적용할 색상입니다.</param>
    public void SetColor(Color color)
    {
        if (bar != null)
        {
            bar.color = color;
        }
    }

    /// <summary>
    /// 바의 채우기 양을 설정합니다.
    /// </summary>
    /// <param name="amount">바의 채우기 양 (0.0에서 1.0 사이의 값).</param>
    public void SetFillAmount(float amount)
    {
        if (bar != null)
        {
            bar.fillAmount = Mathf.Clamp(amount, 0f, 1f);
        }
    }


    #region Show, Close Methode
    /// <summary>
    /// 바를 표시합니다. 기본적으로 바의 채우기 양은 0입니다.
    /// </summary>
    /// <param name="amount">바를 시작할 때의 채우기 양입니다.</param>
    public virtual void Show(float amount = 0)
    {
        // 기반 클래스의 Show 메서드를 호출할 경우 아래 주석을 해제하세요.
        // base.Show();
        SetFillAmount(amount);
    }

    /// <summary>
    /// 바를 닫습니다.
    /// </summary>
    public override void CloseUI()
    {
        // 기반 클래스의 Close 메서드를 호출할 경우 아래 주석을 해제하세요.
        // base.Close();
    }
    #endregion
}
