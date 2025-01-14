using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelSet : MonoBehaviour
{
    public static Action<List<StackLevel>> OnSendStackLevelList;
    public static Action<float> OnSendTargetScoreToReach;

    private List<StackLevel> m_stackLevelList = null;

    [SerializeField] private float m_targetScore = 1;

    public void Initialize()
    {
        SetupLevelSet();
        
        if (m_stackLevelList != null && m_stackLevelList.Count > 0)
        {
            for (int i = 0; i < m_stackLevelList.Count; i++)
            {
                m_stackLevelList[i].Initialize();
            }
            
            OnSendStackLevelList?.Invoke(m_stackLevelList);
        }
        else
            Debug.LogError("List of stack level is null or empty!");
        
        OnSendTargetScoreToReach?.Invoke(m_targetScore);
    }
    
    #region EDITOR

    // Called by editor button
    public void SetupLevelSet()
    {
        m_stackLevelList = new List<StackLevel>();

        FindStackBaseInChildren();
    }

    private void FindStackBaseInChildren()
    {
        foreach (Transform child in transform)
        {
            StackLevel stackLevel = child.GetComponent<StackLevel>();

            if (stackLevel != null)
            {
                m_stackLevelList.Add(stackLevel);
            }
        }
    }
    #endregion
}