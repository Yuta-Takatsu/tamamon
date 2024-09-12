using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー(見下ろし視点)
/// </summary>
public class TopDownPlayer : TopDownCharacterBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
	{
		if ( Input.GetKey(KeyCode.W) ) {
            m_rigidbody2D.position += new Vector2(0,0.1f);
		}

        if ( Input.GetKey(KeyCode.A) ) {
            m_rigidbody2D.position += new Vector2(-0.1f,0);
		}

        if ( Input.GetKey(KeyCode.S) ) {
            m_rigidbody2D.position += new Vector2(0,-0.1f);
		}

        if ( Input.GetKey(KeyCode.D) ) {
            m_rigidbody2D.position += new Vector2(0.1f,0);
		}
	}
}
