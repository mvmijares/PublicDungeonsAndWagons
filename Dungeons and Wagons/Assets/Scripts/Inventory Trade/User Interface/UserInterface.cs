using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for user interface elements.
/// </summary>
public class UserInterface : MonoBehaviour {
    #region Data
    protected Canvas _canvas; // TODO : Need to change this
    protected GameManager _gameManager;
    protected UserInterfaceManager _userInterfaceManager;
    public GameObject interfaceObject;
    [SerializeField] protected bool _windowStatus; //is the loot window currently open?
    public bool windowStatus { get { return _windowStatus; } }
    #endregion

    public virtual void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        _gameManager = gameManager;
        _userInterfaceManager = userInterfaceManager;
        _canvas = GetComponent<Canvas>(); 
        RegisterEvents();
    }
    public virtual void OnDestroyUserInterface() {
        DeregisterEvents();
    }

    public virtual void RegisterEvents() {
        _gameManager.inputHandler.OnEscapeKeyPressedEvent += OnEscapeKeyPressed;
    }
    public virtual void DeregisterEvents() {
        _gameManager.inputHandler.OnEscapeKeyPressedEvent -= OnEscapeKeyPressed;
    }
    public virtual void OnEscapeKeyPressed() { }
    public virtual void CloseInterface() { }
    public virtual void AccessInterface() { }
    public virtual void EnableUserInterface(bool condition) {
        if (interfaceObject) {
            interfaceObject.SetActive(condition);
        }
        _windowStatus = condition;
    }
}
