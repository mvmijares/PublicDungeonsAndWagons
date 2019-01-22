using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity;
using Entity.AI;

public class Enemy : Character {

    #region Data
    private EnemyController _enemyController;
    public EnemyController enemyController { get { return _enemyController; } }

    private EnemyPathfinder _enemyPathfinder;
    public EnemyPathfinder enemyPathfinder { get { return _enemyPathfinder; } }

    #endregion

    public override void InitializeCharacter(GameManager gameManager) {
        base.InitializeCharacter(gameManager);
        _enemyController = GetComponent<EnemyController>();
        //Initialize controller
        _enemyPathfinder = GetComponent<EnemyPathfinder>();
        //Initialize pathfinder

        RegisterEvents();
    }


    private void OnDestroy() {
        DeregisterEvents();
    }

    private void RegisterEvents() {

    }
    private void DeregisterEvents() { }
}
