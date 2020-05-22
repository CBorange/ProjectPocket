using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InputSystem_Mobile : MonoBehaviour, InputSystem
{
    // Controller
    public FollowCamera PlayerCamera;
    public GameObject MobileInputInterfaceObj;
    public JoystickController joystickController;

    // Data
    private Action<float, float> moveCameraCallback;
    private Action<float, float> movePlayerCallback;
    private Action jumpCallback;
    private bool touchedBefore = false;

    public void Initialize(Action<float, float> moveCameraCallback, Action<float, float> movePlayerCallback, Action jumpCallback)
    {
        MobileInputInterfaceObj.gameObject.SetActive(true);

        this.moveCameraCallback = moveCameraCallback;
        this.movePlayerCallback = movePlayerCallback;
        this.jumpCallback = jumpCallback;

        joystickController.Initialize(movePlayerCallback);
    }
    public void FreeFrame_Update()
    {
        if (Input.touchCount > 0)
        {
            if (PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Death)
                return;
            if (UIPanelTurner.Instance.UIPanelCurrentOpen)
                return;
            for (int i = 0; i < Input.touches.Length; ++i)
            {
                Touch touch = Input.touches[i];
                float halfWidth = Screen.width * 0.5f;
                if (touch.phase == TouchPhase.Began) 
                {
                    if (touch.position.x < halfWidth && IsPossibleToMoveJoystick(touch))
                    {
                        joystickController.StartMove(touch.position);
                    }
                    if (touchedBefore)
                        RaycastObject(touch.position);
                    else
                    {
                        touchedBefore = true;
                        Invoke("ReleaseDobbleTouch", 0.25f);
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (Input.touchCount == 1)
                        joystickController.EndMove();
                }
            }
        }
    }
    public void FixedFrame_Update()
    {
        if (Input.touchCount > 0)
        {
            if (PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Death)
                return;
            if (UIPanelTurner.Instance.UIPanelCurrentOpen)
                return;
            moveCameraCallback(0, 0);
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
                            CameraInput(touch);
                    }
                    else if (touch.position.x < halfWidth)
                    {
                        joystickController.MoveJoystick(touch.position);
                    }
                }
            }
            
        }
        else
        {
            moveCameraCallback(0, 0);
            movePlayerCallback(0, 0);
        }
    }
    private bool IsPossibleToMoveJoystick(Touch touch)
    {
        if (touch.position.x > joystickController.JOYSTICK_HALF_WIDTH &&
                    (touch.position.y < (Screen.height - joystickController.JOYSTICK_HALF_HEIGHT)) &&
                    touch.position.y > joystickController.JOYSTICK_HALF_HEIGHT)
        {
            return true;
        }
        else
            return false;
    }
    private void CameraInput(Touch touch)
    {
        Vector2 deltaMove = touch.deltaPosition * touch.deltaTime;
        deltaMove *= -1;
        moveCameraCallback(deltaMove.x, deltaMove.y);
    }
    private void RaycastObject(Vector2 touchPos)
    {
        RaycastHit hit;

        int layerMask = (1 << LayerMask.NameToLayer("Resource")) + (1 << LayerMask.NameToLayer("Building")) + (1 << LayerMask.NameToLayer("NPC"));
        Vector3 screenTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, Camera.main.farClipPlane));
        Debug.DrawRay(PlayerCamera.transform.position, screenTouchPos * 100f, Color.blue, 1f);
        if (Physics.Raycast(PlayerCamera.transform.position, screenTouchPos, out hit, 100f, layerMask))
        {
            string tag = hit.collider.tag;
            switch (tag)
            {
                case "Resource":
                    hit.collider.transform.parent.GetComponent<ResourceController>().StartIteractWithResource();
                    break;
                case "Building":
                    hit.collider.GetComponent<BuildingController>().StartInteract();
                    break;
                case "NPC":
                    hit.collider.GetComponent<NPC_Controller>().Interact();
                    break;
            }
        }
    }
    private void ReleaseDobbleTouch()
    {
        touchedBefore = false;
    }

    public void ExecuteAttack()
    {
        PlayerActManager.Instance.ExecuteAttack();
    }
    public void ExecuteJump()
    {
        jumpCallback();
    }
}
