using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitCamera : MonoBehaviour
{
    public Camera CurrentCam;

    public void LookTarget(Transform targetTransform)
    {
        Vector3 newCamPos = targetTransform.position + (targetTransform.forward * 3);
        transform.position = newCamPos;

        Vector3 beforeCamRot = transform.rotation.eulerAngles;
        transform.LookAt(targetTransform);
        transform.rotation = Quaternion.Euler(beforeCamRot.x, transform.rotation.eulerAngles.y, beforeCamRot.z);

        transform.position = new Vector3(transform.position.x,
                                         newCamPos.y,
                                         transform.position.z);
    }
}
