
using UnityEngine;

public class UI_CoinDestructionInfoFeedback : MonoBehaviour
{
    [SerializeField] private UI_CoinDestructionScoreFeedback m_scoreUIPrefab = null;

    [SerializeField] private Transform m_scoreUIParent = null;
    
    [SerializeField] private Vector2 m_moveTargetOffset = Vector2.zero;

    [SerializeField] private Color m_transparentColor = Color.white;
    [SerializeField] private Color m_fullColor = Color.white;


    private void OnEnable()
    {
        Manager_Score.OnSendGainedScoreInfos += OnSendGainedScoreInfos;
    }

    private void OnDisable()
    {
        Manager_Score.OnSendGainedScoreInfos -= OnSendGainedScoreInfos;
    }
    

    private void OnSendGainedScoreInfos(Vector3 worldPosition, float gainedScore, float comboCount)
    {
        UI_CoinDestructionScoreFeedback scoreUIPrefab = Instantiate(m_scoreUIPrefab, m_scoreUIParent);
        
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        scoreUIPrefab.Initialize(screenPosition, m_moveTargetOffset, m_fullColor, m_transparentColor, gainedScore);
    }

}