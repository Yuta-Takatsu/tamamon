using System;
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

    private TamamonStatusData m_enemyTamamonData = new TamamonStatusData();
    private TamamonStatusData m_playerTamamonData = new TamamonStatusData();

    private int m_commandIndex = 0;

    // 仮データ変数
    private string m_encountMessage = "野生の {0} が現れた!";

    private string m_bringOutMessage = "行け ! {0} !!";

    private string m_waitMessage = "{0} はどうする？";
    private string m_escapeMessage = "うまく逃げ切れた！";

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
        m_enemyTamamonData.OnInitialize(enemyId, TamamonData.SexType.Male, 5, 1, 1, 2, 3, 4);
        m_playerTamamonData.OnInitialize(playerId, TamamonData.SexType.Female, 7, 1, 1, 2, 3);

        // 情報をUIに渡す
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

        m_encountMessage = string.Format(m_encountMessage, m_enemyTamamonData.TamamonStatusDataInfo.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_encountMessage);

        await UniTask.WaitWhile(() => m_battleTextWindowView.BattleUIMessageTextWindow.IsMessageAnimation());

        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        m_battleTamamonView.PlayEncountPlayerAnimation().Forget();

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        m_bringOutMessage = string.Format(m_bringOutMessage, m_playerTamamonData.TamamonStatusDataInfo.Name);
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
        m_waitMessage = string.Format(m_waitMessage, m_playerTamamonData.TamamonStatusDataInfo.Name);
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
        foreach (var data in m_playerTamamonData.TamamonStatusDataInfo.TechniqueList)
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

                m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
                await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync($"{m_playerTamamonData.TamamonStatusDataInfo.Name}の{m_playerTamamonData.TamamonStatusDataInfo.TechniqueList[m_commandIndex].TechniqueData.Name}!!");

                m_battleUIView.UpdateEnemyHpBar(m_enemyTamamonData.TamamonStatusValueDataInfo.HP, m_enemyTamamonData.TamamonStatusDataInfo.NowHP, m_battleModel.GetDamageValue(m_enemyTamamonData, m_playerTamamonData, m_commandIndex));

                await UniTask.WaitWhile(() => m_battleUIView.IsEnemyHpBarAnimation);

                // ディレイをかけてから次に行く
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

                // ターン終了ステートに変更
                m_battleModel.BattleState = BattleModel.BattleStateType.TurnEnd;

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
                m_battleModel.BattleState = BattleModel.BattleStateType.Result;
                break;
        }
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

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync("お前の負け");

        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
    }

    /// <summary>
    /// タマモン選択時処理
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
        if (m_battleModel.BattleExecuteState == BattleModel.BattleExecuteType.Escape)
        {

        }
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

        await SceneManager.Instance.UnLoadSceneAsync("Battle");
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