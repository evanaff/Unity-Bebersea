using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SubmarineSceneTrigger : MonoBehaviour
{
    [SerializeField] private GameObject instructionUI;
    [SerializeField] private string targetSceneName = "SubScene";
    [SerializeField] private InputActionProperty triggerAction;
    [SerializeField] private string playerTag = "Player";

    private bool isPlayerNearby = false;
    private bool isActionSubscribed = false;
    private static int activeInstances = 0; // Track berapa banyak instance aktif

    private void Start()
    {
        activeInstances++;
        Debug.Log($"[{gameObject.name}] Started. Active instances: {activeInstances}");

        if (instructionUI != null)
            instructionUI.SetActive(false);
        else
            Debug.LogWarning($"[{gameObject.name}] InstructionUI not assigned!");

        if (string.IsNullOrEmpty(targetSceneName))
            Debug.LogWarning($"[{gameObject.name}] Target scene name is empty!");

        // PENTING: Pastikan action di-disable di awal
        if (triggerAction.action != null)
        {
            triggerAction.action.Disable();
            Debug.Log($"[{gameObject.name}] Action disabled at start");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[{gameObject.name}] OnTriggerEnter - Object: {other.name}, Tag: {other.tag}");

        if (other.CompareTag(playerTag))
        {
            Debug.Log($"[{gameObject.name}] Player entered trigger zone");

            if (instructionUI != null)
                instructionUI.SetActive(true);

            isPlayerNearby = true;
            SubscribeToTriggerAction();
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log($"[{gameObject.name}] OnTriggerExit - Object: {other.name}, Tag: {other.tag}");

        if (other.CompareTag(playerTag))
        {
            Debug.Log($"[{gameObject.name}] Player exited trigger zone");

            if (instructionUI != null)
                instructionUI.SetActive(false);

            isPlayerNearby = false;
            UnsubscribeFromTriggerAction();
        }
    }

    private void SubscribeToTriggerAction()
    {
        if (triggerAction.action != null && !isActionSubscribed)
        {
            triggerAction.action.Enable();
            triggerAction.action.performed += OnTriggerPressed;
            isActionSubscribed = true;
            Debug.Log($"[{gameObject.name}] Action ENABLED and SUBSCRIBED");
        }
        else if (isActionSubscribed)
        {
            Debug.LogWarning($"[{gameObject.name}] Trying to subscribe but already subscribed!");
        }
    }

    private void UnsubscribeFromTriggerAction()
    {
        if (triggerAction.action != null && isActionSubscribed)
        {
            triggerAction.action.performed -= OnTriggerPressed;
            triggerAction.action.Disable();
            isActionSubscribed = false;
            Debug.Log($"[{gameObject.name}] Action DISABLED and UNSUBSCRIBED");
        }
        else if (!isActionSubscribed)
        {
            Debug.LogWarning($"[{gameObject.name}] Trying to unsubscribe but not subscribed!");
        }
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        Debug.Log($"[{gameObject.name}] Trigger pressed! IsPlayerNearby: {isPlayerNearby}");

        if (isPlayerNearby && !string.IsNullOrEmpty(targetSceneName))
        {
            Debug.Log($"[{gameObject.name}] Loading scene: {targetSceneName}");
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] Trigger pressed but conditions not met. PlayerNearby: {isPlayerNearby}, SceneName: {targetSceneName}");
        }
    }

    private void OnDestroy()
    {
        activeInstances--;
        Debug.Log($"[{gameObject.name}] Destroyed. Active instances: {activeInstances}");
        UnsubscribeFromTriggerAction();
    }

    private void OnDisable()
    {
        Debug.Log($"[{gameObject.name}] Disabled");
        if (isPlayerNearby)
        {
            isPlayerNearby = false;
            if (instructionUI != null)
                instructionUI.SetActive(false);
            UnsubscribeFromTriggerAction();
        }
    }

    // Method untuk debugging manual
    [ContextMenu("Debug Current State")]
    private void DebugCurrentState()
    {
        Debug.Log($"[{gameObject.name}] Current State:");
        Debug.Log($"  - IsPlayerNearby: {isPlayerNearby}");
        Debug.Log($"  - IsActionSubscribed: {isActionSubscribed}");
        Debug.Log($"  - Action Enabled: {(triggerAction.action != null ? triggerAction.action.enabled : "NULL")}");
        Debug.Log($"  - Active Instances: {activeInstances}");
    }
}