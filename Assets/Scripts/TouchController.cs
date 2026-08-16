using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchController : MonoBehaviour
{
    [SerializeField] private FixedTouchField fixedTouchField;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private FixedButton jumpButton;

    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private FixedButton attackButton;

    void Start()
    {
        
    }

    
    void Update()
    {
        cameraController.lockAxis = fixedTouchField.TouchDist;

        bool isKeyboardJump = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;
        playerMove.isPressed = jumpButton.IsPressed() || isKeyboardJump;

        bool isKeyboardAttack = Keyboard.current != null && (Keyboard.current.fKey.isPressed || Keyboard.current.enterKey.isPressed);
        playerAttack.isAttackPressed = attackButton.IsPressed() || isKeyboardAttack;
    }
}
