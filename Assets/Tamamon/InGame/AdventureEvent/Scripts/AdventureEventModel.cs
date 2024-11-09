using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// �A�h�x���`���[�p�[�g��model�N���X
    /// </summary>
    public class AdventureEventModel
    {
        public enum CommandEnum
        {
            Talk = 0,
            SelectStart = 1,
            SelectOr = 2,
            SelectEnd = 3,
            Shop = 4,
            Menu = 5,
            Heal = 6,
            Encount = 7,
            Battle = 8,
            ItemGet = 9,
            TamamonGet = 10,
            Hidden = 11,
            AutoMove = 12,
            Warp = 13,
            FadeIn = 14,
            FadeOut = 15,
            Movie = 16,
            Bgm = 17,
            Se = 18,
            LoopPoint = 19,
            Loop = 20,
            GimmickStart = 21,
            GimmickEnd = 22,
        }

        public Dictionary<CommandEnum, string> CommandTexts { get; private set; }
            = new Dictionary<CommandEnum, string>()
        {
            { CommandEnum.Talk, "" },
            { CommandEnum.SelectStart, "select_start" },
            { CommandEnum.SelectOr, "select_or" },
            { CommandEnum.SelectEnd, "select_end" },
            { CommandEnum.Shop, "shop" },
            { CommandEnum.Menu, "menu" },
            { CommandEnum.Heal, "heal" },
            { CommandEnum.Encount, "encount" },
            { CommandEnum.Battle, "battle" },
            { CommandEnum.ItemGet, "item_get" },
            { CommandEnum.TamamonGet, "tamamon_get" },
            { CommandEnum.Hidden, "hidden" },
            { CommandEnum.AutoMove, "auto_move" },
            { CommandEnum.Warp, "warp" },
            { CommandEnum.FadeIn, "fade_in" },
            { CommandEnum.FadeOut, "fade_out" },
            { CommandEnum.Movie, "movie" },
            { CommandEnum.Bgm, "bgm" },
            { CommandEnum.Se, "se" },
            { CommandEnum.LoopPoint, "loop_point" },
            { CommandEnum.Loop, "loop" },
            { CommandEnum.GimmickStart, "gimmick_start" },
            { CommandEnum.GimmickEnd, "gimmick_end" },
        };

        /// <summary>
        /// ���s�\��̃R�}���h
        /// </summary>
        public string CurrentCommand { get; private set; } = string.Empty;
        /// <summary>
        /// ���s�R�}���h�̈����e�L�X�g
        /// </summary>
        public string CurrentText { get; private set; } = string.Empty;
        /// <summary>
        /// ���s�R�}���h���X�g�̌��݂̎��sIndex
        /// </summary>
        public int CurrentIndex { get; private set; } = 0;
        /// <summary>
        /// ���s�R�}���h���X�g�̍ő吔
        /// </summary>
        public int MaxEventCommandCount { get; private set; } = 0;

        private List<string> m_command = new List<string>();
        private List<string> m_text = new List<string>();

        /// <summary>
        /// �C�x���gID����C�x���g�ꗗ�̃e�L�X�g��Ԃ�
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public List<int> GetEventList(int eventId)
        {
            /*
            List<string> list = new List<string>()
            {
                {"����������" },{"����������"},{"����������"},
                {"����������" },{"����������"},{"����������"},
                {"����������" },{"����������"},{"����������"},
                {"����������"},
            };
            */

            m_command = new List<string>()
            {
                {"loop_point" },
                {"select_start" },{""},{""},
                {"select_start" },{""},{"loop"},
                {"select_or" },{""},{""},
                {"select_end" },
                {"select_or" },{""},{""},
                {"select_or" },{""},{""},
                {"select_end" },
                {""},
            };

            m_text = new List<string>()
            {
                {"" },
                {"" },{"����������"},{"����������"},
                {"" },{"����������"},{""},
                {"" },{"����������"},{"����������"},
                {""},
                {"" },{"����������"},{"�����Ă�"},
                {"" },{"�Ȃɂʂ˂�"},{"�͂Ђӂւ�"},
                {""},
                {"�����" },
            };

            List<int> list = new List<int>();
            for (int i = 0; i < m_command.Count; i++)
            {
                list.Add(i);
            }

            MaxEventCommandCount = list.Count;
            return list;
        }

        /// <summary>
        /// �e�L�X�g����R�}���h�����擾
        /// </summary>
        /// <param name="str"></param>
        public void SetCurrentEventCommand(int str)
        {
            CurrentCommand = m_command[str];
        }

        /// <summary>
        /// �e�L�X�g����e�L�X�g���擾
        /// </summary>
        /// <param name="str"></param>
        public void SetCurrentEventString(int str)
        {
            CurrentText = m_text[str];
        }

        // �R�}���h�����s���邩
        private bool m_isExecute = true;

        /// <summary>
        /// �R�}���h�����s���邩
        /// </summary>
        /// <returns></returns>
        public bool IsExecute()
        {
            if(CurrentCommand == CommandTexts[CommandEnum.SelectOr] ||
                CurrentCommand == CommandTexts[CommandEnum.SelectEnd])
            {
                m_isExecute = true;
            }

            return m_isExecute;
        }


        #region // �R�}���h�֘A

        // �I�����R�}���h�֘A
        private List<int> m_selectEventIndexList = new List<int>();
        private List<int> m_selectEventOrCountList = new List<int>();
        private bool m_isSelect = false;

        // ���[�v�R�}���h�֘A
        public Dictionary<int, int> LoopEventIndexs { get; private set; } = new Dictionary<int, int>();
        private bool m_isLoop = false;

        /// <summary>
        /// �R�}���h�֘A�����s����
        /// </summary>
        public void OnCommandEvent()
        {
            // �I����
            if (m_isSelect)
            {
                OnSelectEvent();
            }

            // ���[�v
            if (m_isLoop)
            {
                AddLoopEventIndexValues();
            }
            CurrentIndex++;
        }

        /// <summary>
        /// �R�}���hIndex���Z�b�g
        /// </summary>
        /// <param name="index"></param>
        public void SetCurrentIndex(int index)
        {
            CurrentIndex+= index;
        }

        /// <summary>
        /// �I�����C�x���g�ǉ�
        /// </summary>
        /// <param name="index"></param>
        public void AddSelectEvent(int index)
        {
            m_isSelect = true;
            m_selectEventIndexList.Add(index);
            m_selectEventOrCountList.Add(0);
        }

        /// <summary>
        /// �I�������򌟒m
        /// </summary>
        public void AddSelectOrEventCount()
        {
            m_selectEventOrCountList[m_selectEventOrCountList.Count - 1]++;
        }

        /// <summary>
        /// �I�����C�x���g�I�����m
        /// </summary>
        public void RemoveSelectEvent()
        {
            m_selectEventIndexList.RemoveAt(m_selectEventIndexList.Count - 1);
            m_selectEventOrCountList.RemoveAt(m_selectEventOrCountList.Count - 1);

            // �I�����C�x���g�����ׂďI��������t���O��false
            if (m_selectEventIndexList.Count == 0)
            {
                m_isSelect = false;
            }
        }

        /// <summary>
        /// �I�����C�x���g�̏������s���f
        /// </summary>
        /// <returns></returns>
        public void OnSelectEvent()
        {
            // ����C�x���g���Ȃ��Ȃ�����I��
            if (m_selectEventIndexList.Count == 0)
            {
                m_isExecute = true;
            }

            // �����̃R�}���h���f
            if (m_selectEventIndexList[m_selectEventIndexList.Count - 1] == m_selectEventOrCountList[m_selectEventOrCountList.Count - 1])
            {
                m_isExecute = true;
            }
            else
            {
                m_isExecute = false;
            }
        }

        /// <summary>
        /// ���[�v�C�x���g�L��
        /// </summary>
        /// <param name="key"></param>
        public void AddLoopEventIndexKey(int key)
        {
            m_isLoop = true;
            if (LoopEventIndexs.ContainsKey(key))
            {
                LoopEventIndexs[key] = 0;
            }
            else
            {
                LoopEventIndexs.Add(key, 0);
            }
        }

        /// <summary>
        /// ���[�v�o�b�N�񐔕ێ�
        /// </summary>
        public void AddLoopEventIndexValues()
        {
            var tmpDictionary = new Dictionary<int, int>(LoopEventIndexs);
            foreach (var data in tmpDictionary)
            {
                LoopEventIndexs[data.Key]++;
            }
        }

        /// <summary>
        /// ���[�v�C�x���g�j��
        /// </summary>
        public void ClearLoopEventIndexs()
        {
            m_isLoop = false;
            LoopEventIndexs.Clear();
        }

        #endregion
    }
}