namespace Tamamon.AdoventureEvent
{
    /// <summary>
    /// �C�x���g�̃C���^�[�t�F�[�X�N���X
    /// </summary>
    public interface IAdoventureEvent
    {
        public void OnInitialize();

        public void OnExecute();

        public void OnFinalize();
    }
}