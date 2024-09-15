/// <summary>
/// 技情報クラス
/// </summary>
public class TechniqueData
{
    /// <summary>
    /// 技情報構造体
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
    /// 技タイプ
    /// </summary>
    public enum TechniqueTypeInfomation
    {
        Physics,   // 物理
        Special,   // 特殊
        Variation, // 変化
    }

    /// <summary>
    /// 技情報を渡す
    /// </summary>
    /// <param name="id"></param>
    public TechniqueDataInfomation GetData(int id)
    {
        // idからマスター検索
        // 仮データ
        TechniqueDataInfomation techniqueDataInfomation = new TechniqueDataInfomation();
        if (id == 1)
        {
            techniqueDataInfomation.Id = id;
            techniqueDataInfomation.PP = 10;
            techniqueDataInfomation.Power = 80;
            techniqueDataInfomation.Accuracy = 100;
            techniqueDataInfomation.Name = "シャドーボール";
            techniqueDataInfomation.DescText = "黒い影の塊を投げつけて攻撃する。\n相手の守りの力をさげることもある。";
            techniqueDataInfomation.Type = TypeData.Type.Ghost;
            techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Special;
        }
        else if (id == 2)
        {
            techniqueDataInfomation.Id = id;
            techniqueDataInfomation.PP = 20;
            techniqueDataInfomation.Power = 80;
            techniqueDataInfomation.Accuracy = 100;
            techniqueDataInfomation.Name = "パワージェム";
            techniqueDataInfomation.DescText = "宝石のように きらめく 光を 発射して 相手を 攻撃する。";
            techniqueDataInfomation.Type = TypeData.Type.Rock;
            techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Special;
        }
        else if (id == 3)
        {
            techniqueDataInfomation.Id = id;
            techniqueDataInfomation.PP = 10;
            techniqueDataInfomation.Power = 90;
            techniqueDataInfomation.Accuracy = 100;
            techniqueDataInfomation.Name = "だいちのちから";
            techniqueDataInfomation.DescText = "相手の 足下へ 大地の力を 放出する。\n相手の 特防を 下げることが ある。";
            techniqueDataInfomation.Type = TypeData.Type.Ground;
            techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Special;
        }
        else if (id == 4)
        {
            techniqueDataInfomation.Id = id;
            techniqueDataInfomation.PP = 5;
            techniqueDataInfomation.Power = 250;
            techniqueDataInfomation.Accuracy = 100;
            techniqueDataInfomation.Name = "大爆発";
            techniqueDataInfomation.DescText = "大きな 爆発で 自分の 周りに いるものを 攻撃する。\n使ったあとに ひんしに なる。";
            techniqueDataInfomation.Type = TypeData.Type.Normal;
            techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Physics;
        }
        else if(id == 5)
        {
            techniqueDataInfomation.Id = id;
            techniqueDataInfomation.PP = 10;
            techniqueDataInfomation.Power = 70;
            techniqueDataInfomation.Accuracy = 100;
            techniqueDataInfomation.Name = "かつだんそう";
            techniqueDataInfomation.DescText = "地面を 割るような 斬撃で 相手を 攻撃する。\n相手の 防御を 下げることが ある。";
            techniqueDataInfomation.Type = TypeData.Type.Ground;
            techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Physics;
        }
        else if (id == 6)
        {
            techniqueDataInfomation.Id = id;
            techniqueDataInfomation.PP = 20;
            techniqueDataInfomation.Power = 70;
            techniqueDataInfomation.Accuracy = 100;
            techniqueDataInfomation.Name = "きりさく";
            techniqueDataInfomation.DescText = "ツメや カマなどで 相手を 切り裂いて 攻撃する。\n急所に 当たりやすい。";
            techniqueDataInfomation.Type = TypeData.Type.Normal;
            techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Physics;
        }

        return techniqueDataInfomation;
    }
}