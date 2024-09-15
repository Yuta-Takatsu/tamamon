using System;
using System.Linq;
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

    private int m_commandIndex = 0;

    private int m_enemyCount = 0;

    private int m_playerCount = 0;

    // ���f�[�^�ϐ�
    private string m_encountMessage = "�쐶�� {0} �����ꂽ�I";

    private string m_bringOutMessage = "�s�� ! {0}�I";

    private string m_waitMessage = "{0} �͂ǂ�����H";
    private string m_escapeMessage = "���܂������؂ꂽ�I";

    private string m_effectiveMessage = "���ʂ� ���Q���I";
    private string m_notEffectiveMessage = "���ʂ� ���܂ЂƂ̂悤��";
    private string m_dontAffectiveMessage = "���ʂ� �����悤��...";

    private string m_faintingMessage = "{0}�͓|�ꂽ";

    private string m_getExpMessage = "{0}��\n{1} �o���l�� �������";

    private string m_loseMessage = "{0}�͖ڂ̑O���^���ÂɂȂ���...";

    private string m_playerName = "�Z��";

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
        TamamonStatusData enemyData_1 = new TamamonStatusData();
        enemyData_1.OnInitialize(enemyId, TamamonData.SexType.Male, 5, 1, 5);
        m_battleModel.AddEnemyList(enemyData_1);

        TamamonStatusData playerData_1 = new TamamonStatusData();
        playerData_1.OnInitialize(playerId, TamamonData.SexType.Female, 7, 1, 1, 2, 3);
        m_battleModel.AddPlayerList(playerData_1);

        //TamamonStatusData playerData_2 = new TamamonStatusData();
        //playerData_2.OnInitialize(enemyId, TamamonData.SexType.Male, 5, 1, 5);
        //m_battleModel.AddPlayerList(playerData_2);

        // �莝���̐�
        m_enemyCount = m_battleModel.GetEnemyList().Count;
        m_playerCount = m_battleModel.GetPlayerList().Count;

        // �莝���擪�����擾
        m_battleModel.EnemyStatusData = m_battleModel.GetEnemyList().First();
        m_battleModel.PlayerStatusData = m_battleModel.GetPlayerList().First();
        
        // ����UI�ɓn��
        m_battleUIView.ShowEnemyUI(m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Sex, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Level, m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.NowHP);
        m_battleUIView.ShowPlayerUI(m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Sex, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Level, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Exp, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowExp, m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowHP);
        m_battleTextWindowView.BattleUIMessageTextWindow.OnInitialize();
        m_battleTamamonView.OnInitialize(enemyId, playerId);
        m_tamamonSelectController.OnInitialize(m_battleModel.GetPlayerList());

        this
            .ObserveEveryValueChanged(_ => m_battleTextWindowView.BattleUITechniqueTextWindow.SelectIndex)
            .Subscribe(_ =>
            {
                if (m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList.Count >= m_battleTextWindowView.BattleUITechniqueTextWindow.SelectIndex)
                {
                    var data = m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList[m_battleTextWindowView.BattleUITechniqueTextWindow.SelectIndex];
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

        m_encountMessage = string.Format(m_encountMessage, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_encountMessage);

        await UniTask.WaitWhile(() => m_battleTextWindowView.BattleUIMessageTextWindow.IsMessageAnimation());

        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        m_battleTamamonView.PlayEncountPlayerAnimation().Forget();

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        m_bringOutMessage = string.Format(m_bringOutMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
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
        m_waitMessage = string.Format(m_waitMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
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
        foreach (var data in m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList)
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
                await OnTechniqueExecute();
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
                await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
                m_battleModel.BattleEndState = BattleModel.BattleEndType.Escape;
                m_battleModel.BattleState = BattleModel.BattleStateType.Result;
                return;
        }
        // �^�[���I���X�e�[�g�ɕύX
        m_battleModel.BattleState = BattleModel.BattleStateType.TurnEnd;
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

        // �^�}������ԃ`�F�b�N
        // �G�l�~�[�^�}�������m���ɂȂ������ǂ���
        if (m_battleModel.IsEnemyFainting())
        {
            m_battleModel.BattleTurnEndState = BattleModel.BattleTurnEndType.EnemyDown;
            m_enemyCount--;

            // �퓬�s�\�A�j���[�V�����Đ�
            await m_battleTamamonView.OnDownAnimation(false);

            m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
            m_faintingMessage = string.Format(m_faintingMessage, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name);
            await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_faintingMessage);
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

            if (m_enemyCount < 1)
            {
                m_battleModel.BattleEndState = BattleModel.BattleEndType.Win;
                m_battleModel.BattleState = BattleModel.BattleStateType.Result;
                return;
            }
        }

        // �v���C���[�^�}�������m���ɂȂ������ǂ���
        if (m_battleModel.IsPlayerFainting())
        {
            m_battleModel.BattleTurnEndState = BattleModel.BattleTurnEndType.PlayerDown;
            m_playerCount--;

            // �퓬�s�\�A�j���[�V�����Đ�
            await m_battleTamamonView.OnDownAnimation(true);

            m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
            m_faintingMessage = string.Format(m_faintingMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
            await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_faintingMessage);
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

            if (m_playerCount < 1)
            {
                m_battleModel.BattleEndState = BattleModel.BattleEndType.Lose;
                m_battleModel.BattleState = BattleModel.BattleStateType.Result;
                return;
            }
        }

        m_battleModel.BattleTurnEndState = BattleModel.BattleTurnEndType.None;
        m_battleModel.BattleState = BattleModel.BattleStateType.ActionSelect;
    }

    /// <summary>
    /// �^�}�����I��������
    /// </summary>
    /// <returns></returns>
    public async UniTask OnTamamonSelect()
    {
        m_tamamonSelectController.UpdateData(m_battleModel.GetPlayerList());

        await m_tamamonSelectController.Show();

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
        switch (m_battleModel.BattleEndState)
        {
            case BattleModel.BattleEndType.Win:

                int exp = 30;
                m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                m_getExpMessage = string.Format(m_getExpMessage, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name, exp);
                await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_getExpMessage);
                await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

                // �o���l�擾
                m_battleUIView.UpdatePlayerExpBar(m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Exp, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowExp, exp);
                m_battleModel.PlayerStatusData.UpdateNowExp(exp);

                await UniTask.WaitWhile(() => m_battleUIView.IsPlayerExpBarAnimation);

                await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
                break;
            case BattleModel.BattleEndType.Lose:

                m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                m_loseMessage = string.Format(m_loseMessage, m_playerName);
                await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_loseMessage);
                await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
                break;
            case BattleModel.BattleEndType.Escape:
                break;
            default:
                break;
        }
        await SceneManager.Instance.UnLoadSceneAsync("Battle");

        await SoundManager.Instance.StopBGMAsync();
    }

    /// <summary>
    /// �Z�I�������s�R�[���o�b�N
    /// </summary>
    /// <returns></returns>
    public async UniTask OnTechniqueExecute()
    {
        // �f�����`�F�b�N
        bool isPlayer = true;
        if (m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.Speed > m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.Speed)
        {
            isPlayer = false;
        }
        else if (m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.Speed < m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.Speed)
        {
            isPlayer = true;
        }
        else if (m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.Speed == m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.Speed)
        {
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            isPlayer = UnityEngine.Random.Range(0, 100) <= 50 ? false : true;
        }

        if (isPlayer)
        {
            m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
            await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name}��{m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList[m_commandIndex].TechniqueData.Name}�I");

            m_battleUIView.UpdateEnemyHpBar(m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(m_commandIndex, true));
            m_battleModel.EnemyStatusData.UpdateNowHP(m_battleModel.GetDamageValue(m_commandIndex, true));

            await UniTask.WaitWhile(() => m_battleUIView.IsEnemyHpBarAnimation);

            // �^�C�v�����e�L�X�g
            switch (m_battleModel.WeaknessTypeState)
            {
                case BattleModel.WeaknessType.None:
                    break;
                case BattleModel.WeaknessType.Effective:
                    m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                    await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_effectiveMessage);
                    break;
                case BattleModel.WeaknessType.NotEffective:
                    m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                    await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_notEffectiveMessage);
                    break;
                case BattleModel.WeaknessType.DontAffective:
                    m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                    await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_dontAffectiveMessage);
                    break;
            }

            if (m_battleModel.IsEnemyFainting())
            {
                // �f�B���C�������Ă��玟�ɍs��
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                // �^�[���I���X�e�[�g�ɕύX
                m_battleModel.BattleState = BattleModel.BattleStateType.TurnEnd;
                return;
            }

            //�m���ɂȂ��Ă��Ȃ���΃G�l�~�[�̍s��
            // �f�B���C�������Ă��玟�ɍs��
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
            await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name}��{m_battleModel.EnemyStatusData.TamamonStatusDataInfo.TechniqueList[0].TechniqueData.Name}�I");

            m_battleUIView.UpdatePlayerHpBar(m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(0, false));
            m_battleModel.PlayerStatusData.UpdateNowHP(m_battleModel.GetDamageValue(0, false));

            await UniTask.WaitWhile(() => m_battleUIView.IsPlayerHpBarAnimation);

            // �^�C�v�����e�L�X�g
            switch (m_battleModel.WeaknessTypeState)
            {
                case BattleModel.WeaknessType.None:
                    break;
                case BattleModel.WeaknessType.Effective:
                    m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                    await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_effectiveMessage);
                    break;
                case BattleModel.WeaknessType.NotEffective:
                    m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                    await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_notEffectiveMessage);
                    break;
                case BattleModel.WeaknessType.DontAffective:
                    m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                    await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_dontAffectiveMessage);
                    break;
            }
        }
        else
        {
            m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
            await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name}��{m_battleModel.EnemyStatusData.TamamonStatusDataInfo.TechniqueList[0].TechniqueData.Name}�I");

            m_battleUIView.UpdatePlayerHpBar(m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(0, false));
            m_battleModel.PlayerStatusData.UpdateNowHP(m_battleModel.GetDamageValue(0, false));

            await UniTask.WaitWhile(() => m_battleUIView.IsPlayerHpBarAnimation);

            // �^�C�v�����e�L�X�g
            switch (m_battleModel.WeaknessTypeState)
            {
                case BattleModel.WeaknessType.None:
                    break;
                case BattleModel.WeaknessType.Effective:
                    m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                    await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_effectiveMessage);
                    break;
                case BattleModel.WeaknessType.NotEffective:
                    m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                    await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_notEffectiveMessage);
                    break;
                case BattleModel.WeaknessType.DontAffective:
                    m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                    await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_dontAffectiveMessage);
                    break;
            }

            if (m_battleModel.IsPlayerFainting())
            {
                // �f�B���C�������Ă��玟�ɍs��
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                // �^�[���I���X�e�[�g�ɕύX
                m_battleModel.BattleState = BattleModel.BattleStateType.TurnEnd;
                return;
            }

            // �m���ɂȂ��Ă��Ȃ���΃v���C���[�̍s��
            // �f�B���C�������Ă��玟�ɍs��
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
            await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name}��{m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList[m_commandIndex].TechniqueData.Name}�I");

            m_battleUIView.UpdateEnemyHpBar(m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(m_commandIndex, true));
            m_battleModel.EnemyStatusData.UpdateNowHP(m_battleModel.GetDamageValue(m_commandIndex, true));

            await UniTask.WaitWhile(() => m_battleUIView.IsEnemyHpBarAnimation);

            // �^�C�v�����e�L�X�g
            switch (m_battleModel.WeaknessTypeState)
            {
                case BattleModel.WeaknessType.None:
                    break;
                case BattleModel.WeaknessType.Effective:
                    m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                    await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_effectiveMessage);
                    break;
                case BattleModel.WeaknessType.NotEffective:
                    m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                    await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_notEffectiveMessage);
                    break;
                case BattleModel.WeaknessType.DontAffective:
                    m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                    await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_dontAffectiveMessage);
                    break;
            }
        }

        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
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