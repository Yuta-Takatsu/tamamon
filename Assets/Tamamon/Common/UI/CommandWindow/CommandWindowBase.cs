using System;
using System.Collections.Generic;
using Framework;
using Cysharp.Threading.Tasks;

namespace Tamamon.UI
{
    /// <summary>
    /// 複数コマンドUIの制御用ベースクラス
    /// </summary>
    public class CommandWindowBase : PoolManager<CommandWindowText>
    {
        // コマンド情報
        protected List<CommandWindowText> m_commandWindowTextList = new List<CommandWindowText>();
        protected List<string> m_commandStringList = new List<string>();

        // 選択情報
        public int SelectIndex { get; protected set; } = default;
        public int PrevSelectIndex { get; protected set; } = default;

        public bool IsShow { get; protected set; } = false;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="commandStringList"></param>
        public virtual void OnInitialize(List<string> commandStringList)
        {
            if (m_objectPool == null)
            {
                base.OnInitialize();
            }

            m_commandStringList = commandStringList;

            // コマンド生成
            foreach (string str in m_commandStringList)
            {
                OnCreateCommand(str);
            }

            // オブジェクト並び替え
            for (int i = m_commandWindowTextList.Count - 1; i >= 0; i--)
            {
                m_commandWindowTextList[i].transform.SetSiblingIndex(i);
            }

            // アローUI初期化
            ResetArrowActive();

            // 入力イベント
            AddInputEvent();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public virtual void OnFinarize()
        {
            var tmpList = new List<CommandWindowText>(m_commandWindowTextList);
            foreach (var command in tmpList)
            {
                OnReleaseCommand(command);
            }
        }

        /// <summary>
        /// コマンドUI生成
        /// </summary>
        /// <param name="command"></param>
        public virtual void OnCreateCommand(string command)
        {
            CommandWindowText commandWindowText = m_objectPool.Get();
            commandWindowText.OnInitialize(command);
            m_commandWindowTextList.Add(commandWindowText);
        }

        /// <summary>
        /// コマンドUI破棄
        /// </summary>
        /// <param name="commandWindowText"></param>
        public virtual void OnReleaseCommand(CommandWindowText commandWindowText)
        {
            m_commandWindowTextList.Remove(commandWindowText);
            commandWindowText.OnFinalize();
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
            m_commandWindowTextList[SelectIndex].SetActiveArrow(true);
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

        protected EventHandler m_decisionInputEventHandler = null;
        protected EventHandler m_topInputEventHandler = null;
        protected EventHandler m_BottomInputEventHandler = null;

        /// <summary>
        /// 入力時の処理実装
        /// </summary>
        public virtual void AddInputEvent()
        {
            m_decisionInputEventHandler = async (object sender, EventArgs e) =>
            {
                await Hide();
            };

            m_topInputEventHandler = (object sender, EventArgs e) =>
            {
                if (SelectIndex > 0)
                {
                    PrevSelectIndex = SelectIndex;
                    SelectIndex--;
                    SetArrowActive();
                }
            };

            m_BottomInputEventHandler = (object sender, EventArgs e) =>
            {
                if (SelectIndex < m_commandWindowTextList.Count - 1)
                {
                    PrevSelectIndex = SelectIndex;
                    SelectIndex++;
                    SetArrowActive();
                }
            };
        }

        /// <summary>
        /// 表示
        /// </summary>
        public virtual async UniTask Show()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            // 入力イベント登録
            InputEventManager.Instance.SetKeyDownEvent(InputManager.Key.Decision, m_decisionInputEventHandler);
            InputEventManager.Instance.SetKeyDownEvent(InputManager.Key.Top, m_topInputEventHandler);
            InputEventManager.Instance.SetKeyDownEvent(InputManager.Key.Bottom, m_BottomInputEventHandler);

            IsShow = true;
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 非表示
        /// </summary>
        protected virtual async UniTask Hide()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            // 入力イベント破棄
            InputEventManager.Instance.RemoveKeyDownEvent(InputManager.Key.Decision, m_decisionInputEventHandler);
            InputEventManager.Instance.RemoveKeyDownEvent(InputManager.Key.Top, m_topInputEventHandler);
            InputEventManager.Instance.RemoveKeyDownEvent(InputManager.Key.Bottom, m_BottomInputEventHandler);

            IsShow = false;
            this.gameObject.SetActive(false);
        }
    }
}