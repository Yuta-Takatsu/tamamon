using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/// <summary>
/// �^�}�����I����ʃr���[�N���X
/// </summary>
public class TamamonSelectView : MonoBehaviour
{
    [SerializeField]
    private TamamonSelectInfo m_firstTamamonInfo = default;

    [SerializeField]
    private TamamonSelectInfo m_tamamonInfo = default;

    [SerializeField]
    private Image m_closeButtonFrame = default;

    private List<TamamonSelectInfo> m_tamamonInfoObjectList = new List<TamamonSelectInfo>();
    public List<TamamonSelectInfo> TamamonInfoObjectList => m_tamamonInfoObjectList;

    /// <summary>
    /// �\��
    /// </summary>
    /// <returns></returns>
    public async UniTask Show(System.Action onCallback = null)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        onCallback?.Invoke();

        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// ��\��
    /// </summary>
    /// <returns></returns>
    public async UniTask Hide(System.Action onCallback = null)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        onCallback?.Invoke();

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// ������
    /// </summary>
    public void OnInitialize(List<TamamonStatusData> tamamonStatusDataList)
    {
        var firstData = tamamonStatusDataList.First();
        m_firstTamamonInfo.OnInitialize(
            firstData.TamamonStatusDataInfo.Id,
            firstData.TamamonStatusDataInfo.NickName,
            firstData.TamamonStatusDataInfo.Level,
            firstData.TamamonStatusDataInfo.Sex,
            firstData.TamamonStatusValueDataInfo.HP,
            firstData.TamamonStatusDataInfo.NowHP);

        m_tamamonInfoObjectList.Add(m_firstTamamonInfo);

        int index = 1;
        foreach (var data in tamamonStatusDataList.Skip(1))
        {
            int i = index;
            var tamamonInfo = Instantiate(m_tamamonInfo, m_tamamonInfo.transform.parent);
            tamamonInfo.OnInitialize(
                data.TamamonStatusDataInfo.Id,
                data.TamamonStatusDataInfo.NickName,
                data.TamamonStatusDataInfo.Level,
                data.TamamonStatusDataInfo.Sex,
                data.TamamonStatusValueDataInfo.HP,
                data.TamamonStatusDataInfo.NowHP);

            m_tamamonInfoObjectList.Add(tamamonInfo);
            index++;
        }

        m_tamamonInfo.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���X�V
    /// </summary>
    /// <param name="tamamonStatusDataList"></param>
    public void UpdateStatusData(List<TamamonStatusData> tamamonStatusDataList)
    {
        int index = 0;
        foreach (var data in tamamonStatusDataList)
        {
            int i = index;
            m_tamamonInfoObjectList[i].OnInitialize(
                data.TamamonStatusDataInfo.Id,
                data.TamamonStatusDataInfo.NickName,
                data.TamamonStatusDataInfo.Level,
                data.TamamonStatusDataInfo.Sex,
                data.TamamonStatusValueDataInfo.HP,
                data.TamamonStatusDataInfo.NowHP);
            index++;
        }
    }

    /// <summary>
    /// �t���[���̕\���ؑ�
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
            m_tamamonInfoObjectList[prevIndex].SetFrame(false);
        }

        if (index >= m_tamamonInfoObjectList.Count)
        {
            m_closeButtonFrame.gameObject.SetActive(true);
        }
        else
        {
            m_tamamonInfoObjectList[index].SetFrame(true);
        }
    }

    /// <summary>
    /// ���������^�}�������I�u�W�F�N�g�̍폜
    /// </summary>
    public void OnTamamonInfoDestroy()
    {
        foreach (var obj in m_tamamonInfoObjectList)
        {
            Destroy(obj.gameObject);
        }
        m_tamamonInfoObjectList.Clear();
    }
}