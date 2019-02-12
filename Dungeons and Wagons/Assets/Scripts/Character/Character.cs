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
        //Prototype stats
        protected int STR;
        protected int DEF;
        protected int SPD;
        protected int ACC;

        protected GameManager _gameManager;
        public string _name;

        [SerializeField] protected int _health;
        public int health { get { return _health; } }
        public int maxHealth;
        public Inventory inventory;
        [SerializeField]
        protected Transform headPoint; // used for calculating UI stuff
        [SerializeField]
        protected Transform bodyPoint;
        protected bool initialized = false;

        public event Action<int> OnHealthEvent;
        public event Action OnDeathEvent;
        #endregion
        public virtual void InitializeCharacter(GameManager gameManager) {
            _gameManager = gameManager;
            inventory = GetComponent<Inventory>();
            if (inventory)
                inventory.InitializeInventory(gameManager, this);

            _health = 100;
            maxHealth = 100;

            initialized = true;
        }
        public virtual void OnDestroyCharacter(){ }

        public Transform GetHeadPosition() {
            if (headPoint != null)
                return headPoint;
            else
                return null;
        }
        public virtual void TakeDamage(int damage) { }

        public Transform GetBodyPosition() {
            if (bodyPoint != null)
                return bodyPoint;
            else
                return null;
        }
        public virtual void Update() {
            if (OnHealthEvent != null)
                OnHealthEvent(_health);

            //if(_health <= 0) {
            //    if (OnDeathEvent != null)
            //        OnDeathEvent();
            //}
        }
    }
}