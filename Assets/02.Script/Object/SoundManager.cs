using Boo.Lang.Environments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoopInfo
{
    public string LoopKey;
    public AudioSource Source;
    public float LoopTime;
    public bool isActive;

    public AudioLoopInfo(string loopKey, AudioSource source, float loopTime, bool active)
    {
        LoopKey = loopKey;
        Source = source;
        LoopTime = loopTime;
        isActive = active;
    }
}
public class SoundManager : MonoBehaviour
{
    // Data
    private Dictionary<string, AudioSource> audioDic;
    private Dictionary<string, AudioLoopInfo> loopAudioDic;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        loopAudioDic = new Dictionary<string, AudioLoopInfo>();
        audioDic = new Dictionary<string, AudioSource>();
        for (int i = 0; i < transform.childCount; ++i)
            audioDic.Add(transform.GetChild(i).name, transform.GetChild(i).GetComponent<AudioSource>());
    }

    public void PlayOneShot(string audioKey)
    {
        AudioSource foundSource = null;
        if (audioDic.TryGetValue(audioKey, out foundSource))
        {
            foundSource.PlayOneShot(foundSource.clip);
        }
        else
        {
            Debug.Log($"SoundManager : {audioKey} 를 key로 사용하는 AudioSource 없음");
            return;
        }
    }
    public void StartAudioLoop(string loopKey, string audioKey, float loopTime)
    {
        AudioLoopInfo foundLoop;
        if (loopAudioDic.TryGetValue(loopKey, out foundLoop))
        {
            return;
        }
        else
        {
            AudioSource foundSource = null;
            if (audioDic.TryGetValue(audioKey, out foundSource))
            {
                AudioLoopInfo newLoop = new AudioLoopInfo(loopKey, foundSource, loopTime, true);
                loopAudioDic.Add(loopKey, newLoop);
                StartCoroutine(IE_LoopAudio(newLoop));
            }
            else
            {
                Debug.Log($"SoundManager : {audioKey} 를 key로 사용하는 AudioSource 없음");
                return;
            }
        }
    }
    public void StopAudioLoop(string loopKey)
    {
        AudioLoopInfo foundLoop;
        if (loopAudioDic.TryGetValue(loopKey, out foundLoop))
        {
            foundLoop.isActive = false;
        }
        else
        {
            return;
        }
    }
    private IEnumerator IE_LoopAudio(AudioLoopInfo loopInfo)
    {
        float elapsedTime = 0f;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
            
            if (!loopInfo.isActive)
            {
                loopAudioDic.Remove(loopInfo.LoopKey);
                yield break;
            }
            if (elapsedTime >= loopInfo.LoopTime)
            {
                loopInfo.Source.PlayOneShot(loopInfo.Source.clip);
                elapsedTime = 0f;
            }
        }
    }
}
