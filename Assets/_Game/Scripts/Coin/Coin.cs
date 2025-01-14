using System;
using DG.Tweening;
using UnityEngine;

public enum CoinType
{
    Default,
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H,
    I,
    J
}

public class Coin : MonoBehaviour
{
    public static Action OnCoinTransferTriggerHapticFeedback;
    public static Action OnCoinDestroyTriggerHapticFeedback;

    [SerializeField] private Transform m_modelParent = null;

    [SerializeField] private Coin_Animations m_coinAnimations = null;

    [SerializeField] private float m_coinSize = 0.15f;

    [SerializeField] private float m_jumpStrength = 1.5f;

    private GameObject m_currentModel;
    private Tweener m_tweener;
    private CoinType m_type;

    public CoinType Type
    {
        get => m_type;
    }

    public float CoinSize
    {
        get => m_coinSize;
    }

    public void InitializeCoin(CoinSkin_SO skinData, CoinType newType, float delay = 0f)
    {
        SetCoinType(newType);
        UpdateCoinSkin(skinData);
        m_coinAnimations.Appear(delay);
    }

    private void UpdateCoinSkin(CoinSkin_SO skinData)
    {
        if (m_currentModel != null)
            Destroy(m_currentModel);
        
        m_currentModel = Instantiate(skinData.GetSkin(m_type), m_modelParent);
    }

    public void SetCoinType(CoinType newType)
    {
        m_type = newType;
    }

    public void MoveCoin(Vector3 targetPosition, CoinTransferAnimationType animationType, float animationDuration,
        bool triggerHapticFeedback = false)
    {
        switch (animationType)
        {
            case CoinTransferAnimationType.Straight:
                if (animationDuration == 0)
                    transform.localPosition = targetPosition;
                else
                    m_tweener = transform.DOLocalMove(targetPosition, animationDuration);
                break;
            case CoinTransferAnimationType.Flip:
                if (animationDuration == 0)
                    transform.localPosition = targetPosition;
                else
                {
                    if (m_tweener != null && m_tweener.IsPlaying())
                        m_tweener.Kill();

                    if (triggerHapticFeedback)
                        OnCoinTransferTriggerHapticFeedback?.Invoke();

                    transform.DOLocalJump(targetPosition, m_jumpStrength, 1, animationDuration);

                    Vector3 direction = targetPosition - transform.localPosition;
                    direction.y = 0f;
                    direction.x = Mathf.Round(direction.x);
                    direction.z = Mathf.Round(direction.z);

                    (direction.x, direction.z) = (direction.z, -direction.x);

                    Vector3 targetRotation = transform.localRotation.eulerAngles + 360 * direction;


                    transform.DOLocalRotate(targetRotation, animationDuration, RotateMode.FastBeyond360).OnComplete(
                        () =>
                            transform.localRotation = Quaternion.Euler(targetRotation));
                }

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(animationType), animationType, null);
        }
    }

    public void DestroyCoin(float delay, float duration)
    {
        OnCoinDestroyTriggerHapticFeedback?.Invoke();
        transform.DOScale(Vector3.zero, duration).SetDelay(delay).OnComplete(() => Destroy(gameObject));
    }
}