using UnityEngine;
using System.Collections;

public class SubmarineIntroMusic : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource playerAudioSource;
    public AudioClip[] introClips;
    public AudioClip foleyClip;

    [Header("Trigger Settings")]
    public float triggerThreshold = 0.01f;
    public float foleyDelay = 2.0f;

    [Header("Foley Movement Settings")]
    public float foleyStartDistance = 10f;
    public float foleySideOffset = 5f;
    public float foleyEndDistance = 10f;

    [Header("References")]
    public Rigidbody submarineRigidbody;
    public Transform foleyTarget;
    public ProgressInfo progressInfo;  // ← Reference to your ScriptableObject

    private bool musicHasPlayedThisScene = false;

    private bool _canCheckMovement = false;

    private Vector3 _lastPosition;

    private float movementTimer = 0f;
    private float movementSustainTime = 0.5f;

    private bool _hasPlayed = false;

    void Start()
    {
        StartCoroutine(DelayedMovementCheck());

        if (!playerAudioSource || introClips.Length == 0 || !submarineRigidbody || !foleyTarget || !progressInfo)
        {
            Debug.LogWarning("Missing required references.");
            enabled = false;
            return;
        }

        _lastPosition = submarineRigidbody.position;
        submarineRigidbody.isKinematic = false;
        submarineRigidbody.WakeUp();
    }

    private IEnumerator DelayedMovementCheck()
    {
        yield return new WaitForSeconds(1f);
        _canCheckMovement = true;
    }

    void Update()
    {
        if (!_canCheckMovement || _hasPlayed || submarineRigidbody == null || playerAudioSource == null || introClips == null)
            return;

        Vector3 localVelocity = submarineRigidbody.transform.InverseTransformDirection(submarineRigidbody.linearVelocity);

        // Compensate for sudden velocity spikes
        if (localVelocity.sqrMagnitude < 0.0001f)
        {
            Vector3 delta = submarineRigidbody.position - _lastPosition;
            float estimatedZVelocity = Vector3.Dot(submarineRigidbody.transform.forward, delta) / Time.deltaTime;
            localVelocity = new Vector3(0, 0, estimatedZVelocity);
        }

        // Check for sustained movement
        if (localVelocity.z > triggerThreshold)
        {
            movementTimer += Time.deltaTime;

            if (movementTimer >= movementSustainTime)
            {
                PlayRandomIntroMusic();
            }
        }
        else
        {
            movementTimer = 0f;
        }

        _lastPosition = submarineRigidbody.position;
    }
    

    private void PlayRandomIntroMusic()
    {
        if (_hasPlayed || introClips == null || introClips.Length == 0 || playerAudioSource == null)
            return;

        var clip = introClips[Random.Range(0, introClips.Length)];
        playerAudioSource.clip = clip;
        playerAudioSource.Play();

        _hasPlayed = true;
    }

    private IEnumerator PlayFoleyOnce()
    {
        yield return new WaitForSeconds(foleyDelay);

        Vector3 startOffset = foleyTarget.forward * foleyStartDistance + foleyTarget.right * foleySideOffset;
        Vector3 endOffset = -foleyTarget.forward * foleyEndDistance;

        Vector3 startPos = foleyTarget.position + startOffset;
        Vector3 endPos = foleyTarget.position + endOffset;

        GameObject whaleObj = new GameObject("WhaleSound");
        whaleObj.transform.position = startPos;

        AudioSource whaleSource = whaleObj.AddComponent<AudioSource>();
        whaleSource.clip = foleyClip;
        whaleSource.spatialBlend = 1.0f;
        whaleSource.rolloffMode = AudioRolloffMode.Logarithmic;
        whaleSource.minDistance = 1f;
        whaleSource.maxDistance = 50f;
        whaleSource.Play();

        float duration = foleyClip.length;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            whaleObj.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        whaleObj.transform.position = endPos;
        Destroy(whaleObj, 1f);
    }
}
