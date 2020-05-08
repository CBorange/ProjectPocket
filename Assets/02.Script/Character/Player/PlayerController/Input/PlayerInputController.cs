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
#if UNITY_EDITOR
        currentInputSystem = GetComponent<InputSystem_PC>();
        currentInputSystem.Initialize(followCamera.MoveCamera, movementController.HorizontalMovement, movementController.Jump);
#endif
    }
    private void Update()
    {
#if UNITY_EDITOR
        currentInputSystem.FreeFrame_Update();
#endif
#if UNITY_ANDROID

#endif
    }
    private void FixedUpdate()
    {
#if UNITY_EDITOR
        currentInputSystem.FixedFrame_Update();
#endif
#if UNITY_ANDROID

#endif
    }
}
