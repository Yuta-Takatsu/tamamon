using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class TamamonSelectController : MonoBehaviour
{
    [SerializeField]
    private TamamonSelectView m_tamamonSelectView = default;

    private TamamonSelectModel m_tamamonSelectModel = default;

    /// <summary>
    /// 初期化
    /// </summary>
    public void OnInitialize(List<TamamonStatusData> tamamonStatusDataList)
    {
        m_tamamonSelectModel = new TamamonSelectModel();

        m_tamamonSelectView.OnInitialize(tamamonStatusDataList);

        m_tamamonSelectModel.MaxSelectIndex = tamamonStatusDataList.Count;
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
    public async UniTask Show(System.Action onShowCallback = null, System.Action onHideCallback = null)
    {
        m_tamamonSelectModel.IsShow = true;
        m_tamamonSelectView.SetFrameActive(0, m_tamamonSelectModel.PrevSelectIndex);

        m_tamamonSelectModel.SelectIndex = 0;
        m_tamamonSelectModel.PrevSelectIndex = 0;

        await m_tamamonSelectView.Show(onShowCallback);

        await OnExecute(onHideCallback);
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
    /// 操作関連
    /// </summary>
    /// <returns></returns>
    public async UniTask OnExecute(System.Action onCallback = null)
    {
        // 入力待機
        await m_tamamonSelectModel.OnInput(() => m_tamamonSelectView.SetFrameActive(m_tamamonSelectModel.SelectIndex, m_tamamonSelectModel.PrevSelectIndex));

        if(m_tamamonSelectModel.SelectIndex == m_tamamonSelectModel.MaxSelectIndex)
        {
            if (m_tamamonSelectModel.IsShow)
            {
                await Hide(onCallback);
                return;
            }
        }
    }
}
