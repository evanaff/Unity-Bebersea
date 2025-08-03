using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class SubmarineMenu : MonoBehaviour
{
    [System.Serializable]
    public class DebugOption
    {
        public string name;
        public enum ActionType { ResetProgress, TriggerCompletion, GoToMainMenu, ExitGame }
        public ActionType action;
    }

    [Header("UI & Options")]
    public DebugOption[] options;
    public TextMeshProUGUI optionLabel;
    public float fadeDuration = 0.25f;

    [Header("Scene & Data")]
    public ProgressInfo progressInfo;
    public string mainMenuScene = "UI";

    private int currentIndex = 0;
    private Coroutine labelFadeRoutine;

    void Start()
    {
        UpdateOptionLabel(immediate: true);
    }

    public void NextOption()
    {
        if (options.Length == 0) return;
        currentIndex = (currentIndex + 1) % options.Length;
        UpdateOptionLabel();
    }

    public void PreviousOption()
    {
        if (options.Length == 0) return;
        currentIndex = (currentIndex - 1 + options.Length) % options.Length;
        UpdateOptionLabel();
    }

    public void SelectCurrentOption()
    {
        if (options.Length == 0 || progressInfo == null) return;

        var selected = options[currentIndex];
        Debug.Log($"⚙️ Selected debug option: {selected.name}");

        switch (selected.action)
        {
            case DebugOption.ActionType.ResetProgress:
                progressInfo.ResetAllProgress();
                progressInfo.endingTriggered = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Debug.Log("🔄 Progress reset and scene reloaded.");
                break;

            case DebugOption.ActionType.TriggerCompletion:
                foreach (var env in progressInfo.environments)
                {
                    env.trashCollected = 100f;
                    env.coralPlanted = 100f;
                    env.animalInteracted = 100f;
                }
                progressInfo.endingTriggered = false; // Allow completion check to trigger
                Debug.Log("✅ All progress set to 100%.");
                break;

            case DebugOption.ActionType.GoToMainMenu:
                SceneManager.LoadScene(mainMenuScene);
                Debug.Log("🔙 Returning to main menu.");
                break;

            case DebugOption.ActionType.ExitGame:
                Debug.Log("🚪 Exiting game...");
                Application.Quit();
                break;
        }
    }

    private void UpdateOptionLabel(bool immediate = false)
    {
        if (optionLabel == null || options.Length == 0) return;

        string newText = options[currentIndex].name;

        if (immediate)
        {
            optionLabel.text = newText;
            SetLabelAlpha(1f);
            return;
        }

        if (labelFadeRoutine != null)
            StopCoroutine(labelFadeRoutine);

        labelFadeRoutine = StartCoroutine(FadeLabelAndChangeText(newText));
    }

    private IEnumerator FadeLabelAndChangeText(string newText)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetLabelAlpha(Mathf.Lerp(1f, 0f, t / fadeDuration));
            yield return null;
        }

        SetLabelAlpha(0f);
        optionLabel.text = newText;
        t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetLabelAlpha(Mathf.Lerp(0f, 1f, t / fadeDuration));
            yield return null;
        }

        SetLabelAlpha(1f);
        labelFadeRoutine = null;
    }

    private void SetLabelAlpha(float alpha)
    {
        if (optionLabel == null) return;
        var color = optionLabel.color;
        color.a = alpha;
        optionLabel.color = color;
    }
}
