using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Tamamon.UI
{
    /// <summary>
    /// 複数コマンドUIの制御用ベースクラス
    /// </summary>
    public class CommandWindowBase : PoolManager<CommandWindowText>
    {
        // 生成元プレハブ
        [SerializeField]
        protected CommandWindowText m_commandWindowText = default;

        // コマンド情報
        protected List<CommandWindowText> m_commandWindowTextList = new List<CommandWindowText>();
        protected List<string> m_commandStringList = new List<string>();

        // 選択情報
        public int SelectIndex { get; protected set; } = default;
        public int PrevSelectIndex { get; protected set; } = default;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="commandStringList"></param>
        public virtual void OnInitialize(List<string> commandStringList)
        {
            m_commandStringList = commandStringList;

            // コマンド生成
            foreach (string str in m_commandStringList)
            {
                CommandWindowText commandWindowText = Instantiate(m_commandWindowText, m_commandWindowText.transform.parent);
                commandWindowText.OnInitialize(str);
                m_commandWindowTextList.Add(commandWindowText);
            }

            // アローUI初期化
            ResetArrowActive();

            // 生成元プレハブを非表示
            m_commandWindowText.gameObject.SetActive(false);
        }

        /// <summary>
        /// アローUIの表示切替
        /// </summary>
        public virtual void SetArrowActive()
        {
            if (PrevSelectIndex < m_commandWindowTextList.Count)
            {
                m_commandWindowTextList[PrevSelectIndex].SetActiveArrow(false);
            }
            m_commandWindowTextList[PrevSelectIndex].SetActiveArrow(true);
        }

        /// <summary>
        /// アローUIの表示を初期に戻す
        /// </summary>
        public virtual void ResetArrowActive()
        {
            PrevSelectIndex = SelectIndex;
            SelectIndex = 0;
            SetArrowActive();
        }
    }
}