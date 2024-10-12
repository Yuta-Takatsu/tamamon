using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MasterData_Monstersに紐づくタマモン情報
/// </summary>
public class MasterData_TamamonMaster : ScriptableObject
{
	[System.Serializable]
	public class MonstersInfo {
		public int id;											// タマモンID
		public int tribeId;									// 種族ID
		public int evoGroupVal;								// 進化値				MEMO:最終進化を<9とし、以後１つ手前の進化になるごとに10倍する
		public string name;									// 名前					MEMO:テキストマスタあるんか？

		public float height;									// 重さ
		public float weight;									// 高さ
		public int tamaDexId;									// タマモン図鑑ID
		public int voiceId;									// 声ID
		public int maleRatio;									// オスになる確率
		public int femaleRatio;								// メスになる確率		MEMO:性別なしは設けず、maleもfemaleも０なら性別なしとする
		public MasterDataDefine.masEnum_expTable expTable;		// 経験値テーブル
		public int captureDifficultyVal;						// 捕獲率
		public int firstFriendnessVal;							// 初期仲良し度
		public MasterDataDefine.masEnum_eggGroup mainEggGroup;	// タマゴグループ１
		public MasterDataDefine.masEnum_eggGroup subEggGroup;	// タマゴグループ２

		public MasterDataDefine.masEnum_tamamonType mainType;	// タイプ１
		public MasterDataDefine.masEnum_tamamonType subType;	// タイプ２
		public int mainAbilityId;								// 特性１
		public int subAbilityId;								// 特性２
		public int hiddenAbilityId;							// かくれ特性

		public int hp;											// HP
		public int attack;										// 攻撃
		public int defense;									// 防御
		public int specialAttack;								// 特攻
		public int specialDefense;								// 特防
		public int speed;										// 素早さ

		public int levelSkillTableId;							// レベル技テーブルID
		public int machineSkillTableId;						// マシン技テーブルID
		public int eggSkillTableId;							// タマゴ技テーブルID
	}
	public MonstersInfo[] m_Items;
}