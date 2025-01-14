using System;
using UnityEngine;

public class StackMover : GameflowBehavior
{
    public static Action<Stack> OnStackPlacedOnLevel;
    public static Action<StackLevel_Animations> OnStackLevelPreSelected;

    [SerializeField] private Vector3 m_movingStackPositionOffset = Vector3.zero;

    [SerializeField] private LayerMask m_floorMask = 0;

    [SerializeField] private LayerMask m_stackBaseLayer = 0;

    [SerializeField] private Stack m_controllerStack = null;

    [SerializeField] private float m_smoothTime = 0.3F;

    [SerializeField] private float m_stackTransferDuration = 0.2f;

    private StackLevel_Animations m_currentStackLevelPreSelected;
    private StackLevel_Animations m_previousStackLevelPreSelected;
    private Stack m_originalStack;
    private Vector3 m_desiredControlledStackPosition;
    private Vector3 velocity = Vector3.zero;
    private bool m_isHolding;
    private bool m_isControllEnabled;

    private void SubscribeToEvents()
    {
        Controller.OnTapBegin += OnTapBegin;
        Controller.OnHold += OnHold;
        Controller.OnRelease += OnRelease;
        CoinDestroyResolver.OnCoinDestroyResolutionComplete += OnCoinDestroyResolutionComplete;
    }

    private void UnsubscribeFromEvents()
    {
        Controller.OnTapBegin -= OnTapBegin;
        Controller.OnHold -= OnHold;
        Controller.OnRelease -= OnRelease;
        CoinDestroyResolver.OnCoinDestroyResolutionComplete -= OnCoinDestroyResolutionComplete;
    }

    private void Update()
    {
        UpdateControlledStackPosition();
    }


    // GAMEFLOW SECTION

    protected override void OnMainMenu()
    {
        base.OnMainMenu();
        DisableControls();
    }

    protected override void OnGameplay()
    {
        base.OnGameplay();
        SubscribeToEvents();
        EnableControls();
    }

    protected override void OnVictory()
    {
        base.OnVictory();
        UnsubscribeFromEvents();
        DisableControls();
    }

    protected override void OnGameover()
    {
        base.OnGameover();
        UnsubscribeFromEvents();
        DisableControls();
    }


    private void OnCoinDestroyResolutionComplete(bool hasResolvedDestructions)
    {
        EnableControls();
    }

    // BEHAVIOR SECTION
    private void EnableControls()
    {
        m_isControllEnabled = true;
    }

    private void DisableControls()
    {
        m_isControllEnabled = false;
    }

    private void UpdateControlledStackPosition()
    {
        if (m_isHolding == false)
            return;

        transform.position = Vector3.SmoothDamp(transform.position, m_desiredControlledStackPosition, ref velocity,
            m_smoothTime);
    }

    private void OnTapBegin(Vector3 cursorPosition)
    {
        if (m_isControllEnabled == false)
            return;

        RaycastHit hitInfo = CastRay(cursorPosition, m_stackBaseLayer);

        if (hitInfo.collider == null)
            return;

        StackBase hitStack = hitInfo.collider.GetComponent<StackBase>();

        if (hitStack == null || hitStack.IsDraggable == false)
            return;

        m_originalStack = hitStack.Stack;

        m_desiredControlledStackPosition = hitStack.transform.position + m_movingStackPositionOffset;
        transform.position = m_desiredControlledStackPosition;

        hitStack.Stack.TransferStackToTargetStack(m_controllerStack, m_stackTransferDuration);

        m_isHolding = true;
    }

    private void OnHold(Vector3 cursorPosition)
    {
        if (m_isControllEnabled == false || m_isHolding == false)
            return;

        // Manage movement
        RaycastHit hitInfo = CastRay(cursorPosition, m_floorMask);
        m_desiredControlledStackPosition = hitInfo.point + m_movingStackPositionOffset;


        // Manage stack level preselection 
        RaycastHit hitInfoStackBase = CastRay(cursorPosition, m_stackBaseLayer);

        if (hitInfoStackBase.collider == null)
        {
            m_previousStackLevelPreSelected = null;
            m_currentStackLevelPreSelected = null;
            OnStackLevelPreSelected?.Invoke(m_currentStackLevelPreSelected);
            return;
        }

        StackLevel hitStack = hitInfoStackBase.collider.GetComponent<StackLevel>();

        if (hitStack == null)
            return;

        if (!hitStack.Stack.IsEmpty)
        {
            OnStackLevelPreSelected?.Invoke(null);
            return;
        }

        m_currentStackLevelPreSelected = hitInfoStackBase.transform.GetComponent<StackLevel_Animations>();

        if (m_currentStackLevelPreSelected != null)
        {
            if (m_currentStackLevelPreSelected != m_previousStackLevelPreSelected)
            {
                m_previousStackLevelPreSelected = m_currentStackLevelPreSelected;
                OnStackLevelPreSelected?.Invoke(m_currentStackLevelPreSelected);
            }
        }
    }

    private void OnRelease(Vector3 cursorPosition)
    {
        if (m_isHolding == false)
            return;

        m_isHolding = false;

        if (m_isControllEnabled == false)
            return;

        RaycastHit hitInfo = CastRay(cursorPosition, m_stackBaseLayer);

        if (hitInfo.collider == null)
        {
            m_controllerStack.TransferStackToTargetStack(m_originalStack, m_stackTransferDuration);
            m_originalStack = null;
            return;
        }

        StackBase hitStack = hitInfo.collider.GetComponent<StackBase>();

        if (hitStack == null)
        {
            m_controllerStack.TransferStackToTargetStack(m_originalStack, m_stackTransferDuration);
            m_originalStack = null;
            return;
        }

        if (!hitStack.IsStackEmpty)
        {
            m_controllerStack.TransferStackToTargetStack(m_originalStack, m_stackTransferDuration);
            m_originalStack = null;
            return;
        }

        m_controllerStack.TransferStackToTargetStack(hitStack.Stack, m_stackTransferDuration);
        DisableControls();
        OnStackPlacedOnLevel?.Invoke(hitStack.Stack);

        m_originalStack = null;
    }

    private RaycastHit CastRay(Vector3 cursorPosition, LayerMask mask)
    {
        if (Camera.main == null)
            return default;

        Ray ray = Camera.main.ScreenPointToRay(cursorPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500f, mask))
            return hit;

        return default;
    }
}