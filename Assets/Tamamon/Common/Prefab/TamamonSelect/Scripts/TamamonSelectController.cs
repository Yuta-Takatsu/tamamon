using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class TamamonSelectController : MonoBehaviour
{
    [SerializeField]
    private List<TamamonSelectInfo> m_tamamonInfoList = new List<TamamonSelectInfo>();

    [SerializeField]
    private Image m_closeImage = default;

    [SerializeField]
    private TamamonSelectTextWindow m_tamamonSelectTextWindow = default;

    private int m_selectIndex = 0;
    private TamamonSelectInfo m_prevTamamonSelectInfo = default;

    private List<string> m_selectCommandList = new List<string>() { "入れ替える", "強さを見る", "閉じる" };

    private readonly int MaxIndex = 7;

    private bool m_isHide = false;
    public bool IsHide => m_isHide;

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

        m_isHide = false;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void OnInitialize()
    {
        m_selectIndex = 0;

        foreach(var info in m_tamamonInfoList)
        {
            info.SetFrame(false);
        }
        m_closeImage.gameObject.SetActive(false);

        m_tamamonInfoList[m_selectIndex].SetFrame(true);
        m_prevTamamonSelectInfo = m_tamamonInfoList[m_selectIndex];
    }

    public async UniTask OnExecute()
    {
        bool isReturnKey = false;
        int index = -1;
        while (!isReturnKey)
        {
            index = await SelectTamamon();
            if (index != -1) isReturnKey = true;
        }

        if (index == 6)
        {
            m_isHide = true;
        }
        else if (index >= 0 && index < MaxIndex - 1)
        {

        }
    }

    /// <summary>
    /// タマモンコマンド表示
    /// </summary>
    /// <returns></returns>
    public async UniTask<int> ShowWindow()
    {
        m_tamamonSelectTextWindow.OnInitialize(m_selectCommandList);
        m_tamamonSelectTextWindow.gameObject.SetActive(true);

        int index = await OnInput(m_tamamonSelectTextWindow);

        // ステートを変更
        if (index == 100)
        {
            m_tamamonSelectTextWindow.gameObject.SetActive(false);
        }
        return index;
    }

    /// <summary>
    /// タマモン選択
    /// </summary>
    /// <returns></returns>
    public async UniTask<int> SelectTamamon()
    {
        await UniTask.WaitUntil(() => Input.anyKeyDown);

        foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                return m_selectIndex;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (m_selectIndex == 6) return m_selectIndex;
                m_selectIndex = 6;
                break;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (m_selectIndex != 0 && m_selectIndex != 6)
                {
                    m_selectIndex = 0;
                }
                else
                {
                    return -1;
                }
                break;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (m_selectIndex == 0)
                {
                    m_selectIndex = 1;
                }
                else
                {
                    return -1;
                }
                break;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (m_selectIndex > 0)
                {
                    m_selectIndex--;
                }
                else
                {
                    return -1;
                }
                break;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (m_selectIndex + 1 < MaxIndex)
                {
                    m_selectIndex++;
                }
                else
                {
                    return -1;
                }
                break;
            }
        }

        if (m_selectIndex == 6)
        {
            m_prevTamamonSelectInfo.SetFrame(false);
            m_closeImage.gameObject.SetActive(true);
        }
        else
        {
            m_closeImage.gameObject.SetActive(false);
            m_prevTamamonSelectInfo.SetFrame(false);
            m_tamamonInfoList[m_selectIndex].SetFrame(true);
            m_prevTamamonSelectInfo = m_tamamonInfoList[m_selectIndex];
        }

        return -1;
    }

    /// <summary>
    /// 入力受付
    /// </summary>
    /// <returns></returns>
    public async UniTask<int> OnInput(CommandWindowBase window)
    {
        bool isReturnKey = false;
        while (!isReturnKey)
        {
            isReturnKey = await window.SelectCommand();
        }
        return window.SelectIndex;
    }
}