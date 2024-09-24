using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MasterData_Monstersに紐づくタマモン情報
/// </summary>
public class MasterData_TamamonMaster : ScriptableObject
{
	[System.Serializable]
	public struct masStruct_monstersInfo {
		int id;											// タマモンID
		int tribeId;									// 種族ID
		int evoGroupVal;								// 進化値				MEMO:最終進化を<9とし、以後１つ手前の進化になるごとに10倍する
		string name;									// 名前					MEMO:テキストマスタあるんか？

		float height;									// 重さ
		float weight;									// 高さ
		int tamaDexId;									// タマモン図鑑ID
		int voiceId;									// 声ID
		int maleRatio;									// オスになる確率
		int femaleRatio;								// メスになる確率		MEMO:性別なしは設けず、maleもfemaleも０なら性別なしとする
		MasterDataDefine.masEnum_expTable expTable;		// 経験値テーブル
		int captureDifficultyVal;						// 捕獲率
		int firstFriendnessVal;							// 初期仲良し度
		MasterDataDefine.masEnum_eggGroup mainEggGroup;	// タマゴグループ１
		MasterDataDefine.masEnum_eggGroup subEggGroup;	// タマゴグループ２

		MasterDataDefine.masEnum_tamamonType mainType;	// タイプ１
		MasterDataDefine.masEnum_tamamonType subType;	// タイプ２
		int mainAbilityId;								// 特性１
		int subAbilityId;								// 特性２
		int hiddenAbilityId;							// かくれ特性

		int hp;											// HP
		int attack;										// 攻撃
		int defense;									// 防御
		int specialAttack;								// 特攻
		int specialDefense;								// 特防
		int speed;										// 素早さ

		int levelSkillTableId;							// レベル技テーブルID
		int machineSkillTableId;						// マシン技テーブルID
		int eggSkillTableId;							// タマゴ技テーブルID
	}
	public masStruct_monstersInfo[] m_Items;
}