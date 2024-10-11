namespace Tamamon.AdoventureEvent
{
    /// <summary>
    /// イベントのインターフェースクラス
    /// </summary>
    public interface IAdoventureEvent
    {
        public void OnInitialize();

        public void OnExecute();

        public void OnFinalize();
    }
}