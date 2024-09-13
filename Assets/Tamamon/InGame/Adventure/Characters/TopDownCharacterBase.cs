using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクター基底(見下ろし視点)
/// </summary>
public class TopDownCharacterBase : MonoBehaviour
{

    [SerializeField]
    protected Rigidbody2D m_rigidbody2D;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void OnInitialize()
    {
        m_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {

    }
}

