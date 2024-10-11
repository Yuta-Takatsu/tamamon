using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        [SerializeField]
        private InventoryController m_inventoryController = default;

        public void LoadInventoryObject()
        {
            m_inventoryController.gameObject.SetActive(true);
            m_inventoryController.OnInitialize();
        }

        public void UnLoadInventoryObject()
        {
            m_inventoryController.gameObject.SetActive(false);
        }
    }
}