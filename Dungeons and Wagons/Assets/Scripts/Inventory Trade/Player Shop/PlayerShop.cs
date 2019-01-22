using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.Events;

using UserInterfaceStates;

/// Encapsulate data from Player shop that is needed for UI events
public class PlayerShopData {
    public PlayerShopState _state;
    public bool _setup;
    public bool _playerWithinBox;
    public int _inventoryCount;

    public PlayerShopData() {
        _state = PlayerShopState.None;
        _setup = false;
        _playerWithinBox = false;
        _inventoryCount = 0;
    }
    public PlayerShopData(PlayerShopState state, bool setup) {
        _state = state;
        _setup = setup;
        
        _playerWithinBox = false;
        _inventoryCount = 0;
    }
}
/// <summary>
/// This class contains information for the player shop.
/// Will display the shop once the player presses correct input
/// Also displays the correct items, based on player placement in shop inventory interface
/// 
/// </summary>
public class PlayerShop : MonoBehaviour {
    #region Data

    private GameManager _gameManager;
    private UserInterfaceManager _userInterfaceManager;
    public InControl.Key setupShopKey;
    public GameObject wagon;
    [SerializeField]
    public PlayerShopState state;
    public bool setup;
    [SerializeField]
    private Inventory _inventory;
    public Inventory inventory { get { return _inventory; } }
    public GameObject shopPlaceholder; // placeholders for our 3D shop objects
    [SerializeField]
    private List<ShopItem> _shopItems;
    public List<ShopItem> shopItems { get { return _shopItems; } }

    public int shopItemNums; // number of shop items we can have

    public bool enableShop; // Starts the NPC trade seqeuence
    private bool finalizeShop; // Player has finialized their shop items 

    private PlayerShopData shopData;
    public event Action<PlayerShopData> OnPlayerShopEvent;
    #endregion

    //Initialization through game manager
    public void InitializePlayerShop(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        _gameManager = gameManager;
        _userInterfaceManager = userInterfaceManager;

        _inventory = GetComponent<Inventory>();
        _shopItems = new List<ShopItem>();

        ShopItem[] shopItemObjects = shopPlaceholder.GetComponentsInChildren<ShopItem>();
        if (shopItemObjects.Length > 0) {
            foreach (ShopItem shopItem in shopItemObjects) {
                shopItem.InitializeShopItem();
                shopItem.SetWireframeBox(false);
                shopItems.Add(shopItem);
            }
        } else {
            Debug.Log("There are no shop objects");
        }
        if (wagon)
            wagon.SetActive(false);

        state = PlayerShopState.None;
        setup = false;
        shopData = new PlayerShopData(state, setup);
    }

    private void Update() {
        if (OnPlayerShopEvent != null) {
            shopData._state = state;
            shopData._setup = setup;
            shopData._inventoryCount = inventory.inventory.Count;
            OnPlayerShopEvent(shopData);
        }
    }

    public void ActionWasPressed(bool condition) {
        if (condition) {
            switch (state) {
                case PlayerShopState.None: {
                        if(_inventory.inventory.Count > 0)
                            CreateShopDisplay();
                        break;
                    }
                case PlayerShopState.Setup: {
                        StartTradeWithNPC();
                        break;
                    }
            }
        }
    }
    /// <summary>
    /// Function to turn display the shop 
    /// </summary>
    void CreateShopDisplay() {
        wagon.SetActive(true);
        setup = true;
        DisplayShop();
        state = PlayerShopState.Setup;
    }
    void StartTradeWithNPC() {
        _gameManager.npcManager.CreateNPCsForTrade();
        state = PlayerShopState.Trade;
    }
    /// <summary>
    /// Add item to the inventory and shop item at index
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slot"></param>
    public void AddItemToShopInventory(Item item, int slot) {
        _inventory.AddItem(item);
        if (SearchShopItemIndex(slot) != null) {
            SearchShopItemIndex(slot).shopItem = item;
        }
    }
    /// <summary>
    /// Function call to each shop item object
    /// </summary>
    void DisplayShop() {
        if (shopItems.Count > 0) {
            foreach (ShopItem i in _shopItems) {
                if (i.shopItem != null && i.shopItem.prefab != null) {
                    i.DisplayShopItem();
                }
            }
        }
    }
    /// <summary>
    /// Returns shop item at index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    ShopItem SearchShopItemIndex(int index) {
        foreach (ShopItem shopItem in _shopItems) {
            if (shopItem.shopIndex == index)
                return shopItem;
        }
        return null;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            _gameManager.withinShopTrigger = true;
            shopData._playerWithinBox = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            _gameManager.withinShopTrigger = false;
            shopData._playerWithinBox = false;
        }
    }
    
}
