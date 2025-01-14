using System.Collections.Generic;
using UnityEngine;

public class StackLevel_Neighbors : MonoBehaviour
{
    [SerializeField] private float m_neighborRangeLength = 1f;

    [SerializeField] private bool m_isDebugEnabled = false;

    private StackLevel_Neighbors m_rightNeighbor;
    private StackLevel_Neighbors m_upNeighbor;
    private StackLevel_Neighbors m_leftNeighbor;
    private StackLevel_Neighbors m_bottomNeighbor;

    public StackLevel StackLevel { get; private set; }
    public Stack Stack { get; private set; }

    private List<StackLevel_Neighbors> m_neighborList = new List<StackLevel_Neighbors>();
    private List<StackLevel_Neighbors> m_neighborWithSameCoinTypeOnTopList = new List<StackLevel_Neighbors>();

    public List<StackLevel_Neighbors> NeighborWithSameCoinTypeOnTopList
    {
        get => m_neighborWithSameCoinTypeOnTopList;
    }

    public bool HasNeighborsWithStack()
    {
        if (m_neighborList.Count == 0)
            return false;

        for (int i = 0; i < m_neighborList.Count; i++)
        {
            if (m_neighborList[i] != null && !m_neighborList[i].Stack.IsEmpty)
                return true;
        }

        return false;
    }

    public int NeighborWithSameCoinTypeOnTopCount
    {
        get => m_neighborWithSameCoinTypeOnTopList.Count;
    }

    public void Initialize(StackLevel stackLevelReference, Stack stackReference)
    {
        StackLevel = stackLevelReference;
        Stack = stackReference;
    }

    public void FindNeighborWithSameCoinTypeOnTopOfStackCount()
    {
        m_neighborWithSameCoinTypeOnTopList.Clear();

        FindNeighborWithSameCoinTypeOnTop(m_rightNeighbor, "right");
        FindNeighborWithSameCoinTypeOnTop(m_upNeighbor, "up");
        FindNeighborWithSameCoinTypeOnTop(m_leftNeighbor, "left");
        FindNeighborWithSameCoinTypeOnTop(m_bottomNeighbor, "bottom");
    }

    private void FindNeighborWithSameCoinTypeOnTop(StackLevel_Neighbors neighbor, string name)
    {
        if (neighbor == null || neighbor.StackLevel.Stack.GetCoinTypeOnTopOfStack == CoinType.Default)
            return;

        if (this.StackLevel.Stack.GetCoinTypeOnTopOfStack == neighbor.StackLevel.Stack.GetCoinTypeOnTopOfStack)
        {
            m_neighborWithSameCoinTypeOnTopList.Add(neighbor);
        }

        Debug.Log(name + " : " + neighbor.StackLevel.Stack.GetCoinTypeOnTopOfStack, gameObject);
    }

    public void FindNeighbors()
    {
        m_neighborList.Clear();

        m_rightNeighbor = null;
        m_upNeighbor = null;
        m_leftNeighbor = null;
        m_bottomNeighbor = null;

        m_rightNeighbor = TryFindNeighborWithDirection(Vector3.right);
        m_upNeighbor = TryFindNeighborWithDirection(Vector3.forward);
        m_leftNeighbor = TryFindNeighborWithDirection(Vector3.left);
        m_bottomNeighbor = TryFindNeighborWithDirection(Vector3.back);
    }

    private StackLevel_Neighbors TryFindNeighborWithDirection(Vector3 direction)
    {
        if (Physics.Raycast(transform.position, direction, out var hitInfo, m_neighborRangeLength))
        {
            StackLevel_Neighbors potentialNeighbor = hitInfo.transform.gameObject.GetComponent<StackLevel_Neighbors>();

            if (potentialNeighbor != null)
                m_neighborList.Add(potentialNeighbor);

            return potentialNeighbor;
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        if (m_isDebugEnabled == false)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * m_neighborRangeLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * m_neighborRangeLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * m_neighborRangeLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.back * m_neighborRangeLength);
    }
}