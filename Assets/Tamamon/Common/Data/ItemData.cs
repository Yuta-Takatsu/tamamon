using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A�C�e�����N���X
/// </summary>
public class ItemData
{
    public int Id;

    public string Name;

    public InventoryCategoryType Category;

    public string Desc;


    /// <summary>
    /// ����J�e�S���[
    /// </summary>
    public enum InventoryCategoryType
    {
        Heel,       // ��
        Item,       // ����
        BattleItem, // �퓬�p����
        Berry,      // ���̂�
        Technique,  // �Z�}�V��
        Capsule,    // �J�v�Z��    
        KeyItem,    // ��؂Ȃ���
    }
}
