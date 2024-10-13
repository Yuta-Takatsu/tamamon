using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Test_AddressableRemoteLoad : MonoBehaviour
{
   [SerializeField] private SpriteRenderer m_Image;
   
   private AsyncOperationHandle<Sprite> m_SpriteHandle;

   private void Start() {
       Addressables.LoadAssetAsync<Sprite>("Assets/AddressableAssets/Tamamon/9.png").Completed += handle => {
           m_SpriteHandle = handle;
           if (handle.Result == null) {
               Debug.Log("Load Error");
               return;
           }
           m_Image.sprite = handle.Result;
       };
   }

   private void OnDestroy() {
       if (m_SpriteHandle.IsValid()) Addressables.Release(m_SpriteHandle);
   }
}