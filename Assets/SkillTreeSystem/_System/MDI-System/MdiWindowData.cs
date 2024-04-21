using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "UI/MdiWindowData")]
public class MdiWindowData : ScriptableObject
{
    public Sprite windowTilebarBackgroundImage;
    public Sprite windowBodyBackgroundImage;
    public Sprite windowBackgroundImage;
    public Sprite windowFrameImage;

    public Sprite windowCloseButtonBackgroundImage;
    public Sprite windowCloseButtonIconImage;

    public Vector2 windowSize = new Vector2(500, 500);
    public Vector2 basePosition = new Vector2(0, 0);
    public float windowTitlebarHeight = 48.0f;

    public bool resizeable;
    public bool fitChildWidth;
    public bool fitChildHeight;
}
