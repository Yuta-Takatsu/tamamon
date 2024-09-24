using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MasterData_Monsters�ɕR�Â��^�}�������
/// </summary>
public class MasterData_TamamonMaster : ScriptableObject
{
	[System.Serializable]
	public struct masStruct_monstersInfo {
		int id;											// �^�}����ID
		int tribeId;									// �푰ID
		int evoGroupVal;								// �i���l				MEMO:�ŏI�i����<9�Ƃ��A�Ȍ�P��O�̐i���ɂȂ邲�Ƃ�10�{����
		string name;									// ���O					MEMO:�e�L�X�g�}�X�^����񂩁H

		float height;									// �d��
		float weight;									// ����
		int tamaDexId;									// �^�}�����}��ID
		int voiceId;									// ��ID
		int maleRatio;									// �I�X�ɂȂ�m��
		int femaleRatio;								// ���X�ɂȂ�m��		MEMO:���ʂȂ��݂͐����Amale��female���O�Ȃ琫�ʂȂ��Ƃ���
		MasterDataDefine.masEnum_expTable expTable;		// �o���l�e�[�u��
		int captureDifficultyVal;						// �ߊl��
		int firstFriendnessVal;							// �������ǂ��x
		MasterDataDefine.masEnum_eggGroup mainEggGroup;	// �^�}�S�O���[�v�P
		MasterDataDefine.masEnum_eggGroup subEggGroup;	// �^�}�S�O���[�v�Q

		MasterDataDefine.masEnum_tamamonType mainType;	// �^�C�v�P
		MasterDataDefine.masEnum_tamamonType subType;	// �^�C�v�Q
		int mainAbilityId;								// �����P
		int subAbilityId;								// �����Q
		int hiddenAbilityId;							// ���������

		int hp;											// HP
		int attack;										// �U��
		int defense;									// �h��
		int specialAttack;								// ���U
		int specialDefense;								// ���h
		int speed;										// �f����

		int levelSkillTableId;							// ���x���Z�e�[�u��ID
		int machineSkillTableId;						// �}�V���Z�e�[�u��ID
		int eggSkillTableId;							// �^�}�S�Z�e�[�u��ID
	}
	public masStruct_monstersInfo[] m_Items;
}