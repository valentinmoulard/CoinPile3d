using UnityEngine;

public class StackBase : GameflowBehavior
{
    [SerializeField] protected bool m_isStackDraggable = false;
    
    [SerializeField] protected Stack m_stack;

    public bool IsStackEmpty
    {
        get => m_stack.IsEmpty;
    }

    public bool IsDraggable
    {
        get => m_isStackDraggable;
    }
    
    public Stack Stack
    {
        get => m_stack;
    }

}
