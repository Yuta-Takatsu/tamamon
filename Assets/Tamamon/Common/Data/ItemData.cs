using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム情報クラス
/// </summary>
public class ItemData
{
    public int Id;

    public string Name;

    public InventoryCategoryType Category;

    public string Desc;


    /// <summary>
    /// 道具カテゴリー
    /// </summary>
    public enum InventoryCategoryType
    {
        Heel,       // 回復
        Item,       // 道具
        BattleItem, // 戦闘用道具
        Berry,      // きのみ
        Technique,  // 技マシン
        Capsule,    // カプセル    
        KeyItem,    // 大切なもの
    }
}
