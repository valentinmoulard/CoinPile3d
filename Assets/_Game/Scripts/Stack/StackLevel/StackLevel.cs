
using UnityEngine;


public class StackLevel : StackBase
{
    private StackLevel_Neighbors m_stackLevelNeighbors;
    
    private StackLevel_Animations m_stackLevelAnimations;

    public StackLevel_Neighbors StackLevelNeighbors
    {
        get => m_stackLevelNeighbors;
    }

    
    public void Initialize()
    {
        SetupStackBase();
        m_stackLevelNeighbors.Initialize(this, m_stack);
        PlayAppearAnimation();
    }

    public void FindCoinsToDestroyInStack(int minCoinsThresholdToDestroy)
    {
        m_stack.FindCoinsToDestroyInStack(minCoinsThresholdToDestroy);
    }

    public void DestroyCoinsOfSameType(float destructionDuration)
    {
        m_stack.DestroyCoinsOfSameType(destructionDuration);
    }
    
    private void PlayAppearAnimation()
    {
        m_stackLevelAnimations = GetComponent<StackLevel_Animations>();
        
        if (m_stackLevelAnimations == null)
        {
            Debug.LogError("Could not retrieve StackBase_Animations component!", gameObject);
            return;
        }
        
        m_stackLevelAnimations.Appear();
    }

    private void SetupStackBase()
    {
        m_stackLevelNeighbors = GetComponent<StackLevel_Neighbors>();

        if (m_stackLevelNeighbors == null)
        {
            Debug.LogError("Could not retrieve StackBase_Neighbors component!", gameObject);
            return;
        }
        
        m_stackLevelNeighbors.FindNeighbors();
    }

}
