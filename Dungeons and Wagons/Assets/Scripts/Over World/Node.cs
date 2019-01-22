using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This contains information on the node that we are traveling to.
/// Contains information about :
///     - Scene we will change to
///     - available resources for character to harvest
/// 
/// </summary>
namespace Overworld {
    public class Node : MonoBehaviour {
        #region Data
        public string nodeName; // name of the node
        public List<ResourceInfo> resourceList; // resource list.
        public string sceneName; //string for loading the scene which node is currently linked to

        public bool condition; //for resource discovery prototyping
        [SerializeField]
        GameObject nodeSelectedSprite;
        #endregion

        private void Awake() {
            Transform[] children = GetComponentsInChildren<Transform>();
            foreach(Transform child in children) {
                if(child.name == "Node Selected Sprite") {
                    nodeSelectedSprite = child.gameObject;
                }
            }

            if (nodeSelectedSprite)
                nodeSelectedSprite.SetActive(false);
        }

        public void NodeOnSelect(bool condition) {
            nodeSelectedSprite.SetActive(condition);

        }
    }


  
}
