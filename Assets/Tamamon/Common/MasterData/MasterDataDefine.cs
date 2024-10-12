using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.MasterData {
	public static class MasterDataDefine
	{
		public static string MASTER_MONSTERS_URL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vRemKLn1RaIW-u-JFw5vE9z2KIztKyyIRoBmdPGmznQJFzKzTZACVTBJ-bpIZ2nYHg5RRSv_QcDNH_t/pub?gid=0&single=true&output=csv";

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
}