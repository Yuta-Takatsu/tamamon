using Cysharp.Threading.Tasks;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// アドベンチャーパートのイベント用インターフェース
    /// 各種イベントはIAdventureEventを持つ
    /// </summary>
    public interface IAdventureEvent
    {
        public UniTask<int> OnExecute(string str);
    }
}