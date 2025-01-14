using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_CoinDestructionScoreFeedback : MonoBehaviour
{
    [SerializeField] private TMP_Text m_scoreText = null;
    
    private RectTransform m_rectTransform;
    private Coroutine m_tweenIntValueTextToCoroutine;
    
    
    public void Initialize(Vector2 screenPosition, Vector2 moveTargetOffset, Color fullColor, Color transparentColor, float gainedScore)
    {
        Destroy(gameObject, 3f);
        
        m_rectTransform = GetComponent<RectTransform>();
        
        m_rectTransform.anchoredPosition = screenPosition;

        m_scoreText.color = transparentColor;
        
        Vector2 targetPosition = screenPosition + moveTargetOffset;

        m_rectTransform.DOAnchorPos(targetPosition, 1.5f);

        m_scoreText.color = transparentColor;
        m_scoreText.DOColor(fullColor, 1f)
            .OnComplete(() => m_scoreText.DOColor(transparentColor, 0.5f));

        if (m_tweenIntValueTextToCoroutine != null)
            StopCoroutine(m_tweenIntValueTextToCoroutine);

        m_tweenIntValueTextToCoroutine = StartCoroutine(TweenIntValueTextToCoroutine(1f, gainedScore));
    }
    
    
    private IEnumerator TweenIntValueTextToCoroutine(float duration, float targetValue)
    {
        float timeStep = duration / targetValue;

        for (int i = 0; i < (int)targetValue; i++)
        {
            m_scoreText.text = "+" + i;
            yield return new WaitForSeconds(timeStep);
        }

        m_scoreText.text = "+" + targetValue;
    }
    
}
