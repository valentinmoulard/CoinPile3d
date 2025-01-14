using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class StackLevel_Animations : MonoBehaviour
{
    [SerializeField] private Transform m_visual = null;

    [SerializeField] private MeshRenderer m_visualRenderer = null;
    
    [SerializeField] private float m_originalScale = 1f;
    [SerializeField] private float m_originalPosition = 0f;

    [Header("Preselected animation")] [SerializeField]
    private float m_preselectedPosition = 1.2f;

    [SerializeField] private float m_preselectedDuration = 0.3f;

    [SerializeField] private Color m_highLightColor = Color.white;
    
    [Header("Appear animation")] [SerializeField]
    private float m_visualAppearStartScale = 0f;

    [SerializeField] private float m_appearDuration = 0.3f;

    [SerializeField] private float m_delayFactor = 0.1f;

    private Tweener m_tweener;
    private Color m_originalColor;


    private void Awake()
    {
        m_originalColor = m_visualRenderer.material.color;
    }

    private void OnEnable()
    {
        StackMover.OnStackLevelPreSelected += OnStackLevelPreSelected;
        StackMover.OnStackPlacedOnLevel += OnStackPlacedOnLevel;
    }

    private void OnDisable()
    {
        StackMover.OnStackLevelPreSelected -= OnStackLevelPreSelected;
        StackMover.OnStackPlacedOnLevel -= OnStackPlacedOnLevel;
    }

    public void Appear()
    {
        float delay = (transform.position.x + transform.position.z) * m_delayFactor;

        m_visual.transform.localScale = Vector3.one * m_visualAppearStartScale;

        if (m_tweener != null && m_tweener.IsPlaying())
            m_tweener.Kill();

        m_tweener = m_visual.DOScale(Vector3.one * m_originalScale, m_appearDuration)
            .SetDelay(delay)
            .SetEase(Ease.OutBack)
            .OnComplete(() => m_visual.transform.localScale = Vector3.one * m_originalScale);
    }

    private void OnStackLevelPreSelected(StackLevel_Animations stackLevelAnimations)
    {
        if (this == stackLevelAnimations)
        {
            PreSelect();
        }
        else
        {
            if (m_visual.transform.localPosition != m_originalPosition * Vector3.up)
            {
                ResetVisuals();
            }
        }
    }
    
    private void OnStackPlacedOnLevel(Stack placedStack)
    {
        ResetVisuals();
    }

    private void PreSelect()
    {
        m_visual.transform.localPosition = Vector3.up * m_originalPosition;

        m_visual.DOLocalMove(Vector3.one * m_preselectedPosition, m_preselectedDuration);

        m_visualRenderer.material.DOColor(m_highLightColor, m_preselectedDuration);
    }
    
    
    private void ResetVisuals()
    {
        m_visual.DOLocalMove(Vector3.up * m_originalPosition, m_preselectedDuration);

        m_visualRenderer.material.DOColor(m_originalColor, m_preselectedDuration);
    }

}