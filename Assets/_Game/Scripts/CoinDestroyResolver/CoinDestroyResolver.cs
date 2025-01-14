using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDestroyResolver : MonoBehaviour
{
    public static Action<bool> OnCoinDestroyResolutionComplete;

    [SerializeField] private float m_delayBeforeCoinDestroyStarts = 0.3f;

    [SerializeField] private float m_destructionDuration = 1f;

    [SerializeField] private int m_minCoinsThresholdToDestroy = 5;

    private List<StackLevel> m_stackLevelList;


    private void OnEnable()
    {
        LevelSet.OnSendStackLevelList += OnSendStackLevelList;
        CoinTransferResolver.OnCoinTransferResolutionComplete += ResolveCoinDestroy;
    }

    private void OnDisable()
    {
        LevelSet.OnSendStackLevelList -= OnSendStackLevelList;
        CoinTransferResolver.OnCoinTransferResolutionComplete -= ResolveCoinDestroy;
    }

    private void OnSendStackLevelList(List<StackLevel> stackLevelList)
    {
        m_stackLevelList = new List<StackLevel>(stackLevelList);
    }


    private void ResolveCoinDestroy(bool hasResolvedTransfers)
    {
        if (hasResolvedTransfers)
            StartCoroutine(ResolveCoinDestroyCoroutine(m_delayBeforeCoinDestroyStarts));
        else
            OnCoinDestroyResolutionComplete?.Invoke(false);
    }

    private IEnumerator ResolveCoinDestroyCoroutine(float delay)
    {
        Debug.Log(">> 3 >> Destruction coroutine");
        
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < m_stackLevelList.Count; i++)
        {
            m_stackLevelList[i].FindCoinsToDestroyInStack(m_minCoinsThresholdToDestroy);
        }

        if (m_stackLevelList.HasCoinsToDestroy())
        {
            
            
            for (int i = 0; i < m_stackLevelList.Count; i++)
            {
                m_stackLevelList[i].DestroyCoinsOfSameType(m_destructionDuration);
            }

            yield return new WaitForSeconds(m_destructionDuration);

            OnCoinDestroyResolutionComplete?.Invoke(true);
        }
        else
        {
            OnCoinDestroyResolutionComplete?.Invoke(false);
        }
    }
}