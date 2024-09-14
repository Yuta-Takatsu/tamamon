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
        Physics,
        Special,
        Variation,
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
        techniqueDataInfomation.Id = id;
        techniqueDataInfomation.PP = 10;
        techniqueDataInfomation.Name = "シャドーボール";
        techniqueDataInfomation.DescText = "黒い影の塊を投げつけて攻撃する。 相手の守りの力をさげることもある。";
        techniqueDataInfomation.Type = TypeData.Type.Ghost;
        techniqueDataInfomation.TechniqueType = TechniqueTypeInfomation.Special;

        return techniqueDataInfomation;
    }
}