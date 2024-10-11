using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using Framework.Sound;
using Framework.Scene;

/// <summary>
/// バトルコントローラークラス
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

    // 仮データ変数
    private string m_encountMessage = "野生の {0} が現れた！";

    private string m_changeMessage = "戻れ！{0}！";

    private string m_bringOutMessage = "行け ! {0}！";

    private string m_waitMessage = "{0} はどうする？";
    private string m_escapeMessage = "うまく逃げ切れた！";

    private string m_effectiveMessage = "効果は 抜群だ！";
    private string m_notEffectiveMessage = "効果は いまひとつのようだ";
    private string m_dontAffectiveMessage = "効果は 無いようだ...";

    private string m_faintingMessage = "{0}は倒れた";

    private string m_getExpMessage = "{0}は\n{1} 経験値を もらった";

    private string m_loseMessage = "{0}は目の前が真っ暗になった...";

    private string m_playerName = "セラ";

    private List<string> m_actionCommandList = new List<string>() { "戦う", "バッグ", "タマモン", "逃げる" };

    public void Start()
    {
        OnInitialize(3);
    }
    /// <summary>
    /// 初期化
    /// </summary>
    public void OnInitialize(int enemyId)
    {
        m_battleModel = new BattleModel();

        SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.Battle);

        // ステート変更時のコールバック登録
        m_battleModel.OnStateExecute();
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.Encount, async () => await OnEncount());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.ActionSelect, async () => await OnActionSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.TechniqueSelect, async () => await OnTechniqueSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.Execute, async () => await OnExecute());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.TurnEnd, async () => await OnTurnEnd());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.TamamonSelect, async () => await OnTamamonSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.ItemSelect, async () => await OnItemSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.Result, async () => await OnResult());


        // タマモン情報初期化  
        TamamonStatusData enemyData_1 = new TamamonStatusData();
        enemyData_1.OnInitialize(enemyId, TamamonData.SexType.Male, 5, 1, 5, 6);
        m_battleModel.AddEnemyList(enemyData_1);

        TamamonStatusData playerData_1 = new TamamonStatusData();
        playerData_1.OnInitialize(2, TamamonData.SexType.Female, 7, 1, 1, 2, 3);
        m_battleModel.AddPlayerList(playerData_1);

        TamamonStatusData playerData_2 = new TamamonStatusData();
        playerData_2.OnInitialize(enemyId, TamamonData.SexType.Male, 5, 1, 5);
        m_battleModel.AddPlayerList(playerData_2);

        // 手持ちの数
        m_enemyCount = m_battleModel.GetEnemyList().Count;
        m_playerCount = m_battleModel.GetPlayerList().Count;

        // 手持ち先頭情報を取得
        m_battleModel.EnemyStatusData = m_battleModel.GetEnemyList().First();
        m_battleModel.PlayerStatusData = m_battleModel.GetPlayerList().First();

        // 情報をUIに渡す
        m_battleUIView.ShowEnemyUI(m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Sex, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Level, m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.NowHP);
        m_battleUIView.ShowPlayerUI(m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Sex, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Level, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Exp, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowExp, m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowHP);
        m_battleTextWindowView.BattleUIMessageTextWindow.OnInitialize();
        m_battleTamamonView.OnInitialize(enemyId, 2);
        m_tamamonSelectController.OnInitialize(m_battleModel.GetPlayerList(), TamamonSelectController.TamamonSelectViewType.Battle);

        // 行動コマンド初期化
        m_battleTextWindowView.BattleUIActionTextWindow.OnInitialize(m_actionCommandList);

        // 技表記初期化
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

        // エンカウントステートに変更
        m_battleModel.BattleState = BattleModel.BattleStateType.Encount;
    }

    /// <summary>
    /// エンカウント時処理
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEncount()
    {

        await SceneManager.Instance.FadeOut();

        await m_battleTamamonView.PlayEncountEnemyAnimation();

        string encountMessage = string.Format(m_encountMessage, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(encountMessage);

        await UniTask.WaitWhile(() => m_battleTextWindowView.BattleUIMessageTextWindow.IsMessageAnimation());

        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        m_battleTamamonView.PlayEncountPlayerAnimation().Forget();

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string bringOutMessage = string.Format(m_bringOutMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(bringOutMessage);

        await UniTask.WaitWhile(() => m_battleTamamonView.IsAnimation);
        await UniTask.WaitWhile(() => m_battleTextWindowView.BattleUIMessageTextWindow.IsMessageAnimation());

        // 行動選択ステートに変更
        m_battleModel.BattleState = BattleModel.BattleStateType.ActionSelect;
    }

    /// <summary>
    /// 行動選択時処理
    /// </summary>
    /// <returns></returns>
    public async UniTask OnActionSelect()
    {
        // TextWindow表示切替
        m_battleTextWindowView.BattleUIMessageTextWindow.gameObject.SetActive(true);
        m_battleTextWindowView.BattleUIActionTextWindow.gameObject.SetActive(true);
        m_battleTextWindowView.BattleUITechniqueTextWindow.gameObject.SetActive(false);
        m_battleTextWindowView.BattleUITechniqueInfoTextWindow.gameObject.SetActive(false);

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string waitMessage = string.Format(m_waitMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageText(waitMessage);

        m_battleTextWindowView.BattleUIActionTextWindow.ResetArrowActive();

        await m_battleTextWindowView.BattleUIActionTextWindow.SelectCommand();

        // ステートを変更
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
    /// 技選択時処理
    /// </summary>
    /// <returns></returns>
    public async UniTask OnTechniqueSelect()
    {
        // TextWindow表示切替
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

        // ステートを変更
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
    /// 戦闘時処理
    /// </summary>
    /// <returns></returns>
    public async UniTask OnExecute()
    {
        // TextWindow表示切替
        m_battleTextWindowView.BattleUIMessageTextWindow.gameObject.SetActive(true);
        m_battleTextWindowView.BattleUIActionTextWindow.gameObject.SetActive(false);
        m_battleTextWindowView.BattleUITechniqueTextWindow.gameObject.SetActive(false);
        m_battleTextWindowView.BattleUITechniqueInfoTextWindow.gameObject.SetActive(false);

        // AI行動決定
        // 仮　今は必ず技を打つ
        BattleModel.BattleExecuteType enemyExecuteType = BattleModel.BattleExecuteType.Technique;

        // 行動順番チェック
        // 行動ステートのウェイトから行動優先度ごとに分岐
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
                // 素早さチェック
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

        // 実行
        if (isPlayerTurn)
        {
            await OnPlayerBattleStateExecute(m_battleModel.BattleExecuteState);

            if (m_battleModel.BattleState == BattleModel.BattleStateType.Result) return;

            // タマモン状態チェック
            // エネミータマモンが瀕死になったかどうか
            if (m_battleModel.IsEnemyFainting())
            {
                await OnEnemyFainting();
                // ターン終了ステートに変更
                m_battleModel.BattleState = BattleModel.BattleStateType.TurnEnd;
                return;
            }
            // プレイヤータマモンが瀕死になったかどうか
            if (m_battleModel.IsPlayerFainting())
            {
                await OnPlayerFainting();
            }

            await OnEnemyBattleStateExecute(enemyExecuteType);

            if (m_battleModel.BattleState == BattleModel.BattleStateType.Result) return;

            // プレイヤータマモンが瀕死になったかどうか
            if (m_battleModel.IsPlayerFainting())
            {
                await OnPlayerFainting();
            }

            // タマモン状態チェック
            // エネミータマモンが瀕死になったかどうか
            if (m_battleModel.IsEnemyFainting())
            {
                await OnEnemyFainting();
            }
        }
        else
        {
            await OnEnemyBattleStateExecute(enemyExecuteType);

            // プレイヤータマモンが瀕死になったかどうか
            if (m_battleModel.IsPlayerFainting())
            {
                await OnPlayerFainting();

                // ターン終了ステートに変更
                m_battleModel.BattleState = BattleModel.BattleStateType.TurnEnd;
                return;
            }

            // タマモン状態チェック
            // エネミータマモンが瀕死になったかどうか
            if (m_battleModel.IsEnemyFainting())
            {
                await OnEnemyFainting();
            }

            await OnPlayerBattleStateExecute(m_battleModel.BattleExecuteState);

            // タマモン状態チェック
            // エネミータマモンが瀕死になったかどうか
            if (m_battleModel.IsEnemyFainting())
            {
                await OnEnemyFainting();
            }

            // プレイヤータマモンが瀕死になったかどうか
            if (m_battleModel.IsPlayerFainting())
            {
                await OnPlayerFainting();
            }
        }
        // ターン終了ステートに変更
        m_battleModel.BattleState = BattleModel.BattleStateType.TurnEnd;
    }

    /// <summary>
    /// ターン終了時処理
    /// </summary>
    /// <returns></returns>
    public async UniTask OnTurnEnd()
    {
        // TextWindow表示切替
        m_battleTextWindowView.BattleUIMessageTextWindow.gameObject.SetActive(true);
        m_battleTextWindowView.BattleUIActionTextWindow.gameObject.SetActive(false);
        m_battleTextWindowView.BattleUITechniqueTextWindow.gameObject.SetActive(false);
        m_battleTextWindowView.BattleUITechniqueInfoTextWindow.gameObject.SetActive(false);

        // 敗北ステートに切り替え
        if (m_playerCount < 1)
        {
            m_battleModel.BattleEndState = BattleModel.BattleEndType.Lose;
            m_battleModel.BattleState = BattleModel.BattleStateType.Result;
            return;
        }

        // 勝利ステートに切り替え
        if (m_enemyCount < 1)
        {
            m_battleModel.BattleEndState = BattleModel.BattleEndType.Win;
            m_battleModel.BattleState = BattleModel.BattleStateType.Result;
            return;
        }

        // 両方倒れていたらステートを上書き
        if (m_battleModel.IsPlayerFainting() && m_battleModel.IsEnemyFainting())
        {
            m_battleModel.BattleTurnEndState = BattleModel.BattleTurnEndType.AllDown;
        }

        // 交代
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
    /// タマモン選択時処理
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
    /// アイテム選択時処理
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
    /// 戦闘終了時処理
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

                // 経験値取得
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

        // バトルシーン破棄
        await BattleManager.Instance.UnLoadBattleScene();

        await SoundManager.Instance.StopBGMAsync();
    }

    /// <summary>
    /// エネミー側技選択時実行コールバック
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEnemyTechniqueExecute()
    {
        // ランダムに技を選択
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        int index = UnityEngine.Random.Range(0, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.TechniqueList.Count);

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();

        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name}の{m_battleModel.EnemyStatusData.TamamonStatusDataInfo.TechniqueList[index].TechniqueData.Name}！");

        m_battleUIView.UpdatePlayerHpBar(m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(index, false));
        m_battleModel.PlayerStatusData.UpdateNowHP(m_battleModel.GetDamageValue(index, false));

        await UniTask.WaitWhile(() => m_battleUIView.IsPlayerHpBarAnimation);

        // タイプ相性テキスト
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

        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
    }

    /// <summary>
    /// エネミー側行動実行
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
    /// プレイヤー側行動実行
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
    /// プレイヤー側技選択時実行コールバック
    /// </summary>
    /// <returns></returns>
    public async UniTask OnPlayerTechniqueExecute()
    {
        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name}の{m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList[m_commandIndex].TechniqueData.Name}！");

        m_battleUIView.UpdateEnemyHpBar(m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(m_commandIndex, true));
        m_battleModel.EnemyStatusData.UpdateNowHP(m_battleModel.GetDamageValue(m_commandIndex, true));
        m_battleModel.PlayerStatusData.UpdateTechniqueNowPP(1, m_commandIndex);

        await UniTask.WaitWhile(() => m_battleUIView.IsEnemyHpBarAnimation);

        // タイプ相性テキスト
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
        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
    }

    /// <summary>
    /// エネミー側タマモン交代実行コールバック
    /// </summary>
    public async UniTask OnEnemyComeBackExecute()
    {
        m_battleTamamonView.OnBackAnimation(true).Forget();
        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string changeMessage = string.Format(m_changeMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(changeMessage);

        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));        
    }

    /// <summary>
    /// エネミー側タマモン交代実行コールバック
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

        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    /// <summary>
    /// プレイヤー側タマモン交代実行コールバック
    /// </summary>
    public async UniTask OnPlayerComeBackExecute()
    {
        m_battleTamamonView.OnBackAnimation(true).Forget();
        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string changeMessage = string.Format(m_changeMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(changeMessage);

        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    /// <summary>
    /// プレイヤー側タマモン交代実行コールバック
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

        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    /// <summary>
    /// エネミー側アイテム使用コールバック
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEnemyItemExecute()
    {
        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    /// <summary>
    /// プレイヤー側アイテム使用コールバック
    /// </summary>
    /// <returns></returns>
    public async UniTask OnPlayerItemExecute()
    {
        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    /// <summary>
    /// エネミー側逃げる実行時コールバック
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEnemyEscapeExecute()
    {
        // 逃げれるか逃げれないか
        // todo 今は確定で逃げれる
        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_escapeMessage);
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        m_battleModel.BattleEndState = BattleModel.BattleEndType.Escape;
        m_battleModel.BattleState = BattleModel.BattleStateType.Result;
        return;
    }

    /// <summary>
    /// プレイヤー側逃げる実行時コールバック
    /// </summary>
    /// <returns></returns>
    public async UniTask OnPlayerEscapeExecute()
    {
        // 逃げれるか逃げれないか
        // todo 今は確定で逃げれる
        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_escapeMessage);
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        m_battleModel.BattleEndState = BattleModel.BattleEndType.Escape;
        m_battleModel.BattleState = BattleModel.BattleStateType.Result;
        return;
    }

    /// <summary>
    /// エネミー戦闘不能コールバック
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEnemyFainting()
    {
        m_battleModel.BattleTurnEndState = BattleModel.BattleTurnEndType.EnemyDown;
        m_enemyCount--;

        // 戦闘不能アニメーション再生
        await m_battleTamamonView.OnDownAnimation(false);

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string faintingMessage = string.Format(m_faintingMessage, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(faintingMessage);
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
    }

    /// <summary>
    /// プレイヤー戦闘不能コールバック
    /// </summary>
    /// <returns></returns>
    public async UniTask OnPlayerFainting()
    {
        m_battleModel.BattleTurnEndState = BattleModel.BattleTurnEndType.PlayerDown;
        m_playerCount--;

        // 戦闘不能アニメーション再生
        await m_battleTamamonView.OnDownAnimation(true);

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        string faintingMessage = string.Format(m_faintingMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(faintingMessage);
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
    }
}