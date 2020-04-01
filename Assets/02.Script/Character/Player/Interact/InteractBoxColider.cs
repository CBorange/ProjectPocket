using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBoxColider : MonoBehaviour
{
    public PlayerInputController inputController;
    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "NPC":
            inputController.InteractCommand_ChangedToNPC(other.GetComponent<NPC_Controller>().Interact);
                break;
            case "Resource":
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        inputController.InteractCommand_ChangedToAttack();
    }
}
