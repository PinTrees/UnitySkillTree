using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UICreator
{
    public static Image CreateNewImageObject(GameObject parent)
    {
        var go = new GameObject("Image");
        go.transform.SetParent(parent.transform, true);
        go.transform.localScale = Vector3.one; 
        go.transform.localPosition = Vector3.zero;

        var image = go.AddComponent<Image>();

        return image;
    }

    public static Image CreateImageComponent(GameObject go)
    {
        var image = go.AddComponent<Image>();
        return image;
    }



    public static Button CreateButtonComponent(GameObject go)
    {
        var button = go.AddComponent<Button>();
        return button;
    }



    public static Text CreateTextObject(GameObject parent)
    {
        var go = new GameObject("Text");
        go.transform.SetParent(parent.transform, true);
        go.transform.localScale = Vector3.one;

        var text = go.AddComponent<Text>();

        return text;
    }
}
