using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entity;
using Entity.AI;

public class HealthBarManager : MonoBehaviour {
    #region Data
    GameManager _gameManager;
    public Transform interfaceObject;
    public Transform healthBarPrefab;
    public Canvas canvas;
    public Vector3 offset;

    #endregion
    public void InitializeHealthBarManager(GameManager gameManager) {
        _gameManager = gameManager;
        canvas = GetComponent<Canvas>();
    }
    public HealthBarInterface CreateNewHealthBar(Character character) {
        if (healthBarPrefab) {
            Transform clone = Instantiate(healthBarPrefab, character.GetHeadPosition().position, healthBarPrefab.rotation);
            if (clone.GetComponent<HealthBarInterface>()) {
                clone.SetParent(interfaceObject);
                clone.GetComponent<HealthBarInterface>().SetCharacter(character, offset);
                clone.GetComponent<HealthBarInterface>().InitializeHealthBar(_gameManager);
                return clone.GetComponent<HealthBarInterface>();
            }
        }
        return null;
    }

}
