using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryGroupUI : UIObjectBase
{
    public InventoryGroupData data;
    public Vector2Int gridTileSize = new Vector2Int(64, 64);

    List<InventoryGridUI> inventoryGridUIs = new();


    public static InventoryGroupUI Create(InventoryGroupData data)
    {
        var go = new GameObject("InventoryGroupUI");
       
        var inventoryGroupUI = go.AddComponent<InventoryGroupUI>();
        inventoryGroupUI.data = data;

        return inventoryGroupUI;
    }

    public void Awake()
    {
        if (data == null)
            return;

        base.InitializeUI();

        data.inventoryDatas.ForEach(e =>
        {
            var inventoruGridUI = InventoryGridUI.Create(this, e);
            inventoryGridUIs.Add(inventoruGridUI);
        });

        rectTransform.sizeDelta = new Vector2(
            data.groupSizeWidth * (gridTileSize.x),
            data.groupSizeHeight * (gridTileSize.y)
        );
    }

    public int GetTileSizeWidth() { return gridTileSize.x; }
    public int GetTileSizeHeight() { return gridTileSize.y; }
}
