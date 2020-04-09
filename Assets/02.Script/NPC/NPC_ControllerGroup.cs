using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_ControllerGroup : MonoBehaviour
{
    #region Singleton
    private static NPC_ControllerGroup instance;
    public static NPC_ControllerGroup Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<NPC_ControllerGroup>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("NPC_ControllerGroup").AddComponent<NPC_ControllerGroup>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<NPC_ControllerGroup>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    // Data
    public NPC_Controller[] npcDatas;
    private Dictionary<int, NPC_Controller> npcControllers;
    public Dictionary<int, NPC_Controller> NpcControllers
    {
        get { return npcControllers; }
    }

    private void Start()
    {
        // Copy ArrayData To Dictionary
        npcControllers = new Dictionary<int, NPC_Controller>();
        for (int i = 0; i < npcDatas.Length; ++i)
        {
            npcControllers.Add(npcDatas[i].NPCCode, npcDatas[i]);
            npcDatas[i].Initialize();
        }
    }

    // Method When PlayerQuest State was Changed
    public void QuestStateWasChanged()
    {
        foreach (var kvp in npcControllers)
        {
            kvp.Value.QuestMarkerChangeAccordingToState();
        }
    }
}
