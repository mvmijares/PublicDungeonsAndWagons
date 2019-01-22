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
    public LevelState _state;
    public Transform playerSpawnLocation;
    #endregion

    public virtual void InitializeLevel(GameManager gameManager) {
        _gameManager = gameManager;
        RegisterEvents();
    }
    public virtual void UpdateLevel() { }
    public virtual void DestroyLevel() { DeregisterEvents(); }

    public virtual void RegisterEvents() { }
    public virtual void DeregisterEvents() { }

    public virtual void InteractedWithNPC(NPC npc) { }
    public virtual void InteractedWithEnemy(Enemy enemy) { }
    //Might change naming convention
    public virtual void InteractedWithLoot(Loot loot) { }
}
