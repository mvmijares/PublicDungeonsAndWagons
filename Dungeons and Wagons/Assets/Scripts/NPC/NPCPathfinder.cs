using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;
using Entity.AI;

namespace Entity {
    public class NPCPathfinder : MonoBehaviour {

        #region Data
        private GameManager _gameManager;
        private Transform targetPosition;
        private Character owner;
        private Controller characterController;
        private Seeker seeker;

        public Path path;
        public float speed;
        public float nextWaypointDistance;
        private int currentWaypoint;
        public float repathRate;
        [SerializeField]
        private float lastRepath = float.NegativeInfinity;

        public bool reachedEndofPath;

        public Transform _target;
        public Vector3 _targetPosition;
        private Vector3 offSet;
        public bool overrideControls = false; // override Pathfinding Controls

        public event Action<bool> EndOfPathReachedEvent;
        #endregion

        public void InitializePathfinder(GameManager gameManager, Character character) {
            _gameManager = gameManager;
            owner = character;
            characterController = GetComponent<Controller>();
            seeker = GetComponent<Seeker>();
            reachedEndofPath = false;
            repathRate = 0.5f;
            currentWaypoint = 0;
            nextWaypointDistance = 2;
            speed = 2;
        }

        /// <summary>
        /// Path callback once the path has completed calculating
        /// </summary>
        /// <param name="p"></param>
        public void OnPathComplete(Path p) {
            p.Claim(this);
            if (!p.error) {
                if (path != null)
                    path.Release(this);

                path = p;
                currentWaypoint = 0;
            } else {
                p.Release(this);
            }
        }
        /// <summary>
        /// Sets new target and recalculates path.
        /// We have to set path to null or else the previous path will be referenced.
        /// TODO: Have another function for setting target by Vector3 position
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Transform target) {
            if (target != _target) {
                _target = target;
                path = null;
                if (_target != null)
                    QueueForNextPath();
            }
        }
        /// <summary>
        /// Queues up fro the next path
        /// </summary>
        void QueueForNextPath() {
            if (_target) {
                seeker.StartPath(transform.position, _target.position, OnPathComplete);
            } 
        }

        public void Update() {
            if (_target == null) {
                if(!overrideControls)
                    MoveTo(Vector2.zero);
                return;
            }
            if (Time.time > lastRepath + repathRate && seeker.IsDone()) {
                lastRepath = Time.time;
                QueueForNextPath();
            }
            if (path == null) {
                return;
            } else {
                float distanceToWaypoint;

                reachedEndofPath = false;
                //Distance check for waypoints / end of path reached
                while (true) {
                    distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                    if (distanceToWaypoint < nextWaypointDistance) {
                        if (currentWaypoint + 1 < path.vectorPath.Count) {
                            currentWaypoint++;
                        } else {
                            reachedEndofPath = true;
                            if (EndOfPathReachedEvent != null)
                                EndOfPathReachedEvent(reachedEndofPath); // we let the behaviour deal with end of path logic

                            break;
                        }
                    } else {
                        break;
                    }
                }
                if (!reachedEndofPath) {
                    //Move / Rotate Controller
                    Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
                    //Set the speed factor to slow down when we are closer to reaching the next point
                    characterController.speedFactor = reachedEndofPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;
                    MoveTo(new Vector2(dir.x, dir.z)); //Convert input movement to vector2
                }
            }
        }
        /// <summary>
        /// Allows the NPC to move and rotate towards a destination
        /// </summary>
        /// <param name="direction"></param>
        public void MoveTo(Vector2 direction) {
            RotateToTarget(direction);
            MoveToTarget(direction);
        }
        /// <summary>
        /// Rotate towards a target
        /// </summary>
        /// <param name="direction"></param>
        public void RotateToTarget(Vector2 direction) {
            characterController.CharacterRotation(direction);
        }
        /// <summary>
        /// Move towards a target
        /// </summary>
        /// <param name="direction"></param>
        public void MoveToTarget(Vector2 direction) {
            characterController.CharacterMovement(direction);
        }
    }
}
