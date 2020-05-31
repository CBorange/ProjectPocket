using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleCacher : MonoBehaviour
{
    #region Singleton
    private static AssetBundleCacher instance;
    public static AssetBundleCacher Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<AssetBundleCacher>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("AssetBundleCacher").AddComponent<AssetBundleCacher>();
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
        var objs = FindObjectsOfType<AssetBundleCacher>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        bundles = new Dictionary<string, AssetBundle>();
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    // Data
    private Dictionary<string, AssetBundle> bundles;

    public bool HasAleadyCachingBundle(string name)
    {
        if (bundles.ContainsKey(name))
            return true;
        return false;
    }
    public Object LoadAndGetAsset(string bundleName, string assetName)
    {
        AssetBundle bundle = null;
        if (!bundles.TryGetValue(bundleName, out bundle))
        {
            bundle = AssetBundle.LoadFromFile($"{Application.streamingAssetsPath}/AssetBundles/{bundleName}");
            bundles.Add(bundleName, bundle);
        }
        return bundle.LoadAsset(assetName);
    }
    public AssetBundle LoadAndGetBundle(string bundleName)
    {
        AssetBundle bundle = null;
        if (!bundles.TryGetValue(bundleName, out bundle))
        {
            bundle = AssetBundle.LoadFromFile($"{Application.streamingAssetsPath}/AssetBundles/{bundleName}");
            bundles.Add(bundleName, bundle);
        }
        return bundle;
    }
    public void CachingBundle(AssetBundle bundle, string name)
    {
        bundles.Add(name, bundle);
    }
    public AssetBundle GetBundle(string name)
    {
        AssetBundle bundle = null;
        if (bundles.TryGetValue(name, out bundle))
        {
            return bundle;
        }
        else
        {
            Debug.Log($"AssetBundleCacher : 캐싱되지 않은 번들을 Get 하려 했습니다. [{name}]");
            return null;
        }
    }
}
