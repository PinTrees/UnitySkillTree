using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryHighlight : MonoBehaviour
{
    private RectTransform highlighter;


    private void Awake()
    {
        var go = new GameObject("HighlighterImage");
        go.transform.SetParent(FindAnyObjectByType<Canvas>().transform, true);
        go.transform.localScale = Vector3.one;

        var image = go.AddComponent<Image>();
        image.color = new Color(1, 1, 1, 0.3f);
        image.raycastTarget = false;

        highlighter = go.GetComponent<RectTransform>();
    }

    public void Show(bool b)
    {
        highlighter.gameObject.SetActive(b);
    }

    public void SetSize(InventoryItemUI targetItem)
    {
        if (inventory_manager.Instance.CusoredInventory == null)
            return;

        Vector2 size = new Vector2();
        size.x = targetItem.Width * inventory_manager.Instance.CusoredInventory.group.GetTileSizeWidth();
        size.y = targetItem.Height * inventory_manager.Instance.CusoredInventory.group.GetTileSizeHeight();
        highlighter.sizeDelta = size;
    }

    public void SetParent(InventoryGridUI targetGrid)
    {
        if (targetGrid == null)
        {
            return;
        }

        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void SetPosition(InventoryGridUI targetGrid, InventoryItemUI targetItem)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);

        highlighter.localPosition = pos;
    }

    public void SetPosition(InventoryGridUI targetGrid, InventoryItemUI targetItem, int posX, int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);

        highlighter.localPosition = pos;
    }
}
