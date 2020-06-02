using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    // Data
    private ParticleSystem mySystem;
    private string particleKey;
    private bool isActive;
    private Action<string, ParticleEffect> returnToPoolCallback;

    public void Initialize(string particleKey, Action<string, ParticleEffect> returnToPoolCallback)
    {
        mySystem = GetComponent<ParticleSystem>();
        this.particleKey = particleKey;
        this.returnToPoolCallback = returnToPoolCallback;
    }
    public void StartParticle()
    {
        isActive = true;
        gameObject.SetActive(true);

        Invoke("EndParticle", mySystem.main.startLifetime.constant + 0.2f);
    }
    private void EndParticle()
    {
        if (!isActive)
            return;
        gameObject.SetActive(false);
        isActive = false;
        returnToPoolCallback(particleKey, this);
    }
    public void ForceEndParticle()
    {
        isActive = false;
        gameObject.SetActive(false);
        returnToPoolCallback(particleKey, this);
    }
}
