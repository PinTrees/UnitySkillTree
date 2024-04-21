using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "UI/Style/DropdownMenu")]
public class DropdownMenuStyle : UIStyle
{
    public DropdownMenuButtonStyle dropdownMenuButtonStyle;

    [Space]
    public Sprite backgroundImage;
    public Color backgroundColor;
    public float width;
    public float heightSpacing;

    public bool fitHeight;
}