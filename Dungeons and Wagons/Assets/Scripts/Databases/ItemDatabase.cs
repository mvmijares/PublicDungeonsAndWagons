using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// Class handles loading all items from resources folder
/// Path : "Resource/Items/"
/// </summary>
/// 
public class ItemDatabase : MonoBehaviour {

    public List<Item> itemList;
    public List<string> itemNameList;

    ImageLibrary imageLibrary;

    public void InitializeItemDatabase() {
        itemList = new List<Item>();
        itemNameList = new List<string>();

        LoadItems();
    }

    void LoadItems() {
        Object[] items = Resources.LoadAll("Items", typeof(Item));
        foreach (Object t in items) {
            itemList.Add(t as Item);
            itemNameList.Add(t.name);
        }
    }
    public Item GetItem(string name) {
        foreach(Item i in itemList) {
            if (i.name == name)
                return Instantiate(i);
        }
        Debug.Log("Item: " + name + " not found...");
        return null;
    }

}
