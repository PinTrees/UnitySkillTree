using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "UI/Style/Background")]
public class BackgroundStyle : UIStyle
{
    public Sprite backgroundImage;
    public Color foregroundColor;

    public bool modal;
}
