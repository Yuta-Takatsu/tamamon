using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        var scene = SceneManager.GetSceneByName("Boot");

        if (!scene.IsValid())
        {
            SceneManager.LoadSceneAsync("Boot", LoadSceneMode.Additive);
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.UnloadSceneAsync("Boot");
    }
}