/// <summary>
/// �Z���N���X
/// </summary>
public class TechniqueData
{
    /// <summary>
    /// �Z���\����
    /// </summary>
    public struct TechniqueDataInfomation
    {
        public int Id;
        public int PP;
        public int Power;
        public int Accuracy;
        public string Name;
        public string DescText;
        public TypeData.Type Type;
        public TechniqueTypeInfomation TechniqueType;
    }

    /// <summary>
    /// �Z�^�C�v
    /// </summary>
    public enum TechniqueTypeInfomation
    {
        Physics,   // ����
        Special,   // ����
        Variation, // �ω�
    }

    /// <summary>
    /// �Z����n��
    /// </summary>
    /// <param name="id"></param>
    public TechniqueDataInfomation GetData(int id)
    {
        // id����}�X�^�[����
        // ���f�[�^
        TechniqueDataInfomation techniqueDataInfomation = new TechniqueDataInfomation();
        if (id == 1)
        {
            techniqueDataInfomation.Id = id;
            techniqueDataInfomation.PP = 10;
            techniqueDataInfomation.Power = 80;
            techniqueDataInfomation.Accuracy = 100;
            techniqueDataInfomation.Name = "�V���h�[�{�[��";
            techniqueDataInfomation.DescText = "�����e�̉�𓊂����čU������B\n����̎��̗͂������邱�Ƃ�����B";
            techniqueDataInfomation.Type = TypeData.Type.Ghost;
            techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Special;
        }
        else if (id == 2)
        {
            techniqueDataInfomation.Id = id;
            techniqueDataInfomation.PP = 20;
            techniqueDataInfomation.Power = 80;
            techniqueDataInfomation.Accuracy = 100;
            techniqueDataInfomation.Name = "�p���[�W�F��";
            techniqueDataInfomation.DescText = "��΂̂悤�� ����߂� ���� ���˂��� ����� �U������B";
            techniqueDataInfomation.Type = TypeData.Type.Rock;
            techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Special;
        }
        else if (id == 3)
        {
            techniqueDataInfomation.Id = id;
            techniqueDataInfomation.PP = 10;
            techniqueDataInfomation.Power = 90;
            techniqueDataInfomation.Accuracy = 100;
            techniqueDataInfomation.Name = "�������̂�����";
            techniqueDataInfomation.DescText = "����� ������ ��n�̗͂� ���o����B\n����� ���h�� �����邱�Ƃ� ����B";
            techniqueDataInfomation.Type = TypeData.Type.Ground;
            techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Special;
        }
        else if (id == 4)
        {
            techniqueDataInfomation.Id = id;
            techniqueDataInfomation.PP = 5;
            techniqueDataInfomation.Power = 250;
            techniqueDataInfomation.Accuracy = 100;
            techniqueDataInfomation.Name = "�唚��";
            techniqueDataInfomation.DescText = "�傫�� ������ ������ ����� ������̂� �U������B\n�g�������Ƃ� �Ђ񂵂� �Ȃ�B";
            techniqueDataInfomation.Type = TypeData.Type.Normal;
            techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Physics;
        }
        else if(id == 5)
        {
            techniqueDataInfomation.Id = id;
            techniqueDataInfomation.PP = 10;
            techniqueDataInfomation.Power = 70;
            techniqueDataInfomation.Accuracy = 100;
            techniqueDataInfomation.Name = "�����񂻂�";
            techniqueDataInfomation.DescText = "�n�ʂ� ����悤�� �a���� ����� �U������B\n����� �h��� �����邱�Ƃ� ����B";
            techniqueDataInfomation.Type = TypeData.Type.Ground;
            techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Physics;
        }
        else if (id == 6)
        {
            techniqueDataInfomation.Id = id;
            techniqueDataInfomation.PP = 20;
            techniqueDataInfomation.Power = 70;
            techniqueDataInfomation.Accuracy = 100;
            techniqueDataInfomation.Name = "���肳��";
            techniqueDataInfomation.DescText = "�c���� �J�}�Ȃǂ� ����� �؂�􂢂� �U������B\n�}���� ������₷���B";
            techniqueDataInfomation.Type = TypeData.Type.Normal;
            techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Physics;
        }

        return techniqueDataInfomation;
    }
}