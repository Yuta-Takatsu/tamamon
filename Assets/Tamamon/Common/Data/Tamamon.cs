using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class Tamamon : MonoBehaviour
{

    [SerializeField]
    private Image m_tamamonImage = default;

    private readonly float EnemyStartLocalPositionX = -1350f;
    private readonly float EnemyEndLocalPositionX = 350f;

    private bool m_isAnimation = false;
    public bool IsAnimation => m_isAnimation;

    private TamamonDataInfo m_tamamonData = default;
    public TamamonDataInfo TamamonData => m_tamamonData;
    public struct TamamonDataInfo
    {
        public int Id;

        public int Index;

        public string Name;

        public int Level;

        public SexType Sex;

        public int MaxExp;

        public int NowExp;

        public int MaxHP;

        public int NowHP;

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
    /// タマモン情報取得
    /// </summary>
    /// <param name="tamamonData"></param>
    public void SetTamamonData(TamamonDataInfo tamamonData)
    {
        m_tamamonData = tamamonData;
    }

    /// <summary>
    /// タマモン画像取得
    /// </summary>
    /// <param name="sprite"></param>
    public void SetTamamonImage(Sprite sprite)
    {
        m_tamamonImage.sprite = sprite;
    }

    /// <summary>
    /// 座標更新
    /// </summary>
    /// <param name="localPosition"></param>
    public void UpdateImageLocalPosition(Vector2 localPosition)
    {
        m_tamamonImage.transform.localPosition = localPosition;
    }

    public void UpdateImageScale(Vector2 scale)
    {
        m_tamamonImage.transform.localScale = scale;
    }

    public void OnEncountAnimationInitialize(bool isPlayer)
    {
        if (isPlayer)
        {
            UpdateImageScale(new Vector2(0f, 0f));
        }
        else
        {
            UpdateImageLocalPosition(new Vector2(EnemyStartLocalPositionX, m_tamamonImage.transform.localPosition.y));
        }
    }

    /// <summary>
    /// バトル開始時アニメーション
    /// </summary>
    /// <param name="isPlayer"></param>
    /// <returns></returns>
    public void OnEncountAnimation(bool isPlayer)
    {
        m_isAnimation = true;

        if (isPlayer)
        {
            m_tamamonImage.transform.DOScale(new Vector3(1, 1, 1), 0.5f).OnComplete(() => { m_isAnimation = false; });
        }
        else
        {
            m_tamamonImage.transform.DOLocalMove(new Vector3(EnemyEndLocalPositionX, m_tamamonImage.transform.localPosition.y, 0), 2f).OnComplete(() => { m_isAnimation = false; });
        }
    }
}