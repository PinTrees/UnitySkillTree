using System.Collections;
using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CanvasRenderer))]
public class ImageBezior : MaskableGraphic
{
    public Vector3 startPoint = new();
    public Vector3 controlPoint = new();
    public Vector3 endPoint = new();

    public float lineThickness = 5f; // 선의 두께를 정의합니다.
    public int segmentCount = 20;

    public Sprite sprite;

    public override Texture mainTexture
    {
        get
        {
            return sprite == null ? s_WhiteTexture : sprite.texture;
        }
    }

    public void Initailize()
    {
        raycastTarget = false;
    }

    protected override void UpdateMaterial()
    {
        base.UpdateMaterial();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        float step = 1.0f / segmentCount;
        Vector2 prevPoint = CalculateBezierPoint(0, startPoint, controlPoint, endPoint);
        Vector2 prevNormal = Vector2.zero;
        Vector2 uvStep = new Vector2(1f / segmentCount, 1);

        for (int i = 1; i <= segmentCount; i++)
        {
            float t = i * step;
            Vector2 pointOnCurve = CalculateBezierPoint(t, startPoint, controlPoint, endPoint);
            Vector2 diff = pointOnCurve - prevPoint;
            Vector2 normal = new Vector2(-diff.y, diff.x).normalized * lineThickness;

            Vector2 uv = new Vector2(uvStep.x * i, 0);
            Vector2 uvNext = new Vector2(uvStep.x * i, 1);

            if (i == 1)
            {
                vh.AddVert(prevPoint - normal, color, new Vector2(uv.x, 0)); // 하단
                vh.AddVert(prevPoint + normal, color, new Vector2(uv.x, 1)); // 상단
            }
            vh.AddVert(pointOnCurve - normal, color, new Vector2(uvNext.x, 0)); // 하단
            vh.AddVert(pointOnCurve + normal, color, new Vector2(uvNext.x, 1)); // 상단

            // 삼각형 추가
            int baseIndex = vh.currentVertCount - 4;
            vh.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
            vh.AddTriangle(baseIndex + 2, baseIndex + 1, baseIndex + 3);

            prevPoint = pointOnCurve;
            prevNormal = normal ;
        }
    }

    private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector2 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    public void SetSegment(int value)
    {
        segmentCount = value;

        SetVerticesDirty();
        SetMaterialDirty();
    }

    public void SetThickness(float value)
    {
        lineThickness = value;

        SetVerticesDirty();
        SetMaterialDirty();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ImageBezior))]
public class ImageBeziorEditor : Editor 
{
    private void OnSceneGUI()
    {
        ImageBezior image = target as ImageBezior;

        if (image == null)
        {
            return;
        }

        // 제어점을 Scene 뷰에 표시하고 조작할 수 있도록 핸들을 그립니다.
        EditorGUI.BeginChangeCheck();

        // 제어점을 조작할 수 있도록 핸들을 그립니다.
        Vector3 newStartPos = Handles.DoPositionHandle(image.startPoint + image.transform.position, Quaternion.identity);
        Vector3 newControlPos = Handles.DoPositionHandle(image.controlPoint + image.transform.position, Quaternion.identity);
        Vector3 newEndPos = Handles.DoPositionHandle(image.endPoint + image.transform.position, Quaternion.identity);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(image, "Move Bezior Point");
            image.startPoint = newStartPos - image.transform.position;
            image.controlPoint = newControlPos - image.transform.position;
            image.endPoint = newEndPos - image.transform.position;

            // 제어점이 변경될 때 OnPopulateMesh를 재구축하도록 더티 상태 설정
            image.SetVerticesDirty();
            image.SetMaterialDirty();
            SceneView.RepaintAll();
        }

        // 올바른 베지어 곡선을 그립니다. 쿼드래틱 베지어 곡선에 대한 적절한 큐빅 베지어 곡선 근사를 사용합니다.
        Handles.DrawBezier(
            image.startPoint + image.transform.position,
            image.endPoint + image.transform.position,
            image.startPoint + image.transform.position + 2.0f / 3.0f * (image.controlPoint - image.startPoint),
            image.endPoint + image.transform.position + 2.0f / 3.0f * (image.controlPoint - image.endPoint),
            Color.red, null, 2f);
    }
}
#endif