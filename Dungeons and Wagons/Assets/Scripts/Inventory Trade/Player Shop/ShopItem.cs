using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for displaying items in shop
/// Contains information for NPCS to trade with
/// </summary>
public class ShopItem : MonoBehaviour {
    #region Data
    public int shopIndex;
    private MeshRenderer meshRenderer; // We will deactivate the wireframe box when we have an item.
    public Item shopItem = null;
    public string itemName = null;
    private Transform _itemObject;
    public Transform itemObject {
        get {
            return _itemObject;
        }
    }
    private Transform _viewPoint;
    public Transform viewPoint { get { return _viewPoint; } }
    #endregion
    /// <summary>
    /// Initialization
    /// </summary>
    public void InitializeShopItem() {
        meshRenderer = GetComponent<MeshRenderer>();
        _viewPoint = transform.GetChild(0);
    }
    private void Update() {
        if (shopItem != null && itemName != null)
            itemName = shopItem.itemName;
    }
    /// <summary>
    /// Enable or disable wireframe box for shop item
    /// </summary>
    /// <param name="condition"></param>
    public void SetWireframeBox(bool condition) {
        meshRenderer.enabled = condition;
    }
    /// <summary>
    /// Spawn a item object at location using item data
    /// </summary>
    public void DisplayShopItem() {
        SetWireframeBox(false);
        _itemObject = Instantiate(shopItem.prefab, transform.position, transform.rotation).transform ;
    }
    /// <summary>
    /// Get Item data from the shop item
    /// </summary>
    /// <returns></returns>
    public Item GetItemData() {
        if(shopItem != null) {
            return shopItem;
        }
        return null;
    }
}
