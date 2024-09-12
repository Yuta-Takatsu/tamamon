using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BattleController : MonoBehaviour
{

    [SerializeField]
    private BattleView m_battleTamamonView = default;


    public void Start()
    {
        OnInitialize().Forget();
    }
    public async UniTask OnInitialize()
    {
        Tamamon.TamamonDataInfo enemyData = new Tamamon.TamamonDataInfo();

        enemyData.Name = "イレイワト";
        enemyData.Level = 5;
        enemyData.Sex = Tamamon.SexType.Male;
        enemyData.MaxHP = 22;
        enemyData.NowHP = enemyData.MaxHP;

        Tamamon.TamamonDataInfo playerData = new Tamamon.TamamonDataInfo();

        playerData.Name = "イレイワト";
        playerData.Level = 5;
        playerData.Sex = Tamamon.SexType.Female;
        playerData.MaxExp = 100;
        playerData.NowExp = 30;
        playerData.MaxHP = 20;
        playerData.NowHP = 15;

        await m_battleTamamonView.OnInitialize(enemyData, playerData);
    }
}