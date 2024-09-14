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
        tamamonDataInfomation.Id = id;
        tamamonDataInfomation.Index = 1;
        tamamonDataInfomation.Name = "�C���C���g";
        tamamonDataInfomation.ExpTableId = 1;
        tamamonDataInfomation.SexTypeId = 0;
        tamamonDataInfomation.HP = 66;
        tamamonDataInfomation.Attack = 66;
        tamamonDataInfomation.Defense = 160;
        tamamonDataInfomation.SpecialAttack = 106;
        tamamonDataInfomation.SpecialDefense = 70;
        tamamonDataInfomation.Speed = 44;

        tamamonDataInfomation.AbilityIdList = new List<int>();
        tamamonDataInfomation.AbilityIdList.Add(1);
        tamamonDataInfomation.AbilityIdList.Add(2);

        return tamamonDataInfomation;
    }
}