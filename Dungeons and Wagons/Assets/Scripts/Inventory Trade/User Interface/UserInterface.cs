using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for user interface elements.
/// </summary>
public class UserInterface : MonoBehaviour {
    #region Data
    protected Canvas _canvas;
    protected GameManager _gameManager;
    protected UserInterfaceManager _userInterfaceManager;
    protected bool init = false;
    #endregion

    public virtual void InitializeUserInterface(GameManager gameManager, UserInterfaceManager userInterfaceManager) {
        _gameManager = gameManager;
        _userInterfaceManager = userInterfaceManager;
        _canvas = GetComponent<Canvas>();
        init = true;
    }
}
