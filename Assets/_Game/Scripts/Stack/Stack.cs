using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CoinTransferAnimationType
{
    Straight,
    Flip
}

public class Stack : MonoBehaviour
{
    public static Action<int> OnDestroyCoins;
    public static Action<Vector3, int> OnSendDestructionInfos;

    private List<Coin> m_coinList = new List<Coin>();
    private List<Coin> m_coinToDestroyList = new List<Coin>();

    public bool HasCoinsToDestroy;

    public bool IsEmpty
    {
        get => m_coinList.Count == 0;
    }

    public CoinType GetCoinTypeOnTopOfStack
    {
        get => m_coinList.Count > 0 ? m_coinList[^1].Type : CoinType.Default;
    }

    public bool HasMultipleColors()
    {
        if (m_coinList.Count <= 1)
            return false;

        CoinType coinTypeReference = m_coinList[0].Type;

        for (int i = 0; i < m_coinList.Count; i++)
        {
            if (coinTypeReference != m_coinList[i].Type)
                return true;
        }

        return false;
    }


    public void FindCoinsToDestroyInStack(int minCoinsThresholdToDestroy)
    {
        HasCoinsToDestroy = false;

        m_coinToDestroyList.Clear();

        if (m_coinList.Count < minCoinsThresholdToDestroy)
            return;

        CoinType coinTypeTarget = m_coinList[0].Type;

        for (int i = 0; i < m_coinList.Count; i++)
        {
            if (m_coinList[i].Type != coinTypeTarget)
            {
                coinTypeTarget = m_coinList[i].Type;
                m_coinToDestroyList.Clear();
            }

            m_coinToDestroyList.Add(m_coinList[i]);
        }

        HasCoinsToDestroy = m_coinToDestroyList.Count >= minCoinsThresholdToDestroy;

        if (!HasCoinsToDestroy)
            m_coinToDestroyList.Clear();
    }

    public void DestroyCoinsOfSameType(float destructionDuration = 0f)
    {
        if (m_coinToDestroyList.Count == 0)
            return;

        Coin coinBuffer;

        float coinDestructionDuration = destructionDuration / 2.5f;
        float delayStep = (destructionDuration - coinDestructionDuration) / (m_coinToDestroyList.Count - 1);


        OnDestroyCoins?.Invoke(m_coinToDestroyList.Count);


        for (int i = m_coinToDestroyList.Count - 1; i >= 0; i--)
        {
            coinBuffer = m_coinToDestroyList[i];
            m_coinList.Remove(coinBuffer);
            coinBuffer.DestroyCoin(delayStep * (m_coinToDestroyList.Count - 1 - i), coinDestructionDuration);
        }

        OnSendDestructionInfos?.Invoke(transform.position, m_coinToDestroyList.Count);
        
        m_coinToDestroyList.Clear();
    }

    public void DestroyAllCoinsFromStack(float destructionDuration = 0f)
    {
        if (m_coinList.Count == 0)
            return;

        Coin coinBuffer;

        float coinDestructionDuration = destructionDuration / 2.5f;
        float delayStep = (destructionDuration - coinDestructionDuration) / (m_coinList.Count - 1);

        for (int i = m_coinList.Count - 1; i >= 0; i--)
        {
            coinBuffer = m_coinList[i];
            m_coinList.Remove(coinBuffer);
            coinBuffer.DestroyCoin(delayStep * (m_coinList.Count - 1 - i), coinDestructionDuration);
        }
    }
    
    public void AddCoinToStack(Coin newCoin,
        CoinTransferAnimationType animationType = CoinTransferAnimationType.Straight,
        float movementDuration = 0f, bool triggerHapticFeedback = false)
    {
        newCoin.transform.parent = this.transform;

        m_coinList.Add(newCoin);

        Vector3 coinLocalPosition = Vector3.zero;
        coinLocalPosition.y = newCoin.CoinSize * m_coinList.Count;

        newCoin.MoveCoin(coinLocalPosition, animationType, movementDuration, triggerHapticFeedback);
    }

    public void TransferStackToTargetStack(Stack targetStack, float movementDuration = 0f)
    {
        Coin coin;

        while (m_coinList.Count > 0)
        {
            coin = m_coinList[0];
            m_coinList.Remove(coin);
            targetStack.AddCoinToStack(coin, CoinTransferAnimationType.Straight, movementDuration);
        }
    }

    public IEnumerator TransferCoinsToStack(CoinType coinType, Stack targetStack, float movementDuration = 0f)
    {
        Coin coin;

        while (GetCoinTypeOnTopOfStack == coinType)
        {
            coin = m_coinList[^1];
            m_coinList.Remove(coin);
            targetStack.AddCoinToStack(coin, CoinTransferAnimationType.Flip, movementDuration, true);
            yield return new WaitForSeconds(movementDuration);
        }
    }
}