using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopButton : MonoBehaviour{
    #region Data
    UserInterfaceManager _userInterfaceManager;
    PlayerShopInterface _playerShopInterface;
    private Image _myIcon;
    public Image myIcon { get { return _myIcon; } }
    public Image itemImage;
    public int invSlot;
    private bool isDragging;
    [SerializeField] private RectTransform _rectTransform;
    public RectTransform rectTransform { get { return _rectTransform; } }
    #endregion
    // Use this for initialization
    
    public void InitializeButton(UserInterfaceManager userInterfaceManager, PlayerShopInterface playerShopInterface) {
        _rectTransform = GetComponent<RectTransform>();
        _userInterfaceManager = userInterfaceManager;
        _playerShopInterface = playerShopInterface;
       
        _myIcon = GetComponent<Image>();
        isDragging = false;
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemImage.color = new Color(0, 0, 0, 0);
    }
    public void SetButtonName(string name) {
        gameObject.name = name;
    }
    public void SetBackgroundSprite(Sprite sprite) {
        _myIcon.sprite = sprite;
    }
    public void SetItemImage(Image image) {
        itemImage.sprite = image.sprite;
        itemImage.color = new Color(1, 1, 1, 1f);
    }
    public void SetInventorySlot(int num) {
        invSlot = num;
    }
}
