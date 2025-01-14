using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CoinStackGenerator : MonoBehaviour
{
    [SerializeField] private Coin m_coinPrefab = null;

    [SerializeField] private List<CoinSkin_SO> m_coinSkinSOList = null;
    [SerializeField] private CoinSkin_SO m_coinSkinSOMax = null;

    [SerializeField] private int m_minStackCount = 2;
    [SerializeField] private int m_maxStackCount = 5;

    [SerializeField] private int m_minDifferentCoinTypeCountForStackGeneration = 1;
    [SerializeField] private int m_maxDifferentCoinTypeCountForStackGeneration = 3;
    [SerializeField] private int m_maxDifferentCoinTypeCountInLevel = 5;

    [SerializeField] private float m_delayBetweenCoinSpawn = 0.2f;

    private Coin m_coinBuffer;
    private CoinSkin_SO m_currentCoinSkinSO;
    private List<CoinType> m_coinTypeList = new List<CoinType>();
    private int m_currentMaxDifferentCoinTypeCount;

    private void OnEnable()
    {
        StackInteractable.OnInteractableStackBaseEmpty += OnInteractableStackBaseEmpty;
        Manager_Level.OnSendCurrentLevel += OnSendCurrentLevel;
    }

    private void OnDisable()
    {
        StackInteractable.OnInteractableStackBaseEmpty -= OnInteractableStackBaseEmpty;
        Manager_Level.OnSendCurrentLevel -= OnSendCurrentLevel;
    }

    private void OnSendCurrentLevel(int levelIndex)
    {
        DetermineCoinSkinData(levelIndex);
    }

    private void DetermineCoinSkinData(int levelIndex)
    {
        m_coinTypeList.Clear();
        
        if (levelIndex >= m_coinSkinSOList.Count)
        {
            m_currentMaxDifferentCoinTypeCount = m_maxDifferentCoinTypeCountInLevel;
            m_currentCoinSkinSO = m_coinSkinSOMax;
            m_coinTypeList = CreateCoinTypeList(levelIndex - m_coinSkinSOList.Count + 1, m_currentMaxDifferentCoinTypeCount,
                m_currentCoinSkinSO.m_coinSkinDictionary.Count);
        }
        else
        {
            m_currentMaxDifferentCoinTypeCount = m_coinSkinSOList[levelIndex].m_coinSkinDictionary.Count;
            m_currentCoinSkinSO = m_coinSkinSOList[levelIndex];
            m_coinTypeList =
                CreateCoinTypeList(0, m_currentMaxDifferentCoinTypeCount,
                    m_currentCoinSkinSO.m_coinSkinDictionary.Count);
        }

        // StringBuilder str = new StringBuilder();
        //
        // for (int i = 0; i < m_coinTypeList.Count; i++)
        // {
        //     str.Append(m_coinTypeList[i]);
        // }
        //
        // Debug.Log(str);
    }

    private void OnInteractableStackBaseEmpty(Stack stackToFill)
    {
        GenerateRandomStack(stackToFill);
    }

    private void GenerateRandomStack(Stack stackToFill)
    {
        int coinCount = Random.Range(m_minStackCount, m_maxStackCount + 1);
        int differentCoinTypeCount = Random.Range(m_minDifferentCoinTypeCountForStackGeneration, m_maxDifferentCoinTypeCountForStackGeneration);
        GenerateCoinStack(stackToFill, coinCount, differentCoinTypeCount);
    }

    private void GenerateCoinStack(Stack stack, int coinStackCount, int differentCoinTypeCount)
    {
        int coinsCount = Mathf.Clamp(coinStackCount, m_minStackCount, m_maxStackCount + 1);
        int differentCoinTypes = Mathf.Clamp(differentCoinTypeCount, m_minDifferentCoinTypeCountForStackGeneration,
            m_maxDifferentCoinTypeCountForStackGeneration);

        if (differentCoinTypes > coinsCount)
            differentCoinTypes = coinsCount;

        float step = (float)coinsCount / differentCoinTypes;

        m_coinTypeList.Shuffle();
        
        for (int i = 0; i < coinsCount; i++)
        {
            CoinType coinType = m_coinTypeList[(int)(i / step)];

            m_coinBuffer = Instantiate(m_coinPrefab, stack.transform);

            m_coinBuffer.InitializeCoin(m_currentCoinSkinSO, coinType, m_delayBetweenCoinSpawn * i);

            stack.AddCoinToStack(m_coinBuffer);
        }
    }

    private List<CoinType> CreateRandomCoinTypeList(int differentCoinTypeCount)
    {
        List<CoinType> randomOrderCoinTypeList = new List<CoinType>();

        for (int i = 0; i < differentCoinTypeCount; i++)
        {
            randomOrderCoinTypeList.Add((CoinType)(i + 1));
        }

        randomOrderCoinTypeList.Shuffle();

        return randomOrderCoinTypeList;
    }

    private List<CoinType> CreateCoinTypeList(int offset, int differentCoinTypeCount, int totalTypeCount)
    {
        List<CoinType> coinTypeList = CoinType.GetValues(typeof(CoinType)).Cast<CoinType>().ToList();
        coinTypeList.Remove(CoinType.Default);

        List<CoinType> desiredCoinTypeList = new List<CoinType>();
        
        int index = 0;
        CoinType currentCoinType;

        while (desiredCoinTypeList.Count < differentCoinTypeCount)
        {
            currentCoinType = coinTypeList[((index + offset) % totalTypeCount)];

            desiredCoinTypeList.Add(currentCoinType);
            
            index++;
        }

        return desiredCoinTypeList;
    }
}