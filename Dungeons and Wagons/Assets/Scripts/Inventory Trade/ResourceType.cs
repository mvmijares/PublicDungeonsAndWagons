using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Used to define the types of resources within the Node
/// </summary>
[System.Serializable]
public class ResourceInfo {
    public enum Type { Ore, Wood, Plantation, Leather, Meat, Skins }
    public Type nodeType;
    public bool discovered; // if player has discovered the resource within the node
}
