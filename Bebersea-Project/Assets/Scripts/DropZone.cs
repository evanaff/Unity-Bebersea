using UnityEngine;
using TMPro;

public class DropZoneHandler : MonoBehaviour
{
    [Header("Progress Settings")]
    public int targetTrashCount = 5;
    public int collectedTrash = 0;

    [Header("UI References")]
    public TMP_Text progressText;
    public ProgressBar progressBar;

    [Header("Progress Save")]
    public ProgressInfo progressInfo;
    public int sceneLoad;

    [Header("SFX Settings")]
    public AudioClip dropSFX;             // <--- Suara saat 1 sampah masuk
    public AudioClip successSFX;          // <--- Suara saat misi selesai
    public AudioSource audioSource;       // <--- AudioSource untuk memutar semua SFX

    void Start()
    {
        UpdateProgressUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            collectedTrash++;
            progressInfo.AddTrashProgress(sceneLoad, 1, targetTrashCount);
            UpdateProgressUI();

            // Putar SFX saat drop sampah
            if (audioSource != null && dropSFX != null)
            {
                audioSource.PlayOneShot(dropSFX);
            }

            Destroy(other.gameObject);

            if (collectedTrash >= targetTrashCount)
            {
                MissionComplete();
            }
        }
    }

    void UpdateProgressUI()
    {
        if (progressText != null)
            progressText.text = $"Collected Trash {collectedTrash}/{targetTrashCount}";

        if (progressBar != null)
        {
            progressBar.maximum = targetTrashCount;
            progressBar.current = collectedTrash;
        }
    }

    void MissionComplete()
    {
        Debug.Log("🎉 Misi selesai!");

        if (audioSource != null && successSFX != null)
        {
            audioSource.PlayOneShot(successSFX);
        }

        // Tambah logika lanjut jika perlu
    }
}
