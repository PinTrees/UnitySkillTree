using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryGridUI : UIObjectBase
{
    public Vector2Int inventorySize = new Vector2Int(5, 5);

    public InventoryGroupUI group { get; private set; }
    InventoryItemUI[,] inventoryItemSlot;
    GridInteract interactor;

    [Header("runtime debug value")]
    [SerializeField] Image backgroundImage;
    [SerializeField] Image tileGridImage;
    [SerializeField] Image frameImage;
    [SerializeField] RectTransform backgroundRectTransform;
    [SerializeField] RectTransform frameRectTransform;
    [SerializeField] RectTransform invenRectTransform;

    public const float baseCanvasWidth = 1920;
    public const float baseCanvasHeight = 1080;
    public static Vector2 canvasResolutionRatio;


    public static InventoryGridUI Create(InventoryGroupUI group, InventoryGridData inventoryData)
    {
        var go = new GameObject("InventoryGridUI");
        go.transform.SetParent(group.rectTransform, true);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;

        go.AddComponent<RectTransform>();
        var ui = go.AddComponent<InventoryGridUI>();

        ui.group = group;
        ui.backgroundImage = UICreator.CreateNewImageObject(go);
        ui.backgroundImage.raycastTarget = false;
        ui.backgroundImage.color = group.data.backgroundColor;
        ui.backgroundRectTransform = ui.backgroundImage.GetComponent<RectTransform>();

        ui.tileGridImage = UICreator.CreateNewImageObject(go);
        ui.interactor = ui.tileGridImage.gameObject.AddComponent<GridInteract>();

        ui.tileGridImage.gameObject.GetComponent<RectTransform>().SetAncherLeftTop();
        ui.tileGridImage.sprite = group.data.tileIconImage;
        ui.tileGridImage.type = Image.Type.Tiled;
        ui.tileGridImage.color = group.data.tileColor;
        ui.tileGridImage.raycastTarget = true;

        ui.frameImage = UICreator.CreateNewImageObject(go);
        ui.frameRectTransform = ui.frameImage.gameObject.GetComponent <RectTransform>();
        ui.frameImage.raycastTarget = false;
        ui.frameImage.sprite = group.data.inventoryFrameImage;
        ui.frameImage.color = group.data.frameColor;
        ui.frameImage.type = Image.Type.Sliced;

        ui.inventorySize = inventoryData.inventorySize;

        float padding = 8f;

        var rectTransform = go.GetComponent<RectTransform>();
        rectTransform.SetAncherLeftTop();   // 앵커를 좌상단으로 설정

        // 패딩을 고려하여 오프셋 계산
        rectTransform.anchoredPosition = new Vector2(
            inventoryData.offset.x * (ui.group.GetTileSizeWidth() + padding),
            -inventoryData.offset.y * (ui.group.GetTileSizeHeight() + padding)
        );

        ui.InitializeUI();

        return ui;
    }


    public override void InitializeUI()
    {
        base.InitializeUI();

        invenRectTransform = GetComponent<RectTransform>();
        rectTransform = tileGridImage.gameObject.GetComponent<RectTransform>();

        interactor.Init();
        canvasResolutionRatio = new Vector2((baseCanvasWidth / rootCanvas.pixelRect.width), (baseCanvasHeight / rootCanvas.pixelRect.height));
      
        Init(inventorySize.x, inventorySize.y);
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItemUI[width, height];
        Vector2 size = new Vector2(width * group.GetTileSizeWidth(), height * group.GetTileSizeHeight());

        tileGridImage.pixelsPerUnitMultiplier = group.data.tileIconImage.textureRect.width / group.GetTileSizeWidth();
        rectTransform.sizeDelta = size;
        invenRectTransform.sizeDelta = size;
        backgroundRectTransform.sizeDelta = size;
        frameRectTransform.sizeDelta = size;
    }

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)((positionOnTheGrid.x / group.GetTileSizeWidth()) * canvasResolutionRatio.x);
        tileGridPosition.y = (int)((positionOnTheGrid.y / group.GetTileSizeHeight()) * canvasResolutionRatio.y);

        return tileGridPosition;
    }

    public bool PlaceItem(InventoryItemUI inventoryItem, int posX, int posY, ref InventoryItemUI overlapItem)
    {
        if (BoundryCheck(posX, posY, inventoryItem.Width, inventoryItem.Height) == false)
        {
            return false;
        }

        if (OverlapCheck(posX, posY, inventoryItem.Width, inventoryItem.Height, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        PlaceItem(inventoryItem, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItemUI inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int x = 0; x < inventoryItem.Width; x++)
        {
            for (int y = 0; y < inventoryItem.Height; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;
        inventoryItem.ownerInventory = this;
        inventoryItem.SetImageScaleFactor(group.GetTileSizeWidth() / group.data.tileIconImage.textureRect.width);

        Vector2 position = new Vector2();
        position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        rectTransform.localPosition = position;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItemUI inventoryItem, int posX, int posY)
    {
        Vector2 position;
        position.x = posX * group.GetTileSizeWidth() + group.GetTileSizeWidth() * inventoryItem.Width / 2;
        position.y = -(posY * group.GetTileSizeHeight() + group.GetTileSizeHeight() * inventoryItem.Height / 2);
        return position;
    }

    public InventoryItemUI PickUpItem(int x, int y)
    {
        InventoryItemUI toReturn = inventoryItemSlot[x, y];

        if (toReturn == null)
        {
            return null;
        }

        CleanGridReference(toReturn);

        return toReturn;
    }

    private void CleanGridReference(InventoryItemUI item)
    {
        for (int ix = 0; ix < item.Width; ix++)
        {
            for (int iy = 0; iy < item.Height; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
        {
            return false;
        }

        if (posX >= inventorySize.x || posY >= inventorySize.y)
        {
            return false;
        }

        return true;
    }

    public bool BoundryCheck(int posX, int posY, int width, int height)
    {
        if (PositionCheck(posX, posY) == false)
        {
            return false;
        }

        posX += width - 1;
        posY += height - 1;

        if (PositionCheck(posX, posY) == false)
        {
            return false;
        }

        return true;
    }
    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItemUI overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = inventoryItemSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if (overlapItem != inventoryItemSlot[posX + x, posY + y])
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    internal InventoryItemUI GetItem(int x, int y)
    {
        return inventoryItemSlot[x, y];
    }

    public Vector2Int? FindSpaceForObject(InventoryItemUI itemToInsert)
    {
        int width = inventorySize.x - itemToInsert.Width + 1;
        int height = inventorySize.y - itemToInsert.Height + 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y, itemToInsert.Width, itemToInsert.Height) == true)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }
}
