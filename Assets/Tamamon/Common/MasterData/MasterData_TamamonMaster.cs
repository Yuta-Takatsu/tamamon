using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MasterData_Monsters�ɕR�Â��^�}�������
/// </summary>
public class MasterData_TamamonMaster : ScriptableObject
{
	[System.Serializable]
	public class MonstersInfo {
		public int id;											// �^�}����ID
		public int tribeId;									// �푰ID
		public int evoGroupVal;								// �i���l				MEMO:�ŏI�i����<9�Ƃ��A�Ȍ�P��O�̐i���ɂȂ邲�Ƃ�10�{����
		public string name;									// ���O					MEMO:�e�L�X�g�}�X�^����񂩁H

		public float height;									// �d��
		public float weight;									// ����
		public int tamaDexId;									// �^�}�����}��ID
		public int voiceId;									// ��ID
		public int maleRatio;									// �I�X�ɂȂ�m��
		public int femaleRatio;								// ���X�ɂȂ�m��		MEMO:���ʂȂ��݂͐����Amale��female���O�Ȃ琫�ʂȂ��Ƃ���
		public MasterDataDefine.masEnum_expTable expTable;		// �o���l�e�[�u��
		public int captureDifficultyVal;						// �ߊl��
		public int firstFriendnessVal;							// �������ǂ��x
		public MasterDataDefine.masEnum_eggGroup mainEggGroup;	// �^�}�S�O���[�v�P
		public MasterDataDefine.masEnum_eggGroup subEggGroup;	// �^�}�S�O���[�v�Q

		public MasterDataDefine.masEnum_tamamonType mainType;	// �^�C�v�P
		public MasterDataDefine.masEnum_tamamonType subType;	// �^�C�v�Q
		public int mainAbilityId;								// �����P
		public int subAbilityId;								// �����Q
		public int hiddenAbilityId;							// ���������

		public int hp;											// HP
		public int attack;										// �U��
		public int defense;									// �h��
		public int specialAttack;								// ���U
		public int specialDefense;								// ���h
		public int speed;										// �f����

		public int levelSkillTableId;							// ���x���Z�e�[�u��ID
		public int machineSkillTableId;						// �}�V���Z�e�[�u��ID
		public int eggSkillTableId;							// �^�}�S�Z�e�[�u��ID
	}
	public MonstersInfo[] m_Items;
}