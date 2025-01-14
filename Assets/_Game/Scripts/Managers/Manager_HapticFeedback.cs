using System;
using UnityEngine;

public class Manager_HapticFeedback : MonoBehaviour
{
    private void OnEnable()
    {
        Option.OnSendOptionState += OnSendOptionState;

        StackMover.OnStackPlacedOnLevel += OnStackPlacedOnLevel;

        Coin.OnCoinTransferTriggerHapticFeedback += OnCoinTransferTriggerHapticFeedback;
        Coin.OnCoinDestroyTriggerHapticFeedback += OnCoinDestroyTriggerHapticFeedback;
        
        Manager_GameState.OnSendCurrentGameState += OnBroadcastGameState;
    }

    private void OnDisable()
    {
        Option.OnSendOptionState -= OnSendOptionState;
        
        StackMover.OnStackPlacedOnLevel -= OnStackPlacedOnLevel;
        
        Coin.OnCoinTransferTriggerHapticFeedback -= OnCoinTransferTriggerHapticFeedback;
        Coin.OnCoinDestroyTriggerHapticFeedback -= OnCoinDestroyTriggerHapticFeedback;
        
        Manager_GameState.OnSendCurrentGameState -= OnBroadcastGameState;
    }

    private void OnSendOptionState(OptionType optionType, bool state)
    {
        if (optionType == OptionType.Vibration)
            Taptic.tapticOn = state;
    }
    
    
    private void OnBroadcastGameState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                break;
            case GameState.Gameplay:
                PlayHeavyHaptic();
                break;
            case GameState.Gameover:
                PlayHeavyHaptic();
                break;
            case GameState.Victory:
                PlayHeavyHaptic();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void OnCoinTransferTriggerHapticFeedback()
    {
        PlayHeavyHaptic();
    }

    private void OnCoinDestroyTriggerHapticFeedback()
    {
        PlayHeavyHaptic();
    }
    
    private void OnStackPlacedOnLevel(Stack placedStack)
    {
        PlayHeavyHaptic();
    }

    private void PlayLightHaptic()
    {
        Taptic.Light();
    }

    private void PlayHeavyHaptic()
    {
        Taptic.Heavy();
    }
}