using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadDll : MonoBehaviour
{
    public Button relaodDll;
    public const string nextKey="test4";
    private int value = 0;
        void Start()
    {
        StartCoroutine(DownLoadDlls(this.StartGame));
        relaodDll.onClick.AddListener((() =>
        {
            if (value==0)
            {
                value = 1;
                PlayerPrefs.SetInt(nextKey,1);
                Debug.Log("set 1");
            }
            else
            {
                value = 0;
                PlayerPrefs.SetInt(nextKey,0);
                Debug.Log("set 0");
            }


        }));
    }

    private static Dictionary<string, byte[]> s_abBytes = new Dictionary<string, byte[]>();

    public static byte[] GetAbBytes(string dllName)
    {
        return s_abBytes[dllName];
    }

    private string GetWebRequestPath(string asset)
    {
        var path = $"{Application.streamingAssetsPath}/{asset}";
        if (!path.Contains("://"))
        {
            path = "file://" + path;
        }
        return path;
    }

    IEnumerator DownLoadDlls(Action onDownloadComplete)
    {
        var abs = new string[]
        {
            "common",
            "common1",

        };
        foreach (var ab in abs)
        {
            string dllPath = GetWebRequestPath(ab);
            Debug.Log($"start download ab:{ab}");
            UnityWebRequest www = UnityWebRequest.Get(dllPath);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Or retrieve results as binary data
                byte[] abBytes = www.downloadHandler.data;
                Debug.Log($"dll:{ab}  size:{abBytes.Length}");
                s_abBytes[ab] = abBytes;
            }
        }

        onDownloadComplete();
    }


    void StartGame()
    {
        LoadGameDll();
        RunMain();
    }

    private System.Reflection.Assembly gameAss;

    public static AssetBundle AssemblyAssetBundle { get; private set; }

    private void LoadGameDll()
    {
        AssetBundle dllAB=null;

        if (PlayerPrefs.GetInt(nextKey)==1)
        {
            dllAB= AssemblyAssetBundle = AssetBundle.LoadFromMemory(GetAbBytes("common1"));
            Debug.Log("common1 Dll");

        }
        else
        {
            dllAB= AssemblyAssetBundle = AssetBundle.LoadFromMemory(GetAbBytes("common"));
            Debug.Log("common Dll");

        }
#if !UNITY_EDITOR
        TextAsset dllBytes1 = dllAB.LoadAsset<TextAsset>("HotFix.dll.bytes");
        System.Reflection.Assembly.Load(dllBytes1.bytes);
        TextAsset dllBytes2 = dllAB.LoadAsset<TextAsset>("HotFix2.dll.bytes");
        gameAss = System.Reflection.Assembly.Load(dllBytes2.bytes);
#else
        gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "HotFix2");
#endif

        GameObject testPrefab = GameObject.Instantiate(dllAB.LoadAsset<UnityEngine.GameObject>("HotUpdatePrefab.prefab"));
    }

    public void RunMain()
    {
        if (gameAss == null)
        {
            UnityEngine.Debug.LogError("dll未加载");
            return;
        }
        var appType = gameAss.GetType("App");
        var mainMethod = appType.GetMethod("Main");
        mainMethod.Invoke(null, null);
    }
}
