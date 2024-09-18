using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

/// <summary>
/// イベント発火ボリューム基底
/// </summary>
public class EventTriggerVolumeBase : MonoBehaviour
{
    [SerializeField]
    BoxCollider2D m_collider;


    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
