using System;
using UnityEngine;

public class Manager_Score : GameflowBehavior
{
    public static Action<float> OnSendCurrentScore;
    public static Action<Vector3, float, float> OnSendGainedScoreInfos;

    private float m_currentScore;
    private float m_currentCombo;
    private bool m_hasDestroyedCoinsThisTurn;

    protected override void OnEnable()
    {
        base.OnEnable();
        Stack.OnSendDestructionInfos += OnSendDestructionInfos;
        StackMover.OnStackPlacedOnLevel += OnStackPlacedOnLevel;
        CoinDestroyResolver.OnCoinDestroyResolutionComplete += OnCoinDestroyResolutionComplete;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Stack.OnSendDestructionInfos -= OnSendDestructionInfos;
        StackMover.OnStackPlacedOnLevel -= OnStackPlacedOnLevel;
        CoinDestroyResolver.OnCoinDestroyResolutionComplete -= OnCoinDestroyResolutionComplete;
    }

    protected override void OnGameplay()
    {
        base.OnGameplay();

        m_currentScore = 0;
        m_currentCombo = 1;

        OnSendCurrentScore?.Invoke(m_currentScore);
    }


    private void OnStackPlacedOnLevel(Stack placedStack)
    {
        m_hasDestroyedCoinsThisTurn = false;
    }

    private void OnSendDestructionInfos(Vector3 destructionWorldPosition, int coinsDestroyed)
    {
        float gainedScore = coinsDestroyed * m_currentCombo;
        OnSendGainedScoreInfos?.Invoke(destructionWorldPosition, gainedScore, m_currentCombo);
        
        m_currentScore += gainedScore;
        
        m_currentCombo++;

        OnSendCurrentScore?.Invoke(m_currentScore);
    }

    private void OnCoinDestroyResolutionComplete(bool hasDetroyedCoins)
    {
        if (hasDetroyedCoins)
        {
            m_hasDestroyedCoinsThisTurn = true;
        }
        else
        {
            if (m_hasDestroyedCoinsThisTurn == false)
            {
                m_currentCombo = 1;
            }
        }
    }
}