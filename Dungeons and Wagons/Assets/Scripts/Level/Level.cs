using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity;
/// <summary>
/// Base class that handles data specifically to the level the player is in
/// Different levels(Like scenes but different) :
/// - Town
/// - Puzzles
/// - Overworld
/// - Road (TBD) 
/// </summary>
/// 

public enum LevelState { Town, Puzzles, Overworld, Road}
public class Level : MonoBehaviour {

    #region Data
    [SerializeField] protected GameManager _gameManager;

    [SerializeField] protected Player _player;
    public Player player { get { return _player; } }

    [SerializeField] protected ThirdPersonCamera _cam;
    public ThirdPersonCamera cam { get { return _cam; } }
    public LevelState _state;
    public Transform playerSpawnLocation;
    protected List<Loot> levelLootObjects;
    public Loot currentLootObject;

    protected List<Enemy> enemyList;
    protected UserInterfaceManager _userInterfaceManager;
    public UserInterfaceManager userInterfaceManager { get { return _userInterfaceManager; } }

    #endregion

    public virtual void InitializeLevel(GameManager gameManager) {
        _gameManager = gameManager;

        _player = FindObjectOfType<Player>();
        if (_player)
            _player.InitializeCharacter(gameManager);

        _userInterfaceManager = FindObjectOfType<UserInterfaceManager>();
        if (_userInterfaceManager)
            _userInterfaceManager.InitializeInterfaces(gameManager);

        _cam = FindObjectOfType<ThirdPersonCamera>();
        if (_cam)
            _cam.InitializeCamera(gameManager);

        levelLootObjects = new List<Loot>();
        enemyList = new List<Enemy>();
    }
    public virtual void UpdateLevel() {
        
    }
    public virtual void LateUpdateLevel() {
        cam.UpdatePlayerCamera();
    }
    public virtual void DestroyLevel() { DeregisterEvents(); }

    public virtual void RegisterEvents() { }
    public virtual void DeregisterEvents() {
        _player.OnDestroyCharacter();
    }

    public virtual void InteractedWithNPC(NPC npc) { }
    public virtual void InteractedWithEnemy(Enemy enemy) { }
    //Might change naming convention
    public virtual void InteractedWithLoot(Loot loot) { }

    public virtual void AddToWorldEnemyList(Enemy enemy) {
        if (!enemyList.Contains(enemy)) {
            enemyList.Add(enemy);
            //Enemy Logic
        }
    }
    public virtual void DeleteFromWorldEnemyList(Enemy enemy) {
        if (enemyList.Contains(enemy)) {
            enemyList.Remove(enemy);
        }
    }

    public virtual void AddToWorldLootList(Loot loot) {
        if (!levelLootObjects.Contains(loot)) {
            levelLootObjects.Add(loot);
            _userInterfaceManager.lootInterface.RegisterEventForLootItem(loot);
        }
    }

    public virtual void DeleteFromWorldLootList(Loot loot) {
        if (levelLootObjects.Contains(loot)) {
            _userInterfaceManager.lootInterface.DeregisterEventForLootItem(loot);
            levelLootObjects.Remove(loot);
        }
    }
    /// <summary>
    /// Adds item to player inventory, and also updates the user interface
    /// </summary>
    /// <param name="item"></param>
    public void AddItemToInventory(Item item) {
        player.inventory.AddItem(item);
        _userInterfaceManager.lootInterface.UpdateinterfaceSlots();
    }
    /// <summary>
    /// Function for adding an item from an user interface panel
    /// </summary>
    /// <param name="slot"></param>
    public void AddItemFromLootInterface(int slot) {
        if (currentLootObject != null) {
            AddItemToInventory(currentLootObject.inventory[slot - 1].GetCopy());
            currentLootObject.DeleteItemSlot(slot);
        } else {
            Debug.Log("There is no loot object");
        }
    }

    /// <summary>
    /// Function to call to delete item from player Inventory.
    /// Use this method instead of direct access to player inventory to update user interface elements.
    /// </summary>
    /// <param name="inventorySlot">slot reference</param>
    public void DeleteItemFromPlayerInventory(int inventorySlot) {
        player.inventory.DeleteItemSlot(inventorySlot);
        // TODO: Update the player inventory after the item has been deleted
        //_userInterfaceManager.inventoryInterface.UpdateInventoryScreen(player.inventory);
    }
}
