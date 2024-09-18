using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class Item : UIBehaviour
{
    [SerializeField]
    Image m_itemIcon = default;

    [SerializeField]
    TextMeshProUGUI m_NameText = default;

    [SerializeField]
    TextMeshProUGUI m_valueText = default;

    private List<ItemData> m_itemDataList = new List<ItemData>();
    private ItemData m_itemData = default;
    public ItemData ItemData => m_itemData;

    public static int ItemLength;

    /// <summary>
    /// 所持アイテム情報を読み込む
    /// </summary>
    private void LoadInventoryData()
    {
        List<ItemData> list = new List<ItemData>();

        for (int i = 0; i < 20; i++)
        {
            ItemData itemData = new ItemData();
            itemData.Id = i;
            itemData.Name = $"アイテム_{i}";
            itemData.Category = ItemData.InventoryCategoryType.Heel;
            itemData.Desc = i.ToString();
            list.Add(itemData);
        }
        m_itemDataList = list;
        ItemLength = list.Count;
    }

    public void UpdateItem(int count)
    {
        LoadInventoryData();

        m_itemData = m_itemDataList[count];
        m_valueText.text = (count + 1).ToString("00");

        if (count < ItemLength)
        {
            m_NameText.text = m_itemData.Name;
        }
    }
}