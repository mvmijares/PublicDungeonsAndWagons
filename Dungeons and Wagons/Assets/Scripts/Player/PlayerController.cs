using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity {
    public class PlayerController : Controller {
        #region Data
        private Player _player;
        private Vector2 _inputDir;
        private InputHandler _inputHandler;

        #endregion
        public override void InitializeController(GameManager gameManager, Character character) {
            base.InitializeController(gameManager, character);
            _player = (Player)character;
            _inputHandler = _gameManager.inputHandler;
        }

        // Update is called once per frame
        private void Update() {
            PlayerInput();
            if (_gameManager.canMove) {
                targetRunSpeed = runSpeed;
                targetWalkSpeed = walkSpeed;
                CharacterRotation(_inputDir, _gameManager.cam.transform.eulerAngles.y );
                CharacterMovement(_inputDir);
            } else {
                _animationController.SetLocomotion(0f, 0f);
            }
        }

        private void PlayerInput() {
            _inputDir = _inputHandler.input.Move;
            running = Input.GetKey(KeyCode.LeftShift);
        }
    }
}
