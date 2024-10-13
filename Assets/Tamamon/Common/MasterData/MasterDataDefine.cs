using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.MasterData {
	public static class MasterDataDefine
	{
		public static string MASTER_MONSTERS_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRemKLn1RaIW-u-JFw5vE9z2KIztKyyIRoBmdPGmznQJFzKzTZACVTBJ-bpIZ2nYHg5RRSv_QcDNH_t/pub?gid=0&single=true&output=csv";

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
}