using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "UI/Style/Row")]
public class RowStyle : UIStyle
{
    public bool fitHeight;
    public bool fitWidth;

    public float spacing;
}
