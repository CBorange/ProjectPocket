using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputController : MonoBehaviour
{
    public PlayerMovementController movementController;
    public FollowCamera followCamera;
    private Action interactAction;

    private void Awake()
    {
        interactAction = PlayerActManager.Instance.AttackOnEquipmentWeapon;
    }
    private void FixedUpdate()
    {
#if UNITY_EDITOR
        PC_CameraInput();
        PC_PlayerMoveInput();
        PC_PlayerActionInput();
#endif
#if UNITY_ANDROID

#endif
    }

    private void PC_CameraInput()
    {
        float horAxis, verAxis;
        bool anyMouseButton = Input.GetMouseButton(1);
        horAxis = anyMouseButton ? Input.GetAxis("Mouse X") : 0f;
        verAxis = anyMouseButton ? Input.GetAxis("Mouse Y") : 0f;
        horAxis *= -1;
        verAxis *= -1;

        followCamera.MoveCamera(horAxis, verAxis);
    }

    private void PC_PlayerMoveInput()
    {
        float horMove, verMove;
        horMove = Input.GetAxisRaw("Horizontal");
        verMove = Input.GetAxisRaw("Vertical");

        movementController.HorizontalMovement(horMove, verMove);

        if (Input.GetKeyDown(KeyCode.Space))
            movementController.Jump();
    }

    private void PC_PlayerActionInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
            interactAction();
    }

    // InteractChange Callback
    public void InteractCommand_ChangedToNPC(Action npcInteractAction)
    {
        interactAction = npcInteractAction;
    }
    public void InteractCommand_ChangedToResource(Action resourceInteractAction)
    {

    }
    public void InteractCommand_ChangedToAttack()
    {
        interactAction = PlayerActManager.Instance.AttackOnEquipmentWeapon;
    }
}
