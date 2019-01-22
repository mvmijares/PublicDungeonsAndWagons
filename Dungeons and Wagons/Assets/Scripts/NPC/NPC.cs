using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity.AI;

namespace Entity {
    public class NPC : Character {
        #region Data
        private NPCController _npcController;
        public NPCController npcController { get { return _npcController; } }

        private NPCPathfinder _npcPathfinder;
        public NPCPathfinder npcPathfinder { get { return _npcPathfinder; } }

        private SpeechBubble _speechBubble;
        public SpeechBubble speechBubble { get { return _speechBubble; } }

        public List<NPCBehaviour> behaviourList;
        public AIState initialBehaviour;
        [SerializeField]
        NPCBehaviour currentBehaviour = null;

        public bool canTalkWith = false;
        #endregion
        public override void InitializeCharacter(GameManager gameManager) {
            base.InitializeCharacter(gameManager);
            _npcController = GetComponent<NPCController>();
            _npcPathfinder = GetComponent<NPCPathfinder>();
            _gameManager = gameManager;

            InitializeBehaviours();
            if(initialBehaviour != AIState.None) {
                switch (initialBehaviour) {
                    case AIState.Patrol: {
                            currentBehaviour = GetBehaviour(typeof(PatrolBehaviour));
                            break;
                        }
                    case AIState.Shop: {
                            currentBehaviour = GetBehaviour(typeof(ShoppingBehaviour));
                            break;
                        }
                    case AIState.Talking: {
                            currentBehaviour = GetBehaviour(typeof(TalkBehaviour));
                            break;
                        }
                }
            }
            _npcController.InitializeController(gameManager, this);
            _npcPathfinder.InitializePathfinder(gameManager, this);

            RegisterEvents();
        }

        private void OnDestroy() {
            DeregisterEvents();
        }
        /// <summary>
        /// Initialize and store our behaviours
        /// </summary>
        void InitializeBehaviours() {
            NPCBehaviour[] behaviours = GetComponents<NPCBehaviour>();
            behaviourList = new List<NPCBehaviour>();
            foreach (NPCBehaviour behaviour in behaviours) {
                behaviour.InitializeBehaviour(_gameManager, this);
                behaviourList.Add(behaviour);
            }
        }
        void RegisterEvents() {
            
        }
        void DeregisterEvents() {
            if (_speechBubble != null) {
                foreach (NPCBehaviour behaviour in behaviourList) {
                    behaviour.SendEmotionEvent -= _speechBubble.OnTalkEvent;
                }
            }
        }
        /// <summary>
        /// Returns a behaviour from behaviour list
        /// </summary>
        /// <param name="behaviourType">Type of behaviour</param>
        /// <returns></returns>
        public NPCBehaviour GetBehaviour(Type behaviourType) {
            foreach(NPCBehaviour behaviour in behaviourList) {
                if(behaviour.GetType() == behaviourType) {
                    return behaviour;
                }
            }
            return null;
        }
        public void SetNPCBehaviour(Type behaviourType) {
            currentBehaviour = GetBehaviour(behaviourType);
        }

        public override void Update() {
            base.Update();
            if(currentBehaviour != null) {
                currentBehaviour.PlayBehaviour();
            }
        }

    }
}
