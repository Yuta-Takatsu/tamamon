using UnityEngine;
using Framework;
using System;

namespace Tamamon.Ingame.Adventure
{
    public class EncountManager : MonoBehaviourSingleton<EncountManager>
    {
        // �O���莞�ɃG���J�E���g���Ȃ������ꍇ�A�����������Ă������ƂŃG���J�E���g���₷������
        public int m_encountMemory = 0;
        public int m_encountMemoryIncrease = 1;
        // �G���J�E���g�J�E���^�ő�l
        // �i���ԂƂ��Ă͂�����x�萔�ƍl�����邪�A�p�����[�^�ŏ���������\�����l�����Ĉ�U�ҏW�\�ɂ��Ă���j
        public int m_encountCounterMaximum = 256;
        // �ŏI���莞�̃G���J�E���g���茋�ʁi�f�o�b�O�p�j
        public int m_lastEncountCounter = 0;
        // �ŏI���莞�̃G���J�E���g���莞��臒l�i�f�o�b�O�p�j
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
            // �f�o�b�O�p�̃G���J�E���g���l�\���̕ύX
            m_lastEncountCounter = encountRate;
            m_lastEncountThroughold = encounterThroughold;
            
            // �G���J�E���g���Ȃ������ꍇ�͎��̃G���J�E���g�����グ�邽�߂ɑ����𑝂₷
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