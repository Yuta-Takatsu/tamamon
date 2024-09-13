using Cysharp.Threading.Tasks;

public interface ITurnEndState
{
    public UniTask<bool> OnInitialize();

    public UniTask<bool> OnExecute();

    public UniTask<bool> OnFinalize();
}
