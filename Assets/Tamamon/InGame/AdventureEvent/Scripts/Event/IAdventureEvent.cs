using Cysharp.Threading.Tasks;

namespace Tamamon.InGame.AdventureEvent
{
    /// <summary>
    /// �A�h�x���`���[�p�[�g�̃C�x���g�p�C���^�[�t�F�[�X
    /// �e��C�x���g��IAdventureEvent������
    /// </summary>
    public interface IAdventureEvent
    {
        public UniTask<int> OnExecute(string str);
    }
}