using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// アドベンチャーパートで起こるイベントの管理クラス
    /// </summary>
    public class AdventureEventManager : MonoBehaviourSingleton<BattleManager>
    {

        public override void Awake()
        {
            base.Awake();

            // BattleSceneのBattleControllerを取得
            AdventureEventController controller = SceneManager.Instance.GetSceneObjectByName("AdventureEvent", "AdventureEventController").GetComponent<AdventureEventController>();
            controller.OnInitialize();
        }
    }
}