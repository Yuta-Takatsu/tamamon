using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectIf : MonoBehaviour
{
    [SerializeField] 
    private Button m_frame = default;
    
    private GameObject m_contentRoot = default;
    private ScrollRect m_scrollRect = default;
    private GameObject m_infiteScrollArea = default;

    public static int co;
    private float pos;
  
    void Start()
    {
        OnInitialize();
    }

    public void OnInitialize()
    {
        m_contentRoot = transform.parent.gameObject;
        m_scrollRect = m_contentRoot.transform.parent.gameObject.GetComponent<ScrollRect>();
        m_infiteScrollArea = m_scrollRect.transform.parent.gameObject;

        pos = 1f / ((float)Item.ItemLength - 5f);
    }

    public void Update()
    {

        if (m_infiteScrollArea.activeSelf == true && co == 0)
        {
            if (this.gameObject.name == "0")
            {
                m_frame.Select();
            }

            co++;   //coは一度だけ選択を合わせるために使用している、これがないとUpdateなので、常に選択され続ける。
        }

        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().position.y >= 800)
            {
                m_scrollRect.verticalNormalizedPosition = m_scrollRect.verticalNormalizedPosition + pos;

            }
            else if (EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().position.y <= 260)
            {
                m_scrollRect.verticalNormalizedPosition = m_scrollRect.verticalNormalizedPosition - pos;
            }
        }
    }
}