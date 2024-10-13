using System.Collections;
using System.Collections.Generic;
using Tamamon.Ingame.Adventure;
using UnityEngine;

/// <summary>
/// プレイヤー(見下ろし視点)
/// </summary>
public class TopDownPlayer : TopDownCharacterBase
{
    public TopDownPlayerController m_controller;
    public TopDownCharacterView m_view;

    // Start is called before the first frame update
    void Start()
    {
        OnInitialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public override void OnInitialize()
    {
        base.OnInitialize();
        m_controller = new TopDownPlayerController();
        m_controller.OnInitialize(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit!(tag:" + collision.tag + ")");
        if (collision.tag == "Encounter")
        {
            EncountField field = collision.gameObject.GetComponent<EncountField>();
            if(field)
            {
                int encountRate = field.GetEncountRate();
                bool res = EncountManager.Instance.CheckEncount(encountRate);
                if(res)
                {
                    Debug.Log("encount");
                }
            }
        }
    }
}
