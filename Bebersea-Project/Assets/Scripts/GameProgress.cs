using UnityEngine;
using System.Collections.Generic;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance;

    // Key: scene index or name ("Scene2", "Scene3", "Scene4")
    public Dictionary<string, ProgressData> sceneProgress = new Dictionary<string, ProgressData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Init scene progress
            sceneProgress["Environment1"] = new ProgressData();
            sceneProgress["Environment2"] = new ProgressData();
            sceneProgress["Environment3"] = new ProgressData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddTrash(string sceneName, int amount)
    {
        if (sceneProgress.ContainsKey(sceneName))
            sceneProgress[sceneName].trashCollected += amount;
    }

    public void AddCoral(string sceneName, int amount)
    {
        if (sceneProgress.ContainsKey(sceneName))
            sceneProgress[sceneName].coralPlanted += amount;
    }

    public int GetTotalTrash()
    {
        int total = 0;
        foreach (var data in sceneProgress.Values)
        {
            total += data.trashCollected;
        }
        return total;
    }

    public int GetTotalCoral()
    {
        int total = 0;
        foreach (var data in sceneProgress.Values)
        {
            total += data.coralPlanted;
        }
        return total;
    }
}

[System.Serializable]
public class ProgressData
{
    public int trashCollected = 0;
    public int coralPlanted = 0;
}
