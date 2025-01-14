using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinSkin_", menuName = "ScriptableObjects/CoinSkin", order = 1)]
public class CoinSkin_SO : SerializedScriptableObject
{
    public Dictionary<CoinType, GameObject> m_coinSkinDictionary;


    public GameObject GetSkin(CoinType type)
    {
        if (m_coinSkinDictionary.ContainsKey(type))
        {
            return m_coinSkinDictionary[type];
        }

        return null;
    }


}