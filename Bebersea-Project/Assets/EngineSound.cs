using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EngineSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip engineLoopClip;
    public float velocityThreshold = 0.1f;
    public float fadeSpeed = 2f;
    public float maxVolume = 1f;

    [Header("References")]
    public Transform fakeSubmarineTransform;  // Use transform, not Rigidbody

    private AudioSource engineSource;
    private float targetVolume = 0f;
    private bool isPlaying = false; // Tracks if the sound is considered "playing"
    private Vector3 lastPosition;
    
    void Start()
    {
        if (fakeSubmarineTransform == null)
        {
            Debug.LogError("No fake submarine Transform assigned.");
            enabled = false;
            return;
        }

        engineSource = GetComponent<AudioSource>();
        if (engineLoopClip == null)
        {
            Debug.LogWarning("No engine clip assigned.");
            enabled = false;
            return;
        }

        engineSource.clip = engineLoopClip;
        engineSource.loop = true;
        engineSource.spatialBlend = 1f;  // 3D sound coming from this GameObject
        engineSource.playOnAwake = false;
        engineSource.volume = 0f;

        engineSource.Play();

        lastPosition = fakeSubmarineTransform.position;
    }

    void Update()
    {
        Vector3 currentPosition = fakeSubmarineTransform.position;
        Vector3 deltaPos = (currentPosition - lastPosition) / Time.deltaTime;

        // Convert world delta velocity into local space relative to fake submarine
        float forwardSpeed = fakeSubmarineTransform.InverseTransformDirection(deltaPos).z;

        bool movingForward = forwardSpeed > velocityThreshold;

        targetVolume = movingForward ? maxVolume : 0f;
        engineSource.volume = Mathf.MoveTowards(engineSource.volume, targetVolume, fadeSpeed * Time.deltaTime);

        if (movingForward && !isPlaying && engineSource.volume > 0f)
        {
            isPlaying = true;
        }
        else if (!movingForward && isPlaying && engineSource.volume <= 0f)
        {
            isPlaying = false;
        }

        lastPosition = currentPosition;
    }
}
