using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine.Networking;
#endif

#if UNITY_EDITOR
public class LoadMasterData : Editor
{
    [MenuItem("Tools/Tamamon/MasterData/ImportMasterData")]
    static void Init()
    {
        string url = MasterDataDefine.MASTER_MONSTERS_URL;
        string assetfile = "Assets/Resources/MasterData/IMPORT_TamamonMaster.asset";

        StartCorountine(DownloadAndImport(url, assetfile));
    }

    static IEnumerator DownloadAndImport(string url, string assetfile)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        while (www.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }

        if (www.error != null)
        {
            Debug.Log("UnityWebRequest.error:" + www.error);
        }
        else if (www.downloadHandler.text == "" || www.downloadHandler.text.IndexOf("<!DOCTYPE") != -1)
        {
            Debug.Log("Uknown Format:" + www.downloadHandler.text);
        }
        else
        {
            ImportTamamonMasterData(www.downloadHandler.text, assetfile);
#if DEBUG_LOG || UNITY_EDITOR
            Debug.Log("Imported Asset: " + assetfile);
#endif
        }
    }

    static void ImportTamamonMasterData(string text, string assetfile)
    {
        List<string[]> rows = CSVSerializer.ParseCSV(text);
        if (rows != null)
        {
            MasterDataDefine.masClass_monsters md = AssetDatabase.LoadAssetAtPath<MasterDataDefine.masClass_monsters>(assetfile);
            if (md == null)
            {
                md = new MasterDataDefine.masClass_monsters();
                AssetDatabase.CreateAsset(md, assetfile);
            }
            md.m_Items = CSVSerializer.Deserialize<MasterDataDefine.masClass_monsters.masStruct_monstersInfo>(rows);

            EditorUtility.SetDirty(md);
            AssetDatabase.SaveAssets();
        }
    }

    // coroutine for unity editor

    static void StartCorountine(IEnumerator routine)
    {
        _coroutine.Add(routine);
        if (_coroutine.Count == 1)
            EditorApplication.update += ExecuteCoroutine;
    }
    static List<IEnumerator> _coroutine = new List<IEnumerator>();
    static void ExecuteCoroutine()
    {
        for (int i = 0; i < _coroutine.Count;)
        {
            if (_coroutine[i] == null || !_coroutine[i].MoveNext())
                _coroutine.RemoveAt(i);
            else
                i++;
        }
        if (_coroutine.Count == 0)
            EditorApplication.update -= ExecuteCoroutine;
    }
}
#endif