using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TitleView : MonoBehaviour
{
    [SerializeField]
    private Image m_titleLogoImage = default;

    [SerializeField]
    private Button m_tapButton = default;

    [SerializeField]
    private CanvasGroup m_tapTextObj = default;

    [SerializeField]
    private Image m_tamamonImage = default;


    public async UniTask OnInitialize()
    {
        m_titleLogoImage.transform.localPosition = new Vector2(1800f, m_titleLogoImage.transform.localPosition.y);

        await UniTask.Delay(TimeSpan.FromSeconds(7f));

        m_titleLogoImage.transform.DOLocalMove(new Vector3(0, m_titleLogoImage.transform.localPosition.y, 0), 0.5f).SetEase(Ease.OutBack).SetLink(gameObject);

        m_tapTextObj.alpha = 1f;
        m_tapTextObj.DOFade(0.0f, 1f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject); ;

        m_tapButton.onClick.AddListener(() => SceneManager.LoadScene("Battle"));

        await UniTask.Delay(TimeSpan.FromSeconds(5f));

        await PlayLeftOutAnimation();
    }

    public async UniTask PlayLeftOutAnimation()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        m_tamamonImage.transform.localScale = new Vector3(1, 1, 1);
        m_tamamonImage.transform.DOLocalJump(new Vector3(-2000f, m_tamamonImage.transform.localPosition.y, 0), 50, 20, 7f).OnComplete(async () => await PlayRightOutAnimation()).SetLink(gameObject); ;
    }

    public async UniTask PlayRightOutAnimation()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        m_tamamonImage.transform.localScale = new Vector3(-1, 1, 1);
        m_tamamonImage.transform.DOLocalJump(new Vector3(2000f, m_tamamonImage.transform.localPosition.y, 0), 50, 20, 5f).OnComplete(async () => await PlayLeftOutAnimation()).SetLink(gameObject); ;
    }
}