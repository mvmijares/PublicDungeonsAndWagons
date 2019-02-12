using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Entity;
using Entity.AI;

public class HealthBarInterface : MonoBehaviour {
    #region Data
    GameManager _gameManager;
    Character character;
    HealthBarManager healthBarManager;
    public Image healthImage;
    public Image blackBar;
    private bool init;
    [SerializeField] private int health;
    private float rectWidth;
    private float healthPercentage;
    private Vector3 offset;
    Rect rect;

    public void OnHealthEventCalled(int health) {
        this.health = health;
    }
    public void OnDeathEventCalled() {
        character.OnHealthEvent -= OnHealthEventCalled;
        character.OnDeathEvent -= OnDeathEventCalled;
    }
    #endregion
    public void InitializeHealthBar(GameManager gameManager) {
        _gameManager = gameManager;
        healthBarManager = _gameManager.healthBarManager;
        
        rect = healthImage.rectTransform.rect;
        rectWidth = rect.width;
    }
    public void SetCharacter(Character character, Vector3 offset) {
        this.offset = offset;
        this.character = character;
        init = true;
    }
    private void Update() {
        if (init) {
            if (_gameManager.ObjectInCameraView(character.GetComponent<Collider>())) {
                EnableHealthBar(true);
                Vector2 canvasPoint;
                Vector2 screenPoint = Camera.main.WorldToScreenPoint(character.GetHeadPosition().position + offset);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(healthBarManager.canvas.GetComponent<RectTransform>(), screenPoint, null, out canvasPoint);
                transform.localPosition = canvasPoint;

                rect = healthImage.rectTransform.rect;

                healthPercentage = (float)health / (float)character.maxHealth;
                healthImage.rectTransform.sizeDelta = new Vector2(rectWidth * healthPercentage, rect.height);
            } else {
                EnableHealthBar(false);
            }
        } else {
            this.gameObject.SetActive(false);
        }
    }
    
    private void EnableHealthBar(bool condition) {
        healthImage.enabled = condition;
        blackBar.enabled = condition;
    }

}
