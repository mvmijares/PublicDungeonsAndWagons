using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class handles all our enemies in our scene
/// </summary>
public class EnemyManager : MonoBehaviour {
    #region Data
    private GameManager _gameManager;
    [SerializeField] private List<Enemy> enemyList;

    #endregion
    //Initialization
    public void InitializeEnemyManager(GameManager gameManager) {
        _gameManager = gameManager;
        enemyList = new List<Enemy>();
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if(enemies != null) {
            foreach (Enemy e in enemies) {
                enemyList.Add(e);
                e.InitializeCharacter(gameManager);
            }
        }
    }

    //Add enemy to our list
    public void AddEnemyToList(Enemy e) {
        if(e != null) {
            enemyList.Add(e);
        }
    }
}
