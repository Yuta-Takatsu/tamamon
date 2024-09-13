using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// プレイヤーコントローラー(見下ろし視点)
/// </summary>
public class TopDownPayerController : MonoBehaviour
{

    private TopDownPlayer m_playerClass;

    // Start is called before the first frame update
    void Start()
    {
        OnInitialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void OnInitialize()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Update is called fixed once per frame
    void FixedUpdate()
	{
        if( m_playerClass ) {

        }
	}
}
