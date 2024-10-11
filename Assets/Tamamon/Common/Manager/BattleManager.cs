using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Framework.Scene;

public class BattleManager : MonoBehaviourSingleton<BattleManager>
{
    
    public async UniTask LoadBattleScene(int enemyId)
    {
        await SceneManager.Instance.LoadSceneAsync("Battle", UnityEngine.SceneManagement.LoadSceneMode.Additive);

        // BattleScene‚ÌBattleController‚ðŽæ“¾
        BattleController controller = SceneManager.Instance.GetSceneObjectByName("Battle", "BattleController").GetComponent<BattleController>();

        controller.OnInitialize(enemyId);
    }

    public async UniTask UnLoadBattleScene()
    {
        await SceneManager.Instance.UnLoadSceneAsync("Battle");
    }
}