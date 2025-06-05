using UnityEngine;
using System.Collections;

public class SubmarineIntroMusic : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource playerAudioSource;
    public AudioClip introClip;
    public AudioClip foleyClip;

    [Header("Trigger Settings")]
    public float triggerThreshold = 0.01f;
    public float foleyDelay = 2.0f;

    [Header("Foley Movement Settings")]
    public float foleyStartDistance = 10f;
    public float foleySideOffset = 5f;
    public float foleyEndDistance = 10f;

    [Header("References")]
    public Rigidbody submarineRigidbody;  // Used for movement detection
    public Transform foleyTarget;         // Used for sound positioning

    private bool _hasPlayed = false;
    private Vector3 _lastPosition;

    void Start()
    {
        if (playerAudioSource == null)
            Debug.LogWarning("No AudioSource assigned.");
        if (introClip == null)
            Debug.LogWarning("No intro AudioClip assigned.");
        if (foleyClip == null)
            Debug.LogWarning("No foley AudioClip assigned.");
        if (submarineRigidbody == null)
            Debug.LogError("Rigidbody not assigned.");
        if (foleyTarget == null)
            Debug.LogError("Foley target Transform not assigned.");

        submarineRigidbody.isKinematic = false;
        submarineRigidbody.WakeUp();

        _lastPosition = submarineRigidbody.position;
    }

    void Update()
    {
        if (_hasPlayed || submarineRigidbody == null || playerAudioSource == null || introClip == null)
            return;

        Vector3 localVelocity = submarineRigidbody.transform.InverseTransformDirection(submarineRigidbody.linearVelocity);

        if (localVelocity.sqrMagnitude < 0.0001f)
        {
            Vector3 delta = submarineRigidbody.position - _lastPosition;
            float estimatedZVelocity = Vector3.Dot(submarineRigidbody.transform.forward, delta) / Time.deltaTime;
            localVelocity = new Vector3(0, 0, estimatedZVelocity);
        }

        if (localVelocity.z > triggerThreshold)
        {
            Debug.Log("Playing intro clip");
            playerAudioSource.clip = introClip;
            playerAudioSource.Play();
            _hasPlayed = true;

            StartCoroutine(PlayPassingWhaleSound());
        }

        _lastPosition = submarineRigidbody.position;
    }

    private IEnumerator PlayPassingWhaleSound()
    {
        yield return new WaitForSeconds(foleyDelay);

        if (foleyClip == null || foleyTarget == null)
            yield break;

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

        Debug.Log("Whale sound started, moving from front-right to back.");

        float duration = foleyClip.length;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            whaleObj.transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        whaleObj.transform.position = endPos;
        Destroy(whaleObj, 1f);
    }
}
