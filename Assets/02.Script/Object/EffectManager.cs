using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // Data
    public GameObject HitDamageTextPrefab;
    private Transform worldCanvas;
    private List<HitDamageText> deActiveHitDamageTextPool;
    private List<HitDamageText> activeHitDamageTextPool;
    private Dictionary<string, List<ParticleEffect>> deactiveParticlePoolDic;
    private Dictionary<string, List<ParticleEffect>> activeParticlePoolDic;

    private void Start()
    {
        Init_ParticleEffect();
    }
    private void Init_ParticleEffect()
    {
        deactiveParticlePoolDic = new Dictionary<string, List<ParticleEffect>>();
        activeParticlePoolDic = new Dictionary<string, List<ParticleEffect>>();

        for (int rootIdx = 0; rootIdx < transform.childCount; ++rootIdx)
        {
            Transform poolTrans = transform.GetChild(rootIdx);

            List<ParticleEffect> deActivePool = new List<ParticleEffect>();
            for (int particleIdx = 0; particleIdx < poolTrans.childCount; ++particleIdx)
            {
                ParticleEffect newEffect = poolTrans.GetChild(particleIdx).GetComponent<ParticleEffect>();
                newEffect.Initialize(poolTrans.name, OverParticleReturnToPool);
                deActivePool.Add(newEffect);
            }
            deactiveParticlePoolDic.Add(poolTrans.name, deActivePool);

            List<ParticleEffect> activePool = new List<ParticleEffect>();
            activeParticlePoolDic.Add(poolTrans.name, activePool);
        }
    }

    public void PlayParticle(string key)
    {
        List<ParticleEffect> deActivePool = null;
        List<ParticleEffect> activePool = null;

        if (!deactiveParticlePoolDic.TryGetValue(key, out deActivePool))
            Debug.Log($"EffectManager : {key} 에 해당하는 Deactive Particle Pool 이 없습니다.");
        if (!activeParticlePoolDic.TryGetValue(key, out activePool))
            Debug.Log($"EffectManager : {key} 에 해당하는 Active Particle Pool 이 없습니다.");

        while (true)
        {
            if (deActivePool.Count > 0)
            {
                deActivePool[0].StartParticle();
                activePool.Add(deActivePool[0]);
                deActivePool.RemoveAt(0);
                return;
            }
            else
                activePool[0].ForceEndParticle();
        }
    }
    public void UseTextEffect()
    {
        worldCanvas = GameObject.Find("UICanvas_World").transform;
        deActiveHitDamageTextPool = new List<HitDamageText>();
        for (int i = 0; i < 5; ++i)
        {
            HitDamageText newText = Instantiate(HitDamageTextPrefab, worldCanvas).GetComponent<HitDamageText>();
            newText.gameObject.SetActive(false);
            newText.Initialize(OverHitTextReturnToPool, transform.parent);
            deActiveHitDamageTextPool.Add(newText);
        }
        activeHitDamageTextPool = new List<HitDamageText>();
    }
    public void PlayHitTextEffect(float damage, Color color)
    {
        while (true)
        {
            if (deActiveHitDamageTextPool.Count > 0)
            {
                deActiveHitDamageTextPool[0].Play(damage, color);
                activeHitDamageTextPool.Add(deActiveHitDamageTextPool[0]);
                deActiveHitDamageTextPool.RemoveAt(0);
                return;
            }
            else
                activeHitDamageTextPool[0].ForceEndMove();
        }
    }

    private void OverParticleReturnToPool(string key, ParticleEffect effect)
    {
        List<ParticleEffect> deActivePool = null;
        List<ParticleEffect> activePool = null;

        if (!deactiveParticlePoolDic.TryGetValue(key, out deActivePool))
            Debug.Log($"EffectManager : {key} 에 해당하는 Deactive Particle Pool 이 없습니다.");
        if (!activeParticlePoolDic.TryGetValue(key, out activePool))
            Debug.Log($"EffectManager : {key} 에 해당하는 Active Particle Pool 이 없습니다.");

        deActivePool.Add(effect);
        activePool.Remove(effect);
    }
    private void OverHitTextReturnToPool(HitDamageText text)
    {
        deActiveHitDamageTextPool.Add(text);
        activeHitDamageTextPool.Remove(text);
    }
}
