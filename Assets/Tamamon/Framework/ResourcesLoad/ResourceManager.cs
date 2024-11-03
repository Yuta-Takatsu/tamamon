using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;


using Framework;

namespace Framework
{
    /// <summary>
    /// リソース管理クラス
    /// </summary>
    public class ResourceManager : MonoBehaviourSingleton<ResourceManager>
    {
        // Start is called before the first frame update
        public override void Awake()
        {
        
        }

        // Update is called once per frame

        /*
        public async UniTask RequestLoad(CancellationToken m_cansellationToken)
        {
             await Addressables.LoadAssetAsync<Sprite>("Assets/AddressableAssets/Tamamon/9.png").Completed += handle => {
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
        */
    }
}
