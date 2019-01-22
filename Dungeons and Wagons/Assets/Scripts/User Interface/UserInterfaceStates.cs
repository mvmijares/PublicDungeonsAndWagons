using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInterfaceStates {
    public enum TextPosition { Center, Left, Right }
    public enum PlayerShopState { None, Setup, Trade } // Add this to Player Shop Interface
    public enum HelpInterfaceState { None, PlayerShop } // Will add more as game development progress
    public enum InventoryState { None, Inventory, Shop, Map, Wagon }
    public enum InventoryTabState { Inventory, Shop, Wagon, Map, Character }
}
