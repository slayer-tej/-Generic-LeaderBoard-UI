using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetBundles : MonoBehaviour
{
    [SerializeField]
    private string bundleUrl;
    [SerializeField]
    private string assetName;
    private AssetBundle remoteAssetBundle;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        using (WWW web = new WWW(bundleUrl))
        {
            yield return web;
            remoteAssetBundle = web.assetBundle;
            if (remoteAssetBundle == null)
            {
                Debug.LogError("Failed to download AssetBundle!");
                yield break;
            }
            Debug.Log("Asset Bundle Successfully Loaded");
        }
    }

    public Object GetBundleObject()
    {
        Object obj = remoteAssetBundle.LoadAsset<Object>(assetName);
        return obj;
    }
}