using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �L�����N�^�[���(�����낵���_)
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
    /// ������
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

