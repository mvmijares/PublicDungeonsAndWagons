using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
    public class PathManager : MonoBehaviour {

        [SerializeField]
        List<Path> paths; // List of all paths

        private void Awake() {
            //paths = new List<Path>();
            //Transform[] children = GetComponentsInChildren<Transform>();
            //foreach (Transform child in transform) {
            //    if(child != this.transform) {
            //        paths.Add(child.GetComponent<Path>());
            //    }
            //}
        }

        /// <summary>
        /// Return path based on which nodes they have access to
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="destinationNode"></param>
        /// <returns></returns>
        public Path GetPath(Node currentNode, Node destinationNode) {
            foreach(Path p in paths) {
                if (p.a != null && p.b != null) {
                    if (currentNode == p.a) {
                        if (destinationNode == p.b)
                            return p;
                    } else if (currentNode == p.b) {
                        if (destinationNode == p.a) {
                            return p;
                        }
                    }
                } else {
                    Debug.Log("Path does not have nodes setup.");
                }
            }
            return null;
        }
    }
}