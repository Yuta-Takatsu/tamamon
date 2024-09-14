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
        Physics,
        Special,
        Variation,
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
        techniqueDataInfomation.Id = id;
        techniqueDataInfomation.PP = 10;
        techniqueDataInfomation.Name = "�V���h�[�{�[��";
        techniqueDataInfomation.DescText = "�����e�̉�𓊂����čU������B ����̎��̗͂������邱�Ƃ�����B";
        techniqueDataInfomation.Type = TypeData.Type.Ghost;
        techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Special;

        return techniqueDataInfomation;
    }
}