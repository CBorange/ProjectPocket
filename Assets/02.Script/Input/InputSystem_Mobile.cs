using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InputSystem_Mobile : MonoBehaviour, InputSystem
{
    // Controller
    public FollowCamera PlayerCamera;
    public PlayerMovementController MovementController;
    public MobileInputInterface InputInterface;
    public JoystickController joystickController;

    // Data
    private Action interactAction;
    public Action InteractAction
    {
        get { return interactAction; }
        set { interactAction = value; }
    }

    private int joystickTouchID = 0;

    public void Initialize()
    {
        InputInterface.Initialize();
        InputInterface.gameObject.SetActive(true);
        joystickController.Initialize();
    }
    public void FreeFrame_Update()
    {
        if (Input.touchCount > 0)
        {
            if (!PossibleToControll())
                return;
            for (int i = 0; i < Input.touches.Length; ++i)
            {
                Touch touch = Input.touches[i];
                float halfWidth = Screen.width * 0.5f;
                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x < halfWidth && joystickController.IsPossibleToMoveJoystick(touch.position))
                    {
                        joystickController.StartMove(touch.position);
                        joystickTouchID = touch.fingerId;
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (joystickController.JoystickIsActive &&
                        touch.fingerId == joystickTouchID) 
                        joystickController.EndMove();
                }
            }
        }
    }
    public void FixedFrame_Update()
    {
        if (Input.touchCount > 0)
        {
            if (!PossibleToControll())
                return;
            for (int i = 0; i < Input.touches.Length; ++i)
            {
                Touch touch = Input.touches[i];
                float halfWidth = Screen.width * 0.5f;
                if (touch.phase == TouchPhase.Moved ||
                    touch.phase == TouchPhase.Stationary)
                {
                    if (touch.position.x >= halfWidth)
                    {
                        if (UITouchStateContainer.Instance.PossibleToControll)
                        {
                            if (!joystickController.JoystickIsActive)
                                MovementController.HorizontalMovement(0, 0);
                            CameraInput(touch);
                        }
                    }
                    else if (touch.position.x < halfWidth)
                    {
                        PlayerCamera.MoveCamera(0, 0);
                        joystickController.MoveJoystick(touch.position);
                    }
                }
            }

        }
        else
        {
            PlayerCamera.MoveCamera(0, 0);
            MovementController.HorizontalMovement(0, 0);
        }
    }
    public void ChangeInteractAction(Action interactAction, string actionType)
    {
        this.interactAction = interactAction;
        InputInterface.ChangeInteractAction(interactAction, actionType);
    }

    private bool PossibleToControll()
    {
        if (PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Death ||
            UIPanelTurner.Instance.UIPanelCurrentOpen ||
            !UITouchStateContainer.Instance.PossibleToControll)
            return false;
        return true;
    }
    
    private void CameraInput(Touch touch)
    {
        Vector2 deltaMove = touch.deltaPosition * touch.deltaTime;
        deltaMove *= -1;
        PlayerCamera.MoveCamera(deltaMove.x, deltaMove.y);
    }

    
    
}
