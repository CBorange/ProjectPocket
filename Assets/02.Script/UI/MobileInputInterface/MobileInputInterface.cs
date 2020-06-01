using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileInputInterface : MonoBehaviour
{
    // Controller
    public PlayerMovementController MovementController;

    // UI
    public GameObject InteractButton;
    public Image InteractImage;

    // Data
    private Action interactAction;
    private Dictionary<string, Sprite> interactSpriteDic;
    public Sprite[] InteractSprties;

    public void Initialize()
    {
        interactSpriteDic = new Dictionary<string, Sprite>();
        interactSpriteDic.Add("NPC", InteractSprties[0]);
        interactSpriteDic.Add("Building", InteractSprties[1]);
        interactSpriteDic.Add("Resource_Axe", InteractSprties[2]);
        interactSpriteDic.Add("Resource_Pickaxe", InteractSprties[3]);
    }
    public void ChangeInteractAction(Action interactAction, string actionType)
    {
        if (interactAction == null)
        {
            InteractButton.gameObject.SetActive(false);
            return;
        }

        this.interactAction = interactAction;
        Sprite foundSprite;
        if (interactSpriteDic.TryGetValue(actionType, out foundSprite))
        {
            InteractImage.sprite = foundSprite;
            InteractButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log($"{actionType}에 해당하는 Interact UI 이미지가 없습니다.");
        }
    }
    // Button Action
    public void ExecuteInteract()
    {
        interactAction();
    }
    public void ExecuteAttack()
    {
        PlayerActManager.Instance.ExecuteAttack();
    }
    public void ExecuteJump()
    {
        MovementController.Jump();
    }
}
