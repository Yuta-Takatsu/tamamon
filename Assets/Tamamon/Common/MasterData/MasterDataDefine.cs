using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MasterDataDefine
{
	public static string MASTER_MONSTERS_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRemKLn1RaIW-u-JFw5vE9z2KIztKyyIRoBmdPGmznQJFzKzTZACVTBJ-bpIZ2nYHg5RRSv_QcDNH_t/pub?gid=0&single=true&output=csv";

	/// <summary>
	/// MasterData_Monstersに紐づくタマモン情報
	/// </summary>
	public class masClass_monsters : ScriptableObject
	{
		[System.Serializable]
		public struct masStruct_monstersInfo {
			int id;							// タマモンID
			int tribeId;					// 種族ID
			int evoGroupVal;				// 進化値				MEMO:最終進化を<9とし、以後１つ手前の進化になるごとに10倍する
			string name;					// 名前					MEMO:テキストマスタあるんか？

			float height;					// 重さ
			float weight;					// 高さ
			int tamaDexId;					// タマモン図鑑ID
			int voiceId;					// 声ID
			int maleRatio;					// オスになる確率
			int femaleRatio;				// メスになる確率		MEMO:性別なしは設けず、maleもfemaleも０なら性別なしとする
			masEnum_expTable expTable;		// 経験値テーブル
			int captureDifficultyVal;		// 捕獲率
			int firstFriendnessVal;			// 初期仲良し度
			masEnum_eggGroup mainEggGroup;	// タマゴグループ１
			masEnum_eggGroup subEggGroup;	// タマゴグループ２

			masEnum_tamamonType mainType;	// タイプ１
			masEnum_tamamonType subType;	// タイプ２
			int mainAbilityId;				// 特性１
			int subAbilityId;				// 特性２
			int hiddenAbilityId;			// かくれ特性

			int hp;							// HP
			int attack;						// 攻撃
			int defense;					// 防御
			int specialAttack;				// 特攻
			int specialDefense;				// 特防
			int speed;						// 素早さ

			int levelSkillTableId;			// レベル技テーブルID
			int machineSkillTableId;		// マシン技テーブルID
			int eggSkillTableId;			// タマゴ技テーブルID
		}
		public masStruct_monstersInfo[] m_Items;
	}


	/// <summary>
    /// MasterDataで使用する経験値テーブル
    /// </summary>
	public enum masEnum_expTable
	{
		FASTEST=0,		// 俗にいう60テーブル　最速
		QUICK,			// 80テーブル　早め
		NORMAL,			// 100テーブル　ふつう
		PRECOCIOUS,		// 105テーブル　早熟
		LATEBLOOM,		// 125テーブル　大器晩成
		LATEST,			// 164テーブル　つくんな
	}

	/// <summary>
    /// MasterDataで使用するタマゴグループ
    /// </summary>
	public enum masEnum_eggGroup
	{
		NONE=0,			// なし
		KAIJU,			// かいじゅう
		MINERAL,		// 鉱物
		PLANT,			// 植物
		WATER1,			// 水中１
		WATER2,			// 水中２
		WATER3,			// 水中３
		DRAGON,			// ドラゴン
		FLYING,			// 飛行
		HUMANOID,		// 人型
		UNFORMED,		// 不定形
		INCECT,			// 虫
		FAILY,			// 妖精
		GROUND,			// 陸上
		JOKER,			// なんでも
		NOTHING,		// タマゴ未発見
	}

	/// <summary>
    /// MasterDataで使用するタイプ
    /// </summary>
	public enum masEnum_tamamonType
	{
		NONE=0,			// なし
		NORMAL,			// ノーマル
		FIRE,			// ほのお
		WATER,			// みず
		GLASS,			// くさ
		ELECTLIC,		// でんき
		INCECT,			// むし
		FLYING,			// ひこう
		FIGHTER,		// かくとう
		ROCK,			// いわ
		METAL,			// はがね
		GROUND,			// じめん
		POISON,			// どく
		HEEL,			// あく
		PSYCHIC,		// エスパー
		GOAST,			// ゴースト
		DRAGON,			// ドラゴン
		FAILY,			// フェアリー
		ICE,			// こおり
		LEGEND,			// レジェンド？
		STELLA,			// ステラ？
	}

}
