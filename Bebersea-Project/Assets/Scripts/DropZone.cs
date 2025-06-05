using UnityEngine;
using TMPro;

public class DropZoneHandler : MonoBehaviour
{
    public int targetTrashCount = 5;
    public int collectedTrash = 0;

    public TMP_Text progressText;
    public ProgressBar progressBar;

    void Start()
    {
        UpdateProgressUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            collectedTrash++;
            Destroy(other.gameObject);
            UpdateProgressUI();

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
        // Tambahkan efek misi selesai di sini
    }
}
