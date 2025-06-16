using UnityEngine;

[CreateAssetMenu(fileName = "ProgressInfo", menuName = "Persistence/ProgressInfo")]
public class ProgressInfo : ScriptableObject
{
    [System.Serializable]
    public class EnvironmentProgress
    {
        [Range(0, 100)]
        public float trashCollected;

        [Range(0, 100)]
        public float coralPlanted;

        [Range(0, 100)]
        public float animalInteracted; // << Tambahan baru
    }

    public EnvironmentProgress[] environments = new EnvironmentProgress[3]; // 3 scene bawah laut

    public void ResetAllProgress()
    {
        foreach (var env in environments)
        {
            env.trashCollected = 0;
            env.coralPlanted = 0;
            env.animalInteracted = 0; // Reset juga progress hewan
        }
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
}
