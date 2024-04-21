// Designed by YM, 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*
 * drag content
 * zoom content
 * fit to content size
 * 
 * maskable content
 */
[RequireComponent(typeof(Image))]
public class ImageViewer : UIObjectBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public ContentSizeFillterExtention contentSizeFillter;
    public Image backgroundImage;

    [Header("Scroll Setting")]
    public bool fixedRoot;
    public bool zoom;
    public float zoomSpeed = 0.1f; 
    public Vector2 zoomLimit = new Vector2(0.5f, 2.0f);


    private void Awake()
    {
        base.InitializeUI();

        if(contentSizeFillter == null)
        {
            contentSizeFillter = GetComponentInChildren<ContentSizeFillterExtention>();
        }

        contentSizeFillter.InitializeUI();

        if(fixedRoot)
        {
            contentSizeFillter.transform.localPosition = Vector3.zero;
        }
        else
        {
            ContentSizeFillterExtention.SetFitChildRectSize(rectTransform, contentSizeFillter.rectTransform);
        }

        contentSizeFillter.UpdateContentSizeRect();
    }

    public void SetupZoom(float low, float max)
    {
        zoomLimit = new Vector2(low, max);
    }

    private void Update()
    {
        if(zoom)
        {
            ZoomUpdate();
        }
    }


    private void ZoomUpdate()
    {
        // if canvas render type == screen space - overay -> camera param = null;
        // else canvas render type == camera -> camera param = render target camera;
        if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition, null))
        {
            return;
        }

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");

        if(Mathf.Abs(mouseScroll) <= 0.01f)
        {
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(contentSizeFillter.rectTransform, Input.mousePosition, rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : rootCanvas.worldCamera, out Vector2 localPoint);

        Vector3 scaleChange = Vector3.one * (1 + mouseScroll * zoomSpeed);
        Vector3 newScale = contentSizeFillter.transform.localScale * scaleChange.x; // Uniform scale assuming x and y are scaled equally

        // 새 스케일 한계값 적용
        newScale.x = Mathf.Clamp(newScale.x, zoomLimit.x, zoomLimit.y);
        newScale.y = Mathf.Clamp(newScale.y, zoomLimit.x, zoomLimit.y);
        newScale.z = 1f; // Z 축 스케일 변경하지 않음

        // 이미 최소, 최대화 상태일 경우 리턴
        if(contentSizeFillter.transform.localScale.x == newScale.x)
        {
            return;
        }

        contentSizeFillter.transform.localScale = newScale;

        // 위치 보정 로직
        Vector2 delta = localPoint * (scaleChange.x - 1);
        contentSizeFillter.rectTransform.anchoredPosition -= delta;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (contentSizeFillter == null)
            return;
        if (contentSizeFillter.rectTransform == null)
            return;

        contentSizeFillter.rectTransform.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (contentSizeFillter == null)
            return;
        if (contentSizeFillter.rectTransform == null)
            return;
    }
}

#if UNITY_EDITOR

#endif