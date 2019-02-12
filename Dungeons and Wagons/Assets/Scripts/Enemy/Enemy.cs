using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity;
using Entity.AI;
/// <summary>
/// Enemy Info class is used to store information to be sent as an event for
/// classes that require information about it.
/// </summary>
public class EnemyInfo {
    public int health;
    public Transform headPosition;
    public Inventory inventory;
    
    public EnemyInfo(int health, Transform headPosition, Inventory inventory) {
        this.health = health;
        this.headPosition = headPosition;
        this.inventory = inventory;
    }
}
/// <summary>
/// Enemy class
/// </summary>
public class Enemy : Character {

    #region Data
    private EnemyController _enemyController;
    public EnemyController enemyController { get { return _enemyController; } }

    private EnemyPathfinder _enemyPathfinder;
    public EnemyPathfinder enemyPathfinder { get { return _enemyPathfinder; } }

    EnemyInfo info;
    #endregion

    #region Event Data
    public event Action<EnemyInfo> EnemyInfoEvent;
    #endregion

    public override void InitializeCharacter(GameManager gameManager) {
        base.InitializeCharacter(gameManager);
        _enemyController = GetComponent<EnemyController>();
        //Initialize controller
        _enemyPathfinder = GetComponent<EnemyPathfinder>();
        //Initialize pathfinder
        info = new EnemyInfo(health, headPoint, inventory);
        RegisterEvents();
    }
    public override void Update() {
        base.Update();
        if (initialized) {
            if (EnemyInfoEvent != null) {
                info.headPosition = headPoint;
                info.health = health;
                info.inventory = inventory;
                EnemyInfoEvent(info);
            }
        }
    }
    public override void TakeDamage(int damage) {
        base.TakeDamage(damage);
        _health -= damage; // Add modifiers at a later date
    }
    private void OnDestroy() {
        DeregisterEvents();
    }

    private void RegisterEvents() {

    }
    private void DeregisterEvents() { }
}
