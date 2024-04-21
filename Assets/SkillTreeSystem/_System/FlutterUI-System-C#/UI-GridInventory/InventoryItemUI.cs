using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


/*
 * Grid Inventory - Diablo Style
 * Item Container
 */
public class InventoryItemUI : UIObjectBase
{
    public object item;                     // container data
    public Vector2Int itemSize;             // item size
    public InventoryGridUI ownerInventory;  // in inventory ref  
    public Image itemIconImage;             // item icon image

    public int Height                       // item height - y
    {
        get
        {
            if (rotated == false)
            {
                return itemSize.y;
            }
            return itemSize.x;
        }
    }
    public int Width                        // item width - x
    {
        get
        {
            if (rotated == false)
            {
                return itemSize.x;
            }
            return itemSize.y;
        }
    }

    public int onGridPositionX;             // in inventory grid position
    public int onGridPositionY;             // in inventory grid position

    public bool rotated = false;            // is rotated


    public static InventoryItemUI Create()
    {
        var go = new GameObject("InventoryItemUI");

        var ui = go.AddComponent<InventoryItemUI>();
        ui.itemIconImage = ui.AddComponent<Image>();
        ui.itemIconImage.preserveAspect = true;
        ui.itemIconImage.raycastTarget = false;

        ui.InitializeUI();

        return ui;
    }


    /*
     * container set data
     */
    public void SetData(object item)
    {
        this.item = item;
    }

    /*
     * get container data
     */
    public T GetData<T>() where T : class
    {
        return this.item as T;
    }

    /*
     * set item icon sprite
     */
    public void SetIconSprite(Sprite icon)
    {
        itemIconImage.sprite = icon;
    }

    /*
     * set item width an height (vector2 int)
     */
    public void SetItemSize(Vector2Int size)
    {
        this.itemSize = size;
        Vector2 itemUISize = new Vector2();
        itemUISize.x = size.x * 80;
        itemUISize.y = size.y * 80;
        rectTransform.sizeDelta = itemUISize / InventoryGridUI.canvasResolutionRatio;
    }

    /*
     * set item icon sprite - scale factor
     * original icon image to fit inventory tile grid scale
     */
    public void SetImageScaleFactor(float scaleFactor)
    {
        Vector2 itemUISize = new Vector2();
        itemUISize.x = itemSize.x * ownerInventory.group.data.tileIconImage.textureRect.width;
        itemUISize.y = itemSize.y * ownerInventory.group.data.tileIconImage.textureRect.height;
        rectTransform.sizeDelta = itemUISize / InventoryGridUI.canvasResolutionRatio * scaleFactor;
    }

    /*
     * item rotated
     */
    internal void Rotate()
    {
        rotated = !rotated;
        rectTransform.rotation = Quaternion.Euler(0f, 0f, rotated == false ? 0f : 90f);
    }
}