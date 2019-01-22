using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity;

public class LevelManager : MonoBehaviour {

    #region Data
    [SerializeField] private GameManager _gameManager;

    #endregion

    public void InitializeLevelManager(GameManager gameManager) {
        _gameManager = gameManager;
    }

}
