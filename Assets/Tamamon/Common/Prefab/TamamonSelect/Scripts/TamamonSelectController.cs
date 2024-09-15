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

        // 戻るボタンも考慮して+1
        m_tamamonSelectModel.MaxSelectIndex = tamamonStatusDataList.Count + 1;
    }

    /// <summary>
    /// 表示
    /// </summary>
    /// <returns></returns>
    public async UniTask Show()
    {
        m_tamamonSelectModel.IsShow = true;
        m_tamamonSelectView.SetFrameActive(0, 0);

        m_tamamonSelectModel.SelectIndex = 0;
        m_tamamonSelectModel.PrevSelectIndex = 0;

        await m_tamamonSelectView.Show();
    }

    /// <summary>
    /// 非表示
    /// </summary>
    /// <returns></returns>
    public async UniTask Hide()
    {
        m_tamamonSelectModel.IsShow = false;
        await m_tamamonSelectView.Hide();
    }
}