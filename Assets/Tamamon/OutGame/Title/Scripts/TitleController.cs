using UnityEngine;
using Cysharp.Threading.Tasks;
using Tamamon.Framework;

namespace Tamamon.OutGame
{
    public class TitleController : MonoBehaviour
    {

        [SerializeField]
        private TitleView m_titleView = default;

        void Start()
        {
            m_titleView.OnInitialize().Forget();

            // BGMçƒê∂
            SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.Title);
        }
    }
}