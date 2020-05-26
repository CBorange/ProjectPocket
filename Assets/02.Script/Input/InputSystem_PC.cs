using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputSystem_PC : MonoBehaviour, InputSystem
{
    // Controller
    public FollowCamera PlayerCamera;
    public PlayerMovementController MovementController;

    // Data
    private Action interactAction;
    public Action InteractAction
    {
        get { return interactAction; }
        set { interactAction = value; }
    }

    public void Initialize()
    {

    }
    public void FreeFrame_Update()
    {
        if (PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Death)
            return;
        PlayerActionInput();
        PlayerJumpInput();
    }
    public void FixedFrame_Update()
    {
        if (PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Death)
            return;
        CameraInput();
        PlayerMoveInput();
    }
    public void ChangeInteractAction(Action interactAction, string actionType)
    {
        this.interactAction = interactAction;
    }

    private void CameraInput()
    {
        float horAxis, verAxis;
        bool anyMouseButton = Input.GetMouseButton(1);
        horAxis = anyMouseButton ? Input.GetAxis("Mouse X") : 0f;
        verAxis = anyMouseButton ? Input.GetAxis("Mouse Y") : 0f;
        horAxis *= -1;
        verAxis *= -1;

        PlayerCamera.MoveCamera(horAxis, verAxis);
    }

    private void PlayerMoveInput()
    {
        float horMove, verMove;
        horMove = Input.GetAxisRaw("Horizontal");
        verMove = Input.GetAxisRaw("Vertical");

        MovementController.HorizontalMovement(horMove, verMove);
    }
    private void PlayerJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            MovementController.Jump();
    }

    private void PlayerActionInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerActManager.Instance.ExecuteAttack();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            interactAction?.Invoke();
        }
        
    }
}
