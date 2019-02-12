using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class inherits from the character base class
/// <summary>
/// Primary function for this class is to only have functionality for players
/// </summary>
namespace Entity {
    //Busy State is for accessing user interface
    public enum PlayerState { None, Locomotion, Talking, Busy, Combat }

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
        public PlayerState state;
        public PlayerState subState;
        public Enemy currentTarget;
        [SerializeField] private bool _canMove;
        public bool canMove { get { return _canMove; } }
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
        [SerializeField] private float timeSinceLastAttack;
        private float maxAttackSpeed = 1f;
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

            state = PlayerState.Locomotion;
            subState = PlayerState.None;
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

        public override void Update() {
            base.Update();
            playerInput.UpdatePlayerInput();
            if (accessingInterface) {
                _gameManager.level.cam.lockCamera = true;
                state = PlayerState.Busy;
            } else {
                _gameManager.level.cam.lockCamera = false;

            }
            if (!_playerAnimationController.animState) {
                _playerAnimationController.ResumeAnimator();
            }

            switch (state) {
                case PlayerState.Combat: {
                        CharacterAttack(selectedEnemy);
                        break;
                    }
                case PlayerState.Locomotion: {
                        CharacterLocomotion();
                        break;
                    }
            }

            if(subState != PlayerState.None) {
                switch (state) {
                    case PlayerState.Combat: {
                            _playerAnimationController.StopAnimator();
                            timeSinceLastAttack = 0f;
                            selectedEnemy = null;
                            subState = PlayerState.Locomotion;
                            break;
                        }
                }
                state = subState;
                subState = PlayerState.None;
            }
        }
        private void CharacterLocomotion() {
            _playerAnimationController.SetControllerAnimation(AnimationController.AnimationName.None);
        }
        private void CharacterAttack(Enemy enemy) {
            if (enemy.health > 0 && state == PlayerState.Combat) {
                //Distance check between enemy and player
                float distance = (enemy.transform.position - transform.position).magnitude;

                if (distance <= _gameManager.meeleCombatDistance) {
                    _playerAnimationController.SetControllerAnimation(AnimationController.AnimationName.Attacking);
                    timeSinceLastAttack += Time.deltaTime;
                    if (_playerInput.movementInput) {
                        subState = PlayerState.Locomotion;
                        return;
                    }
                    if (timeSinceLastAttack >= maxAttackSpeed) {
                        enemy.TakeDamage(CalculateCharacterDamage());
                        timeSinceLastAttack = 0f;
                    }
                }
            } else {
                subState = PlayerState.Locomotion;
                return;
            }
        }
        /// <summary>
        /// Function that calculates total damage before sending to enemy
        /// </summary>
        /// <returns></returns>
        public int CalculateCharacterDamage() {
            
            int totalDamage = 10;

            return totalDamage;
        }
    }
}
