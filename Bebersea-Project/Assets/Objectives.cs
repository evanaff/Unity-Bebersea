using UnityEngine;
using TMPro;

public class Objectives : MonoBehaviour
{
    [System.Serializable]
    public class EnvironmentUI
    {
        public TextMeshProUGUI environmentName;

        public TMP_Text trashProgressText;
        public TMP_Text coralProgressText;
        public TMP_Text animalProgressText;
    }

    public ProgressInfo progressInfo;

    // Set target counts here per environment and per objective
    [System.Serializable]
    public class EnvironmentTargets
    {
        public int trashTarget;
        public int coralTarget;
        public int animalTarget;
    }

    public EnvironmentTargets[] targets = new EnvironmentTargets[3]; // 3 environments

    public EnvironmentUI[] environmentUIs = new EnvironmentUI[3]; // assign in inspector

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < environmentUIs.Length; i++)
        {
            environmentUIs[i].environmentName.text = progressInfo.environments[i].environmentName;

            // Calculate actual done count based on percent and target count
            int trashDone = Mathf.RoundToInt(progressInfo.environments[i].trashCollected / 100f * targets[i].trashTarget);
            int coralDone = Mathf.RoundToInt(progressInfo.environments[i].coralPlanted / 100f * targets[i].coralTarget);
            int animalDone = Mathf.RoundToInt(progressInfo.environments[i].animalInteracted / 100f * targets[i].animalTarget);

            // Format: "x / y"
            environmentUIs[i].trashProgressText.text = $"Trash: {trashDone} / {targets[i].trashTarget}";
            environmentUIs[i].coralProgressText.text = $"Coral: {coralDone} / {targets[i].coralTarget}";
            environmentUIs[i].animalProgressText.text = $"Animal: {animalDone} / {targets[i].animalTarget}";
        }
    }
}
