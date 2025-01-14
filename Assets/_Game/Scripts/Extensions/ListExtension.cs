using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public static class ListExtension
{
    public static void Shuffle<T>(this List<T> inputList)
    {
        for (int i = 0; i < inputList.Count - 1; i++)
        {
            T temp = inputList[i];
            int rand = Random.Range(i, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }
    }

    public static void SortByCoinTypeWithPriority(this List<StackLevel> inputList,
        CoinType priorityType = CoinType.Default)
    {
        if (inputList.Count == 0)
            return;

        inputList = inputList.OrderBy(c => (int)c.Stack.GetCoinTypeOnTopOfStack).ToList();

        StackLevel levelStackBuffer;

        for (int i = 0; i < inputList.Count; i++)
        {
            if (inputList[i].Stack.GetCoinTypeOnTopOfStack == priorityType)
            {
                levelStackBuffer = inputList[i];
                inputList.Remove(levelStackBuffer);
                inputList.Insert(0, levelStackBuffer);
            }
        }
    }

    public static bool HasSomethingToResolve(this List<StackLevel> inputList)
    {
        int neighborsCount = 0;

        for (int i = 0; i < inputList.Count; i++)
        {
            if (!inputList[i].Stack.IsEmpty && inputList[i].StackLevelNeighbors.HasNeighborsWithStack() == true)
            {
                neighborsCount++;
                if (neighborsCount >= 2)
                    return true;
            }
        }

        return false;
    }

    public static bool HasCoinsToDestroy(this List<StackLevel> inputList)
    {
        for (int i = 0; i < inputList.Count; i++)
        {
            if (inputList[i].Stack.HasCoinsToDestroy)
                return true;
        }

        return false;
    }

    public static bool HasEmptyStackLevel(this List<StackLevel> inputList)
    {
        for (int i = 0; i < inputList.Count; i++)
        {
            if (inputList[i].Stack.IsEmpty)
                return true;
        }

        return false;
    }


    public static void GenerateUniqueRandomCoinTypes(this List<CoinType> inputList, int maxNumber, int desiredRandomNumberCount)
    {
        inputList.Clear();

        while (inputList.Count < desiredRandomNumberCount)
        {
            // range is 1 to maxNumber because 0 is the default value
            CoinType coinTypeToAdd = (CoinType)Random.Range(1, maxNumber + 1);
            
            if(!inputList.Contains(coinTypeToAdd))
                inputList.Add(coinTypeToAdd);
        }
        
    }
}