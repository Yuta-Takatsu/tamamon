using System.Collections.Generic;

/// <summary>
/// �^�C�v�֘A���
/// </summary>
public static class TypeData
{
    /// <summary>
    /// �^�C�v�ꗗ
    /// </summary>
    public enum Type
    {
        Normal,   // ��
        Fire,     // ��
        Water,    // ��
        Electric, // �d�C
        Grass,    // ��
        Ice,      // �X
        Fighting, // �i��
        Poison,   // ��
        Ground,   // �n��
        Flying,   // ��s
        Psychic,  // ��
        Bug,      // ��
        Rock,     // ��
        Ghost,    // ��
        Dragon,   // ��
        Dark,     // ��
        Steel,    // �|
        Fairy,    // �d
    }

    public static Dictionary<Type, string> TypeNameDictionary = new Dictionary<Type, string>()
    {
        {Type.Normal, "�m�[�}��" },
        {Type.Fire, "��" },
        {Type.Water, "��" },
        {Type.Electric, "�d�C" },
        {Type.Grass, "��" },
        {Type.Ice, "�X" },
        {Type.Fighting, "�i��" },
        {Type.Poison, "��" },
        {Type.Ground, "�n��" },
        {Type.Flying, "��s" },
        {Type.Psychic, "��" },
        {Type.Bug, "��" },
        {Type.Rock, "��" },
        {Type.Ghost, "��" },
        {Type.Dragon, "��" },
        {Type.Dark, "��" },
        {Type.Steel, "�|" },
        {Type.Fairy, "�d" },
    };

    /// <summary>
    /// ���ʔ��Q���
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
    /// ���ʂ��܂ЂƂ��
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
    /// �������
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