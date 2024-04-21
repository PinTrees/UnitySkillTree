using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class inventory_manager : Singleton<inventory_manager>
{
    public InventoryGridUI CusoredInventory
    {
        get => cursoredInventory;
        set
        {
            cursoredInventory = value;
            inventoryHighlight.SetParent(value);
        }
    }

    [Header("runtime debug value")]
    [SerializeField] InventoryGridUI cursoredInventory;
    [SerializeField] Vector2Int cusorGridPosition;

    [SerializeField] InventoryGridUI dragStartInventory;
    [SerializeField] Vector2Int dragStartGridPosition;

    [SerializeField] InventoryItemUI cusoredItem;
    [SerializeField] InventoryItemUI dragingItem;
    InventoryItemUI overlapItem;

    [SerializeField] List<ItemData> items;
    [SerializeField] Transform canvasTransform;

    InventoryHighlight inventoryHighlight;

    public Action<InventoryItemUI> onRightClickItem;


    protected override void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (dragingItem == null)
            {
                CreateRandomItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            InsertRandomItem();
        }

        InputRotateItem();

        UpdateCusorGridPosition();
        UpdateCurrentCusorItem();
        HandleHighlight();

        if (InputItemRightMouseButton()) return;

        if (InputPlaceItem()) return;
        if (InputPickUpItem()) return;
    }


    private void InputRotateItem()
    {
        if (dragingItem == null)
            return;

        if (!Input.GetKeyDown(KeyCode.R))
            return;

        dragingItem.Rotate();
    }
    private bool InputPickUpItem()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return false;

        if (dragingItem != null) 
            return false;

        if (cursoredInventory == null)
            return false;

        dragingItem = cursoredInventory.PickUpItem(cusorGridPosition.x, cusorGridPosition.y);
        if (dragingItem != null)
        {
            dragStartInventory = cursoredInventory;
            dragStartGridPosition = new Vector2Int(dragingItem.onGridPositionX, dragingItem.onGridPositionY);

            dragingItem.rectTransform.SetParent(canvasTransform, true);
            dragingItem.rectTransform.SetAsLastSibling();
        }

        return true;
    }
    private bool InputPlaceItem()
    {
        if (dragingItem == null)
            return false;

        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return false;

        bool complete;

        if (cursoredInventory == null)
        {
            if (dragStartInventory == null) 
                return false;
            
            complete = dragStartInventory.PlaceItem(dragingItem, dragStartGridPosition.x, dragStartGridPosition.y, ref overlapItem);
            if(complete)
            {
                dragingItem = null;
                dragStartInventory = null;
                return true;
            }
        }
        else
        {
            complete = cursoredInventory.PlaceItem(dragingItem, cusorGridPosition.x, cusorGridPosition.y, ref overlapItem);
            if (complete)
            {
                dragingItem = null;
                if (overlapItem != null)
                {
                    dragingItem = overlapItem;
                    overlapItem = null;
                    dragingItem.rectTransform.SetParent(canvasTransform);
                    dragingItem.rectTransform.SetAsLastSibling();
                }
            }
            else if(dragStartInventory != null)
            {
                complete = dragStartInventory.PlaceItem(dragingItem, dragStartGridPosition.x, dragStartGridPosition.y, ref overlapItem);
                if (complete)
                {
                    dragingItem = null;
                    dragStartInventory = null;
                }
            }
        }

        return true;
    }
    private bool InputItemRightMouseButton()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse1))
            return false;

        if (cusoredItem == null)
            return false;

        if (onRightClickItem == null)
            return false;

        onRightClickItem(cusoredItem);

        return true;
    }

    private void InsertRandomItem()
    {
        CreateRandomItem();
        InventoryItemUI itemToInsert = cusoredItem;
        cusoredItem = null;
        InsertItem(itemToInsert);
    }

    private void InsertItem(InventoryItemUI itemToInsert)
    {
        if (cursoredInventory == null)
        {
            return;
        }

        Vector2Int? posOnGrid = cursoredInventory.FindSpaceForObject(itemToInsert);

        if (posOnGrid == null)
        {
            return;
        }

        CusoredInventory.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    InventoryItemUI itemToHighlight;

    private void UpdateCusorGridPosition()
    {
        if (cursoredInventory == null)
            return;

        Vector2Int positionOnGrid = GetTileGridPosition();
        if (cusorGridPosition == positionOnGrid)
        {
            return;
        }
        cusorGridPosition = positionOnGrid;
    }
    private void UpdateCurrentCusorItem()
    {
        if (cursoredInventory != null)
        {
            cusoredItem = cursoredInventory.GetItem(cusorGridPosition.x, cusorGridPosition.y);
        }
        else
        {
            cusoredItem = null;
        }
    }
    private void HandleHighlight()
    {
        if (cursoredInventory == null)
            return;

        if (dragingItem == null)
        {
            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(cursoredInventory, itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(cursoredInventory.BoundryCheck(
                cusorGridPosition.x,
                cusorGridPosition.y,
                dragingItem.Width,
                dragingItem.Height)
                );
            inventoryHighlight.SetSize(dragingItem);
            inventoryHighlight.SetPosition(cursoredInventory, dragingItem, cusorGridPosition.x, cusorGridPosition.y);
        }
    }

    private void CreateRandomItem()
    {
        InventoryItemUI inventoryItem = InventoryItemUI.Create();
        dragingItem = inventoryItem;

        inventoryItem.rectTransform.SetParent(canvasTransform);
        inventoryItem.rectTransform.SetAsLastSibling();

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.SetData(items[selectedItemID]);
        inventoryItem.SetIconSprite(items[selectedItemID].itemIcon);
        inventoryItem.SetItemSize(new Vector2Int(items[selectedItemID].width, items[selectedItemID].height));
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (cusoredItem != null)
        {
            position.x -= (cusoredItem.Width - 1) * cursoredInventory.group.GetTileSizeWidth() / 2;
            position.y += (cusoredItem.Height - 1) * cursoredInventory.group.GetTileSizeHeight() / 2;
        }

        return cursoredInventory.GetTileGridPosition(position);
    }

    private void ItemIconDrag()
    {
        if (dragingItem != null)
        {
            dragingItem.rectTransform.position = Input.mousePosition;
        }
    }
}
