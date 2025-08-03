//using UnityEngine;
//using TMPro;

//public class CoralPlantingProgressTracker : MonoBehaviour
//{
//    public int targetCoralCount = 5;
//    public TMP_Text progressText;
//    public ProgressBar progressBar;

//    public int plantedCorals = 0;

//    void Start()
//    {
//        UpdateUI();
//    }

//    public void IncrementCoral()
//    {
//        plantedCorals++;
//        UpdateUI();

//        if (plantedCorals >= targetCoralCount)
//        {
//            MissionComplete();
//        }
//    }

//    void UpdateUI()
//    {
//        if (progressText != null)
//            progressText.text = $"Planted Coral {plantedCorals}/{targetCoralCount}";

//        if (progressBar != null)
//        {
//            progressBar.maximum = targetCoralCount;
//            progressBar.current = plantedCorals;
//        }
//    }

//    void MissionComplete()
//    {
//        Debug.Log("🎉 Semua coral sudah ditanam!");
//        // Tambahkan efek sukses, suara, animasi, dsb.
//    }
//}

using UnityEngine;
using TMPro;

public class CoralPlantingProgressTracker : MonoBehaviour
{
    public int targetCoralCount = 5;
    public TMP_Text progressText;

    public int sceneIndex = 0; // Match the scene you're tracking
    public ProgressInfo progressInfo;

    [Header("Success Sound")]
    public AudioClip successClip;
    public AudioSource audioSource;

    void Start()
    {
        int current = Mathf.RoundToInt(progressInfo.environments[sceneIndex].coralPlanted / 100f * targetCoralCount);
        UpdateUI(current);
    }

    public void IncrementCoral()
    {
        int current = Mathf.RoundToInt(progressInfo.environments[sceneIndex].coralPlanted / 100f * targetCoralCount);
        UpdateUI(current);

        if (current >= targetCoralCount)
        {
            MissionComplete();
        }
    }

    void UpdateUI(int planted)
    {
        if (progressText != null)
            progressText.text = $"Planted Coral {planted}/{targetCoralCount}";
    }

    void MissionComplete()
    {
        Debug.Log("🎉 Semua coral sudah ditanam!");

        if (successClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(successClip);
        }
    }
}
