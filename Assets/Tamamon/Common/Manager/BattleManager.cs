using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Framework;

public class BattleManager : MonoBehaviourSingleton<BattleManager>
{
    
    public async UniTask LoadScene(int enemyId, UnityEngine.SceneManagement.LoadSceneMode mode = UnityEngine.SceneManagement.LoadSceneMode.Additive)
    {
        await SceneManager.Instance.LoadSceneAsync("Battle", mode);

        // BattleScene‚ÌBattleController‚ðŽæ“¾
        BattleController controller = SceneManager.Instance.GetSceneObjectByName("Battle", "BattleController").GetComponent<BattleController>();

        controller.OnInitialize(enemyId);
    }

    public async UniTask UnLoadScene()
    {
        await SceneManager.Instance.UnLoadSceneAsync("Battle");
    }
}