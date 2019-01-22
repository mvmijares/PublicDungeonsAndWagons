using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Entity.AI;

/// <summary>
/// Script for AI Behaviour
/// Enum Description : 
/// AIbehaviour - Self explanatory
/// Shopping Decision
///     - Shop - AI will walk towards the shop, if they decide to
///     - Browse - AI will go through each item in the shop to look for items they want
///     - Accept or Decline - AI will either buy or not buy an item. We need to control the emotion that they will show
///     - Buy - Triggers the conversation with the AI to buy an item.
/// Emotion - Controls the emotion we send to the speech bubble.
/// </summary>

/// TODO :  Make a base behaviour class that includes basic functionality.
///         NPCBehaviour and EnemyBehaviour can inherit directly from that base class
///         Each behaviour can inherit from those behaviour classes.
///         If there is a behaviour that both classes can use, make sure to inherit from base class
///         rather than both sub-classes. "Diamond of Death"
///         
namespace Entity.AI {


    /// <summary>
    /// Class describes the type of behviour for the AI
    /// </summary>
    [Serializable]
    public class NPCBehaviour : MonoBehaviour, NPCInterface {
        #region Data
        [SerializeField]
        protected NPC _npc;
        protected GameManager _gameManager;
 
        protected SpeechBubble speechBubble;
        protected NPCController _npcController;
        protected NPCPathfinder _npcPathfinder;
        protected NPCAnimationController _npcAnimationController;
        protected NPCManager _npcManager;

        private bool setTarget;
        protected bool reachedEndofPath;
        [SerializeField]
        private bool pauseBehaviour;
        private bool crRunning = false;
        private bool initialized = false;

        public event Action<NPC, AIAction> SendEmotionEvent;
        protected bool pause;
        protected AIAction action;
        bool pauseIsRunning;
        #endregion

        public virtual void PlayBehaviour() { }
        public virtual void OnPauseBehaviourEnd() { }
        /// <summary>
        /// Initialization for NPC behaviour
        /// </summary>
        /// <param name="gameManager"></param>
        public virtual void InitializeBehaviour(GameManager gameManager, NPC npc) {
            _gameManager = gameManager;
            _npcController = GetComponent<NPCController>();
            _npcPathfinder = GetComponent<NPCPathfinder>();
            _npcAnimationController = (NPCAnimationController)_npcController.animationController;
            _npcManager = _gameManager.npcManager;
            
            _npc = npc;
            pause = false;
            RegisterEvents();
        }

        private void OnDestroy() {
            DeregisterEvents();
        }

        void RegisterEvents() {
            _npcPathfinder.EndOfPathReachedEvent += EndOfPathReached;
        }
        void DeregisterEvents() {
            _npcPathfinder.EndOfPathReachedEvent -= EndOfPathReached;
        }
        /// <summary>
        /// Decide what to do once the end of path is reached
        /// </summary>
        public virtual void EndOfPathReached(bool condition) { }

        public virtual void SendEmotion() {
            if (SendEmotionEvent != null) {
                SendEmotionEvent(_npc, action);
            }
        }

        public void Update() {

        }
        /// <summary>
        /// Set the default animation of the NPC Animation Controller
        /// </summary>
        /// <param name="talkPattern"></param>
        void SetDefaultAnimation(TalkPattern talkPattern) {
            switch (talkPattern) {
                case TalkPattern.Talking: {
                        _npcAnimationController.defaultAnimation = AnimationController.AnimationName.Talking;
                        break;
                    }
                case TalkPattern.Agreeing: {
                        _npcAnimationController.defaultAnimation = AnimationController.AnimationName.Agreeing;
                        break;
                    }
                case TalkPattern.None: {
                        _npcAnimationController.defaultAnimation = AnimationController.AnimationName.None;
                        break;
                    }
            }
        }

        public void SetPauseBehaviour(bool condition, float waitTime) {
            if (condition) {
                if (!pauseIsRunning) {
                    StartCoroutine(StopBehaviourCoroutine(waitTime));
                }
            }
        }
        /// <summary>
        /// Coroutine for pausing the behaviour
        /// </summary>
        /// <returns></returns>
        public IEnumerator StopBehaviourCoroutine(float waitTime) {
            pauseIsRunning = true;
            _npcPathfinder.SetTarget(null);
            yield return new WaitForSeconds(waitTime);
            StopCoroutine(StopBehaviourCoroutine(waitTime));
            OnPauseBehaviourEnd();
            pauseIsRunning = false;
        }
        /// <summary>
        /// Function call for setting the npc animation controller.
        /// </summary>
        public void PlayControllerAnimation() {
            _npcAnimationController.SetControllerAnimation(_npcAnimationController.defaultAnimation);
        }
        /// <summary>
        /// Returns the state of the animation controller
        /// </summary>
        /// <returns></returns>
        public AnimationController.AnimationState GetAnimationState() {
            return _npcAnimationController.state;
        }
    }
}
