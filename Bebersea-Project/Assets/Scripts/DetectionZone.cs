using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectionZone : MonoBehaviour
{
    public GlowTarget[] glowTargets;
    public MainMonitor mainMonitor;
    public string sceneToLoad;

    // Add a public field to hold the new texture you want to apply.
    // You can drag and drop a texture onto this field in the Unity Inspector.
    public Texture2D environmentTexture;
    public Texture2D noSignalTexture;

    [Header("New Scaling Functionality")]
    public GameObject planeToScale; // The plane you want to animate
    public Vector3 targetScale = new Vector3(2, 1, 2); // The scale to grow to (Y is 1 so it remains a flat plane)
    public float scaleDuration = 1.0f; // How long the scaling animation takes in seconds

    // NEW: Add this variable to control the strength of the easing.
    [Tooltip("Controls the curve of the ease. > 1 for easing. 2=Normal, 4=Stronger.")]
    public float easePower = 3f; // Let's default to a stronger cubic ease.

    private Vector3 originalScale; // To store the plane's starting scale
    private Coroutine runningCoroutine; // To keep track of the running animation

    void Start()
    {
        // If a plane is assigned, store its original scale when the game starts
        if (planeToScale != null)
        {
            originalScale = planeToScale.transform.localScale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("FakeSub")) return;

        Debug.Log("OnTriggerEnter fired with FakeSub!"); // <-- ADD THIS LINE

        foreach (var glow in glowTargets)
            glow.EnableGlow();

        if (mainMonitor != null && environmentTexture != null)
        {
            Renderer screenRenderer = mainMonitor.GetComponent<Renderer>();
            if (screenRenderer != null)
            {
                Debug.Log("Found Renderer, attempting to change texture..."); // <-- ADD THIS LINE
                screenRenderer.material.SetTexture("_BaseMap", environmentTexture);
            }
            else
            {
                // If you see this message, the problem is in step 3.
                Debug.LogError("Renderer component NOT FOUND on the MainMonitor GameObject!"); // <-- MODIFIED LINE
            }
        }
        else
        {
            // If you see this, the problem is in step 2.
            Debug.LogWarning("MainMonitor or environmentTexture is not assigned in the Inspector!"); // <-- ADD THIS LINE
        }

        GlowTarget.RegisterAction(() =>
        {
            SceneManager.LoadScene(sceneToLoad);
        });

        // --- New scaling functionality ---
        if (planeToScale != null)
        {
            // If an animation is already running, stop it first
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }
            // Start the coroutine to scale the plane UP to the targetScale
            runningCoroutine = StartCoroutine(ScaleObject(planeToScale, targetScale, scaleDuration));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("FakeSub")) return;

        foreach (var glow in glowTargets)
            glow.DisableGlow();

        if (mainMonitor != null && noSignalTexture != null)
        {
            Renderer screenRenderer = mainMonitor.GetComponent<Renderer>();
            if (screenRenderer != null)
            {
                Debug.Log("Found Renderer, attempting to change texture..."); // <-- ADD THIS LINE
                screenRenderer.material.SetTexture("_BaseMap", noSignalTexture);
            }
            else
            {
                // If you see this message, the problem is in step 3.
                Debug.LogError("Renderer component NOT FOUND on the MainMonitor GameObject!"); // <-- MODIFIED LINE
            }
        }
        else
        {
            // If you see this, the problem is in step 2.
            Debug.LogWarning("MainMonitor or noSignalTexture is not assigned in the Inspector!"); // <-- ADD THIS LINE
        }

        GlowTarget.ClearAction();

        // --- New scaling functionality ---
        if (planeToScale != null)
        {
            // If an animation is already running, stop it first
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }
            // Start the coroutine to scale the plane DOWN to its originalScale
            runningCoroutine = StartCoroutine(ScaleObject(planeToScale, originalScale, scaleDuration));
        }
    }
    
    private IEnumerator ScaleObject(GameObject obj, Vector3 newScale, float duration)
    {
        Vector3 currentScale = obj.transform.localScale;
        float elapsedTime = 0;

        // Determine if we are scaling up or down to choose the correct ease.
        // If the target scale is larger than the current scale, we use Ease Out.
        bool isScalingUp = newScale.x > currentScale.x;

        while (elapsedTime < duration)
        {
            // Calculate our progress from 0 to 1
            float t = elapsedTime / duration;

            // NEW: A flexible interpolation factor based on easePower
            float interpolationFactor;

            if (isScalingUp) // Scaling UP (e.g. OnTriggerEnter), we use Ease Out
            {
                // Ease Out: Starts fast, ends slow.
                interpolationFactor = 1f - Mathf.Pow(1f - t, easePower);
            }
            else // Scaling DOWN (e.g. OnTriggerExit), we use Ease In
            {
                // Ease In: Starts slow, ends fast.
                interpolationFactor = Mathf.Pow(t, easePower);
            }

            // Apply the eased interpolation factor to the Lerp function
            obj.transform.localScale = Vector3.Lerp(currentScale, newScale, interpolationFactor);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is set exactly to the target
        obj.transform.localScale = newScale;
        runningCoroutine = null;
    }
}