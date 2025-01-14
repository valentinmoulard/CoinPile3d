using DG.Tweening;
using UnityEngine;

public class Coin_Animations : MonoBehaviour
{
    [SerializeField] private Transform m_visual = null;

    [SerializeField] private float m_visualAppearStartScale = 0f;

    [SerializeField] private float m_visualAppearTargetScale = 1f;

    [SerializeField] private float m_appearDuration = 0.3f;

    private Tweener m_tweener;


    public void Appear(float delay)
    {
        m_visual.transform.localScale = Vector3.one * m_visualAppearStartScale;

        m_tweener = m_visual.DOScale(Vector3.one * m_visualAppearTargetScale, m_appearDuration)
            .SetDelay(delay)
            .SetEase(Ease.OutBack)
            .OnComplete(() => m_visual.transform.localScale = Vector3.one * m_visualAppearTargetScale);
    }
}
