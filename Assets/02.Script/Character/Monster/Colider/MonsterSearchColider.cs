using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterSearchColider : MonoBehaviour
{
    // Data
    public string targetTag;
    public SphereCollider IdentifyColider;

    private Action<Transform> targetEnterCallback;
    private Action targetExitCallback;
    public void Initiailize(Action<Transform> enterCallback, Action exitCallback, float identifyRange)
    {
        targetEnterCallback = enterCallback;
        targetExitCallback = exitCallback;
        IdentifyColider.radius = identifyRange;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(targetTag))
        {
            targetEnterCallback(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(targetTag))
        {
            targetExitCallback();
        }
    }
}
