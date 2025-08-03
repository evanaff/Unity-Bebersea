using UnityEngine;

[CreateAssetMenu(fileName = "ProgressInfo", menuName = "Persistence/ProgressInfo")]
public class ProgressInfo : ScriptableObject
{
    [System.Serializable]
    public class EnvironmentProgress
    {
        public string environmentName = "Unnamed";

        [Range(0, 100)]
        public float trashCollected;

        [Range(0, 100)]
        public float coralPlanted;

        [Range(0, 100)]
        public float animalInteracted; // << Tambahan baru
    }

    public EnvironmentProgress[] environments = new EnvironmentProgress[3]; // 3 scene bawah laut

    public Vector3 submarineMapPosition = Vector3.zero;
    public Quaternion submarineMapRotation = Quaternion.identity;

    Vector3 defaultPos = new Vector3(-21.1f, -6.8f, 61.4f);
    Quaternion defaultRot = Quaternion.Euler(0f, 90f, 0f);

    public bool foleyPlayed = false;

    public bool endingTriggered = false;

    public void ResetAllProgress()
    {
        foreach (var env in environments)
        {
            env.trashCollected = 0;
            env.coralPlanted = 0;
            env.animalInteracted = 0; // Reset juga progress hewan
        }

        submarineMapPosition = defaultPos;
        submarineMapRotation = defaultRot;

        foleyPlayed = false;

    }

    public void AddTrashProgress(int index, float amount, float max)
    {
        environments[index].trashCollected = Mathf.Clamp(environments[index].trashCollected + (amount / max * 100f), 0, 100);
    }

    public void AddCoralProgress(int index, float amount, float max)
    {
        environments[index].coralPlanted = Mathf.Clamp(environments[index].coralPlanted + (amount / max * 100f), 0, 100);
    }

    public void AddAnimalProgress(int index, float amount, float max)
    {
        environments[index].animalInteracted = Mathf.Clamp(environments[index].animalInteracted + (amount / max * 100f), 0, 100);
    }

    public void SaveMapSubmarineTransform(Vector3 position, Quaternion rotation)
    {
        submarineMapPosition = position;
        submarineMapRotation = rotation;
    }

    public void LoadMapSubmarineTransform(Transform target)
    {
        target.position = submarineMapPosition;
        target.rotation = submarineMapRotation;
    }

    [System.Serializable]
    public class EncyclopediaProgress
    {
        public bool[] unlockedEntries = new bool[3];
    }

    public EncyclopediaProgress encyclopediaProgress = new EncyclopediaProgress();
    
    public bool IsFullyCompleted()
    {
        if (endingTriggered) return false;

        foreach (var env in environments)
        {
            if (env.trashCollected < 100 || env.coralPlanted < 100 || env.animalInteracted < 100)
                return false;
        }

        return true;
    }

}
