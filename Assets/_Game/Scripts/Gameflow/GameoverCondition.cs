using System;
using System.Collections.Generic;
using UnityEngine;

public class GameoverCondition : MonoBehaviour
{
    public static Action OnGameover;

    private List<StackLevel> m_stackLevelList;


    private void OnEnable()
    {
        LevelSet.OnSendStackLevelList += OnSendStackLevelList;
        CoinDestroyResolver.OnCoinDestroyResolutionComplete += OnCoinDestroyResolutionComplete;
    }

    private void OnDisable()
    {
        LevelSet.OnSendStackLevelList -= OnSendStackLevelList;
        CoinDestroyResolver.OnCoinDestroyResolutionComplete -= OnCoinDestroyResolutionComplete;
    }


    private void OnSendStackLevelList(List<StackLevel> stackLevelList)
    {
        m_stackLevelList = new List<StackLevel>(stackLevelList);
    }


    private void OnCoinDestroyResolutionComplete(bool hasResolvedDestructions)
    {
        if (hasResolvedDestructions == false)
        {
            CheckGameoverCondition();
        }
    }

    private void CheckGameoverCondition()
    {
        if (m_stackLevelList.HasEmptyStackLevel() == false)
            OnGameover?.Invoke();
    }
}