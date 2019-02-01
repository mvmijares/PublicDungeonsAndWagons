using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Entity {
    public class PlayerController : Controller {
        #region Data
        [SerializeField] private Player _player;
        [SerializeField] public Vector2 moveDirection;

        #endregion
        public override void InitializeController(GameManager gameManager, Character character) {
            base.InitializeController(gameManager, character);
            _player = (Player)character;

            _player.playerPathfinder.EndOfPathReachedEvent += EndOfPathReachedCalled;
        }
        private void OnDestroy() {
            _player.playerPathfinder.EndOfPathReachedEvent -= EndOfPathReachedCalled;
        }
        // Update is called once per frame
        private void Update() {
            if (_player.canMove) {
                targetRunSpeed = runSpeed;
                targetWalkSpeed = walkSpeed;
                speedFactor = _player.movementSpeedFactor;
                CharacterRotation(moveDirection, _gameManager.level.cam.transform.eulerAngles.y );
                CharacterMovement(moveDirection);
            } else {
                _animationController.SetLocomotion(0f, 0f);
            }
        }

        public void EndOfPathReachedCalled(bool condition) {
            if (condition) {
                _player.playerPathfinder.SetTarget(null);
                _player.isAttacking = true;
            }
        }
    }
}
