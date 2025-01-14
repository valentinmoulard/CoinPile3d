using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Slider_Score : MonoBehaviour
{
    [SerializeField] private Slider m_scoreSlider = null;

    [SerializeField] private TMP_Text m_scoreText = null;

    private float m_targetScore;
    
    private void OnEnable()
    {
        Manager_Score.OnSendCurrentScore += UpdateScoreText;
        Manager_Score.OnSendCurrentScore += UpdateScoreSlider;

        LevelSet.OnSendTargetScoreToReach += OnSendTargetScoreToReach;
    }

    private void OnDisable()
    {
        Manager_Score.OnSendCurrentScore -= UpdateScoreText;
        Manager_Score.OnSendCurrentScore -= UpdateScoreSlider;
        
        LevelSet.OnSendTargetScoreToReach -= OnSendTargetScoreToReach;
    }

    private void OnSendTargetScoreToReach(float targetScore)
    {
        m_targetScore = targetScore;
    }
    
    private void UpdateScoreText(float currentScore)
    {
        float scoreToDisplay = Mathf.Clamp(currentScore, 0f, m_targetScore);
        
        m_scoreText.text = scoreToDisplay.ToString("F0") + " / " + m_targetScore.ToString("F0");
    }

    private void UpdateScoreSlider(float currentScore)
    {
        m_scoreSlider.value = Mathf.Clamp01(currentScore / m_targetScore);
    }
    
}
