using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class inherits from the character base class
/// <summary>
/// Primary function for this class is to only have functionality for players
/// </summary>
namespace Entity {
    public class Player : Character {
        #region Data
        private PlayerController _playerController;
        public PlayerController playerController { get { return _playerController; } }

        private PlayerInput _playerInput;
        public PlayerInput playerInput { get { return _playerInput; } }

        public Transform clickObject;

        [SerializeField] private bool _playerCamera;

        public bool playerCamera {
            get {
                if (talkingWithNPC || accessingInterface)
                    return false;
                else
                    return true;
            }
            set {
                _playerCamera = value;
            }
        }

        [SerializeField] public bool canRotateCamera {
            get {
                if (talkingWithNPC || accessingInterface)
                    return false;
                else
                    return true;
            }
        }

        [SerializeField] public bool canMoveCamera {
            get {
                if (talkingWithNPC || accessingInterface)
                    return false;
                else
                    return true;
            }
        }
        public bool talkingWithNPC;
        public bool accessingInterface;
        #endregion

        public override void InitializeCharacter(GameManager gameManager) {
            base.InitializeCharacter(gameManager);
            _playerController = GetComponent<PlayerController>();
            _playerController.InitializeController(_gameManager, this);
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.InitializePlayerInput(gameManager, this);

            _playerCamera = true;
            talkingWithNPC = false;
            accessingInterface = false;

            clickObject = null;
            RegisterEvents();
        }

        public override void RegisterEvents() {
            base.RegisterEvents();
            _gameManager.inputHandler.OnEscapeKeyPressedEvent += _playerInput.OnEscapeKeyPressed;
            _gameManager.inputHandler.OnInventoryKeyPressedEvent += _playerInput.OnInventoryKeyPressed;
            _gameManager.inputHandler.OnActionKeyPressedEvent += _playerInput.OnActionKeyPressed;
            _gameManager.inputHandler.OnMouseRightClickEvent += _playerInput.OnMouseRightClickPressed;
            _gameManager.inputHandler.OnMouseLeftClickEvent += _playerInput.OnMouseLeftClickPressed;
            _gameManager.inputHandler.OnMouseScrollWheelEvent += _playerInput.OnMouseScrollWheel;
        }
        public override void DeregisterEvents() {
            base.DeregisterEvents();
            _gameManager.inputHandler.OnEscapeKeyPressedEvent -= _playerInput.OnEscapeKeyPressed;
            _gameManager.inputHandler.OnInventoryKeyPressedEvent -= _playerInput.OnInventoryKeyPressed;
            _gameManager.inputHandler.OnActionKeyPressedEvent -= _playerInput.OnActionKeyPressed;
            _gameManager.inputHandler.OnMouseRightClickEvent -= _playerInput.OnMouseRightClickPressed;
            _gameManager.inputHandler.OnMouseLeftClickEvent -= _playerInput.OnMouseLeftClickPressed;
            _gameManager.inputHandler.OnMouseScrollWheelEvent -= _playerInput.OnMouseScrollWheel;
        }

        public override void Update() {
            base.Update();
        }
    }
}
