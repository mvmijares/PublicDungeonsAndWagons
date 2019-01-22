using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Overworld {
    public class Character : MonoBehaviour {
        #region Data
        OverworldManager overworldManager;

        public enum CharacterState { Idle, Traveling}
        CharacterState state;
        public float speed;
        Path targetPath;
        int pathIndex;
        private bool traveling;
        private Vector2 _direction;
        public Vector2 direction { get { return _direction; } }
        Overworld.PathManager pathManager;
        Animator anim;
        float dist;
        #endregion
        private void Start() {
            overworldManager = FindObjectOfType<OverworldManager>();
            anim = GetComponent<Animator>();
            state = CharacterState.Idle;
            pathManager = overworldManager.pathManager;
        }
        /// <summary>
        /// Move torwards node location
        /// </summary>
        /// <param name="target"></param>
        public void Move(Path p) {
            if (state != CharacterState.Traveling) {
                targetPath = p;
                state = CharacterState.Traveling;
            }
        }
        private void Update() {
            if (targetPath != null && state == CharacterState.Traveling) {
                transform.position = targetPath.path.GetPointByDistance(dist, true);
                dist += speed * Time.deltaTime;
                if(targetPath.path.DistanceToPercent(dist) >= 1f) {
                    state = CharacterState.Idle;
                    overworldManager.LoadIntoWorld();
                    targetPath = null;
                }
            }
        }
    }
}
