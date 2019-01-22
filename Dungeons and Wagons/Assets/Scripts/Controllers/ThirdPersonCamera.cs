using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
    #region Data
    public Transform target;
    [SerializeField]
    InputHandler _inputHandler;
    GameManager _gameManager;
    public float dstFromNPC = 2f;
    public float dstFromTarget = 2f;
    public float cameraMoveSpeed;
    public Vector2 minMaxDstFromTarget;

    public Vector2 pitchMinMax = new Vector3(-40, 85); //x is low value, y is high value

    public float rotationSmoothTime = .12f;
    [SerializeField] Vector3 rotationSmoothVel;
    Vector3 currentRotation;

    float yaw;
    float pitch;
    #endregion

    public void InitializeCamera(GameManager gameManager) {
        _gameManager = gameManager;
    }
    
    // Update is called once per frame
    public void UpdatePlayerCamera() {
        if (_gameManager.player.playerCamera) {
            if (!_gameManager.player.playerInput.lockCursor) {
                yaw += Input.GetAxis("Mouse X") * _gameManager.player.playerInput.mouseSensitivity;
                pitch -= Input.GetAxis("Mouse Y") * _gameManager.player.playerInput.mouseSensitivity;
                pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            }
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVel, rotationSmoothTime);
            transform.eulerAngles = currentRotation;

            dstFromTarget = _gameManager.player.playerInput.CalculateCharacterZoom(dstFromTarget);
            dstFromTarget = Mathf.Clamp(dstFromTarget, minMaxDstFromTarget.x, minMaxDstFromTarget.y);
            transform.position = target.position - (transform.forward * dstFromTarget);
            
        } else {
            Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
            currentRotation = Vector3.SmoothDamp(currentRotation, lookRotation.eulerAngles, ref rotationSmoothVel, rotationSmoothTime);

            transform.eulerAngles = currentRotation;
        
            transform.position = Vector3.Lerp(transform.position, target.position + (target.transform.forward * dstFromNPC), Time.deltaTime * cameraMoveSpeed);
        }
    }

  
}
