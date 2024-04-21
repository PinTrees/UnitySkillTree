using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "UI/Style/Container")]
public class ContainerStyle : UIStyle
{
    public Sprite backgroundImage;
    public Color backgroundImageColor;

    public float width;
    public float height;

    public bool fitWidth;
    public bool fitHeight;
}
