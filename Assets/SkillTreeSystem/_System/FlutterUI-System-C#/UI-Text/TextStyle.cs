using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "UI/Style/TextStyle")]
public class TextStyle : UIStyle
{
    public Font font;
    public int fontSize = 24;
    public Color textColor = Color.black;

    public bool overflow = true;
}
