using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
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

    private TamamonStatusData m_enemyTamamonData = new TamamonStatusData();
    private TamamonStatusData m_playerTamamonData = new TamamonStatusData();

    private int m_commandIndex = 0;

    // ���f�[�^�ϐ�
    private string m_encountMessage = "�쐶�� {0} �����ꂽ!";

    private string m_bringOutMessage = "�s�� ! {0} !!";

    private string m_waitMessage = "{0} �͂ǂ�����H";
    private string m_escapeMessage = "���܂������؂ꂽ�I";

    private List<string> m_actionCommandList = new List<string>() { "�키", "�o�b�O", "�^�}����", "������" };

    public void Start()
    {
        OnInitialize(3, 2);
    }

    /// <summary>
    /// ������
    /// </summary>
    public void OnInitialize(int enemyId,int playerId)
    {
        m_battleModel = new BattleModel();

        SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.Battle);

        // �X�e�[�g�ύX���̃R�[���o�b�N�o�^
        m_battleModel.OnStateExecute();
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.Encount, async () => await OnEncount());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.ActionSelect, async () => await OnActionSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.TechniqueSelect, async () => await OnTechniqueSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.Execute, async () => await OnExecute());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.TurnEnd, async () => await OnTurnEnd());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.TamamonSelect, async () => await OnTamamonSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.ItemSelect, async () => await OnItemSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.Result, async () => await OnResult());

        // �^�}������񏉊���
        m_enemyTamamonData.OnInitialize(enemyId, TamamonData.SexType.Male, 5, 1, 1, 2, 3, 4);
        m_playerTamamonData.OnInitialize(playerId, TamamonData.SexType.Female, 7, 1, 1, 2, 3);

        // ����UI�ɓn��
        m_battleUIView.ShowEnemyUI(m_enemyTamamonData.TamamonStatusDataInfo.Name, m_enemyTamamonData.TamamonStatusDataInfo.Sex, m_enemyTamamonData.TamamonStatusDataInfo.Level, m_enemyTamamonData.TamamonStatusValueDataInfo.HP, m_enemyTamamonData.TamamonStatusDataInfo.NowHP);
        m_battleUIView.ShowPlayerUI(m_playerTamamonData.TamamonStatusDataInfo.Name, m_playerTamamonData.TamamonStatusDataInfo.Sex, m_playerTamamonData.TamamonStatusDataInfo.Level, m_playerTamamonData.TamamonStatusDataInfo.Exp, m_playerTamamonData.TamamonStatusDataInfo.NowExp, m_playerTamamonData.TamamonStatusValueDataInfo.HP, m_playerTamamonData.TamamonStatusDataInfo.NowHP);
        m_battleTextWindowView.BattleUIMessageTextWindow.OnInitialize();
        m_battleTamamonView.OnInitialize(enemyId, playerId);

        this
            .ObserveEveryValueChanged(_ => m_battleTextWindowView.BattleUITechniqueTextWindow.SelectIndex)
            .Subscribe(_ =>
            {
                if (m_playerTamamonData.TamamonStatusDataInfo.TechniqueList.Count >= m_battleTextWindowView.BattleUITechniqueTextWindow.SelectIndex)
                {
                    var data = m_playerTamamonData.TamamonStatusDataInfo.TechniqueList[m_battleTextWindowView.BattleUITechniqueTextWindow.SelectIndex];
                    m_battleTextWindowView.BattleUITechniqueInfoTextWindow.ShowText(data.TechniquePP, data.TechniqueNowPP, TypeData.TypeNameDictionary[data.TechniqueData.Type]);
                }
            });

        // �G���J�E���g�X�e�[�g�ɕύX
        m_battleModel.BattleState = BattleModel.BattleStateType.Encount;
    }

    /// <summary>
    /// �G���J�E���g������
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEncount()
    {

        await SceneManager.Instance.FadeOut();

        await m_battleTamamonView.PlayEncountEnemyAnimation();

        m_encountMessage = string.Format(m_encountMessage, m_enemyTamamonData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_encountMessage);

        await UniTask.WaitWhile(() => m_battleTextWindowView.BattleUIMessageTextWindow.IsMessageAnimation());

        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        m_battleTamamonView.PlayEncountPlayerAnimation().Forget();

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        m_bringOutMessage = string.Format(m_bringOutMessage, m_playerTamamonData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_bringOutMessage);

        await UniTask.WaitWhile(() => m_battleTamamonView.IsAnimation);
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
        m_waitMessage = string.Format(m_waitMessage, m_playerTamamonData.TamamonStatusDataInfo.Name);
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

        List<string> commandNameList = new List<string>();
        foreach (var data in m_playerTamamonData.TamamonStatusDataInfo.TechniqueList)
        {
            commandNameList.Add(data.TechniqueData.Name);
        }

        m_battleTextWindowView.BattleUITechniqueTextWindow.OnInitialize(commandNameList);

        m_commandIndex = await OnInput(m_battleTextWindowView.BattleUITechniqueTextWindow, true);

        // �X�e�[�g��ύX
        if (m_battleTextWindowView.BattleUITechniqueTextWindow.IsEscape)
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

                m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_playerTamamonData.TamamonStatusDataInfo.Name}��{m_playerTamamonData.TamamonStatusDataInfo.TechniqueList[m_commandIndex].TechniqueData.Name}!!");

                m_battleUIView.UpdateEnemyHpBar(m_enemyTamamonData.TamamonStatusValueDataInfo.HP, m_enemyTamamonData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(m_enemyTamamonData, m_playerTamamonData, m_commandIndex));

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
        while (!isReturnKey)
        {
            isReturnKey = await window.SelectCommand();
            if (!isEscape && window.IsEscape) isReturnKey = false;
        }
        return window.SelectIndex;
    }
}