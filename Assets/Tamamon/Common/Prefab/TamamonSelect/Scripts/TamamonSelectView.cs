using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class TamamonSelectView : MonoBehaviour
{
    [SerializeField]
    private TamamonSelectInfo m_firstTamamonInfo = default;

    [SerializeField]
    private TamamonSelectInfo m_tamamonInfo = default;

    [SerializeField]
    private Image m_closeButtonFrame = default;

    [SerializeField]
    private TamamonSelectTextWindow m_tamamonSelectTextWindow = default;

    private List<TamamonSelectInfo> m_tamamonInfoObjectList = new List<TamamonSelectInfo>();
    public List<TamamonSelectInfo> TamamonInfoObjectList => m_tamamonInfoObjectList;


    /// <summary>
    /// 表示
    /// </summary>
    /// <returns></returns>
    public async UniTask Show(System.Action onCallback = null)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        onCallback?.Invoke();

        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// 非表示
    /// </summary>
    /// <returns></returns>
    public async UniTask Hide(System.Action onCallback = null)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        onCallback?.Invoke();

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void OnInitialize(List<TamamonStatusData> tamamonStatusDataList)
    {
        var firstData = tamamonStatusDataList.First();
        m_firstTamamonInfo.OnInitialize(
            0,
            firstData.TamamonStatusDataInfo.NickName,
            firstData.TamamonStatusDataInfo.Level,
            firstData.TamamonStatusDataInfo.Sex,
            firstData.TamamonStatusValueDataInfo.HP,
            firstData.TamamonStatusDataInfo.NowHP);

        m_tamamonInfoObjectList.Add(m_firstTamamonInfo);

        foreach (var data in tamamonStatusDataList.Skip(1))
        {
            int index = 1;
            var tamamonInfo = Instantiate(m_tamamonInfo, m_tamamonInfo.transform.parent);
            tamamonInfo.OnInitialize(
                index,
                data.TamamonStatusDataInfo.NickName,
                data.TamamonStatusDataInfo.Level,
                data.TamamonStatusDataInfo.Sex,
                data.TamamonStatusValueDataInfo.HP,
                data.TamamonStatusDataInfo.NowHP);

            m_tamamonInfoObjectList.Add(tamamonInfo);
        }
    }

    /// <summary>
    /// フレームの表示切替
    /// </summary>
    /// <param name="index"></param>
    /// <param name="prevIndex"></param>
    public void SetFrameActive(int index, int prevIndex)
    {
        if (prevIndex >= m_tamamonInfoObjectList.Count)
        {
            m_closeButtonFrame.gameObject.SetActive(false);
        }
        else
        {
            m_tamamonInfoObjectList[prevIndex].gameObject.SetActive(false);
        }

        if (index >= m_tamamonInfoObjectList.Count)
        {
            m_closeButtonFrame.gameObject.SetActive(true);
        }
        else
        {
            m_tamamonInfoObjectList[index].gameObject.SetActive(true);
        }
    }
}