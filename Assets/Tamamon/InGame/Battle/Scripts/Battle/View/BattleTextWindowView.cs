using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テキスト表示クラス
/// </summary>
public class BattleTextWindowView : MonoBehaviour
{
    [SerializeField]
    private BattleUIMessageTextWindow m_battleUIMessageTextWindow = default;
    public BattleUIMessageTextWindow BattleUIMessageTextWindow => m_battleUIMessageTextWindow;

    [SerializeField]
    private BattleUICommandTextWindow m_battleUIActionTextWindow = default;
    public BattleUICommandTextWindow BattleUIActionTextWindow => m_battleUIActionTextWindow;

    [SerializeField]
    private BattleUICommandTextWindow m_battleUITechniqueTextWindow = default;
    public BattleUICommandTextWindow BattleUITechniqueTextWindow => m_battleUITechniqueTextWindow;

    [SerializeField]
    private BattleUITechniqueInfoTextWindow m_battleUITechniqueInfoTextWindow = default;
    public BattleUITechniqueInfoTextWindow BattleUITechniqueInfoTextWindow => m_battleUITechniqueInfoTextWindow;
}