using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�X�|�i�[(�t�B�[���h)
/// </summary>
public class PlayerSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject m_spawnPlayerPref;

    private GameObject m_spawningPlayer;

    // Start is called before the first frame update
    void Start()
    {
        m_spawningPlayer = SpawnPlayer();

        if( !m_spawningPlayer ) return;

        var playerPos = new Vector3(
            this.transform.position.x,
            this.transform.position.y,
            -1);

        m_spawningPlayer.transform.position = playerPos;
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// �v���C���[���X�|�[������
    /// </summary>
    /// <returns></returns>
    GameObject SpawnPlayer()
	{
        if( !m_spawnPlayerPref ) return null;

        return Instantiate(m_spawnPlayerPref);
	}
}
