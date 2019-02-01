using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity {
    public class Controller : MonoBehaviour {
        #region Data
        protected GameManager _gameManager;
        [SerializeField]
        protected AnimationController _animationController;
        public AnimationController animationController { get { return _animationController; } }

        protected float targetSpeed;
        protected float targetRunSpeed;
        protected float targetWalkSpeed;

        public bool running;
        public float speedFactor;
        public float walkSpeed = 2f;
        public float runSpeed = 6f;

        public float turnSmoothTime = 0.2f; // num of secs for smooth damp to go from curr to target val;
        protected float turnSmoothVel;

        public float speedSmoothTime = 0.1f;
        public Vector2 direction;
        protected float speedSmoothVel;
        protected float currentSpeed;
  
        #endregion
        public virtual void InitializeController(GameManager gameManager, Character character) {
            _animationController = GetComponentInChildren<AnimationController>();
            _animationController.InitializeAnimationController(gameManager);
            _gameManager = gameManager;
        }
        /// <summary>
        /// Rotate the character based on direction
        /// </summary>
        /// <param name="direction">direction the character will rotate</param>
        /// <param name="upVector"></param>
        public virtual void CharacterRotation(Vector2 direction, float upVector = 0) {
            if (direction != Vector2.zero) {
                float targetRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + upVector;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, turnSmoothTime);
            }
        }

        /// <summary>
        /// Controls character movement based on direction
        /// </summary>
        /// <param name="direction">direction character will move in</param>
        public virtual void CharacterMovement(Vector2 direction) {
            targetSpeed = ((running) ? (targetRunSpeed * speedFactor) : (targetWalkSpeed * speedFactor)) * direction.magnitude; //speed is 0 if no input happens
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVel, speedSmoothTime);

            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
            float animationSpeedPercent = ((running) ? 1 : .5f) * direction.magnitude * 2f;
            
            if (_animationController) {
                _animationController.SetLocomotion(animationSpeedPercent, speedSmoothTime);
            } else {
                Debug.Log("No animator found");
            }
        }
    }
}
