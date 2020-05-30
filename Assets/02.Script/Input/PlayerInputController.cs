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

    // Data
    public bool ForceMode_Mobile;

    private void Start()
    {
        if (ForceMode_Mobile)
        {
            currentInputSystem = GetComponent<InputSystem_Mobile>();
            currentInputSystem.Initialize();
            return;
        }
#if UNITY_STANDALONE
        currentInputSystem = GetComponent<InputSystem_PC>();
        currentInputSystem.Initialize();
        return;
#endif
#if UNITY_EDITOR
        currentInputSystem = GetComponent<InputSystem_PC>();
        currentInputSystem.Initialize();
        return;
#endif
#if UNITY_ANDROID
        currentInputSystem = GetComponent<InputSystem_Mobile>();
        currentInputSystem.Initialize();
        return;
#endif
    }
    public void ChangeInteractAction(Action interactAction, string actionType)
    {
        currentInputSystem.ChangeInteractAction(interactAction, actionType);
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
