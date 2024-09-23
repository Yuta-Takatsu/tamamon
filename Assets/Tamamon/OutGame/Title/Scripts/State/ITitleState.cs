namespace Tamamon.OutGame.Title
{
    public interface ITitleState
    {
        public void OnInitialize(ITitleView titleView);
        public void OnExecute();

        public void OnFinalize();
    }
}