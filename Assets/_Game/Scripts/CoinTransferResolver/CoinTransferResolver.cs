using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CoinTransferResolver : MonoBehaviour
{
    public static Action<bool> OnCoinTransferResolutionComplete;

    [SerializeField] private float m_delayBeforeCoinTransferStarts = 0.3f;

    [SerializeField] private float m_singleCoinTransferDuration = 0.3f;

    private List<StackLevel> m_stackLevelList;


    private void OnEnable()
    {
        LevelSet.OnSendStackLevelList += OnSendStackLevelList;
        StackMover.OnStackPlacedOnLevel += ResolveCoinTransfers;

        CoinDestroyResolver.OnCoinDestroyResolutionComplete += OnCoinDestroyResolutionComplete;
    }

    private void OnDisable()
    {
        LevelSet.OnSendStackLevelList -= OnSendStackLevelList;
        StackMover.OnStackPlacedOnLevel -= ResolveCoinTransfers;

        CoinDestroyResolver.OnCoinDestroyResolutionComplete -= OnCoinDestroyResolutionComplete;
    }


    private void OnSendStackLevelList(List<StackLevel> stackLevelList)
    {
        m_stackLevelList = new List<StackLevel>(stackLevelList);
    }

    private void OnCoinDestroyResolutionComplete(bool hasResolvedDestructions)
    {
        if (hasResolvedDestructions)
        {
            Debug.Log(">> 5 >> Transfer coroutine AGAIN");
            if (m_stackLevelList.HasSomethingToResolve())
                StartCoroutine(ResolveCoinTransfersCoroutine(m_delayBeforeCoinTransferStarts));
            else
            {
                Debug.Log("================================= Resolve end");
            }
        }
        else
        {
            Debug.Log("================================= Resolve end");
        }
    }

    private void ResolveCoinTransfers(Stack placedStack)
    {
        //ConsoleUtilities.ClearConsole();
        Debug.Log("================================= Resolve starts");

        if (m_stackLevelList.HasSomethingToResolve())
            StartCoroutine(ResolveCoinTransfersCoroutine(m_delayBeforeCoinTransferStarts, placedStack));
        else
            OnCoinTransferResolutionComplete?.Invoke(false);
    }

    private IEnumerator ResolveCoinTransfersCoroutine(float delay, Stack placedStack = null)
    {
        Debug.Log(">> 2 >> Transfer coroutine");

        yield return new WaitForSeconds(delay);

        CoinType priorityType = CoinType.Default;

        if (placedStack != null)
            priorityType = placedStack.GetCoinTypeOnTopOfStack;

        yield return Resolve(true, priorityType);
        yield return Resolve(false, priorityType);
        yield return ResolveSquareCondition(priorityType);

        OnCoinTransferResolutionComplete?.Invoke(true);
    }

    private IEnumerator ResolveSquareCondition(CoinType priorityType)
    {
        UpdateNeighborsWithSameCoinTypeOnTop();
        
        for (int i = 0; i < m_stackLevelList.Count; i++)
        {
            if (m_stackLevelList[i].StackLevelNeighbors.NeighborWithSameCoinTypeOnTopCount > 1)
            {
                Debug.Log("SQUARE CONDITION");
                StackLevel stackLevel = m_stackLevelList[i];

                if (stackLevel.StackLevelNeighbors.NeighborWithSameCoinTypeOnTopList.Count == 0)
                    continue;

                yield return stackLevel.Stack.TransferCoinsToStack(
                    stackLevel.Stack.GetCoinTypeOnTopOfStack,
                    stackLevel.StackLevelNeighbors.NeighborWithSameCoinTypeOnTopList[0].Stack,
                    m_singleCoinTransferDuration);

                UpdateNeighborsWithSameCoinTypeOnTop();

                yield return Resolve(true, priorityType);
                yield return Resolve(false, priorityType);
                yield break;
            }
        }
    }

    private IEnumerator Resolve(bool isPriorityMulticolor, CoinType priorityType = CoinType.Default)
    {
        bool hasToResolve = true;

        Debug.Log(isPriorityMulticolor ? "With prio " : "No prio");

        while (hasToResolve)
        {
            // MANAGE STACKS WITH 1 SIMILAR NEIGHBOR

            hasToResolve = false;

            UpdateNeighborsWithSameCoinTypeOnTop();

            List<StackLevel> stackWithOneNeighborColorList = new List<StackLevel>();

            for (int i = 0; i < m_stackLevelList.Count; i++)
            {
                if (m_stackLevelList[i].StackLevelNeighbors.NeighborWithSameCoinTypeOnTopCount == 1)
                    if (m_stackLevelList[i].Stack.HasMultipleColors() == isPriorityMulticolor)
                        stackWithOneNeighborColorList.Add(m_stackLevelList[i]);
            }

            stackWithOneNeighborColorList.SortByCoinTypeWithPriority(priorityType);

            Debug.Log("Transfer count : " + stackWithOneNeighborColorList.Count);

            for (int i = 0; i < stackWithOneNeighborColorList.Count; i++)
            {
                if (stackWithOneNeighborColorList[i].StackLevelNeighbors.NeighborWithSameCoinTypeOnTopList.Count == 0)
                    continue;

                yield return stackWithOneNeighborColorList[i].Stack.TransferCoinsToStack(
                    stackWithOneNeighborColorList[i].Stack.GetCoinTypeOnTopOfStack,
                    stackWithOneNeighborColorList[i].StackLevelNeighbors.NeighborWithSameCoinTypeOnTopList[0].Stack,
                    m_singleCoinTransferDuration);

                hasToResolve = true;

                UpdateNeighborsWithSameCoinTypeOnTop();
            }
        }
    }

    private void UpdateNeighborsWithSameCoinTypeOnTop()
    {
        for (int i = 0; i < m_stackLevelList.Count; i++)
        {
            m_stackLevelList[i].StackLevelNeighbors.FindNeighborWithSameCoinTypeOnTopOfStackCount();
        }
    }
}