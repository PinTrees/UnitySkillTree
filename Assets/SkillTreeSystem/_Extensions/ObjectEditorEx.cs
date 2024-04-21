using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectEditorEx 
{
    public static void DestroyMonoObject(MonoBehaviour mono)
    {
        if(mono == null)
        {
            return;
        }

        GameObject.DestroyImmediate(mono.gameObject);
    }

    public static void Destroy(GameObject go)
    {
        if (go == null)
        {
            return;
        }

        GameObject.DestroyImmediate(go);
    }
}
