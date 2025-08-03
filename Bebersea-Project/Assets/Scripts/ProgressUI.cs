using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressDisplay : MonoBehaviour
{
    public TMP_Text trashText;
    public TMP_Text coralText;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // Pastikan update jika scene ini sudah aktif sejak awal
        if (SceneManager.GetActiveScene().name == "SubScene")
        {
            UpdateProgressUI();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Loaded Scene: {scene.name}");
        if (scene.name == "SubScene")
        {
            UpdateProgressUI();
        }
    }

    void UpdateProgressUI()
    {
        Debug.Log($"Trash Text Ref: {trashText.text}, Coral Text Ref: {coralText.text}");

        int totalTrash = GameProgressManager.Instance.GetTotalTrash();
        int totalCoral = GameProgressManager.Instance.GetTotalCoral();

        trashText.text = $"Total Trash Collected: {totalTrash}/30";
        coralText.text = $"Total Coral Planted: {totalCoral}/12";
    }
}
