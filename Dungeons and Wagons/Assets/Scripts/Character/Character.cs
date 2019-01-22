using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Generic class for characters within the game
/// </summary>
namespace Entity {
    public class Character : MonoBehaviour {
        #region Data
        protected GameManager _gameManager;
        public string _name;

        [SerializeField]
        protected int _health;
        public Inventory inventory;
        [SerializeField]
        protected Transform headPoint; // used for calculating UI stuff
        [SerializeField]
        protected Transform bodyPoint;

        public int health {
            get { return _health; }
            set {
                _health = value;
                if (_health < 0)
                    _health = 0;
            }
        }

        public event Action<Character> OnHealthEvent;

        #endregion
        public virtual void InitializeCharacter(GameManager gameManager) {
            _gameManager = gameManager;
            inventory = GetComponent<Inventory>();
            if (inventory)
                inventory.InitializeInventory(gameManager, this);

            _health = 100;
        }
        public virtual void RegisterEvents() { }
        public virtual void DeregisterEvents() { }
        public Transform GetHeadPosition() {
            if (headPoint != null)
                return headPoint;
            else
                return null;
        }

        public Transform GetBodyPosition() {
            if (bodyPoint != null)
                return bodyPoint;
            else
                return null;
        }
        public virtual void Update() {
            if (OnHealthEvent != null)
                OnHealthEvent(this);
        }
    }
}