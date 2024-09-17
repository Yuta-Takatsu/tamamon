using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private InfiniteScroll m_infiniteScroll = default;

    public void OnInitialize()
    {
        m_infiniteScroll.OnInitialize();
    }
}
