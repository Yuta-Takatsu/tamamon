using System.Collections.Generic;

/// <summary>
/// タマモン情報クラス
/// </summary>
public class TamamonData
{

    /// <summary>
    /// タマモン情報
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
    /// 性別
    /// </summary>
    public enum SexType
    {
        Male,   // 男
        Female, // 女
        None,   // 性別無し
    }

    /// <summary>
    /// タマモン情報を渡す
    /// </summary>
    /// <param name="id"></param>
    public TamamonDataInfomation GetTamamonData(int id)
    {
        // idからマスター検索
        // 仮データ
        TamamonDataInfomation tamamonDataInfomation = new TamamonDataInfomation();
        if (id == 3)
        {
            tamamonDataInfomation.Id = id;
            tamamonDataInfomation.Index = id;
            tamamonDataInfomation.Name = "ヤドシハンシ";
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
            tamamonDataInfomation.Name = "イレイワト";
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