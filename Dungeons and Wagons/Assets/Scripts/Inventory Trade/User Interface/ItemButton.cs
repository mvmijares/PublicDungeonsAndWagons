using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {
    #region Data
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private UserInterfaceManager _userInterfaceManager;
    [SerializeField] private Canvas _canvas = null;

    private Image _myIcon;
    public Image myIcon { get { return _myIcon; } }
    public int invSlot; // reference to which inventory slot


    GameObject copy; //copy of this for positional purposes

    private bool origin;
    private int index; // index in the hiearchy
    private Transform originalParent;
    private Vector3 originalPosition;

    public event Action<ItemButton, int> ItemOnRightClickEvent;
    public event Action<ItemButton, int> ItemOnLeftClickEvent;

    #endregion

    public void InitializeItemButton(GameManager gameManager, Canvas canvas) {
        _myIcon = GetComponent<Image>();
        _gameManager = gameManager;
        _canvas = canvas;
        _userInterfaceManager = gameManager.level.userInterfaceManager;
        origin = false;
    }
    public void SetIcon(Sprite sprite) {
        myIcon.sprite = sprite;
    }
    public void SetInventorySlot(int num) {
        invSlot = num;
    }

    public void OnPointerClick(PointerEventData pointerEventData) {
        if(!pointerEventData.dragging && pointerEventData.button == PointerEventData.InputButton.Right) {
            if (ItemOnRightClickEvent != null)
                ItemOnRightClickEvent(this, invSlot);
        }
    }

    //Right click to trigger deleting item
    public void OnBeginDrag(PointerEventData pointerEventData) {
        if (_canvas == null)
            return;

        if (_userInterfaceManager.canDrag) {
            if (pointerEventData.button == PointerEventData.InputButton.Left) {
                if (!origin) {
                    originalParent = transform.parent;
                    originalPosition = transform.position;
                    index = this.transform.GetSiblingIndex();

                    copy = Instantiate(this.gameObject) as GameObject;

                    copy.GetComponent<Image>().sprite = null;
                    copy.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                    copy.transform.SetParent(transform.parent);
                    copy.transform.position = transform.position;
                    copy.transform.SetSiblingIndex(index);
                    origin = true;
                }

                transform.SetParent(_canvas.transform);
                transform.position = Input.mousePosition;
                _userInterfaceManager.SetDragObject(this.gameObject);
            }
        }
    }
    public void OnDrag(PointerEventData pointerEventData) {
        if (_userInterfaceManager.canDrag) {
            transform.position = Input.mousePosition;
        }
    }
    public void OnEndDrag(PointerEventData pointerEventData) {
        if (_userInterfaceManager.canDrag) {
            transform.SetParent(originalParent);
            transform.position = originalPosition;
            origin = false;
            transform.SetSiblingIndex(index);
            Destroy(copy.gameObject);
            _userInterfaceManager.EndDragWasCalled();
        }
    }
}
