using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonInterface : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField]
    Sprite defaultImage;
    [SerializeField]
    Sprite hoverImage;

    private OverworldInterfaceManager _overworldInterfaceManager;

    Button button;
    Vector2 textOffset;
    Vector2 textPivot;
    Vector2 textDefaultPivot;
    Text text;
    RectTransform textRectTransform;

    public void InitializeButton(OverworldInterfaceManager overworldInterfaceManager) {
        _overworldInterfaceManager = overworldInterfaceManager;

        button = GetComponent<Button>();
        if (defaultImage)
            button.image.sprite = defaultImage;
        textDefaultPivot = new Vector2(0.5f, 0.5f);
        textPivot = new Vector2(0.5f, 1f);
        textOffset = new Vector2(0, 20f);

        text = button.transform.GetChild(0).GetComponent<Text>();
        textRectTransform = text.GetComponent<RectTransform>();
    }
    public void OnPointerEnter(PointerEventData pointerEventData) {
        if (hoverImage) {
            button.image.sprite = hoverImage;
            textRectTransform.pivot = textPivot;
            textRectTransform.sizeDelta = textOffset;
        }
    }
    public void OnPointerExit(PointerEventData pointerEventData) {
        ResetButtonInterface();
    }
    public void ResetButtonInterface() {
        if (defaultImage) {
            button.image.sprite = defaultImage;
            textRectTransform.pivot = textDefaultPivot;
            text.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }
    }
}
