using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using Framework.Sound;
using Framework.Scene;

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

    private string m_changeMessage = "�߂�I{0}�I";

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
        OnInitialize(3);
    }
    /// <summary>
    /// ������
    /// </summary>
    public void OnInitialize(int enemyId)
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
        enemyData_1.OnInitialize(enemyId, TamamonData.SexType.Male, 5, 1, 5, 6);
        m_battleModel.AddEnemyList(enemyData_1);

        TamamonStatusData playerData_1 = new TamamonStatusData();
        playerData_1.OnInitialize(2, TamamonData.SexType.Female, 7, 1, 1, 2, 3);
        m_battleModel.AddPlayerList(playerData_1);

        TamamonStatusData playerData_2 = new TamamonStatusData();
        playerData_2.OnInitialize(enemyId, TamamonData.SexType.Male, 5, 1, 5);
        m_battleModel.AddPlayerList(playerData_2);

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
        m_battleTamamonView.OnInitialize(enemyId, 2);
        m_tamamonSelectController.OnInitialize(m_battleModel.GetPlayerList(), TamamonSelectController.TamamonSelectViewType.Battle);

        // �s���R�}���h������
        m_battleTextWindowView.BattleUIActionTextWindow.OnInitialize(m_actionCommandList);

        // �Z�\�L������
        List<string> commandNameList = new List<string>();
        foreach (var data in m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList)
        {
            commandNameList.Add(data.TechniqueData.Name);
        }
        m_battleTextWindowView.BattleUITechniqueTextWindow.OnInitialize(commandNameList, true);

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

        string encountMessage = string.Format(m_encountMessage, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(encountMessage);

        await UniTask.WaitWhile(() => m_battleTextWindowView.BattleUIMessageTextWindow.IsMessageAnimation());

        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        m_battleTamamonView.PlayEncountPlayerAnimation().Forget();

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string bringOutMessage = string.Format(m_bringOutMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(bringOutMessage);

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
        string waitMessage = string.Format(m_waitMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageText(waitMessage);

        m_battleTextWindowView.BattleUIActionTextWindow.ResetArrowActive();

        await m_battleTextWindowView.BattleUIActionTextWindow.SelectCommand();

        // �X�e�[�g��ύX
        if (m_battleTextWindowView.BattleUIActionTextWindow.SelectIndex == 0)
        {
            m_battleModel.BattleExecuteState = BattleModel.BattleExecuteType.Technique;
            m_battleModel.BattleState = BattleModel.BattleStateType.TechniqueSelect;
        }
        else if (m_battleTextWindowView.BattleUIActionTextWindow.SelectIndex == 1)
        {
            m_battleModel.BattleExecuteState = BattleModel.BattleExecuteType.Item;
            m_battleModel.BattleState = BattleModel.BattleStateType.ItemSelect;
        }
        else if (m_battleTextWindowView.BattleUIActionTextWindow.SelectIndex == 2)
        {
            m_battleModel.BattleExecuteState = BattleModel.BattleExecuteType.Change;
            m_battleModel.BattleState = BattleModel.BattleStateType.TamamonSelect;
        }
        else if (m_battleTextWindowView.BattleUIActionTextWindow.SelectIndex == 3)
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
        foreach (var statusData in m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList)
        {
            commandNameList.Add(statusData.TechniqueData.Name);
        }
        m_battleTextWindowView.BattleUITechniqueTextWindow.UpdateCommandText(commandNameList, true);
        m_battleTextWindowView.BattleUITechniqueTextWindow.ResetArrowActive();
        var data = m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList[m_battleTextWindowView.BattleUITechniqueTextWindow.SelectIndex];
        m_battleTextWindowView.BattleUITechniqueInfoTextWindow.ShowText(data.TechniquePP, data.TechniqueNowPP, TypeData.TypeNameDictionary[data.TechniqueData.Type]);

        await m_battleTextWindowView.BattleUITechniqueTextWindow.SelectCommand();

        m_commandIndex = m_battleTextWindowView.BattleUITechniqueTextWindow.SelectIndex;

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

        // AI�s������
        // ���@���͕K���Z��ł�
        BattleModel.BattleExecuteType enemyExecuteType = BattleModel.BattleExecuteType.Technique;

        // �s�����ԃ`�F�b�N
        // �s���X�e�[�g�̃E�F�C�g����s���D��x���Ƃɕ���
        int enemyMoveWait = (int)enemyExecuteType;
        int playerMoveWait = (int)m_battleModel.BattleExecuteState;
        bool isPlayerTurn = true;

        if (enemyMoveWait > playerMoveWait)
        {
            isPlayerTurn = false;
        }
        else if (enemyMoveWait < playerMoveWait)
        {
            isPlayerTurn = true;
        }
        else if (enemyMoveWait == playerMoveWait)
        {
            if (m_battleModel.BattleExecuteState == enemyExecuteType)
            {
                // �f�����`�F�b�N
                if (m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.Speed > m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.Speed)
                {
                    isPlayerTurn = false;
                }
                else if (m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.Speed < m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.Speed)
                {
                    isPlayerTurn = true;
                }
                else if (m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.Speed == m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.Speed)
                {
                    UnityEngine.Random.InitState(DateTime.Now.Millisecond);
                    isPlayerTurn = UnityEngine.Random.Range(0, 100) <= 50 ? false : true;
                }
            }
        }

        // ���s
        if (isPlayerTurn)
        {
            await OnPlayerBattleStateExecute(m_battleModel.BattleExecuteState);

            if (m_battleModel.BattleState == BattleModel.BattleStateType.Result) return;

            // �^�}������ԃ`�F�b�N
            // �G�l�~�[�^�}�������m���ɂȂ������ǂ���
            if (m_battleModel.IsEnemyFainting())
            {
                await OnEnemyFainting();
                // �^�[���I���X�e�[�g�ɕύX
                m_battleModel.BattleState = BattleModel.BattleStateType.TurnEnd;
                return;
            }
            // �v���C���[�^�}�������m���ɂȂ������ǂ���
            if (m_battleModel.IsPlayerFainting())
            {
                await OnPlayerFainting();
            }

            await OnEnemyBattleStateExecute(enemyExecuteType);

            if (m_battleModel.BattleState == BattleModel.BattleStateType.Result) return;

            // �v���C���[�^�}�������m���ɂȂ������ǂ���
            if (m_battleModel.IsPlayerFainting())
            {
                await OnPlayerFainting();
            }

            // �^�}������ԃ`�F�b�N
            // �G�l�~�[�^�}�������m���ɂȂ������ǂ���
            if (m_battleModel.IsEnemyFainting())
            {
                await OnEnemyFainting();
            }
        }
        else
        {
            await OnEnemyBattleStateExecute(enemyExecuteType);

            // �v���C���[�^�}�������m���ɂȂ������ǂ���
            if (m_battleModel.IsPlayerFainting())
            {
                await OnPlayerFainting();

                // �^�[���I���X�e�[�g�ɕύX
                m_battleModel.BattleState = BattleModel.BattleStateType.TurnEnd;
                return;
            }

            // �^�}������ԃ`�F�b�N
            // �G�l�~�[�^�}�������m���ɂȂ������ǂ���
            if (m_battleModel.IsEnemyFainting())
            {
                await OnEnemyFainting();
            }

            await OnPlayerBattleStateExecute(m_battleModel.BattleExecuteState);

            // �^�}������ԃ`�F�b�N
            // �G�l�~�[�^�}�������m���ɂȂ������ǂ���
            if (m_battleModel.IsEnemyFainting())
            {
                await OnEnemyFainting();
            }

            // �v���C���[�^�}�������m���ɂȂ������ǂ���
            if (m_battleModel.IsPlayerFainting())
            {
                await OnPlayerFainting();
            }
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

        // �s�k�X�e�[�g�ɐ؂�ւ�
        if (m_playerCount < 1)
        {
            m_battleModel.BattleEndState = BattleModel.BattleEndType.Lose;
            m_battleModel.BattleState = BattleModel.BattleStateType.Result;
            return;
        }

        // �����X�e�[�g�ɐ؂�ւ�
        if (m_enemyCount < 1)
        {
            m_battleModel.BattleEndState = BattleModel.BattleEndType.Win;
            m_battleModel.BattleState = BattleModel.BattleStateType.Result;
            return;
        }

        // �����|��Ă�����X�e�[�g���㏑��
        if (m_battleModel.IsPlayerFainting() && m_battleModel.IsEnemyFainting())
        {
            m_battleModel.BattleTurnEndState = BattleModel.BattleTurnEndType.AllDown;
        }

        // ���
        if (m_battleModel.BattleTurnEndState == BattleModel.BattleTurnEndType.PlayerDown || m_battleModel.BattleTurnEndState == BattleModel.BattleTurnEndType.AllDown)
        {
            isTamamonSelectEscape = false;
            m_tamamonSelectController.UpdateData(m_battleModel.GetPlayerList());

            await m_tamamonSelectController.Show(isTamamonSelectEscape);
            await OnPlayerGoExecute();
        }

        if (m_battleModel.BattleTurnEndState == BattleModel.BattleTurnEndType.EnemyDown || m_battleModel.BattleTurnEndState == BattleModel.BattleTurnEndType.AllDown)
        {
            await OnEnemyGoExecute(1);
        }
        m_battleModel.BattleTurnEndState = BattleModel.BattleTurnEndType.None;
        m_battleModel.BattleState = BattleModel.BattleStateType.ActionSelect;
    }

    private bool isTamamonSelectEscape = true;
    /// <summary>
    /// �^�}�����I��������
    /// </summary>
    /// <returns></returns>
    public async UniTask OnTamamonSelect()
    {
        m_tamamonSelectController.UpdateData(m_battleModel.GetPlayerList());
        isTamamonSelectEscape = true;
        await m_tamamonSelectController.Show(isTamamonSelectEscape);

        switch (m_tamamonSelectController.TamamonSelectState)
        {
            case TamamonSelectController.TamamonSelectStateType.Change:
                m_battleModel.BattleExecuteState = BattleModel.BattleExecuteType.Change;
                m_battleModel.BattleState = BattleModel.BattleStateType.Execute;
                break;
            case TamamonSelectController.TamamonSelectStateType.Close:
                m_battleModel.BattleState = BattleModel.BattleStateType.ActionSelect;
                break;
        }
    }

    /// <summary>
    /// �A�C�e���I��������
    /// </summary>
    /// <returns></returns>
    public async UniTask OnItemSelect()
    {
        UIManager.Instance.LoadInventoryObject();

        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

        UIManager.Instance.UnLoadInventoryObject();

        m_battleModel.BattleState = BattleModel.BattleStateType.ActionSelect;
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
                string loseMessage = string.Format(m_loseMessage, m_playerName);
                await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(loseMessage);
                await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
                break;
            case BattleModel.BattleEndType.Escape:
                break;
            default:
                break;
        }

        // �o�g���V�[���j��
        await BattleManager.Instance.UnLoadBattleScene();

        await SoundManager.Instance.StopBGMAsync();
    }

    /// <summary>
    /// �G�l�~�[���Z�I�������s�R�[���o�b�N
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEnemyTechniqueExecute()
    {
        // �����_���ɋZ��I��
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        int index = UnityEngine.Random.Range(0, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.TechniqueList.Count);

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();

        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name}��{m_battleModel.EnemyStatusData.TamamonStatusDataInfo.TechniqueList[index].TechniqueData.Name}�I");

        m_battleUIView.UpdatePlayerHpBar(m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(index, false));
        m_battleModel.PlayerStatusData.UpdateNowHP(m_battleModel.GetDamageValue(index, false));

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

        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
    }

    /// <summary>
    /// �G�l�~�[���s�����s
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEnemyBattleStateExecute(BattleModel.BattleExecuteType state)
    {
        switch (state)
        {
            case BattleModel.BattleExecuteType.None:
                break;
            case BattleModel.BattleExecuteType.Technique:
                await OnEnemyTechniqueExecute();
                break;
            case BattleModel.BattleExecuteType.Item:
                await OnEnemyItemExecute();
                break;
            case BattleModel.BattleExecuteType.Change:
                await OnEnemyComeBackExecute();
                await OnEnemyGoExecute(1);
                break;
            case BattleModel.BattleExecuteType.Escape:
                await OnEnemyEscapeExecute();
                break;
        }
    }

    /// <summary>
    /// �v���C���[���s�����s
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public async UniTask OnPlayerBattleStateExecute(BattleModel.BattleExecuteType state)
    {
        switch (state)
        {
            case BattleModel.BattleExecuteType.None:
                break;
            case BattleModel.BattleExecuteType.Technique:
                await OnPlayerTechniqueExecute();
                break;
            case BattleModel.BattleExecuteType.Item:
                await OnPlayerItemExecute();
                break;
            case BattleModel.BattleExecuteType.Change:
                await OnPlayerComeBackExecute();
                await OnPlayerGoExecute();
                break;
            case BattleModel.BattleExecuteType.Escape:
                await OnPlayerEscapeExecute();
                break;
        }
    }

    /// <summary>
    /// �v���C���[���Z�I�������s�R�[���o�b�N
    /// </summary>
    /// <returns></returns>
    public async UniTask OnPlayerTechniqueExecute()
    {
        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name}��{m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList[m_commandIndex].TechniqueData.Name}�I");

        m_battleUIView.UpdateEnemyHpBar(m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(m_commandIndex, true));
        m_battleModel.EnemyStatusData.UpdateNowHP(m_battleModel.GetDamageValue(m_commandIndex, true));
        m_battleModel.PlayerStatusData.UpdateTechniqueNowPP(1, m_commandIndex);

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
        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
    }

    /// <summary>
    /// �G�l�~�[���^�}���������s�R�[���o�b�N
    /// </summary>
    public async UniTask OnEnemyComeBackExecute()
    {
        m_battleTamamonView.OnBackAnimation(true).Forget();
        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string changeMessage = string.Format(m_changeMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(changeMessage);

        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));        
    }

    /// <summary>
    /// �G�l�~�[���^�}���������s�R�[���o�b�N
    /// </summary>
    public async UniTask OnEnemyGoExecute(int index)
    {
        m_battleModel.EnemyStatusData = m_battleModel.GetPlayerList()[index];
        m_battleUIView.ShowEnemyUI(m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Sex, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Level, m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.NowHP);

        m_battleTamamonView.UpdateEnemyImage(m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Id);
        m_battleTamamonView.OnGoAnimation(false).Forget();
        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string bringOutMessage = string.Format(m_bringOutMessage, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(bringOutMessage);

        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    /// <summary>
    /// �v���C���[���^�}���������s�R�[���o�b�N
    /// </summary>
    public async UniTask OnPlayerComeBackExecute()
    {
        m_battleTamamonView.OnBackAnimation(true).Forget();
        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string changeMessage = string.Format(m_changeMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(changeMessage);

        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    /// <summary>
    /// �v���C���[���^�}���������s�R�[���o�b�N
    /// </summary>
    public async UniTask OnPlayerGoExecute()
    {
        m_battleModel.PlayerStatusData = m_battleModel.GetPlayerList()[m_tamamonSelectController.GetSelectIndex()];
        m_battleUIView.ShowPlayerUI(m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Sex, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Level, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Exp, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowExp, m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowHP);

        m_battleTamamonView.UpdatePlayerImage(m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Id);
        m_battleTamamonView.OnGoAnimation(true).Forget();
        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string bringOutMessage = string.Format(m_bringOutMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(bringOutMessage);

        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    /// <summary>
    /// �G�l�~�[���A�C�e���g�p�R�[���o�b�N
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEnemyItemExecute()
    {
        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    /// <summary>
    /// �v���C���[���A�C�e���g�p�R�[���o�b�N
    /// </summary>
    /// <returns></returns>
    public async UniTask OnPlayerItemExecute()
    {
        // �f�B���C�������Ă��玟�ɍs��
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    /// <summary>
    /// �G�l�~�[����������s���R�[���o�b�N
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEnemyEscapeExecute()
    {
        // ������邩������Ȃ���
        // todo ���͊m��œ������
        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_escapeMessage);
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        m_battleModel.BattleEndState = BattleModel.BattleEndType.Escape;
        m_battleModel.BattleState = BattleModel.BattleStateType.Result;
        return;
    }

    /// <summary>
    /// �v���C���[����������s���R�[���o�b�N
    /// </summary>
    /// <returns></returns>
    public async UniTask OnPlayerEscapeExecute()
    {
        // ������邩������Ȃ���
        // todo ���͊m��œ������
        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_escapeMessage);
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        m_battleModel.BattleEndState = BattleModel.BattleEndType.Escape;
        m_battleModel.BattleState = BattleModel.BattleStateType.Result;
        return;
    }

    /// <summary>
    /// �G�l�~�[�퓬�s�\�R�[���o�b�N
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEnemyFainting()
    {
        m_battleModel.BattleTurnEndState = BattleModel.BattleTurnEndType.EnemyDown;
        m_enemyCount--;

        // �퓬�s�\�A�j���[�V�����Đ�
        await m_battleTamamonView.OnDownAnimation(false);

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string faintingMessage = string.Format(m_faintingMessage, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(faintingMessage);
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
    }

    /// <summary>
    /// �v���C���[�퓬�s�\�R�[���o�b�N
    /// </summary>
    /// <returns></returns>
    public async UniTask OnPlayerFainting()
    {
        m_battleModel.BattleTurnEndState = BattleModel.BattleTurnEndType.PlayerDown;
        m_playerCount--;

        // �퓬�s�\�A�j���[�V�����Đ�
        await m_battleTamamonView.OnDownAnimation(true);

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string faintingMessage = string.Format(m_faintingMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(faintingMessage);
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
    }
}