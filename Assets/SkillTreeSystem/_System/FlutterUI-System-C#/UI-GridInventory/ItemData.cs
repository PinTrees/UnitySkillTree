using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * custom item data class
 */
[CreateAssetMenu(menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    public int width = 1;               // item size
    public int height = 1;

    public Sprite itemIcon;             // runtime create image
    public GameObject itemObject;       // item 3d model

    public bool isBag;                  // has inventory
    public InventoryGroupData inventoryGroupData;   // inventory data
}
