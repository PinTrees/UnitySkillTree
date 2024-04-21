// Designed by YM, 2024

using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*
 * ContentSizeFilter - custom
 */
public class ContentSizeFillterExtention : UIObjectBase
{
    [SerializeField]
    private Padding padding;

    [Space]
    [Button("UpdateContentSizeRect")]
    public string __editor_update_rect;

    public static ContentSizeFillterExtention _()
    {
        var go = new GameObject("ContentSizeFillterExtention");
        go.AddComponent<RectTransform>();

        var ui = go.AddComponent<ContentSizeFillterExtention>();
        ui.InitializeUI();

        return ui;
    }

    private void Awake()
    {
        InitializeUI();
    }

    public override void InitializeUI()
    {
        base.InitializeUI();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
    }

    public void UpdateContentSizeRect()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        List<RectTransform> rectTransforms = new();

        if(rectTransform.childCount == 0)
        {
            rectTransform.sizeDelta = Vector2.zero;
            return;
        }

        for (int i = 0; i < transform.childCount; ++i)
            rectTransforms.Add(transform.GetChild(i).GetComponent<RectTransform>());

        rectTransforms.ForEach(e => e.SetParent(null, true));

        // �̰����� �ڽ��� ũ�⸸ŭ ���� ��ƮƮ����������ġ�� ũ�⸦ �缳��
        // �ڽ��� ��踦 �������� �ּ� �� �ִ� ��ġ�� ����մϴ�.
        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;

        foreach (RectTransform child in rectTransforms)
        {
            Vector2 childPosition = child.position;
            Vector2 size = child.rect.size;
            Vector2 min = childPosition - (size * 0.5f);
            Vector2 max = childPosition + (size * 0.5f);

            minX = Mathf.Min(minX, min.x);
            minY = Mathf.Min(minY, min.y);
            maxX = Mathf.Max(maxX, max.x);
            maxY = Mathf.Max(maxY, max.y);
        }

        // �θ� RectTransform�� ���ο� ��ġ�� ũ�⸦ ����մϴ�.
        Vector2 center = new Vector2((minX + maxX) / 2, (minY + maxY) / 2);
        Vector2 sizeDelta = new Vector2(maxX - minX, maxY - minY);

        // �θ� RectTransform�� ������Ʈ�մϴ�.
        rectTransform.position = center;
        rectTransform.sizeDelta = sizeDelta;

        rectTransforms.ForEach(e => e.SetParent(transform, true));
    }

    /*
     * set parent rect 
     * fit child rect size
     */
    public static void SetFitChildRectSize(RectTransform origin, RectTransform child)
    {
        child.SetParent(null, true);

        origin.transform.position = child.position;
        origin.transform.rotation = child.rotation;

        origin.sizeDelta = child.sizeDelta;

        child.SetParent(origin, true);
    }
}
