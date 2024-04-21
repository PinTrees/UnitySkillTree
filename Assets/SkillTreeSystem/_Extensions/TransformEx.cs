using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformEx 
{
    public static void SetLayerAll(this Transform transform, int layer)
    {
        if (transform == null)
        {
            return;
        }

        transform.gameObject.layer = layer;

        foreach (Transform child in transform)
        {
            SetLayerAll(child, layer);
        }
    }

    public static void SetZeroLocalPositonAndRotation(this Transform transform)
    {
        if (transform == null) return;

        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }

    public static void SetTransformForTarget(Transform parent, Transform child, Transform target)
    {
        parent.SetParent(target, true);
        parent.SetZeroLocalPositonAndRotation();

        var dir = (child.position - parent.position).normalized;
        var dit = Vector3.Distance(parent.position, child.position);

        parent.position -= dir * dit; 
    }
}
