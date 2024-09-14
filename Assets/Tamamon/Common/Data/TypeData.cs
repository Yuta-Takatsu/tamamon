using System.Collections.Generic;

/// <summary>
/// タイプ関連情報
/// </summary>
public static class TypeData
{
    /// <summary>
    /// タイプ一覧
    /// </summary>
    public enum Type
    {
        Normal,   // 普
        Fire,     // 炎
        Water,    // 水
        Electric, // 電気
        Grass,    // 草
        Ice,      // 氷
        Fighting, // 格闘
        Poison,   // 毒
        Ground,   // 地面
        Flying,   // 飛行
        Psychic,  // 超
        Bug,      // 虫
        Rock,     // 岩
        Ghost,    // 霊
        Dragon,   // 竜
        Dark,     // 悪
        Steel,    // 鋼
        Fairy,    // 妖
    }

    public static Dictionary<Type, string> TypeNameDictionary = new Dictionary<Type, string>()
    {
        {Type.Normal, "ノーマル" },
        {Type.Fire, "ほのお" },
        {Type.Water, "みず" },
        {Type.Electric, "でんき" },
        {Type.Grass, "くさ" },
        {Type.Ice, "こおり" },
        {Type.Fighting, "かくとう" },
        {Type.Poison, "どく" },
        {Type.Ground, "じめん" },
        {Type.Flying, "ひこう" },
        {Type.Psychic, "エスパー" },
        {Type.Bug, "むし" },
        {Type.Rock, "いわ" },
        {Type.Ghost, "ゴースト" },
        {Type.Dragon, "ドラゴン" },
        {Type.Dark, "あく" },
        {Type.Steel, "はがね" },
        {Type.Fairy, "フェアリー" },
    };

    /// <summary>
    /// 効果抜群情報
    /// </summary>
    public static Dictionary<Type, List<Type>> EffectiveDictionary = new Dictionary<Type, List<Type>>()
    {
        { Type.Normal,new List<Type>{ } },
        { Type.Fire,new List<Type>{ Type.Grass,Type.Ice,Type.Bug,Type.Steel} },
        { Type.Water,new List<Type>{ Type.Fire,Type.Ground,Type.Rock} },
        { Type.Electric,new List<Type>{ Type.Water,Type.Flying} },
        { Type.Grass,new List<Type>{ Type.Water,Type.Ground,Type.Rock} },
        { Type.Ice,new List<Type>{ Type.Grass,Type.Ground,Type.Flying,Type.Dragon} },
        { Type.Fighting,new List<Type>{ Type.Normal,Type.Ice,Type.Rock,Type.Dark,Type.Steel} },
        { Type.Poison,new List<Type>{ Type.Grass,Type.Fairy} },
        { Type.Ground,new List<Type>{ Type.Fire,Type.Electric,Type.Poison,Type.Rock,Type.Steel} },
        { Type.Flying,new List<Type>{ Type.Grass,Type.Fighting,Type.Bug} },
        { Type.Psychic,new List<Type>{ Type.Fighting,Type.Poison} },
        { Type.Bug,new List<Type>{ Type.Grass,Type.Psychic,Type.Dark} },
        { Type.Rock,new List<Type>{ Type.Fire,Type.Ice,Type.Flying,Type.Bug} },
        { Type.Ghost,new List<Type>{ Type.Psychic,Type.Ghost} },
        { Type.Dragon,new List<Type>{ Type.Dragon} },
        { Type.Dark,new List<Type>{ Type.Psychic,Type.Ghost} },
        { Type.Steel,new List<Type>{ Type.Ice,Type.Rock,Type.Fairy} },
        { Type.Fairy,new List<Type>{ Type.Fighting,Type.Dragon,Type.Dark} },
    };

    /// <summary>
    /// 効果いまひとつ情報
    /// </summary>
    public static Dictionary<Type, List<Type>> NotEffectiveDictionary = new Dictionary<Type, List<Type>>()
    {
        { Type.Normal,new List<Type>{ Type.Rock,Type.Steel} },
        { Type.Fire,new List<Type>{ Type.Fire,Type.Water,Type.Rock,Type.Dragon} },
        { Type.Water,new List<Type>{Type.Water,Type.Grass,Type.Dragon } },
        { Type.Electric,new List<Type>{Type.Electric,Type.Grass,Type.Dragon } },
        { Type.Grass,new List<Type>{ Type.Fire,Type.Grass,Type.Poison,Type.Flying,Type.Bug,Type.Dragon,Type.Steel} },
        { Type.Ice,new List<Type>{Type.Fire,Type.Water,Type.Ice,Type.Steel } },
        { Type.Fighting,new List<Type>{ Type.Poison,Type.Flying,Type.Psychic,Type.Bug,Type.Fairy} },
        { Type.Poison,new List<Type>{ Type.Poison,Type.Ground,Type.Rock,Type.Ghost} },
        { Type.Ground,new List<Type>{ Type.Grass,Type.Bug} },
        { Type.Flying,new List<Type>{ Type.Electric,Type.Rock,Type.Steel} },
        { Type.Psychic,new List<Type>{ Type.Psychic,Type.Steel} },
        { Type.Bug,new List<Type>{ Type.Fire,Type.Fighting,Type.Poison,Type.Flying,Type.Ghost,Type.Steel,Type.Fairy} },
        { Type.Rock,new List<Type>{Type.Fighting,Type.Ground,Type.Steel} },
        { Type.Ghost,new List<Type>{ Type.Dark} },
        { Type.Dragon,new List<Type>{ Type.Steel} },
        { Type.Dark,new List<Type>{ Type.Fighting,Type.Dark,Type.Fairy} },
        { Type.Steel,new List<Type>{Type.Fire,Type.Water,Type.Electric,Type.Steel } },
        { Type.Fairy,new List<Type>{Type.Fire,Type.Poison,Type.Steel } },
    };

    /// <summary>
    /// 無効情報
    /// </summary>
    public static Dictionary<Type, List<Type>> DontAffectDictionary = new Dictionary<Type, List<Type>>()
    {
        { Type.Normal,new List<Type>{ Type.Ghost} },
        { Type.Fire,new List<Type>{ } },
        { Type.Water,new List<Type>{ } },
        { Type.Electric,new List<Type>{Type.Ground } },
        { Type.Grass,new List<Type>{ } },
        { Type.Ice,new List<Type>{ } },
        { Type.Fighting,new List<Type>{Type.Ghost } },
        { Type.Poison,new List<Type>{ Type.Steel} },
        { Type.Ground,new List<Type>{Type.Flying } },
        { Type.Flying,new List<Type>{ } },
        { Type.Psychic,new List<Type>{Type.Dark } },
        { Type.Bug,new List<Type>{ } },
        { Type.Rock,new List<Type>{} },
        { Type.Ghost,new List<Type>{Type.Normal } },
        { Type.Dragon,new List<Type>{ Type.Fairy} },
        { Type.Dark,new List<Type>{ } },
        { Type.Steel,new List<Type>{ } },
        { Type.Fairy,new List<Type>{ } },
    };
}