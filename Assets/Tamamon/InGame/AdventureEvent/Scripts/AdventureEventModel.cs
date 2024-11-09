using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// アドベンチャーパートのmodelクラス
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
        /// 実行予定のコマンド
        /// </summary>
        public string CurrentCommand { get; private set; } = string.Empty;
        /// <summary>
        /// 実行コマンドの引数テキスト
        /// </summary>
        public string CurrentText { get; private set; } = string.Empty;
        /// <summary>
        /// 実行コマンドリストの現在の実行Index
        /// </summary>
        public int CurrentIndex { get; private set; } = 0;
        /// <summary>
        /// 実行コマンドリストの最大数
        /// </summary>
        public int MaxEventCommandCount { get; private set; } = 0;

        private List<string> m_command = new List<string>();
        private List<string> m_text = new List<string>();

        /// <summary>
        /// イベントIDからイベント一覧のテキストを返す
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public List<int> GetEventList(int eventId)
        {
            /*
            List<string> list = new List<string>()
            {
                {"あいうえお" },{"かきくけこ"},{"さしすせそ"},
                {"あいうえお" },{"かきくけこ"},{"さしすせそ"},
                {"あいうえお" },{"かきくけこ"},{"さしすせそ"},
                {"さしすせそ"},
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
                {"" },{"あいうえお"},{"かきくけこ"},
                {"" },{"さしすせそ"},{""},
                {"" },{"あいうえお"},{"かきくけこ"},
                {""},
                {"" },{"さしすせそ"},{"たちつてと"},
                {"" },{"なにぬねの"},{"はひふへほ"},
                {""},
                {"おわり" },
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
        /// テキストからコマンド情報を取得
        /// </summary>
        /// <param name="str"></param>
        public void SetCurrentEventCommand(int str)
        {
            CurrentCommand = m_command[str];
        }

        /// <summary>
        /// テキストからテキスト情報取得
        /// </summary>
        /// <param name="str"></param>
        public void SetCurrentEventString(int str)
        {
            CurrentText = m_text[str];
        }

        // コマンドを実行するか
        private bool m_isExecute = true;

        /// <summary>
        /// コマンドを実行するか
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


        #region // コマンド関連

        // 選択肢コマンド関連
        private List<int> m_selectEventIndexList = new List<int>();
        private List<int> m_selectEventOrCountList = new List<int>();
        private bool m_isSelect = false;

        // ループコマンド関連
        public Dictionary<int, int> LoopEventIndexs { get; private set; } = new Dictionary<int, int>();
        private bool m_isLoop = false;

        /// <summary>
        /// コマンド関連毎実行処理
        /// </summary>
        public void OnCommandEvent()
        {
            // 選択肢
            if (m_isSelect)
            {
                OnSelectEvent();
            }

            // ループ
            if (m_isLoop)
            {
                AddLoopEventIndexValues();
            }
            CurrentIndex++;
        }

        /// <summary>
        /// コマンドIndexをセット
        /// </summary>
        /// <param name="index"></param>
        public void SetCurrentIndex(int index)
        {
            CurrentIndex+= index;
        }

        /// <summary>
        /// 選択肢イベント追加
        /// </summary>
        /// <param name="index"></param>
        public void AddSelectEvent(int index)
        {
            m_isSelect = true;
            m_selectEventIndexList.Add(index);
            m_selectEventOrCountList.Add(0);
        }

        /// <summary>
        /// 選択肢分岐検知
        /// </summary>
        public void AddSelectOrEventCount()
        {
            m_selectEventOrCountList[m_selectEventOrCountList.Count - 1]++;
        }

        /// <summary>
        /// 選択肢イベント終了検知
        /// </summary>
        public void RemoveSelectEvent()
        {
            m_selectEventIndexList.RemoveAt(m_selectEventIndexList.Count - 1);
            m_selectEventOrCountList.RemoveAt(m_selectEventOrCountList.Count - 1);

            // 選択肢イベントがすべて終了したらフラグをfalse
            if (m_selectEventIndexList.Count == 0)
            {
                m_isSelect = false;
            }
        }

        /// <summary>
        /// 選択肢イベントの処理実行判断
        /// </summary>
        /// <returns></returns>
        public void OnSelectEvent()
        {
            // 分岐イベントがなくなったら終了
            if (m_selectEventIndexList.Count == 0)
            {
                m_isExecute = true;
            }

            // 分岐先のコマンド判断
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
        /// ループイベント記憶
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
        /// ループバック回数保持
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
        /// ループイベント破棄
        /// </summary>
        public void ClearLoopEventIndexs()
        {
            m_isLoop = false;
            LoopEventIndexs.Clear();
        }

        #endregion
    }
}