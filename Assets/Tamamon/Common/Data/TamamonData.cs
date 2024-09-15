using System.Collections.Generic;

/// <summary>
/// �^�}�������N���X
/// </summary>
public class TamamonData
{

    /// <summary>
    /// �^�}�������
    /// </summary>
    public struct TamamonDataInfomation
    {
        public int Id;

        public int Index;

        public string Name;

        public List<TypeData.Type> TypeList;

        public List<int> AbilityIdList;

        public int ExpTableId;

        public int SexTypeId;

        public int HP;

        public int Attack;

        public int Defense;

        public int SpecialAttack;

        public int SpecialDefense;

        public int Speed;
    }

    /// <summary>
    /// ����
    /// </summary>
    public enum SexType
    {
        Male,   // �j
        Female, // ��
        None,   // ���ʖ���
    }

    /// <summary>
    /// �^�}��������n��
    /// </summary>
    /// <param name="id"></param>
    public TamamonDataInfomation GetTamamonData(int id)
    {
        // id����}�X�^�[����
        // ���f�[�^
        TamamonDataInfomation tamamonDataInfomation = new TamamonDataInfomation();
        if (id == 3)
        {
            tamamonDataInfomation.Id = id;
            tamamonDataInfomation.Index = id;
            tamamonDataInfomation.Name = "���h�V�n���V";
            tamamonDataInfomation.ExpTableId = 1;
            tamamonDataInfomation.SexTypeId = 0;
            tamamonDataInfomation.HP = 999;// 80;
            tamamonDataInfomation.Attack = 130;
            tamamonDataInfomation.Defense = 115;
            tamamonDataInfomation.SpecialAttack = 65;
            tamamonDataInfomation.SpecialDefense = 85;
            tamamonDataInfomation.Speed = 999;// 55;

            tamamonDataInfomation.TypeList = new List<TypeData.Type>();
            tamamonDataInfomation.TypeList.Add(TypeData.Type.Water);
            tamamonDataInfomation.TypeList.Add(TypeData.Type.Ground);

            tamamonDataInfomation.AbilityIdList = new List<int>();
            tamamonDataInfomation.AbilityIdList.Add(1);
            tamamonDataInfomation.AbilityIdList.Add(2);
        }
        else
        {
            tamamonDataInfomation.Id = id;
            tamamonDataInfomation.Index = id;
            tamamonDataInfomation.Name = "�C���C���g";
            tamamonDataInfomation.ExpTableId = 1;
            tamamonDataInfomation.SexTypeId = 0;
            tamamonDataInfomation.HP = 999;// 66;
            tamamonDataInfomation.Attack = 66;
            tamamonDataInfomation.Defense = 160;
            tamamonDataInfomation.SpecialAttack = 106;
            tamamonDataInfomation.SpecialDefense = 70;
            tamamonDataInfomation.Speed = 999;// 44;

            tamamonDataInfomation.TypeList = new List<TypeData.Type>();
            tamamonDataInfomation.TypeList.Add(TypeData.Type.Rock);
            tamamonDataInfomation.TypeList.Add(TypeData.Type.Ghost);

            tamamonDataInfomation.AbilityIdList = new List<int>();
            tamamonDataInfomation.AbilityIdList.Add(1);
            tamamonDataInfomation.AbilityIdList.Add(2);
        }
        return tamamonDataInfomation;
    }
}