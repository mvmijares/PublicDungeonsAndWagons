using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Overworld;

/// <summary>
/// Used to manage the overworld scene. It is different from Game Manager
/// - Holds information on the grid of nodes of the entire over world map
/// </summary>

namespace Overworld {
    public enum NodeType {
        Home, Village, Town, City, Cave
    }
    public class OverworldManager : MonoBehaviour {
        #region Data
        private InputHandler _inputHandler;
        public InputHandler inputHandler { get { return _inputHandler; } }

        private PathManager _pathManager;
        public PathManager pathManager { get { return _pathManager; } }

        OverworldInterfaceManager _overworldInterfaceManager;

        public Character character;

        List<Node> _nodeList; // List of all nodes in our map;
        public List<Node> nodeList { get { return _nodeList; } }
        public Node startNode;
        private Node _playerNode;
        public Node playerNode { get { return _playerNode; } } // the current node that the player is on.
   

        private AsyncOperation operation;
        [SerializeField]
        private Node selectedNode;

        public Vector2 cameraMinMaxSize; // used to set the orthographic size of camera
        public float zoomSpeed = 5f;
        public Vector3 originalPosition; // used to zoom back out to original position.
        bool zoomCondition;
        public float zoomDistance = 0.5f; //how far we are to zoom destination

        #endregion
        public void InitializeOverworldManager(GameManager gameManager) {
            _inputHandler = gameManager.inputHandler;
            _overworldInterfaceManager =  FindObjectOfType<OverworldInterfaceManager>();
            _overworldInterfaceManager.InitializeOverworldInterface(gameManager, this);
            _pathManager = FindObjectOfType<PathManager>();

            selectedNode = null;
            zoomCondition = false;
            _playerNode = startNode;
            originalPosition = Camera.main.transform.position;
        }

        private void Update() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);

            foreach (RaycastHit2D hit in hits) {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Node")) {
                    if (_inputHandler.input.MouseLeftClick.IsPressed) {
                        if (selectedNode == null) {
                            selectedNode = hit.transform.GetComponent<Node>();
                            NodeWasPressed(selectedNode);
                        }
                    }
                }
            }

            if (zoomCondition) {
                CameraZoom(true);
            } else {
                CameraZoom(false);
            }
        }
        /// <summary>
        /// Moves character to node
        /// </summary>
        /// <param name="node"></param>
        void NodeWasPressed(Node node) {
            node.NodeOnSelect(true);
            selectedNode = node;
            zoomCondition = true;
            _overworldInterfaceManager.NodeWasSelected(node);
           
        }
        /// <summary>
        /// Calls character to move to node.
        /// </summary>
        public void EnterNode() {
            if (selectedNode != null) {
                zoomCondition = false;
                if (selectedNode != playerNode) {
                    Path targetPath = _pathManager.GetPath(_playerNode, selectedNode);
                    if (targetPath != null)
                        character.Move(targetPath);
                } else {
                    LoadIntoWorld();
                }
            } else {
                Debug.Log("There is no node selected");
            }
        }
        /// <summary>
        /// Zooms out back to the full view of overworld 
        /// </summary>
        public void BackToWorld() {
            if (selectedNode) {
                zoomCondition = false;
                selectedNode.NodeOnSelect(false);
                selectedNode = null;
            }
        }
        /// <summary>
        /// Function called when a node is selected.
        /// We want to zoom into a node once it has been selected
        /// Returns a bool if the character has reached its destination
        /// </summary>
        /// <param name="condition">true if zooming in, else zooming out</param>
        void CameraZoom(bool condition) {
            if(selectedNode != null) {
                Camera cam = Camera.main;
                if (condition) {
                    cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 
                        cameraMinMaxSize.x, Time.deltaTime * zoomSpeed);
                    Vector3 destination = new Vector3(selectedNode.gameObject.transform.position.x, 
                        selectedNode.gameObject.transform.position.y, cam.transform.position.z);
                    cam.transform.position = Vector3.Lerp(cam.transform.position, 
                        destination, Time.deltaTime * zoomSpeed);
                } else {
                    Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, cameraMinMaxSize.y, Time.deltaTime * zoomSpeed);
                    Vector3 destination = originalPosition;
                    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, destination, Time.deltaTime * zoomSpeed);
                } 
            } else {
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, cameraMinMaxSize.y, Time.deltaTime * zoomSpeed);
                Vector3 destination = originalPosition;
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, destination, Time.deltaTime * zoomSpeed);
            }
        }
        /// <summary>
        /// Function to load into the current node map
        /// </summary>
        public void LoadIntoWorld() {
            _overworldInterfaceManager.EnableLoadingScreen(true);
            StartCoroutine(BeginLoading(selectedNode.sceneName));
        }
        /// <summary>
        /// Process for loading, Update UI with the progression
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private IEnumerator BeginLoading(string sceneName) {

            operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone) {
                _overworldInterfaceManager.loadingScreen.UpdateProgressUI(operation.progress);
                yield return null;
            }
            _overworldInterfaceManager.loadingScreen.UpdateProgressUI(operation.progress); // 100% completion
            operation = null;
            
        }
        /// <summary>
        /// Simple helper function
        /// </summary>
        /// <param name="v1">vector 1</param>
        /// <param name="v2">vector 2 </param>
        /// <param name="distance">distance between 2 vectors to check</param>
        /// <returns></returns>
        bool DistanceCheck(Vector3 v1, Vector3 v2, float distance) {
            if ((v2 - v1).magnitude < distance)
                return true;
            else
                return false;
        }

    }
}
