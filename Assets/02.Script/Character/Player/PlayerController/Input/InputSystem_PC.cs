using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputSystem_PC : MonoBehaviour, InputSystem
{
    public FollowCamera PlayerCamera;
    // Data
    private Action<float, float> moveCameraCallback;
    private Action<float, float> movePlayerCallback;
    private Action jumpCallback;
    private Action actionCallback;
    private bool mouseClicked;

    public void Initialize(Action<float, float> moveCameraCallback, Action<float, float> movePlayerCallback, Action jumpCallback, Action actionCallback)
    {
        this.moveCameraCallback = moveCameraCallback;
        this.movePlayerCallback = movePlayerCallback;
        this.jumpCallback = jumpCallback;
        this.actionCallback = actionCallback;

        mouseClicked = false;
    }
    public void FreeFrame_Update()
    {
        PlayerActionInput();
        PlayerJumpInput();
    }
    public void FixedFrame_Update()
    {
        CameraInput();
        PlayerMoveInput();
    }
    public void ChangeActionEvent(Action callback)
    {
        actionCallback = callback;
    }

    private void CameraInput()
    {
        float horAxis, verAxis;
        bool anyMouseButton = Input.GetMouseButton(1);
        horAxis = anyMouseButton ? Input.GetAxis("Mouse X") : 0f;
        verAxis = anyMouseButton ? Input.GetAxis("Mouse Y") : 0f;
        horAxis *= -1;
        verAxis *= -1;

        moveCameraCallback(horAxis, verAxis);
    }

    private void PlayerMoveInput()
    {
        float horMove, verMove;
        horMove = Input.GetAxisRaw("Horizontal");
        verMove = Input.GetAxisRaw("Vertical");

        movePlayerCallback(horMove, verMove);
    }
    private void PlayerJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            jumpCallback();
    }

    private void PlayerActionInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            actionCallback();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (mouseClicked)
            {
                
                RaycastToObject();
            }
            else
            {
                mouseClicked = true;
                Invoke("ReleaseMouseDoubleClickWait", 0.2f);
            }
        }
    }
    private void RaycastToObject()
    {
        RaycastHit hit;

        int layerMask = (1 << LayerMask.NameToLayer("Resource")) + (1 << LayerMask.NameToLayer("Building"));
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));
        Debug.DrawRay(PlayerCamera.transform.position, mousePos * 100f, Color.blue, 1f);
        if (Physics.Raycast(PlayerCamera.transform.position, mousePos, out hit, 100f, layerMask)) 
        {
            string tag = hit.collider.tag;
            switch(tag)
            {
                case "Resource":
                    hit.collider.transform.parent.GetComponent<ResourceController>().StartIteractWithResource();
                    break;
                case "Building":
                    hit.collider.GetComponent<BuildingController>().StartInteract();
                    break;
            }
        }
    }
    private void ReleaseMouseDoubleClickWait()
    {
        mouseClicked = false;
    }
}
