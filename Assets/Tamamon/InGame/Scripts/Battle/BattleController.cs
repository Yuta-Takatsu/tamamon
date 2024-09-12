using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

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

    private Tamamon.TamamonDataInfo m_enemyTamamon = new Tamamon.TamamonDataInfo();
    private Tamamon.TamamonDataInfo m_playerTamamon = new Tamamon.TamamonDataInfo();

    // 仮データ変数
    private string m_encountMessage = "野生の {0} が現れた!";

    private string m_bringOutMessage = "行け ! {0} !!";

    private string m_waitMessage = "{0} はどうする？";

    private List<string> m_actionCommandList = new List<string>() { "戦う", "バッグ", "タマモン", "逃げる" };
    private List<string> m_TechiqueCommandList = new List<string>() { "シャドーボール", "パワージェム", "大地の力" };

    public void Start()
    {
        OnInitialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void OnInitialize()
    {
        m_battleModel = new BattleModel();

        m_battleModel.OnInitialize();
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.Encount, async () => await OnEncount());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.ActionSelect, async () => await OnActionSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.TechniqueSelect, async () => await OnTechniqueSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.Execute, async () => await OnExecute());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.TurnEnd, async () => await OnTurnEnd());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.TamamonSelect, async () => await OnTamamonSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.ItemSelect, async () => await OnItemSelect());
        m_battleModel.SetCallbackDictionary(BattleModel.BattleStateType.Result, async () => await OnResult());

        m_enemyTamamon.Name = "イレイワト";
        m_enemyTamamon.Level = 5;
        m_enemyTamamon.Sex = Tamamon.SexType.Male;
        m_enemyTamamon.MaxHP = 22;
        m_enemyTamamon.NowHP = m_enemyTamamon.MaxHP;

        m_playerTamamon.Name = "イレイワト";
        m_playerTamamon.Level = 5;
        m_playerTamamon.Sex = Tamamon.SexType.Female;
        m_playerTamamon.MaxExp = 100;
        m_playerTamamon.NowExp = 30;
        m_playerTamamon.MaxHP = 20;
        m_playerTamamon.NowHP = 15;

        m_battleTamamonView.OnInitialize(m_enemyTamamon, m_playerTamamon);

        // 情報をUIに渡す
        m_battleUIView.ShowEnemyUI(m_enemyTamamon.Name, m_enemyTamamon.Sex, m_enemyTamamon.Level, m_enemyTamamon.MaxHP, m_enemyTamamon.NowHP);
        m_battleUIView.ShowPlayerUI(m_playerTamamon.Name, m_playerTamamon.Sex, m_playerTamamon.Level, m_playerTamamon.MaxExp, m_playerTamamon.NowExp, m_playerTamamon.MaxHP, m_playerTamamon.NowHP);

        m_battleTextWindowView.BattleUIMessageTextWindow.OnInitialize();

        // エンカウントステートに変更
        m_battleModel.BattleState = BattleModel.BattleStateType.Encount;
    }

    /// <summary>
    /// エンカウント時処理
    /// </summary>
    /// <returns></returns>
    public async UniTask OnEncount()
    {
        m_battleTamamonView.PlayEncountEnemyAnimation();

        await UniTask.WaitWhile(() => m_battleTamamonView.IsEncountEnemyAnimation());

        m_encountMessage = string.Format(m_encountMessage, m_enemyTamamon.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_encountMessage);

        await UniTask.WaitWhile(() => m_battleTextWindowView.BattleUIMessageTextWindow.IsMessageAnimation());

        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

        m_battleTamamonView.PlayEncountPlayerAnimation();

        m_battleTextWindowView.BattleUIMessageTextWindow.ClearText();
        m_bringOutMessage = string.Format(m_bringOutMessage, m_playerTamamon.Name);
        await m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageTextAsync(m_bringOutMessage);

        await UniTask.WaitWhile(() => m_battleTamamonView.IsEncountPlayerAnimation());
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
        m_waitMessage = string.Format(m_waitMessage, m_playerTamamon.Name);
        m_battleTextWindowView.BattleUIMessageTextWindow.ShowMessageText(m_waitMessage);

        m_battleTextWindowView.BattleUIActionTextWindow.OnInitialize(m_actionCommandList);

        int index = await OnInput(m_battleTextWindowView.BattleUIActionTextWindow);

        // ステートを変更
        if (index == 0)
        {
            m_battleModel.BattleState = BattleModel.BattleStateType.TechniqueSelect;
        }
        else if (index == 1)
        {
            m_battleModel.BattleState = BattleModel.BattleStateType.ItemSelect;
        }
        else if (index == 2)
        {
            m_battleModel.BattleState = BattleModel.BattleStateType.TamamonSelect;
        }
        else if (index == 3)
        {
            m_battleModel.BattleState = BattleModel.BattleStateType.Result;
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

        m_battleTextWindowView.BattleUITechniqueTextWindow.OnInitialize(m_TechiqueCommandList);
        m_battleTextWindowView.BattleUITechniqueInfoTextWindow.ShowText(35, 2, "ノーマル");

        int index = await OnInput(m_battleTextWindowView.BattleUITechniqueTextWindow, true);

        // ステートを変更
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

        m_battleUIView.UpdatePlayerHpBar(m_playerTamamon.MaxHP, m_playerTamamon.NowHP, m_playerTamamon.MaxHP);

        await UniTask.WaitWhile(() => m_battleUIView.IsPlayerHpBarAnimation);

        m_battleUIView.UpdateEnemyHpBar(m_enemyTamamon.MaxHP, m_enemyTamamon.NowHP, 20);

        await UniTask.WaitWhile(() => m_battleUIView.IsEnemyHpBarAnimation);

        // ディレイをかけてから次に行く
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

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
        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
    }

    /// <summary>
    /// 入力受付
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