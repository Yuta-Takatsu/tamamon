using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Tamamon.Framework;

/// <summary>
/// �o�g���R���g���[���[�N���X
/// </summary>
public class BattleController : MonoBehaviour
{
    [SerializeField]
    private BattleTamamonView m_battleTamamonView = default;

    [SerializeField]
    private BattleUIView m_battleUIView = default;

    [SerializeField]
    private BattleTextWindowView m_battleTextWindowView = default;

    [SerializeField]
    private TamamonSelectController m_tamamonSelectController = default;

    private BattleModel m_battleModel = default;

    private TamamonData.TamamonDataInfomation m_enemyTamamon = new TamamonData.TamamonDataInfomation();
    private TamamonData.TamamonDataInfomation m_playerTamamon = new TamamonData.TamamonDataInfomation();

    // ���f�[�^�ϐ�
    private string m_encountMessage = "�쐶�� {0} �����ꂽ!";

    private string m_bringOutMessage = "�s�� ! {0} !!";

    private string m_waitMessage = "{0} �͂ǂ�����H";
    private string m_escapeMessage = "{0} �͓�����";

    private List<string> m_actionCommandList = new List<string>() { "�키", "�o�b�O", "�^�}����", "������" };
    private List<string> m_TechiqueCommandList = new List<string>() { "�V���h�[�{�[��", "�p���[�W�F��", "��n�̗�" };

    public void Start()
    {
        OnInitialize();
    }

    /// <summary>
    /// ������
    /// </summary>
    public void OnInitialize()
    {
        m_battleModel = new BattleModel();

        SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.Battle);

        m_battleModel.OnInitialize();
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.Encount, async () => await OnEncount());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.ActionSelect, async () => await OnActionSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.TechniqueSelect, async () => await OnTechniqueSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.Execute, async () => await OnExecute());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.TurnEnd, async () => await OnTurnEnd());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.TamamonSelect, async () => await OnTamamonSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.ItemSelect, async () => await OnItemSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.Result, async () => await OnResult());

        m_enemyTamamon.Name = "�C���C���g";
        m_enemyTamamon.Level = 5;
        m_enemyTamamon.Sex = TamamonData.SexType.Male;
        m_enemyTamamon.MaxHP = 22;
        m_enemyTamamon.NowHP = m_enemyTamamon.MaxHP;

        m_playerTamamon.Name = "�C���C���g";
        m_playerTamamon.Level = 5;
        m_playerTamamon.Sex = TamamonData.SexType.Female;
        m_playerTamamon.MaxExp = 100;
        m_playerTamamon.NowExp = 30;
        m_playerTamamon.MaxHP = 20;
        m_playerTamamon.NowHP = 15;

        m_battleTamamonView.OnInitialize(m_enemyTamamon, m_playerTamamon);

        // ����UI�ɓn��
        m_battleUIView.ShowEnemyUI(m_enemyTamamon.Name, m_enemyTamamon.Sex, m_enemyTamamon.Level, m_enemyTamamon.MaxHP, m_enemyTamamon.NowHP);
        m_battleUIView.ShowPlayerUI(m_playerTamamon.Name, m_playerTamamon.Sex, m_playerTamamon.Level, m_playerTamamon.MaxExp, m_playerTamamon.NowExp, m_playerTamamon.MaxHP, m_playerTamamon.NowHP);

        m_battleTextWindowView.BattleUIMessageTextWindow.OnInitialize();

        // �G���J�E���g�X�e�[�g�ɕύX
        m_battleModel.BattleState = BattleModel.BattleStateType.Encount;
    }

    /// <summary>
    /// �G���J�E���g������
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEncount()
    {
        m_battleTamamonView.PlayEncountEnemyAnimation();

        await UniTask.WaitWhile(() => m_battleTamamonView.IsEncountEnemyAnimation());

        m_encountMessage = string.Format(m_encountMessage, m_enemyTamamon.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_encountMessage);

        await UniTask.WaitWhile(() => m_battleTextWindowView.BattleUIMessageTextWindow.IsMessageAnimation());

        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        m_battleTamamonView.PlayEncountPlayerAnimation();

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        m_bringOutMessage = string.Format(m_bringOutMessage, m_playerTamamon.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_bringOutMessage);

        await UniTask.WaitWhile(() => m_battleTamamonView.IsEncountPlayerAnimation());
        await UniTask.WaitWhile(() => m_battleTextWindowView.BattleUIMessageTextWindow.IsMessageAnimation());

        // �s���I���X�e�[�g�ɕύX
        m_battleModel.BattleState = BattleModel.BattleStateType.ActionSelect;
    }

    /// <summary>
    /// �s���I��������
    /// </summary>
    /// <returns></returns>
    public async UniTask OnActionSelect()
    {
        // TextWindow�\���ؑ�
        m_battleTextWindowView.BattleUIMessageTextWindow.gameObject.SetActive(true);
        m_battleTextWindowView.BattleUIActionTextWindow.gameObject.SetActive(true);
        m_battleTextWindowView.BattleUITechniqueTextWindow.gameObject.SetActive(false);
        m_battleTextWindowView.BattleUITechniqueInfoTextWindow.gameObject.SetActive(false);

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        m_waitMessage = string.Format(m_waitMessage, m_playerTamamon.Name);
        m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageText(m_waitMessage);

        m_battleTextWindowView.BattleUIActionTextWindow.OnInitialize(m_actionCommandList);

        int index = await OnInput(m_battleTextWindowView.BattleUIActionTextWindow);

        // �X�e�[�g��ύX
        if (index == 0)
        {
            m_battleModel.BattleExecuteState = BattleModel.BattleExecuteType.Technique;
            m_battleModel.BattleState = BattleModel.BattleStateType.TechniqueSelect;
        }
        else if (index == 1)
        {
            m_battleModel.BattleExecuteState = BattleModel.BattleExecuteType.Item;
            m_battleModel.BattleState = BattleModel.BattleStateType.ItemSelect;
        }
        else if (index == 2)
        {
            m_battleModel.BattleExecuteState = BattleModel.BattleExecuteType.Change;
            m_battleModel.BattleState = BattleModel.BattleStateType.TamamonSelect;
        }
        else if (index == 3)
        {
            m_battleModel.BattleExecuteState = BattleModel.BattleExecuteType.Escape;
            m_battleModel.BattleState = BattleModel.BattleStateType.Execute;
        }
    }

    /// <summary>
    /// �Z�I��������
    /// </summary>
    /// <returns></returns>
    public async UniTask OnTechniqueSelect()
    {
        // TextWindow�\���ؑ�
        m_battleTextWindowView.BattleUIMessageTextWindow.gameObject.SetActive(false);
        m_battleTextWindowView.BattleUIActionTextWindow.gameObject.SetActive(false);
        m_battleTextWindowView.BattleUITechniqueTextWindow.gameObject.SetActive(true);
        m_battleTextWindowView.BattleUITechniqueInfoTextWindow.gameObject.SetActive(true);

        m_battleTextWindowView.BattleUITechniqueTextWindow.OnInitialize(m_TechiqueCommandList);
        m_battleTextWindowView.BattleUITechniqueInfoTextWindow.ShowText(35, 2, "�m�[�}��");

        int index = await OnInput(m_battleTextWindowView.BattleUITechniqueTextWindow, true);

        // �X�e�[�g��ύX
        if (index == 100)
        {
            m_battleModel.BattleState = BattleModel.BattleStateType.ActionSelect;
        }
        else
        {
            m_battleModel.BattleState = BattleModel.BattleStateType.Execute;
        }
    }

    /// <summary>
    /// �퓬������
    /// </summary>
    /// <returns></returns>
    public async UniTask OnExecute()
    {
        // TextWindow�\���ؑ�
        m_battleTextWindowView.BattleUIMessageTextWindow.gameObject.SetActive(true);
        m_battleTextWindowView.BattleUIActionTextWindow.gameObject.SetActive(false);
        m_battleTextWindowView.BattleUITechniqueTextWindow.gameObject.SetActive(false);
        m_battleTextWindowView.BattleUITechniqueInfoTextWindow.gameObject.SetActive(false);

        // �s���X�e�[�g�ɂ���ĕ���
        switch (m_battleModel.BattleExecuteState)
        {
            case BattleModel.BattleExecuteType.Technique:

                m_battleUIView.UpdatePlayerHpBar(m_playerTamamon.MaxHP, m_playerTamamon.NowHP, m_playerTamamon.MaxHP);

                await UniTask.WaitWhile(() => m_battleUIView.IsPlayerHpBarAnimation);

                m_battleUIView.UpdateEnemyHpBar(m_enemyTamamon.MaxHP, m_enemyTamamon.NowHP, 20);

                await UniTask.WaitWhile(() => m_battleUIView.IsEnemyHpBarAnimation);

                // �f�B���C�������Ă��玟�ɍs��
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

                // �^�[���I���X�e�[�g�ɕύX
                m_battleModel.BattleState = BattleModel.BattleStateType.TurnEnd;

                break;
            case BattleModel.BattleExecuteType.Change:
                break;
            case BattleModel.BattleExecuteType.Item:
                break;
            case BattleModel.BattleExecuteType.Escape:

                // ������邩������Ȃ���
                // todo ���͊m��œ������

                m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                m_escapeMessage = string.Format(m_escapeMessage, m_playerTamamon.Name);
                await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_escapeMessage);
                m_battleModel.BattleState = BattleModel.BattleStateType.Result;
                break;
        }
    }

    /// <summary>
    /// �^�[���I��������
    /// </summary>
    /// <returns></returns>
    public async UniTask OnTurnEnd()
    {
        // TextWindow�\���ؑ�
        m_battleTextWindowView.BattleUIMessageTextWindow.gameObject.SetActive(true);
        m_battleTextWindowView.BattleUIActionTextWindow.gameObject.SetActive(false);
        m_battleTextWindowView.BattleUITechniqueTextWindow.gameObject.SetActive(false);
        m_battleTextWindowView.BattleUITechniqueInfoTextWindow.gameObject.SetActive(false);

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync("���O�̕���");

        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
    }

    /// <summary>
    /// �^�}�����I��������
    /// </summary>
    /// <returns></returns>
    public async UniTask OnTamamonSelect()
    {
        m_tamamonSelectController.OnInitialize();

        await m_tamamonSelectController.Show();

        await m_tamamonSelectController.OnExecute();

        await UniTask.WaitUntil(() => m_tamamonSelectController.IsHide);

        await m_tamamonSelectController.Hide();

        m_battleModel.BattleState = BattleModel.BattleStateType.ActionSelect;
    }

    /// <summary>
    /// �A�C�e���I��������
    /// </summary>
    /// <returns></returns>
    public async UniTask OnItemSelect()
    {
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
    }

    /// <summary>
    /// �퓬�I��������
    /// </summary>
    /// <returns></returns>
    public async UniTask OnResult()
    {

        if (m_battleModel.BattleExecuteState == BattleModel.BattleExecuteType.Escape)
        {

        }
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

        await SceneManager.Instance.UnLoadSceneAsync("Battle");
    }

    /// <summary>
    /// ���͎�t
    /// </summary>
    /// <returns></returns>
    public async UniTask<int> OnInput(CommandWindowBase window, bool isEscape = false)
    {
        bool isReturnKey = false;
        int index = -1;
        while (!isReturnKey)
        {
            index = await window.SelectCommand();
            if (isEscape)
            {
                if (index != -1) isReturnKey = true;
            }
            else
            {
                if (index != -1 && index != 100) isReturnKey = true;
            }
        }
        return index;
    }
}