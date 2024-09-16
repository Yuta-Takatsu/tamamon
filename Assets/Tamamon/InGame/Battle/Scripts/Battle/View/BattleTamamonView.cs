using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using Cysharp.Threading.Tasks;
using DG.Tweening;

/// <summary>
/// �^�}�����\���N���X
/// </summary>
public class BattleTamamonView : MonoBehaviour
{
    [SerializeField]
    private Image m_enemyTamamonImage = default;

    [SerializeField]
    private Image m_playerTamamonImage = default;

    [SerializeField]
    private SpriteAtlas m_tamamonSpriteAtlas = default;

    private readonly float EnemyStartLocalPositionX = -1350f;
    private readonly float EnemyEndLocalPositionX = 350f;

    private readonly float TamamonFirstLocalPositionY = -256f;
    private readonly float TamamonDownEndLocalPositionY = -768f;

    private bool m_isAnimation = false;
    public bool IsAnimation => m_isAnimation;

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="enemyIndex"></param>
    /// <param name="playerIndex"></param>
    public void OnInitialize(int enemyIndex, int playerIndex)
    {
        // �摜�\��
        SetEnemyTamamonImage(enemyIndex);
        SetPlayerTamamonImage(playerIndex);

        // �G���J�E���g�A�j���[�V����������
        OnEncountAnimationInitialize(false);
        OnEncountAnimationInitialize(true);
    }

    /// <summary>
    /// �G�l�~�[�C���[�W�X�V
    /// </summary>
    /// <param name="index"></param>
    public void UpdateEnemyImage(int index)
    {
        SetEnemyTamamonImage(index);
    }

    /// <summary>
    /// �v���C���[�C���[�W�X�V
    /// </summary>
    /// <param name="index"></param>
    public void UpdatePlayerImage(int index)
    {
        SetPlayerTamamonImage(index);
    }

    /// <summary>
    /// �G�l�~�[�G���J�E���g�A�j���[�V�����Đ�
    /// </summary>
    public async UniTask PlayEncountEnemyAnimation()
    {
        await OnEncountAnimation(false);
    }

    /// <summary>
    /// �v���C���[�G���J�E���g�A�j���[�V�����Đ�
    /// </summary>
    public async UniTask PlayEncountPlayerAnimation()
    {
        await OnEncountAnimation(true);
    }

    /// <summary>
    /// �G�l�~�[�^�}�����摜�擾
    /// </summary>
    /// <param name="sprite"></param>
    public void SetEnemyTamamonImage(int id)
    {
        m_enemyTamamonImage.sprite = m_tamamonSpriteAtlas.GetSprite($"tamamon_{id}");
    }

    /// <summary>
    /// �v���C���[�^�}�����摜�擾
    /// </summary>
    /// <param name="sprite"></param>
    public void SetPlayerTamamonImage(int id)
    {
        m_playerTamamonImage.sprite = m_tamamonSpriteAtlas.GetSprite($"tamamon_{id}");
    }

    /// <summary>
    /// ���W�X�V
    /// </summary>
    /// <param name="localPosition"></param>
    public void UpdateEnemyLocalPosition(Vector2 localPosition)
    {
        m_enemyTamamonImage.transform.localPosition = localPosition;
    }

    /// <summary>
    /// �X�P�[���X�V
    /// </summary>
    /// <param name="scale"></param>
    public void UpdatePlayerImageScale(Vector2 scale)
    {
        m_playerTamamonImage.transform.localScale = scale;
    }

    /// <summary>
    /// �G���J�E���g�A�j���[�V����������
    /// </summary>
    /// <param name="isPlayer"></param>
    public void OnEncountAnimationInitialize(bool isPlayer)
    {
        if (isPlayer)
        {
            UpdatePlayerImageScale(new Vector2(0f, 0f));
        }
        else
        {
            UpdateEnemyLocalPosition(new Vector2(EnemyStartLocalPositionX, m_enemyTamamonImage.transform.localPosition.y));
        }
    }

    /// <summary>
    /// �o�g���J�n���A�j���[�V����
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    public async UniTask OnEncountAnimation(bool isPlayer)
    {
        m_isAnimation = true;

        if (isPlayer)
        {
            m_playerTamamonImage.transform.DOScale(new Vector3(-1, 1, 1), 0.5f).OnComplete(() => { m_isAnimation = false; });
        }
        else
        {
            m_enemyTamamonImage.transform.DOLocalMove(new Vector3(EnemyEndLocalPositionX, m_enemyTamamonImage.transform.localPosition.y, 0), 2f).OnComplete(() => { m_isAnimation = false; });
        }

        await UniTask.WaitWhile(() => m_isAnimation);
    }

    /// <summary>
    /// �o�g���߂鎞�A�j���[�V����
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    public async UniTask OnBackAnimation(bool isPlayer)
    {
        m_isAnimation = true;

        if (isPlayer)
        {
            m_playerTamamonImage.transform.DOScale(new Vector3(0, 0, 1), 0.5f).OnComplete(() => { m_isAnimation = false; });
        }
        else
        {
            m_enemyTamamonImage.transform.DOScale(new Vector3(0, 0, 1), 0.5f).OnComplete(() => { m_isAnimation = false; });
        }

        await UniTask.WaitWhile(() => m_isAnimation);
    }

    /// <summary>
    /// �o�g���J��o�����A�j���[�V����
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    public async UniTask OnGoAnimation(bool isPlayer)
    {
        m_isAnimation = true;

        if (isPlayer)
        {
            m_playerTamamonImage.transform.localPosition = new Vector3(m_playerTamamonImage.transform.localPosition.x, TamamonFirstLocalPositionY, m_playerTamamonImage.transform.localPosition.z);
            m_playerTamamonImage.transform.DOScale(new Vector3(-1, 1, 1), 0.5f).OnComplete(() => { m_isAnimation = false; });
        }
        else
        {
            m_enemyTamamonImage.transform.localPosition = new Vector3(m_enemyTamamonImage.transform.localPosition.x, TamamonFirstLocalPositionY, m_enemyTamamonImage.transform.localPosition.z);
            m_enemyTamamonImage.transform.DOScale(new Vector3(1, 1, 1), 0.5f).OnComplete(() => { m_isAnimation = false; });
        }

        await UniTask.WaitWhile(() => m_isAnimation);
    }

    /// <summary>
    /// �^�}�����퓬�s�\�A�j���[�V����
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    public async UniTask OnDownAnimation(bool isPlayer)
    {
        m_isAnimation = true;

        if (isPlayer)
        {
            m_playerTamamonImage.transform.DOLocalMove(new Vector3(m_playerTamamonImage.transform.localPosition.x, TamamonDownEndLocalPositionY, 0), 0.3f).OnComplete(() => { m_isAnimation = false; });
        }
        else
        {
            m_enemyTamamonImage.transform.DOLocalMove(new Vector3(m_enemyTamamonImage.transform.localPosition.x, TamamonDownEndLocalPositionY, 0), 0.3f).OnComplete(() => { m_isAnimation = false; });
        }

        await UniTask.WaitWhile(() => m_isAnimation);
    }
}