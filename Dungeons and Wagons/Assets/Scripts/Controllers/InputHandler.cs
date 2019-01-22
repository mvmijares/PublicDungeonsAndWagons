using InControl;
using System;
using UnityEngine;

//handles setting up input for game
public class KeyboardAction : PlayerActionSet {
    public PlayerAction Quit;
    public PlayerAction Action1;
    public PlayerAction Inventory;
    public PlayerAction MouseLeftClick;
    public PlayerAction MouseRightClick;
    public PlayerAction Left;
    public PlayerAction Right;
    public PlayerAction Up;
    public PlayerAction Down;
    public PlayerTwoAxisAction Move;
    public PlayerAction MouseZoomUp;
    public PlayerAction MouseZoomDown;
    public PlayerOneAxisAction MouseZoom;
    
    //Run. Inventory
    public KeyboardAction() {
        Quit = CreatePlayerAction("Quit");
        Action1 = CreatePlayerAction("Action1");
        Inventory = CreatePlayerAction("Inventory");
        MouseZoomUp = CreatePlayerAction("MouseZoomUp");
        MouseZoomDown = CreatePlayerAction("MouseZoomDown");
        MouseLeftClick = CreatePlayerAction("MouseLeftClick");
        MouseRightClick = CreatePlayerAction("MouseRightClick");

        Left = CreatePlayerAction("Left");
        Right = CreatePlayerAction("Right");
        Up = CreatePlayerAction("Up");
        Down = CreatePlayerAction("Down");
        Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);
        MouseZoom = CreateOneAxisPlayerAction(MouseZoomDown, MouseZoomUp);
    }

    public static KeyboardAction CreateDefaultBindings() {
        KeyboardAction keyboardActions = new KeyboardAction();

        keyboardActions.Quit.AddDefaultBinding(Key.Escape);
        keyboardActions.Action1.AddDefaultBinding(Key.Space);
        keyboardActions.Inventory.AddDefaultBinding(Key.I);
        keyboardActions.MouseLeftClick.AddDefaultBinding(Mouse.LeftButton);
        keyboardActions.MouseRightClick.AddDefaultBinding(Mouse.RightButton);

        keyboardActions.Left.AddDefaultBinding(Key.A);
        keyboardActions.Right.AddDefaultBinding(Key.D);
        keyboardActions.Up.AddDefaultBinding(Key.W);
        keyboardActions.Down.AddDefaultBinding(Key.S);

        keyboardActions.MouseZoomUp.AddDefaultBinding(Mouse.PositiveScrollWheel);
        keyboardActions.MouseZoomDown.AddDefaultBinding(Mouse.NegativeScrollWheel);

        keyboardActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
        keyboardActions.ListenOptions.IncludeUnknownControllers = true;

        //keyboardActions.ListenOptions.OnBindingFound = (action, binding) => {
        //    if(binding == new KeyBindingSource(Key.Escape)) {
        //        action.StopListeningForBinding();
        //        return false;
        //    }
        //    return true;
        //};

        return keyboardActions;
    }
}
//Handles all player input
public class InputHandler : MonoBehaviour {
    #region Data
    KeyboardAction _input;
    public KeyboardAction input { get { return _input; } set { _input = value; } }
    [SerializeField]
    GameManager _gameManager;

    public event Action<bool> OnActionKeyPressedEvent;
    public event Action<bool> OnInventoryKeyPressedEvent;
    public event Action<bool> OnMouseRightClickEvent;
    public event Action<bool> OnMouseLeftClickEvent;
    public event Action<bool> OnEscapeKeyPressedEvent;
    public event Action<Vector2> OnMovementEvent;
    public event Action<float> OnMouseScrollWheelEvent;
    #endregion
    public void InitializeInputHandler(GameManager gameManager) {
        _gameManager = gameManager;
        _input = KeyboardAction.CreateDefaultBindings();
    }
    private void OnDisable() {
        _input.Destroy();
    }
    public void UpdateInputControls() { 
        KeyboardControls();
    }
    void KeyboardControls() {
        if (_input.Quit.WasReleased) {
            if (OnEscapeKeyPressedEvent != null)
                OnEscapeKeyPressedEvent(true);
        } else {
            if (OnEscapeKeyPressedEvent != null)
                OnEscapeKeyPressedEvent(false);
        }
        if (_input.Inventory.WasReleased) {
            if (OnInventoryKeyPressedEvent != null)
                OnInventoryKeyPressedEvent(true);
        } else {
            if (OnInventoryKeyPressedEvent != null)
                OnInventoryKeyPressedEvent(false);
        }
        if (_input.Action1.WasPressed) {
            if (OnActionKeyPressedEvent != null)
                OnActionKeyPressedEvent(true);
        } else {
            if (OnActionKeyPressedEvent != null)
                OnActionKeyPressedEvent(false);
        }

        if (_input.MouseRightClick.IsPressed) {
            if (OnMouseRightClickEvent != null)
                OnMouseRightClickEvent(true);
        } else {
            if (OnMouseRightClickEvent != null)
                OnMouseRightClickEvent(false);
        }
        if (_input.MouseLeftClick.IsPressed) {
            if (OnMouseLeftClickEvent != null)
                OnMouseLeftClickEvent(true);
        } else {
            if (OnMouseLeftClickEvent != null)
                OnMouseLeftClickEvent(false);
        }

        if (OnMouseScrollWheelEvent != null) {
            OnMouseScrollWheelEvent(_input.MouseZoom);
        }

    }
}