using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("runtimer debug value")]
    [SerializeField] inventory_manager inventoryController;
    [SerializeField] InventoryGridUI itemGrid;


    public void Init()
    {
        inventoryController = GameObject.FindAnyObjectByType<inventory_manager>();
        itemGrid = GetComponentInParent<InventoryGridUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.CusoredInventory = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.CusoredInventory = null;
    }
}
