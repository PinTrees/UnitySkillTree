using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[System.Serializable]
[CreateAssetMenu(menuName = "UI/InventoryGroupData")]
public class InventoryGroupData : ScriptableObject
{
    [Range(1, 10)] public int groupSizeWidth = 1;
    [Range(1, 10)] public int groupSizeHeight = 1;

    [Header("sprite")]
    public Sprite backgroundImage;
    public Sprite tileIconImage;
    public Sprite inventoryFrameImage;

    [Header("color")]
    public Color tileColor;
    public Color backgroundColor;
    public Color frameColor;

    public List<InventoryGridData> inventoryDatas = new();
}


[System.Serializable]
public class InventoryGridData
{
    public Vector2Int offset;
    public Vector2Int inventorySize;
}



#if UNITY_EDITOR
[CustomEditor(typeof(InventoryGroupData), true)]
public class InventoryGroupDataEditor : Editor
{
    InventoryGroupData owner;

    public static Color[] inventoryEditorColors =
    {
        Color.yellow,
        Color.blue,
        Color.red,
        Color.green
    };

    public void OnEnable()
    {
        owner = target as InventoryGroupData;
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();

        GUILayout.BeginVertical();
        float maxWidth = EditorGUIUtility.currentViewWidth;

        for (int y = 0; y < owner.groupSizeHeight; ++y)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < owner.groupSizeWidth; ++x)
            {
                float gridWidht = maxWidth / (owner.groupSizeWidth) - 8;
                gridWidht = gridWidht > 100 ? 100 : gridWidht;

                var color = FindInventoryGridColor(x, y);
                if(color != null)
                {
                    GUI.backgroundColor = color.Value;
                }

                if (GUILayout.Button("", GUILayout.Width(gridWidht), GUILayout.Height(gridWidht)))
                {

                }

                if (color != null)
                {
                    GUI.backgroundColor = Color.white;
                }
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }
    }

    Color? FindInventoryGridColor(int x, int y)
    {
        for(int i = 0; i < owner.inventoryDatas.Count; ++ i)
        {
            var inventoryGrid = owner.inventoryDatas[i];

            // x, y가 inventoryGrid 내에 있는지 확인
            if (x >= inventoryGrid.offset.x && x < inventoryGrid.offset.x + inventoryGrid.inventorySize.x &&
                y >= inventoryGrid.offset.y && y < inventoryGrid.offset.y + inventoryGrid.inventorySize.y)
            {
                return inventoryEditorColors[i]; // 조건에 맞으면 해당 색상을 반환
            }
        }

        return null;
    }
}
#endif