using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;

public class AnimalInteraction : MonoBehaviour
{
    public ProgressInfo progressInfo;
    public int environmentIndex = 0;
    public float animalIncrement = 1f;
    public int maxAnimals = 1;
    public int interactedAnimals = 0;
    public float holdDuration = 2f; // waktu tahan trigger
    public int entryIndexToUnlock = 0;

    [Header("UI References")]
    public TMP_Text progressText;
    public ProgressBar progressBar;

    private bool interacted = false;
    private Coroutine interactionCoroutine;

    void Awake()
    {
        Debug.Log("Awake dipanggil pada: " + gameObject.name);

        var interactable = GetComponent<XRSimpleInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnTurtleSelected);
            interactable.selectExited.AddListener(OnTurtleUnselected); // saat trigger dilepas
            Debug.Log("XRSimpleInteractable ditemukan dan listener ditambahkan.");
        }
        else
        {
            Debug.LogError("XRSimpleInteractable tidak ditemukan di " + gameObject.name);
        }
    }

    private void OnTurtleSelected(SelectEnterEventArgs args)
    {
        if (interacted) return;

        Debug.Log("Player mulai menahan trigger ke hewan");
        interactionCoroutine = StartCoroutine(HoldInteractionCoroutine());
    }

    private void OnTurtleUnselected(SelectExitEventArgs args)
    {
        // Jika trigger dilepas sebelum selesai hold
        if (!interacted && interactionCoroutine != null)
        {
            Debug.Log("Trigger dilepas terlalu cepat");
            StopCoroutine(interactionCoroutine);
            interactionCoroutine = null;
        }
    }

    private IEnumerator HoldInteractionCoroutine()
    {
        Debug.Log("Coroutine dimulai... menunggu hold selama " + holdDuration + " detik.");

        float timer = 0f;

        while (timer < holdDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (!interacted)
        {
            Debug.Log("Interaksi berhasil setelah hold!");
            progressInfo.AddAnimalProgress(environmentIndex, animalIncrement, maxAnimals);
            interactedAnimals++; // ✅ tambahkan progress lokal
            UpdateProgressUI();  // ✅ update UI
            interacted = true;

            progressInfo.encyclopediaProgress.unlockedEntries[entryIndexToUnlock] = true;
        }
    }

    void UpdateProgressUI()
    {
        if (progressText != null)
            progressText.text = $"Animal {interactedAnimals}/{maxAnimals}";

        if (progressBar != null)
        {
            progressBar.maximum = maxAnimals;
            progressBar.current = interactedAnimals;
        }

        Debug.Log($"UI diperbarui: {interactedAnimals}/{maxAnimals}");
    }
}
