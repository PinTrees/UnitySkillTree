using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Padding 
{
    public float right;
    public float left;  
    public float top;
    public float bottom;

    public Padding(float right, float left, float top, float bottom)
    {
        this.right = right;
        this.left = left;
        this.top = top;
        this.bottom = bottom;
    }   

    public void All(float amount)
    {
        right = amount;
        left = amount;
        top = amount;
        bottom = amount;    
    }


    public void SetRectTransformPadding(UIObjectBase ui)
    {
        // 부모 UI 요소의 sizeDelta 조정으로 패딩 적용
        RectTransform uiRectTransform = ui.rectTransform;
        // 부모의 크기를 자식 크기 + 패딩으로 조정
        uiRectTransform.sizeDelta = new Vector2(uiRectTransform.sizeDelta.x + left + right, uiRectTransform.sizeDelta.y + top + bottom);

        // 자식 UI 요소의 위치 조정
        if (ui.child != null)
        {
            RectTransform childRectTransform = ui.child.rectTransform;

            // 부모 UI의 현재 크기를 기반으로 자식의 anchoredPosition을 조정하여 가운데 위치시킴
            // 부모 크기의 절반에서 패딩을 제외한 위치로 자식을 이동시키되, 패딩이 적용되므로 실제 위치는 패딩만큼 더해진 위치가 됨
            float posX = (left - right) / 2.0f;
            float posY = (bottom - top) / 2.0f;
            childRectTransform.anchoredPosition = new Vector2(posX, posY);
        }
    }
}
