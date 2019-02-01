using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Entity;
/// <summary>
/// This class will only be for inventory for world items
/// Contains an inventory of items that the player can loot from
/// Has function to access the loot menu
/// 
/// </summary>
[RequireComponent(typeof(Character))]
public class Loot : Inventory {
    #region Data



    #endregion
    #region Event Data
    public Action<Loot> AccessLootInterfaceEvent;
    public Action LootDestructionEvent;
    #endregion
    public override void InitializeInventory(GameManager gameManager, Character character) {
        base.InitializeInventory(gameManager, character);
        LoadInventory();
    }
    public void OnDestroy() {
        if (LootDestructionEvent != null)
            LootDestructionEvent();
    }
    public override void Update() {
        base.Update();
    }

    /// <summary>
    /// Called when the user clicks on an object that contains loot.
    /// This will hook up the loot with the loot interface
    /// </summary>
    public void AccessLoot() {
        if (!_gameManager.level.player.accessingInterface) {
            if (AccessLootInterfaceEvent != null)
                AccessLootInterfaceEvent(this);

            _gameManager.level.currentLootObject = this;
        }
    }


}
