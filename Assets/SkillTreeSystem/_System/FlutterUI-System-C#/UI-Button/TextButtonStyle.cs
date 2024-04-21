using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "UI/Style/TextButton")]
public class TextButtonStyle : UIStyle
{
    [SerializeField] public TextStyle textStyle;
    [SerializeField] public ElevationIStyle elevationStyle = new();

    public Sprite backgroundImage;
    public Color backgroundImageColor;

    public float width;
    public float height;
}
