using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MasterDataDefine
{
	public static string MASTER_MONSTERS_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRemKLn1RaIW-u-JFw5vE9z2KIztKyyIRoBmdPGmznQJFzKzTZACVTBJ-bpIZ2nYHg5RRSv_QcDNH_t/pub?gid=0&single=true&output=csv";

	/// <summary>
	/// MasterData_Monsters�ɕR�Â��^�}�������
	/// </summary>
	public class masClass_monsters : ScriptableObject
	{
		[System.Serializable]
		public struct masStruct_monstersInfo {
			int id;							// �^�}����ID
			int tribeId;					// �푰ID
			int evoGroupVal;				// �i���l				MEMO:�ŏI�i����<9�Ƃ��A�Ȍ�P��O�̐i���ɂȂ邲�Ƃ�10�{����
			string name;					// ���O					MEMO:�e�L�X�g�}�X�^����񂩁H

			float height;					// �d��
			float weight;					// ����
			int tamaDexId;					// �^�}�����}��ID
			int voiceId;					// ��ID
			int maleRatio;					// �I�X�ɂȂ�m��
			int femaleRatio;				// ���X�ɂȂ�m��		MEMO:���ʂȂ��݂͐����Amale��female���O�Ȃ琫�ʂȂ��Ƃ���
			masEnum_expTable expTable;		// �o���l�e�[�u��
			int captureDifficultyVal;		// �ߊl��
			int firstFriendnessVal;			// �������ǂ��x
			masEnum_eggGroup mainEggGroup;	// �^�}�S�O���[�v�P
			masEnum_eggGroup subEggGroup;	// �^�}�S�O���[�v�Q

			masEnum_tamamonType mainType;	// �^�C�v�P
			masEnum_tamamonType subType;	// �^�C�v�Q
			int mainAbilityId;				// �����P
			int subAbilityId;				// �����Q
			int hiddenAbilityId;			// ���������

			int hp;							// HP
			int attack;						// �U��
			int defense;					// �h��
			int specialAttack;				// ���U
			int specialDefense;				// ���h
			int speed;						// �f����

			int levelSkillTableId;			// ���x���Z�e�[�u��ID
			int machineSkillTableId;		// �}�V���Z�e�[�u��ID
			int eggSkillTableId;			// �^�}�S�Z�e�[�u��ID
		}
		public masStruct_monstersInfo[] m_Items;
	}


	/// <summary>
    /// MasterData�Ŏg�p����o���l�e�[�u��
    /// </summary>
	public enum masEnum_expTable
	{
		FASTEST=0,		// ���ɂ���60�e�[�u���@�ő�
		QUICK,			// 80�e�[�u���@����
		NORMAL,			// 100�e�[�u���@�ӂ�
		PRECOCIOUS,		// 105�e�[�u���@���n
		LATEBLOOM,		// 125�e�[�u���@���Ӑ�
		LATEST,			// 164�e�[�u���@�����
	}

	/// <summary>
    /// MasterData�Ŏg�p����^�}�S�O���[�v
    /// </summary>
	public enum masEnum_eggGroup
	{
		NONE=0,			// �Ȃ�
		KAIJU,			// �������イ
		MINERAL,		// �z��
		PLANT,			// �A��
		WATER1,			// �����P
		WATER2,			// �����Q
		WATER3,			// �����R
		DRAGON,			// �h���S��
		FLYING,			// ��s
		HUMANOID,		// �l�^
		UNFORMED,		// �s��`
		INCECT,			// ��
		FAILY,			// �d��
		GROUND,			// ����
		JOKER,			// �Ȃ�ł�
		NOTHING,		// �^�}�S������
	}

	/// <summary>
    /// MasterData�Ŏg�p����^�C�v
    /// </summary>
	public enum masEnum_tamamonType
	{
		NONE=0,			// �Ȃ�
		NORMAL,			// �m�[�}��
		FIRE,			// �ق̂�
		WATER,			// �݂�
		GLASS,			// ����
		ELECTLIC,		// �ł�
		INCECT,			// �ނ�
		FLYING,			// �Ђ���
		FIGHTER,		// �����Ƃ�
		ROCK,			// ����
		METAL,			// �͂���
		GROUND,			// ���߂�
		POISON,			// �ǂ�
		HEEL,			// ����
		PSYCHIC,		// �G�X�p�[
		GOAST,			// �S�[�X�g
		DRAGON,			// �h���S��
		FAILY,			// �t�F�A���[
		ICE,			// ������
		LEGEND,			// ���W�F���h�H
		STELLA,			// �X�e���H
	}

}
