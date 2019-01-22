using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;

/// <summary>
/// This class will only be for inventory for world items
/// Contains an inventory of items that the player can loot from
/// Has function to access the loot menu
/// 
/// </summary>
[RequireComponent(typeof(Entity.Character))]
public class Loot : Inventory {

    public override void InitializeInventory(GameManager gameManager, Character character) {
        base.InitializeInventory(gameManager, character);
        LoadInventory();
    }

    public override void Update() {
        base.Update();
    }

    public void AccessLootMenu() {
        _gameManager.AccessLoot(this);
    }
   
        
}
