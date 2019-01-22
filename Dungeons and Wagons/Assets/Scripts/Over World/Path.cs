using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overworld {
    public class Path : MonoBehaviour {
        public string pathName;
        public Node a; //each path is represented by 2 points
        public Node b;

        public EasySplinePath2D path;

        private void Awake() {
            path = GetComponent<EasySplinePath2D>();
        }
    }
}
