
using UnityEngine;

public class UI_ComboFeedback : MonoBehaviour
{
    [SerializeField] private UI_Combo m_comboUIPrefab = null;

    [SerializeField] private Transform m_comboUIParent = null;

    private UI_Combo m_currentComboUIInstance;

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
        if (comboCount <= 1)
            return;

        if(m_currentComboUIInstance != null)
            Destroy(m_currentComboUIInstance.gameObject);
        
        m_currentComboUIInstance = Instantiate(m_comboUIPrefab, m_comboUIParent);

        m_currentComboUIInstance.Initialize(comboCount);
    }
}