﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using UserInterfaceStates;

public class PlayerShopInterface : UserInterface {
    #region Data
    public GameObject buttonTemplate;
    public Sprite emptySprite; // display a empty box for nothing
    public GameObject playerShopContent; // Will change this later

    [SerializeField] private ShopButton overlapButton = null;
    [SerializeField] private ShopButton destinationButton = null;
    [SerializeField] private GameObject dragObjectReference = null;

    [SerializeField] private Button _inventoryTab;
    [SerializeField] private List<Rect> shopButtonRects;
    [SerializeField] private List<ShopButton> shopButtons;
   
    bool initialized; // if this is the first time we are access shop.// Delete items at a specific slot location.
    private bool shopInteractable;
    public event Action<int> DeleteItemEvent;

    /// <summary>
    /// TODO:   1. Check for when slots are already filled
    ///         2. If the item is stackable, check if we can add more into the same slot
    /// </summary>

    #endregion

    /// <summary>
    /// Class initialization
    /// </summary>
    public override void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        base.InitializeUserInterface(gameManager, userInterfaceManager);
        ShopButton[] buttons = playerShopContent.GetComponentsInChildren<ShopButton>();
        shopButtons = new List<ShopButton>();
        foreach (ShopButton button in buttons) {
            shopButtons.Add(button);
        }
        initialized = false;
        RegisterEvents();
    }
    public override void AccessInterface() {
        base.AccessInterface();
        int slot = 0;
        foreach (ShopButton button in shopButtons) {
            button.InitializeButton(_userInterfaceManager, this);
            button.SetBackgroundSprite(emptySprite);
            button.SetInventorySlot(slot);
            slot++;
        }
        initialized = true;
    }
    private void OnDestroy() {
        DeregisterEvents();
    }

    private void Update() {
        if (_userInterfaceManager.state == InventoryState.Shop)
            CheckForDragItem();
    }
    public void OnPlayerShopEventCalled(PlayerShopData data) {
        shopInteractable = data._playerWithinBox;
    }

    /// <summary>
    /// Creates a new button 
    /// </summary>
    /// <param name="sprite">Sprite for button</param>
    /// <param name="slot">Reference to slot number</param>
    ItemButton CreateNewItemButton(Sprite sprite, int slot) {
        GameObject newButton = Instantiate(buttonTemplate) as GameObject;
        newButton.SetActive(true);
        newButton.GetComponent<ItemButton>().SetIcon(sprite);
        newButton.GetComponent<ItemButton>().SetInventorySlot(slot);
        newButton.transform.SetParent(buttonTemplate.transform.parent, false);

        return newButton.GetComponent<ItemButton>();
    }
    /// <summary>
    /// Used to check if the object is over one of the shop buttons
    /// </summary>
    void CheckForDragItem() {
        if (overlapButton == null) {
            foreach (ShopButton button in shopButtons) {
                if (RectTransformExt.GetWorldRect(button.rectTransform, Vector2.one).Contains(Input.mousePosition)) {
                    overlapButton = button;
                }
            }
        } else {
            if (!RectTransformExt.GetWorldRect(overlapButton.GetComponent<RectTransform>(), Vector2.one).Contains(Input.mousePosition)) {
                overlapButton = null;
            }
        }
    }
    /// <summary>
    /// Function called when user ends dragging in the shop state
    /// </summary>
    public void EndDragWasCalled() {
        if (overlapButton != null && _userInterfaceManager.dragObject != null) {
            destinationButton = overlapButton;
            ItemButton button = _userInterfaceManager.dragObject.GetComponent<ItemButton>();
            CheckItemDetails(button);
        }
    }
    /// <summary>
    /// Checks if the button is stackable.
    /// If it is, we give players options to split stack to put into shop
    /// else, we put it into shop, and delete the item from inventory
    /// </summary>
    /// <param name="itemButton"></param>
    void CheckItemDetails(ItemButton itemButton) {
        Item newItem = _gameManager.level.player.inventory.GetItem(itemButton.invSlot);

        if (newItem.isStackable && newItem.itemCount > 1) {
            Item newItemCopy = newItem.GetCopy();
            _userInterfaceManager.EnableStackInterface(newItemCopy, itemButton, overlapButton.invSlot);
        } else {
            _gameManager.playerShop.AddItemToShopInventory(newItem, overlapButton.invSlot);
            //Delete item from player inventory
            //_gameManager.DeleteItemFromPlayerInventory(itemButton.invSlot);
            DisplayNewButton(itemButton);
        }
    }

    public void DisplayNewButton(ItemButton itemButton) {
        if (destinationButton != null) {
            destinationButton.GetComponent<ShopButton>().SetItemImage(itemButton.myIcon);
        }
        destinationButton = null;
    }
}
