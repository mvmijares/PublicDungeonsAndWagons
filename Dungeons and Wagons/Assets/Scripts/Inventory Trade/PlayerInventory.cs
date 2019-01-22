using System.Collections;
using System.Collections.Generic;
using Entity;
using UnityEngine;


/// <summary>
/// Extra functionality that is specific to the player
/// </summary>
public class PlayerInventory : Inventory {
    #region Data

    #endregion

    public override void InitializeInventory(GameManager gameManager, Character character) {
        base.InitializeInventory(gameManager, character);
        LoadInventory();
    }
    public override void Update() {
        base.Update();
    }
    /// <summary>
    /// Updates inventory interface after Add Item is called
    /// </summary>
    /// <param name="item"></param>
    public override void AddItem(Item item) {
        base.AddItem(item);
        
    }
}
