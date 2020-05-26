using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCheckBox : MonoBehaviour
{
    // Controller
    public PlayerInputController InputController;

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Resource":
                ResourceController controller = other.transform.parent.GetComponent<ResourceController>();
                InputController.ChangeInteractAction(controller.StartIteractWithResource,
                    $"Resource_{controller.CurrentData.CanGatheringTool}");
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
        InputController.ChangeInteractAction(null, string.Empty);
    }
}
