using System.Collections.Generic;

/// <summary>
/// �^�}�����X�e�[�^�X���N���X
/// </summary>
public class TamamonStatusData
{

    private TamamonStatusDataInfomation m_tamamonStatusDataInfo = default;

    public TamamonStatusDataInfomation TamamonStatusDataInfo => m_tamamonStatusDataInfo;

    private TamamonStatusValueInformation m_tamamonStatusValueDataInfo = default;

    public TamamonStatusValueInformation TamamonStatusValueDataInfo => m_tamamonStatusValueDataInfo;


    private readonly int EffortMaxValue = 252;
    private readonly int PPUpMaxCount = 3;
    private readonly int MaxLevel = 100;

    /// <summary>
    /// �X�e�[�^�X���
    /// </summary>
    public struct TamamonStatusDataInfomation
    {
        // �}�X�^�[ID
        public int Id;

        // �}�X�^�[���
        public TamamonData.TamamonDataInfomation tamamonDataInfomation;
        // �}��No.
        public int Index;

        // �푰��
        public string Name;

        // �j�b�N�l�[��
        public string NickName;

        // ���݂̃��x��
        public int Level;

        // ����ID
        public int AbilityId;

        // ����
        public TamamonData.SexType Sex;

        // ���̃��x���܂ł̌o���l
        public int Exp;

        // ���݂̌o���l
        public int NowExp;

        // ���݂�HP
        public int NowHP;

        // HP�w�͒l
        public int EffortHPValue;

        // �U���w�͒l
        public int EffortAttackValue;

        // �h��w�͒l
        public int EffortDefenseValue;

        // ���U�w�͒l
        public int EffortSpecialAttackValue;

        // ���h�w�͒l
        public int EffortSpecialDefenseValue;

        // �f�����w�͒l
        public int EffortSpeedValue;

        // �擾�Z���X�g
        public List<TamamonTechniqueDataInformation> TechniqueList;
    }

    /// <summary>
    /// �w�͒l���f�ł̍ŏI�X�e�[�^�X
    /// </summary>
    public struct TamamonStatusValueInformation
    {
        public int HP;

        public int Attack;

        public int Defense;

        public int SpecialAttack;

        public int SpecialDefense;

        public int Speed;
    }

    /// <summary>
    /// �Z���
    /// </summary>
    public struct TamamonTechniqueDataInformation
    {
        // �ZID
        public int TechniqueId;

        // �Z�}�X�^�[
        public TechniqueData.TechniqueDataInfomation TechniqueData;

        // PP�㏸��
        public int TechniquePPUpCount;

        // �ő�PP
        public int TechniquePP;

        // �c��PP
        public int TechniqueNowPP;
    }

    /// <summary>
    /// ������
    /// </summary>
    public void OnInitialize(int id, TamamonData.SexType sex, int level, int abilityId, int techniqueId_1 = 0, int techniqueId_2 = 0, int techniqueId_3 = 0, int techniqueId_4 = 0)
    {
        SetTamamonData(id);
        SetSexType(sex);

        UpdateNickName(m_tamamonStatusDataInfo.Name);
        UpdateAbilityId(abilityId);

        m_tamamonStatusDataInfo.Level = level;
        m_tamamonStatusDataInfo.Exp = 100;
        m_tamamonStatusDataInfo.NowExp = 0;
        m_tamamonStatusDataInfo.NowHP = m_tamamonStatusDataInfo.tamamonDataInfomation.HP;
        m_tamamonStatusDataInfo.EffortHPValue = 0;
        m_tamamonStatusDataInfo.EffortAttackValue = 0;
        m_tamamonStatusDataInfo.EffortDefenseValue = 0;
        m_tamamonStatusDataInfo.EffortSpecialAttackValue = 0;
        m_tamamonStatusDataInfo.EffortSpecialDefenseValue = 0;
        m_tamamonStatusDataInfo.EffortSpeedValue = 0;

        UpdateTamamonStatusValueData();

        UpdateTechnique(techniqueId_1, 0);
        UpdateTechnique(techniqueId_2, 1);
        UpdateTechnique(techniqueId_3, 2);
        UpdateTechnique(techniqueId_4, 3);
    }

    /// <summary>
    /// �X�e�[�^�X���擾
    /// </summary>
    /// <param name="data"></param>
    public void SetTamamonData(int id)
    {
        TamamonData.TamamonDataInfomation tamamonDataInfomation = new TamamonData().GetTamamonData(id);

        m_tamamonStatusDataInfo.Id = tamamonDataInfomation.Id;
        m_tamamonStatusDataInfo.Index = tamamonDataInfomation.Index;
        m_tamamonStatusDataInfo.Name = tamamonDataInfomation.Name;
        m_tamamonStatusDataInfo.tamamonDataInfomation = tamamonDataInfomation;
    }

    /// <summary>
    /// ���ʎ擾
    /// </summary>
    /// <param name="sex"></param>
    public void SetSexType(TamamonData.SexType sex)
    {
        m_tamamonStatusDataInfo.Sex = sex;
    }

    /// <summary>
    /// �����l���X�V
    /// </summary>
    public void UpdateTamamonStatusValueData()
    {
        m_tamamonStatusValueDataInfo.HP = m_tamamonStatusDataInfo.tamamonDataInfomation.HP;
        m_tamamonStatusValueDataInfo.Attack = m_tamamonStatusDataInfo.tamamonDataInfomation.Attack;
        m_tamamonStatusValueDataInfo.Defense = m_tamamonStatusDataInfo.tamamonDataInfomation.Defense;
        m_tamamonStatusValueDataInfo.SpecialAttack = m_tamamonStatusDataInfo.tamamonDataInfomation.SpecialAttack;
        m_tamamonStatusValueDataInfo.SpecialDefense = m_tamamonStatusDataInfo.tamamonDataInfomation.SpecialDefense;
        m_tamamonStatusValueDataInfo.Speed = m_tamamonStatusDataInfo.tamamonDataInfomation.Speed;
    }

    /// <summary>
    /// �j�b�N�l�[���X�V
    /// </summary>
    /// <param name="niclName"></param>
    public void UpdateNickName(string niclName)
    {
        m_tamamonStatusDataInfo.NickName = niclName;
    }

    /// <summary>
    /// ���x���X�V
    /// </summary>
    /// <param name="level"></param>
    public void UpdateLevel(int level)
    {
        m_tamamonStatusDataInfo.Level += level;

        if (m_tamamonStatusDataInfo.Level > MaxLevel)
        {
            m_tamamonStatusDataInfo.Level = MaxLevel;
        }

        UpdateTamamonStatusValueData();
    }

    /// <summary>
    /// �o���l�X�V
    /// </summary>
    /// <param name="exp"></param>
    public void UpdateNowExp(int exp)
    {
        m_tamamonStatusDataInfo.NowExp += exp;
    }

    /// <summary>
    /// �����X�V
    /// </summary>
    /// <param name="id"></param>
    public void UpdateAbilityId(int id)
    {
        m_tamamonStatusDataInfo.AbilityId = id;
    }

    /// <summary>
    /// ���݂�HP�X�V
    /// </summary>
    /// <param name="damage"></param>
    public void UpdateNowHP(int damage)
    {
        m_tamamonStatusDataInfo.NowHP -= damage;

        if (m_tamamonStatusDataInfo.NowHP < 0)
        {
            m_tamamonStatusDataInfo.NowHP = 0;
        }

        if (m_tamamonStatusDataInfo.NowHP > m_tamamonStatusValueDataInfo.HP)
        {
            m_tamamonStatusDataInfo.NowHP = m_tamamonStatusValueDataInfo.HP;
        }
    }

    /// <summary>
    /// HP�w�͒l�X�V
    /// </summary>
    /// <param name="value"></param>
    public void UpdateEffortHPValue(int value)
    {
        m_tamamonStatusDataInfo.EffortHPValue += value;

        if (m_tamamonStatusDataInfo.EffortHPValue > EffortMaxValue)
        {
            m_tamamonStatusDataInfo.EffortHPValue = EffortMaxValue;
        }

        UpdateTamamonStatusValueData();
    }

    /// <summary>
    /// �U���w�͒l�X�V
    /// </summary>
    /// <param name="value"></param>
    public void UpdateEffortAttackValue(int value)
    {
        m_tamamonStatusDataInfo.EffortAttackValue += value;

        if (m_tamamonStatusDataInfo.EffortAttackValue > EffortMaxValue)
        {
            m_tamamonStatusDataInfo.EffortAttackValue = EffortMaxValue;
        }

        UpdateTamamonStatusValueData();
    }

    /// <summary>
    /// �h��w�͒l�X�V
    /// </summary>
    /// <param name="value"></param>
    public void UpdateEffortDefenseValue(int value)
    {
        m_tamamonStatusDataInfo.EffortDefenseValue += value;

        if (m_tamamonStatusDataInfo.EffortDefenseValue > EffortMaxValue)
        {
            m_tamamonStatusDataInfo.EffortDefenseValue = EffortMaxValue;
        }

        UpdateTamamonStatusValueData();
    }

    /// <summary>
    /// ���U�w�͒l�X�V
    /// </summary>
    /// <param name="value"></param>
    public void UpdateEffortSpecialAttackValue(int value)
    {
        m_tamamonStatusDataInfo.EffortSpecialAttackValue += value;

        if (m_tamamonStatusDataInfo.EffortSpecialAttackValue > EffortMaxValue)
        {
            m_tamamonStatusDataInfo.EffortSpecialAttackValue = EffortMaxValue;
        }

        UpdateTamamonStatusValueData();
    }

    /// <summary>
    /// ���h�w�͒l�X�V
    /// </summary>
    /// <param name="value"></param>
    public void UpdateEffortSpecialDefenseValue(int value)
    {
        m_tamamonStatusDataInfo.EffortSpecialDefenseValue += value;

        if (m_tamamonStatusDataInfo.EffortSpecialDefenseValue > EffortMaxValue)
        {
            m_tamamonStatusDataInfo.EffortSpecialDefenseValue = EffortMaxValue;
        }

        UpdateTamamonStatusValueData();
    }

    /// <summary>
    /// �f�����w�͒l�X�V
    /// </summary>
    /// <param name="value"></param>
    public void UpdateEffortSpeedValue(int value)
    {
        m_tamamonStatusDataInfo.EffortSpeedValue += value;

        if (m_tamamonStatusDataInfo.EffortSpeedValue > EffortMaxValue)
        {
            m_tamamonStatusDataInfo.EffortSpeedValue = EffortMaxValue;
        }

        UpdateTamamonStatusValueData();
    }

    /// <summary>
    /// �Z�X�V
    /// </summary>
    /// <param name="id"></param>
    public void UpdateTechnique(int id, int index)
    {
        if (id == 0) return;

        if (m_tamamonStatusDataInfo.TechniqueList == null)
        {
            m_tamamonStatusDataInfo.TechniqueList = new List<TamamonTechniqueDataInformation>();
        }

        TechniqueData techniqueData = new TechniqueData();
        TamamonTechniqueDataInformation tamamonTechniqueDataInformation = new TamamonTechniqueDataInformation();
        tamamonTechniqueDataInformation.TechniqueData = techniqueData.GetData(id);
        tamamonTechniqueDataInformation.TechniqueId = tamamonTechniqueDataInformation.TechniqueData.Id;
        tamamonTechniqueDataInformation.TechniquePPUpCount = 0;
        tamamonTechniqueDataInformation.TechniquePP = tamamonTechniqueDataInformation.TechniqueData.PP;
        tamamonTechniqueDataInformation.TechniqueNowPP = tamamonTechniqueDataInformation.TechniqueData.PP;

        if (m_tamamonStatusDataInfo.TechniqueList.Count <= index)
        {
            m_tamamonStatusDataInfo.TechniqueList.Add(tamamonTechniqueDataInformation);
        }
        else
        {
            m_tamamonStatusDataInfo.TechniqueList[index] = tamamonTechniqueDataInformation;
        }
    }

    /// <summary>
    /// �Z�c��PP�X�V
    /// </summary>
    /// <param name="value"></param>
    public void UpdateTechniqueNowPP(int value, int index)
    {
        TamamonTechniqueDataInformation tamamonTechniqueDataInformation = m_tamamonStatusDataInfo.TechniqueList[index];
        tamamonTechniqueDataInformation.TechniqueNowPP -= value;

        if (tamamonTechniqueDataInformation.TechniqueNowPP < 0)
        {
            tamamonTechniqueDataInformation.TechniqueNowPP = 0;
        }

        if (tamamonTechniqueDataInformation.TechniqueNowPP > tamamonTechniqueDataInformation.TechniquePP)
        {
            tamamonTechniqueDataInformation.TechniqueNowPP = tamamonTechniqueDataInformation.TechniquePP;
        }
        m_tamamonStatusDataInfo.TechniqueList[index] = tamamonTechniqueDataInformation;
    }

    /// <summary>
    /// �ZPP���Z�񐔍X�V
    /// </summary>
    /// <param name="count"></param>
    public void UpdateTechniquePP(int count, int index)
    {
        TamamonTechniqueDataInformation tamamonTechniqueDataInformation = m_tamamonStatusDataInfo.TechniqueList[index];

        tamamonTechniqueDataInformation.TechniquePPUpCount += count;

        if (tamamonTechniqueDataInformation.TechniquePPUpCount > PPUpMaxCount)
        {
            tamamonTechniqueDataInformation.TechniquePPUpCount = PPUpMaxCount;
        }
        m_tamamonStatusDataInfo.TechniqueList[index] = tamamonTechniqueDataInformation;
    }
}