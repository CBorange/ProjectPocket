using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCheckBox : MonoBehaviour
{
    // Controller
    public PlayerInputController InputController;
    private ResourceController interactableResource;

    public void UnEquipWeapon()
    {
        InputController.ChangeInteractAction(null, string.Empty);
    }
    public void EquipWeapon()
    {
        if (interactableResource != null)
        {
            if (!interactableResource.IsPossibleToInteract())
                InputController.ChangeInteractAction(null, string.Empty);
            else
            {
                InputController.ChangeInteractAction(interactableResource.StartIteractWithResource,
                    $"Resource_{interactableResource.CurrentData.CanGatheringTool}");
            }
        }
    }
    public void ResetInteractAction()
    {
        interactableResource = null;
        InputController.ChangeInteractAction(null, string.Empty);
    }
    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Resource":
                interactableResource = other.transform.parent.GetComponent<ResourceController>();
                if (!interactableResource.IsPossibleToInteract())
                    return;
                InputController.ChangeInteractAction(interactableResource.StartIteractWithResource,
                    $"Resource_{interactableResource.CurrentData.CanGatheringTool}");
                break;
            case "Building":
                InputController.ChangeInteractAction(other.GetComponent<BuildingController>().StartInteract,
                    "Building");
                break;
            case "NPC":
                InputController.ChangeInteractAction(other.GetComponent<NPC_Controller>().Interact,
                    "NPC");
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        ResetInteractAction();
    }
}
