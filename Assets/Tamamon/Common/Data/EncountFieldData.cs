using UnityEngine;

namespace Tamamon.Data
{
    [CreateAssetMenu(menuName = "Tamamon/Data/EncountFieldData")]
    public class EncountFieldData : ScriptableObject
    {
        // エンカウント閾値
        // 乱数 < エンカウント閾値（＋エンカウント蓄積値）でエンカウントするため
        // この数値が大きいほどエンカウントしやすくなる
        public int m_encounterRate = 5;
    }

}