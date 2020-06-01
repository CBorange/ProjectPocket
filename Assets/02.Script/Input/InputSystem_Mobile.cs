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

    private int joystickTouchID = -1;
    private int cameraTouchID = -1;

    public void Initialize()
    {
        InputInterface.Initialize();
        InputInterface.gameObject.SetActive(true);
        joystickController.Initialize();
    }
    public void FreeFrame_Update()
    {
        if (Input.touchCount > 2)
            return;
        if (Input.touchCount > 0)
        {
            if (PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Death ||
                UIPanelTurner.Instance.UIPanelCurrentOpen)
                return;
            for (int i = 0; i < Input.touches.Length; ++i)
            {
                Touch touch = Input.touches[i];
                float halfWidth = Screen.width * 0.5f;
                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x < halfWidth && joystickController.IsPossibleToMoveJoystick(touch.position) &&
                        !IsPointerOverUIObject(touch.position))
                    {
                        joystickController.StartMove(touch.position);
                        joystickTouchID = touch.fingerId;
                    }
                    else if (touch.position.x >= halfWidth) 
                    {
                        if (!IsPointerOverUIObject(touch.position))
                        {
                            cameraTouchID = touch.fingerId;
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (joystickController.JoystickIsActive &&
                        touch.fingerId == joystickTouchID)
                    {
                        MovementController.HorizontalMovement(0, 0);
                        joystickController.EndMove();
                        joystickTouchID = -1;
                    }
                    else if (touch.fingerId == cameraTouchID)
                    {
                        cameraTouchID = -1;
                    }
                }
            }
        }
        if (Input.touchCount == 0)
        {
            joystickController.EndMove();
        }
    }
    private bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition
            = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();


        EventSystem.current
        .RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
    public void FixedFrame_Update()
    {
        if (Input.touchCount > 2)
            return;
        if (Input.touchCount > 0)
        {
            if (PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Death ||
                UIPanelTurner.Instance.UIPanelCurrentOpen)
                return;
            Vector2 camDeltaPos = Vector2.zero;
            for (int i = 0; i < Input.touches.Length; ++i)
            {
                Touch touch = Input.touches[i];
                if (touch.phase == TouchPhase.Moved ||
                    touch.phase == TouchPhase.Stationary)
                {
                    if (touch.fingerId == cameraTouchID)
                    {
                        camDeltaPos = touch.deltaPosition;
                    }
                    else if (touch.fingerId == joystickTouchID) 
                    {
                        if (cameraTouchID == -1)
                            camDeltaPos = Vector2.zero;
                        joystickController.MoveJoystick(touch.position);
                    }
                }
            }
            CameraInput(camDeltaPos);
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
    
    private void CameraInput(Vector2 deltaPos)
    {
        Vector2 deltaMove = deltaPos * Time.deltaTime;
        deltaMove *= -1;
        PlayerCamera.MoveCamera(deltaMove.x, deltaMove.y);
    }

    
    
}
