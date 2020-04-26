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
    private Action interactAction;

    private void Awake()
    {
        interactAction = PlayerActManager.Instance.ExecuteAttack;
#if UNITY_EDITOR
        currentInputSystem = GetComponent<InputSystem_PC>();
        currentInputSystem.Initialize(followCamera.MoveCamera, movementController.HorizontalMovement, movementController.Jump, interactAction);
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

    // InteractChange Callback
    public void InteractCommand_ChangedToNPC(Action npcInteractAction)
    {
        interactAction = npcInteractAction;
        currentInputSystem.ChangeActionEvent(interactAction);
    }
    public void InteractCommand_ChangedToResource(Action resourceInteractAction)
    {

    }
    public void InteractCommand_ChangedToAttack()
    {
        interactAction = PlayerActManager.Instance.ExecuteAttack;
        currentInputSystem.ChangeActionEvent(interactAction);
    }
}
