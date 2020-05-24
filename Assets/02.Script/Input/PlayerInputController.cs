using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputController : MonoBehaviour
{
    // Controller
    public PlayerMovementController movementController;
    public FollowCamera followCamera;
    private InputSystem currentInputSystem;

    private void Start()
    {
        //currentInputSystem = GetComponent<InputSystem_Mobile>();
        //currentInputSystem.Initialize(followCamera.MoveCamera, movementController.HorizontalMovement, movementController.Jump);
        //return;
#if UNITY_STANDALONE
        currentInputSystem = GetComponent<InputSystem_PC>();
        currentInputSystem.Initialize(followCamera.MoveCamera, movementController.HorizontalMovement, movementController.Jump);
        return;
#endif
#if UNITY_EDITOR
        currentInputSystem = GetComponent<InputSystem_PC>();
        currentInputSystem.Initialize(followCamera.MoveCamera, movementController.HorizontalMovement, movementController.Jump);
        return;
#endif
#if UNITY_ANDROID
        currentInputSystem = GetComponent<InputSystem_Mobile>();
        currentInputSystem.Initialize(followCamera.MoveCamera, movementController.HorizontalMovement, movementController.Jump);
        return;
#endif
    }
    private void Update()
    {
        currentInputSystem.FreeFrame_Update();
    }
    private void FixedUpdate()
    {
        currentInputSystem.FixedFrame_Update();
    }
}
