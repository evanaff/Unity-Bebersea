using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SubmarineSceneTrigger : MonoBehaviour
{
    public GameObject instructionUI;
    public string targetSceneName = "SubScene"; // Ganti dengan nama scene tujuan

    public InputActionProperty triggerAction;

    private bool isPlayerNearby = false;

    private void Start()
    {
        instructionUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Scene Triggered");
            instructionUI.SetActive(true);
            isPlayerNearby = true;
            triggerAction.action.Enable();
            triggerAction.action.performed += OnTriggerPressed;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            instructionUI.SetActive(false);
            isPlayerNearby = false;
            triggerAction.action.performed -= OnTriggerPressed;
            triggerAction.action.Disable();
        }
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        if (isPlayerNearby)
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
