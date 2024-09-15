using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using Tamamon.Framework;

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
        OnInitialize(3, 2);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void OnInitialize(int enemyId,int playerId)
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
        enemyData_1.OnInitialize(enemyId, TamamonData.SexType.Male, 5, 1, 5);
        m_battleModel.AddEnemyList(enemyData_1);

        TamamonStatusData playerData_1 = new TamamonStatusData();
        playerData_1.OnInitialize(playerId, TamamonData.SexType.Female, 7, 1, 1, 2, 3);
        m_battleModel.AddPlayerList(playerData_1);

        //TamamonStatusData playerData_2 = new TamamonStatusData();
        //playerData_2.OnInitialize(enemyId, TamamonData.SexType.Male, 5, 1, 5);
        //m_battleModel.AddPlayerList(playerData_2);

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

        m_encountMessage = string.Format(m_encountMessage, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_encountMessage);

        await UniTask.WaitWhile(() => m_battleTextWindowView.BattleUIMessageTextWindow.IsMessageAnimation());

        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        m_battleTamamonView.PlayEncountPlayerAnimation().Forget();

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        m_bringOutMessage = string.Format(m_bringOutMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_bringOutMessage);

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
        m_waitMessage = string.Format(m_waitMessage, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name);
        m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageText(m_waitMessage);

        m_battleTextWindowView.BattleUIActionTextWindow.OnInitialize(m_actionCommandList);

        int index = await OnInput(m_battleTextWindowView.BattleUIActionTextWindow);
       
        // ステートを変更
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
        foreach (var data in m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList)
        {
            commandNameList.Add(data.TechniqueData.Name);
        }

        m_battleTextWindowView.BattleUITechniqueTextWindow.OnInitialize(commandNameList);

        m_commandIndex = await OnInput(m_battleTextWindowView.BattleUITechniqueTextWindow, true);

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

        // 行動ステートによって分岐
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

                // 逃げれるか逃げれないか
                // todo 今は確定で逃げれる
                m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_escapeMessage);
                await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
                m_battleModel.BattleEndState = BattleModel.BattleEndType.Escape;
                m_battleModel.BattleState = BattleModel.BattleStateType.Result;
                return;
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

        // タマモン状態チェック
        // エネミータマモンが瀕死になったかどうか
        if (m_battleModel.IsEnemyFainting())
        {
            m_battleModel.BattleTurnEndState = BattleModel.BattleTurnEndType.EnemyDown;
            m_enemyCount--;

            // 戦闘不能アニメーション再生
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

        // プレイヤータマモンが瀕死になったかどうか
        if (m_battleModel.IsPlayerFainting())
        {
            m_battleModel.BattleTurnEndState = BattleModel.BattleTurnEndType.PlayerDown;
            m_playerCount--;

            // 戦闘不能アニメーション再生
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
    /// タマモン選択時処理
    /// </summary>
    /// <returns></returns>
    public async UniTask OnTamamonSelect()
    {
        m_tamamonSelectController.UpdateData(m_battleModel.GetPlayerList());

        await m_tamamonSelectController.Show();

        m_battleModel.BattleState = BattleModel.BattleStateType.ActionSelect;
    }

    /// <summary>
    /// アイテム選択時処理
    /// </summary>
    /// <returns></returns>
    public async UniTask OnItemSelect()
    {
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
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
    /// 技選択時実行コールバック
    /// </summary>
    /// <returns></returns>
    public async UniTask OnTechniqueExecute()
    {
        // 素早さチェック
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
            await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name}の{m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList[m_commandIndex].TechniqueData.Name}！");

            m_battleUIView.UpdateEnemyHpBar(m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(m_commandIndex, true));
            m_battleModel.EnemyStatusData.UpdateNowHP(m_battleModel.GetDamageValue(m_commandIndex, true));

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

            if (m_battleModel.IsEnemyFainting())
            {
                // ディレイをかけてから次に行く
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                // ターン終了ステートに変更
                m_battleModel.BattleState = BattleModel.BattleStateType.TurnEnd;
                return;
            }

            //瀕死になっていなければエネミーの行動
            // ディレイをかけてから次に行く
            await UniTask.Delay(TimeSpan.FromSeconds(1f));
            m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
            await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name}の{m_battleModel.EnemyStatusData.TamamonStatusDataInfo.TechniqueList[0].TechniqueData.Name}！");

            m_battleUIView.UpdatePlayerHpBar(m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(0, false));
            m_battleModel.PlayerStatusData.UpdateNowHP(m_battleModel.GetDamageValue(0, false));

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
        }
        else
        {
            m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
            await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_battleModel.EnemyStatusData.TamamonStatusDataInfo.Name}の{m_battleModel.EnemyStatusData.TamamonStatusDataInfo.TechniqueList[0].TechniqueData.Name}！");

            m_battleUIView.UpdatePlayerHpBar(m_battleModel.PlayerStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.PlayerStatusData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(0, false));
            m_battleModel.PlayerStatusData.UpdateNowHP(m_battleModel.GetDamageValue(0, false));

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

            if (m_battleModel.IsPlayerFainting())
            {
                // ディレイをかけてから次に行く
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                // ターン終了ステートに変更
                m_battleModel.BattleState = BattleModel.BattleStateType.TurnEnd;
                return;
            }

            // 瀕死になっていなければプレイヤーの行動
            // ディレイをかけてから次に行く
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
            await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_battleModel.PlayerStatusData.TamamonStatusDataInfo.Name}の{m_battleModel.PlayerStatusData.TamamonStatusDataInfo.TechniqueList[m_commandIndex].TechniqueData.Name}！");

            m_battleUIView.UpdateEnemyHpBar(m_battleModel.EnemyStatusData.TamamonStatusValueDataInfo.HP, m_battleModel.EnemyStatusData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(m_commandIndex, true));
            m_battleModel.EnemyStatusData.UpdateNowHP(m_battleModel.GetDamageValue(m_commandIndex, true));

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
        }

        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
    }

    /// <summary>
    /// 入力受付
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