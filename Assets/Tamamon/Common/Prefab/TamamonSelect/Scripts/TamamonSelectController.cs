using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/// <summary>
/// タマモン選択画面コントローラークラス
/// </summary>
public class TamamonSelectController : MonoBehaviour
{
    [SerializeField]
    private TamamonSelectView m_tamamonSelectView = default;

    private TamamonSelectModel m_tamamonSelectModel = default;

    [SerializeField]
    private CommandWindowBase m_tamamonSelectTextWindow = default;

    private TamamonSelectViewType m_tamamonSelectViewType = TamamonSelectViewType.None;
    /// <summary>
    /// どこの画面から開かれたかどうかのタイプ
    /// </summary>
    public enum TamamonSelectViewType
    {
        None,
        Adventure,
        Battle,
    }

    private TamamonSelectStateType m_tamamonSelectState = TamamonSelectStateType.None;
    public TamamonSelectStateType TamamonSelectState => m_tamamonSelectState;
    /// <summary>
    /// コマンドの実行タイプ
    /// </summary>
    public enum TamamonSelectStateType
    {
        None,
        Change,      // 入れ替える
        IndexChange, // 並び替える
        StatusOpen,  // 強さを見る
        Item,        // 持ち物
        Close,       // 閉じる
    }

    private List<string> m_battleCommandTextList = new List<string>() { { "入れ替える" }, { "強さを見る" }, { "閉じる" } };
    private List<string> m_adventureCommandTextList = new List<string>() { { "強さを見る" }, { "並び替える" }, { "持ち物" }, { "閉じる" } };

    /// <summary>
    /// 初期化
    /// </summary>
    public void OnInitialize(List<TamamonStatusData> tamamonStatusDataList, TamamonSelectViewType type)
    {
        m_tamamonSelectModel = new TamamonSelectModel();
        m_tamamonSelectViewType = type;
        m_tamamonSelectView.OnInitialize(tamamonStatusDataList);

        m_tamamonSelectModel.MaxSelectIndex = tamamonStatusDataList.Count;

        // 選択時コマンド初期化
        m_tamamonSelectTextWindow.OnInitialize(m_battleCommandTextList);
    }

    /// <summary>
    /// 情報更新
    /// </summary>
    /// <param name="tamamonStatusDataList"></param>
    public void UpdateData(List<TamamonStatusData> tamamonStatusDataList)
    {
        m_tamamonSelectView.UpdateStatusData(tamamonStatusDataList);

        m_tamamonSelectModel.MaxSelectIndex = tamamonStatusDataList.Count;
    }

    /// <summary>
    /// 表示
    /// </summary>
    /// <returns></returns>
    public async UniTask Show(bool isEscape = true, System.Action onShowCallback = null, System.Action onHideCallback = null)
    {
        m_tamamonSelectModel.IsShow = true;
        m_tamamonSelectModel.PrevSelectIndex = m_tamamonSelectModel.SelectIndex;
        m_tamamonSelectView.SetFrameActive(0, m_tamamonSelectModel.PrevSelectIndex);

        m_tamamonSelectModel.SelectIndex = 0;
        m_tamamonSelectModel.PrevSelectIndex = 0;

        await m_tamamonSelectView.Show(onShowCallback);

        await OnExecute(isEscape,onHideCallback);
    }

    /// <summary>
    /// 非表示
    /// </summary>
    /// <returns></returns>
    public async UniTask Hide(System.Action onCallback = null)
    {
        m_tamamonSelectModel.IsShow = false;
        await m_tamamonSelectView.Hide(onCallback);
    }

    /// <summary>
    /// 選択したindexを渡す
    /// </summary>
    /// <returns></returns>
    public int GetSelectIndex()
    {
        return m_tamamonSelectModel.SelectIndex;
    }

    /// <summary>
    /// 操作関連
    /// </summary>
    /// <returns></returns>
    public async UniTask OnExecute(bool isEscape = true, System.Action onCallback = null)
    {
        m_tamamonSelectState = TamamonSelectStateType.None;
        m_tamamonSelectModel.IsEscape = isEscape;

        while (m_tamamonSelectState != TamamonSelectStateType.Change && m_tamamonSelectState != TamamonSelectStateType.Close)
        {
            // 入力待機
            await m_tamamonSelectModel.OnInput(() => m_tamamonSelectView.SetFrameActive(m_tamamonSelectModel.SelectIndex, m_tamamonSelectModel.PrevSelectIndex));

            if (m_tamamonSelectModel.SelectIndex == m_tamamonSelectModel.MaxSelectIndex)
            {
                if (m_tamamonSelectModel.IsShow)
                {
                    m_tamamonSelectState = TamamonSelectStateType.Close;
                    await Hide(onCallback);
                    return;
                }
            }

            await m_tamamonSelectTextWindow.Show();

            await m_tamamonSelectTextWindow.SelectCommand();

            await OnBattleCommand();
        }
        await Hide(onCallback);
        await m_tamamonSelectTextWindow.Hide();
    }

    /// <summary>
    /// バトル画面から開かれた際のコマンドの挙動
    /// </summary>
    /// <returns></returns>
    public async UniTask OnBattleCommand()
    {
        if (m_tamamonSelectTextWindow.SelectIndex == 0)
        {
            m_tamamonSelectState = TamamonSelectStateType.Change;
        }
        else if (m_tamamonSelectTextWindow.SelectIndex == 1)
        {
            await m_tamamonSelectTextWindow.Hide();
        }
        else if (m_tamamonSelectTextWindow.SelectIndex == 2)
        {
            await m_tamamonSelectTextWindow.Hide();
        }
    }
}