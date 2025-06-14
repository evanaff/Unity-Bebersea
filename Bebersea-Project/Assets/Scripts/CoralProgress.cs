using UnityEngine;
using TMPro;

public class CoralPlantingProgressTracker : MonoBehaviour
{
    public int targetCoralCount = 5;
    public TMP_Text progressText;
    public ProgressBar progressBar;

    public int plantedCorals = 0;

    void Start()
    {
        UpdateUI();
    }

    public void IncrementCoral()
    {
        plantedCorals++;
        UpdateUI();

        if (plantedCorals >= targetCoralCount)
        {
            MissionComplete();
        }
    }

    void UpdateUI()
    {
        if (progressText != null)
            progressText.text = $"Planted Coral {plantedCorals}/{targetCoralCount}";

        if (progressBar != null)
        {
            progressBar.maximum = targetCoralCount;
            progressBar.current = plantedCorals;
        }
    }

    void MissionComplete()
    {
        Debug.Log("🎉 Semua coral sudah ditanam!");
        // Tambahkan efek sukses, suara, animasi, dsb.
    }
}
