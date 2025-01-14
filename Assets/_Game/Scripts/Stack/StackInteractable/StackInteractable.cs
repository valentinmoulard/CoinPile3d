using System;

public class StackInteractable : StackBase
{
    public static Action<Stack> OnInteractableStackBaseEmpty;


    protected override void OnEnable()
    {
        base.OnEnable();
        StackInteractableController.OnAllStackInteractableEmpty += OnAllStackInteractableEmpty;
        UI_ButtonDebug_NextLevel.OnDebugNextLevelButtonPressed += OnDebugNextLevelButtonPressed;

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StackInteractableController.OnAllStackInteractableEmpty -= OnAllStackInteractableEmpty;
        UI_ButtonDebug_NextLevel.OnDebugNextLevelButtonPressed -= OnDebugNextLevelButtonPressed;
    }

    protected override void OnMainMenu()
    {
        base.OnMainMenu();
        m_stack.DestroyAllCoinsFromStack();
    }

    private void OnAllStackInteractableEmpty()
    {
        OnInteractableStackBaseEmpty?.Invoke(m_stack);
    }

    private void OnDebugNextLevelButtonPressed()
    {
        m_stack.DestroyAllCoinsFromStack();
        OnInteractableStackBaseEmpty?.Invoke(m_stack);
    }
    
}