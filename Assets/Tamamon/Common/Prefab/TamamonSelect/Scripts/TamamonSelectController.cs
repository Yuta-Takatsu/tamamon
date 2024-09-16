using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/// <summary>
/// �^�}�����I����ʃR���g���[���[�N���X
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
    /// �ǂ��̉�ʂ���J���ꂽ���ǂ����̃^�C�v
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
    /// �R�}���h�̎��s�^�C�v
    /// </summary>
    public enum TamamonSelectStateType
    {
        None,
        Change,      // ����ւ���
        IndexChange, // ���ёւ���
        StatusOpen,  // ����������
        Item,        // ������
        Close,       // ����
    }

    private List<string> m_battleCommandTextList = new List<string>() { { "����ւ���" }, { "����������" }, { "����" } };
    private List<string> m_adventureCommandTextList = new List<string>() { { "����������" }, { "���ёւ���" }, { "������" }, { "����" } };

    /// <summary>
    /// ������
    /// </summary>
    public void OnInitialize(List<TamamonStatusData> tamamonStatusDataList, TamamonSelectViewType type)
    {
        m_tamamonSelectModel = new TamamonSelectModel();
        m_tamamonSelectViewType = type;
        m_tamamonSelectView.OnInitialize(tamamonStatusDataList);

        m_tamamonSelectModel.MaxSelectIndex = tamamonStatusDataList.Count;

        // �I�����R�}���h������
        m_tamamonSelectTextWindow.OnInitialize(m_battleCommandTextList);
    }

    /// <summary>
    /// ���X�V
    /// </summary>
    /// <param name="tamamonStatusDataList"></param>
    public void UpdateData(List<TamamonStatusData> tamamonStatusDataList)
    {
        m_tamamonSelectView.UpdateStatusData(tamamonStatusDataList);

        m_tamamonSelectModel.MaxSelectIndex = tamamonStatusDataList.Count;
    }

    /// <summary>
    /// �\��
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
    /// ��\��
    /// </summary>
    /// <returns></returns>
    public async UniTask Hide(System.Action onCallback = null)
    {
        m_tamamonSelectModel.IsShow = false;
        await m_tamamonSelectView.Hide(onCallback);
    }

    /// <summary>
    /// �I������index��n��
    /// </summary>
    /// <returns></returns>
    public int GetSelectIndex()
    {
        return m_tamamonSelectModel.SelectIndex;
    }

    /// <summary>
    /// ����֘A
    /// </summary>
    /// <returns></returns>
    public async UniTask OnExecute(bool isEscape = true, System.Action onCallback = null)
    {
        m_tamamonSelectState = TamamonSelectStateType.None;
        m_tamamonSelectModel.IsEscape = isEscape;

        while (m_tamamonSelectState != TamamonSelectStateType.Change && m_tamamonSelectState != TamamonSelectStateType.Close)
        {
            // ���͑ҋ@
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
    /// �o�g����ʂ���J���ꂽ�ۂ̃R�}���h�̋���
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