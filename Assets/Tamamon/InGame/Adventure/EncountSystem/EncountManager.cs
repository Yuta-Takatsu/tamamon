using UnityEngine;
using Framework;
using System;

namespace Tamamon.Ingame.Adventure
{
    public class EncountManager : MonoBehaviourSingleton<EncountManager>
    {
        // 前判定時にエンカウントしなかった場合、メモリをしておくことでエンカウントしやすくする
        public int m_encountMemory = 0;
        public int m_encountMemoryIncrease = 1;
        // エンカウントカウンタ最大値
        // （実態としてはある程度定数と考えられるが、パラメータで書き換える可能性を考慮して一旦編集可能にしている）
        public int m_encountCounterMaximum = 256;
        // 最終判定時のエンカウント判定結果（デバッグ用）
        public int m_lastEncountCounter = 0;
        // 最終判定時のエンカウント判定時の閾値（デバッグ用）
        public int m_lastEncountThroughold = 0;

        public override void Awake()
        {
            base.Awake();
        }

        public bool CheckEncount(int standingCellEncounterThroughold = 5)
        {
            int encountRate = UnityEngine.Random.Range(0,m_encountCounterMaximum);
            int encounterThroughold = (standingCellEncounterThroughold + m_encountMemory);
            bool encountResult = (encountRate < standingCellEncounterThroughold);
            // デバッグ用のエンカウント数値表示の変更
            m_lastEncountCounter = encountRate;
            m_lastEncountThroughold = encounterThroughold;
            
            // エンカウントしなかった場合は次のエンカウント率を上げるために増分を増やす
            if(!encountResult)
            {
                m_encountMemory = Math.Min((m_encountMemory + m_encountMemoryIncrease), m_encountCounterMaximum);
            }
            else
            {
                m_encountMemory = 0;
            }
            return encountResult;
        }
    }
}