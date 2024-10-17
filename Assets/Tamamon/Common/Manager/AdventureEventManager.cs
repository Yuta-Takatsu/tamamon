using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// �A�h�x���`���[�p�[�g�ŋN����C�x���g�̊Ǘ��N���X
    /// </summary>
    public class AdventureEventManager : MonoBehaviourSingleton<BattleManager>
    {

        public override void Awake()
        {
            base.Awake();

            // BattleScene��BattleController���擾
            AdventureEventController controller = SceneManager.Instance.GetSceneObjectByName("AdventureEvent", "AdventureEventController").GetComponent<AdventureEventController>();
            controller.OnInitialize();
        }
    }
}