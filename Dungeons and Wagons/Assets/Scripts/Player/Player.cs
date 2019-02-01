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
        [SerializeField] private PlayerController _playerController;
        public PlayerController playerController { get { return _playerController; } }

        [SerializeField] private PlayerInput _playerInput;
        public PlayerInput playerInput { get { return _playerInput; } }

        [SerializeField] private NPCPathfinder _playerPathfinder;
        public NPCPathfinder playerPathfinder { get { return _playerPathfinder; } }

        [SerializeField] private AnimationController _playerAnimationController;
        public AnimationController playerAnimationController { get { return _playerAnimationController; } }

        public Transform clickObject; //Other object we can click on
        public Enemy selectedEnemy = null; // Enemy that the player has selected

        [SerializeField] private bool _playerCamera;

        [SerializeField] private bool _canMove;
        public bool canMove { get { return _canMove; } }

        public bool isAttacking;

        public float movementSpeedFactor;
        public bool playerCamera {
            get {
                if (talkingWithNPC)
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
        public bool accessingLoot;

        #endregion

        public override void InitializeCharacter(GameManager gameManager) {
            base.InitializeCharacter(gameManager);


            _playerInput = GetComponent<PlayerInput>();
            if (_playerInput)
                _playerInput.InitializePlayerInput(gameManager, this);
        
            _playerPathfinder = GetComponent<NPCPathfinder>();
            if (_playerPathfinder)
                _playerPathfinder.InitializePathfinder(gameManager, this);

            _playerController = GetComponent<PlayerController>();
            if (_playerController)
                _playerController.InitializeController(_gameManager, this);

            _playerAnimationController = GetComponentInChildren<AnimationController>();
            if (_playerAnimationController)
                _playerAnimationController.InitializeAnimationController(gameManager);

            _playerCamera = true;
            _canMove = true;
            talkingWithNPC = false;
            accessingInterface = false;

            clickObject = null;
            RegisterEvents();
        }
        public override void OnDestroyCharacter() {
            base.OnDestroyCharacter();
            DeregisterEvents();
        }
        private void RegisterEvents() {
            Debug.Log("Registering Player Events");
            _gameManager.inputHandler.OnRunningKeyPressedEvent += _playerInput.OnRunningKeyPressed;
            _gameManager.inputHandler.OnEscapeKeyPressedEvent += _playerInput.OnEscapeKeyPressed;
            _gameManager.inputHandler.OnInventoryKeyPressedEvent += _playerInput.OnInventoryKeyPressed;
            _gameManager.inputHandler.OnActionKeyPressedEvent += _playerInput.OnActionKeyPressed;
            _gameManager.inputHandler.OnMouseRightClickEvent += _playerInput.OnMouseRightClickPressed;
            _gameManager.inputHandler.OnMouseLeftClickEvent += _playerInput.OnMouseLeftClickPressed;
            _gameManager.inputHandler.OnMouseScrollWheelEvent += _playerInput.OnMouseScrollWheel;
            _gameManager.inputHandler.OnMovementEvent += _playerInput.OnMovementPressed;
        }
        private  void DeregisterEvents() {
            _gameManager.inputHandler.OnRunningKeyPressedEvent -= _playerInput.OnRunningKeyPressed;
            _gameManager.inputHandler.OnEscapeKeyPressedEvent -= _playerInput.OnEscapeKeyPressed;
            _gameManager.inputHandler.OnInventoryKeyPressedEvent -= _playerInput.OnInventoryKeyPressed;
            _gameManager.inputHandler.OnActionKeyPressedEvent -= _playerInput.OnActionKeyPressed;
            _gameManager.inputHandler.OnMouseRightClickEvent -= _playerInput.OnMouseRightClickPressed;
            _gameManager.inputHandler.OnMouseLeftClickEvent -= _playerInput.OnMouseLeftClickPressed;
            _gameManager.inputHandler.OnMouseScrollWheelEvent -= _playerInput.OnMouseScrollWheel;
            _gameManager.inputHandler.OnMovementEvent -= _playerInput.OnMovementPressed;
        }

        public override void UpdateCharacter() {
            base.UpdateCharacter();
            playerInput.UpdatePlayerInput();
            if (accessingInterface)
                _gameManager.level.cam.lockCamera = true;
            else
                _gameManager.level.cam.lockCamera = false;
        }
    }
}
