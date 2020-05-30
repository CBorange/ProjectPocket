using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BundleMenu : MonoBehaviour
{
    [MenuItem("Bundles/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles($"{Application.streamingAssetsPath}/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
    }
}
