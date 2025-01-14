using System;
using System.Collections.Generic;
using UnityEngine;

public class StackInteractableController : GameflowBehavior
{
    public static Action OnAllStackInteractableEmpty;

    private List<StackInteractable> m_stackInteractableList = new List<StackInteractable>();

    private void Awake()
    {
        RetreiveStackInteractables();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StackMover.OnStackPlacedOnLevel += OnStackPlacedOnLevel;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StackMover.OnStackPlacedOnLevel -= OnStackPlacedOnLevel;
    }


    protected override void OnGameplay()
    {
        base.OnGameplay();
        CheckStackInteractableListEmptyState();
    }

    private void RetreiveStackInteractables()
    {
        StackInteractable stackInteractableBuffer;

        foreach (Transform child in transform)
        {
            stackInteractableBuffer = child.GetComponent<StackInteractable>();

            if (stackInteractableBuffer != null)
                m_stackInteractableList.Add(stackInteractableBuffer);
        }
    }

    private void OnStackPlacedOnLevel(Stack placedStack)
    {
        CheckStackInteractableListEmptyState();
    }
    
    private void CheckStackInteractableListEmptyState()
    {
        for (int i = 0; i < m_stackInteractableList.Count; i++)
        {
            if (m_stackInteractableList[i].IsStackEmpty == false)
                return;
        }

        OnAllStackInteractableEmpty?.Invoke();
    }
}