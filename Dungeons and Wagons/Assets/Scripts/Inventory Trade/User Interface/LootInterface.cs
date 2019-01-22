using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
///  This class handles functionality for loot drops within a menu
///  TODO   - Maybe, make a parent class that loot user interface inherits from
///         - Inventory User interface has the same functionality
/// </summary>
/// 
public class LootInterface : UserInterface {
    #region Data
    GameManager gameManager;
    ImageLibrary imageLibrary;
    [SerializeField]
    bool _windowStatus; //is the loot window currently open?
    public bool windowStatus { get { return _windowStatus; } }

    [SerializeField]
    List<ItemButton> itemButtons;
    public Button closeWindowButton;
    //TODO make a button template prefab that we load from 
    public GameObject buttonTemplate;

    public GameObject interfaceObject;

    public event Action<int> TakeItemInterfaceEvent; // We take item instead of trashing it
    #endregion

    public override void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        base.InitializeUserInterface(gameManager, userInterfaceManager);
        imageLibrary = gameManager.imageLibrary;
        itemButtons = new List<ItemButton>();
        closeWindowButton.onClick.AddListener(CloseWindow);
        _windowStatus = false;
        SetGraphics(false);
    }
    /// <summary>
    /// This function just sets the graphics to invisible at start.
    /// We can reactive it when we want to start using it
    /// </summary>
    public void SetGraphics(bool condition) {
        interfaceObject.SetActive(condition);
        _windowStatus = condition;
    }
    public void GenerateLootInterface(Inventory inventory) {
        int slot = 1;
        if (inventory.inventory.Count > 0) {
            foreach (Item i in inventory.inventory) {
                itemButtons.Add(CreateNewItemButton(imageLibrary.GetSpriteReference(i.itemName), slot));
                slot++;
            }
        }
        SetGraphics(true);
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
        newButton.GetComponent<ItemButton>().ItemOnRightClickEvent += ItemRightClickEvent;
        newButton.transform.SetParent(buttonTemplate.transform.parent, false);

        return newButton.GetComponent<ItemButton>();
    }
    /// <summary>
    /// This function is called when we want to close the current window
    /// </summary>
    void CloseWindow() {
        if (_windowStatus) {
            foreach (ItemButton iButton in itemButtons) {
                iButton.ItemOnRightClickEvent -= ItemRightClickEvent;
                Destroy(iButton.gameObject);
            }
            itemButtons.Clear();
            _windowStatus = false;
            SetGraphics(false);
            gameManager.DoneAccessingLoot();
        }
    }

    public void ItemRightClickEvent(ItemButton iButton, int slot) {
        itemButtons.Remove(iButton);
        Destroy(iButton.gameObject);
        iButton.ItemOnRightClickEvent -= ItemRightClickEvent;
        gameManager.AddItemFromLootInterface(slot);
    }

    public void UpdateinterfaceSlots() {
        int slot = 1;
        foreach (ItemButton i in itemButtons) {
            i.invSlot = slot;
            slot++;
        }
    }

}
