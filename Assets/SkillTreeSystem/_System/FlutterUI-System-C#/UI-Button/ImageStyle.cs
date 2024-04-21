using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "UI/Style/Image")]
public class ImageStyle : UIStyle
{
    public string url;
    public Color color;

    public float width;
    public float height;

    public bool fitWidth;
    public bool fitHeight;
}
