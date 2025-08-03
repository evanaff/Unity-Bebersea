//using UnityEngine;
//using UnityEngine.XR.Interaction.Toolkit;
//using System.Collections;
//using UnityEngine.XR.Interaction.Toolkit.Interactables;
//using TMPro;

//public class AnimalInteraction : MonoBehaviour
//{
//    public ProgressInfo progressInfo;
//    public int environmentIndex = 0;
//    public float animalIncrement = 1f;
//    public int maxAnimals = 1;
//    public int interactedAnimals = 0;
//    public float holdDuration = 2f; // waktu tahan trigger
//    public int entryIndexToUnlock = 0;

//    [Header("UI References")]
//    public TMP_Text progressText;
//    public ProgressBar progressBar;

//    private bool interacted = false;
//    private Coroutine interactionCoroutine;

//    void Awake()
//    {
//        Debug.Log("Awake dipanggil pada: " + gameObject.name);

//        var interactable = GetComponent<XRSimpleInteractable>();
//        if (interactable != null)
//        {
//            interactable.selectEntered.AddListener(OnTurtleSelected);
//            interactable.selectExited.AddListener(OnTurtleUnselected); // saat trigger dilepas
//            Debug.Log("XRSimpleInteractable ditemukan dan listener ditambahkan.");
//        }
//        else
//        {
//            Debug.LogError("XRSimpleInteractable tidak ditemukan di " + gameObject.name);
//        }
//    }

//    private void OnTurtleSelected(SelectEnterEventArgs args)
//    {
//        if (interacted) return;

//        Debug.Log("Player mulai menahan trigger ke hewan");
//        interactionCoroutine = StartCoroutine(HoldInteractionCoroutine());
//    }

//    private void OnTurtleUnselected(SelectExitEventArgs args)
//    {
//        // Jika trigger dilepas sebelum selesai hold
//        if (!interacted && interactionCoroutine != null)
//        {
//            Debug.Log("Trigger dilepas terlalu cepat");
//            StopCoroutine(interactionCoroutine);
//            interactionCoroutine = null;
//        }
//    }

//    private IEnumerator HoldInteractionCoroutine()
//    {
//        Debug.Log("Coroutine dimulai... menunggu hold selama " + holdDuration + " detik.");

//        float timer = 0f;

//        while (timer < holdDuration)
//        {
//            timer += Time.deltaTime;
//            yield return null;
//        }

//        if (!interacted)
//        {
//            Debug.Log("Interaksi berhasil setelah hold!");
//            progressInfo.AddAnimalProgress(environmentIndex, animalIncrement, maxAnimals);
//            interactedAnimals++; // ✅ tambahkan progress lokal
//            UpdateProgressUI();  // ✅ update UI
//            interacted = true;

//            progressInfo.encyclopediaProgress.unlockedEntries[entryIndexToUnlock] = true;
//        }
//    }

//    void UpdateProgressUI()
//    {
//        if (progressText != null)
//            progressText.text = $"Animal {interactedAnimals}/{maxAnimals}";

//        if (progressBar != null)
//        {
//            progressBar.maximum = maxAnimals;
//            progressBar.current = interactedAnimals;
//        }

//        Debug.Log($"UI diperbarui: {interactedAnimals}/{maxAnimals}");
//    }
//}

// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using System.Collections;
// using TMPro;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;

// public class AnimalInteract : MonoBehaviour
// {
//     public ProgressInfo progressInfo;
//     public int environmentIndex = 0;
//     public float animalIncrement = 1f;
//     public int maxAnimals = 1;
//     public int interactedAnimals = 0;
//     public float holdDuration = 2f;
//     public int entryIndexToUnlock = 0;

//     [Header("UI References")]
//     public TMP_Text progressText;

//     private bool interacted = false;
//     private Coroutine interactionCoroutine;

//     void Start()
//     {
//         // Sync interactedAnimals with persistent data
//         interactedAnimals = Mathf.RoundToInt(progressInfo.environments[environmentIndex].animalInteracted / 100f * maxAnimals);
//         UpdateProgressUI();
//     }

//     void Awake()
//     {
//         var interactable = GetComponent<XRSimpleInteractable>();
//         if (interactable != null)
//         {
//             interactable.selectEntered.AddListener(OnTurtleSelected);
//             interactable.selectExited.AddListener(OnTurtleUnselected);
//         }
//     }

//     private void OnTurtleSelected(SelectEnterEventArgs args)
//     {
//         if (interacted) return;

//         interactionCoroutine = StartCoroutine(HoldInteractionCoroutine());
//     }

//     private void OnTurtleUnselected(SelectExitEventArgs args)
//     {
//         if (!interacted && interactionCoroutine != null)
//         {
//             StopCoroutine(interactionCoroutine);
//             interactionCoroutine = null;
//         }
//     }

//     private IEnumerator HoldInteractionCoroutine()
//     {
//         float timer = 0f;
//         while (timer < holdDuration)
//         {
//             timer += Time.deltaTime;
//             yield return null;
//         }

//         if (!interacted)
//         {
//             progressInfo.AddAnimalProgress(environmentIndex, animalIncrement, maxAnimals);
//             interactedAnimals++;
//             interacted = true;

//             UpdateProgressUI();
//             progressInfo.encyclopediaProgress.unlockedEntries[entryIndexToUnlock] = true;
//         }
//     }

//     void UpdateProgressUI()
//     {
//         if (progressText != null)
//             progressText.text = $"Animal {interactedAnimals}/{maxAnimals}";
//     }
// }

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AnimalInteract : MonoBehaviour
{
    public ProgressInfo progressInfo;
    public int environmentIndex = 0;
    public float animalIncrement = 1f;
    public int maxAnimals = 1;
    public int interactedAnimals = 0;
    public float holdDuration = 2f;
    public int entryIndexToUnlock = 0;

    [Header("UI References")]
    public TMP_Text progressText;
    public Image interactionProgressImage;

    [Header("Animation + Audio")]
    public Transform animatedObject;
    public AnimationCurve bounceCurve = AnimationCurve.EaseInOut(0, 1, 1, 1.2f);
    public float bounceDuration = 0.5f;
    public AudioSource completionAudio;
    public AudioClip successClip;

    private bool interacted = false;
    private Coroutine interactionCoroutine;

    // void Start()
    // {
    //     interactedAnimals = Mathf.RoundToInt(progressInfo.environments[environmentIndex].animalInteracted / 100f * maxAnimals);
    //     UpdateProgressUI();

    //     if (interactionProgressImage)
    //         interactionProgressImage.fillAmount = 0f;
    // }

    void Start()
    {
        // Sync interactedAnimals with persistent data
        interactedAnimals = Mathf.RoundToInt(progressInfo.environments[environmentIndex].animalInteracted / 100f * maxAnimals);

        // Mark as interacted if already completed
        interacted = interactedAnimals >= maxAnimals;

        UpdateProgressUI();

        if (interactionProgressImage)
            interactionProgressImage.fillAmount = interacted ? 1f : 0f;

        // Optional: prevent interaction after completion
        if (interacted)
        {
            var interactable = GetComponent<XRSimpleInteractable>();
            if (interactable != null)
                interactable.enabled = false;
        }
    }


    void Awake()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnTurtleSelected);
            interactable.selectExited.AddListener(OnTurtleUnselected);
        }
    }

    private void OnTurtleSelected(SelectEnterEventArgs args)
    {
        if (interacted) return;

        interactionCoroutine = StartCoroutine(HoldInteractionCoroutine());
    }

    private void OnTurtleUnselected(SelectExitEventArgs args)
    {
        if (!interacted && interactionCoroutine != null)
        {
            StopCoroutine(interactionCoroutine);
            interactionCoroutine = null;

            if (interactionProgressImage)
                interactionProgressImage.fillAmount = 0f;
        }
    }

    private IEnumerator HoldInteractionCoroutine()
    {
        float timer = 0f;

        while (timer < holdDuration)
        {
            timer += Time.deltaTime;

            if (interactionProgressImage)
                interactionProgressImage.fillAmount = timer / holdDuration;

            yield return null;
        }

        if (!interacted)
        {
            interacted = true;
            interactedAnimals++;

            progressInfo.AddAnimalProgress(environmentIndex, animalIncrement, maxAnimals);
            progressInfo.encyclopediaProgress.unlockedEntries[entryIndexToUnlock] = true;

            UpdateProgressUI();

            if (interactionProgressImage)
                interactionProgressImage.fillAmount = 1f;

            if (completionAudio)
            {
                if (successClip)
                    completionAudio.PlayOneShot(successClip);
                else
                    completionAudio.Play();
            }


            if (animatedObject)
                StartCoroutine(BounceAnimation());
        }
    }

    private IEnumerator BounceAnimation()
    {
        Vector3 originalScale = animatedObject.localScale;
        float initialBounceDuration = 0.3f;
        int bounceCount = 5;
        float bounceScaleAmount = 1.2f;

        for (int i = 0; i < bounceCount; i++)
        {
            float t = 0f;
            float duration = initialBounceDuration * Mathf.Pow(0.7f, i); // gets faster

            while (t < duration)
            {
                t += Time.deltaTime;
                float normalized = t / duration;
                float curve = Mathf.Sin(normalized * Mathf.PI); // smooth in-out
                float scale = Mathf.Lerp(1f, bounceScaleAmount, curve);
                animatedObject.localScale = originalScale * scale;
                yield return null;
            }

            // Reset scale before next bounce
            animatedObject.localScale = originalScale;
        }
    }


    void UpdateProgressUI()
    {
        if (progressText != null)
            progressText.text = $"Animal {interactedAnimals}/{maxAnimals}";
    }
}
