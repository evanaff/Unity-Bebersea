using UnityEngine;
using TMPro;

public class Encyclopedia : MonoBehaviour
{
    public GameObject[] entries; // An array of all your bestiary entry GameObjects
    public TextMeshProUGUI[] entryNames;
    private int currentIndex = 0; // The index of the currently displayed entry
    public ProgressInfo progressInfo;

    private string GetEntryName(int index)
    {
        switch (index)
        {
            case 0: return "Sea Turtle";
            case 1: return "Dory";
            case 2: return "Clownfish";
            // Add more as needed
            default: return "Unknown";
        }
    }

    void Start()
    {
        bool foundUnlocked = false;

        for (int i = 0; i < entries.Length; i++)
        {
            bool isUnlocked = progressInfo.encyclopediaProgress.unlockedEntries[i];

            // Hide all entries first
            entries[i].SetActive(false);

            // Show/hide name list, and set its label depending on unlock status
            if (entryNames != null && entryNames.Length > i && entryNames[i] != null)
            {
                entryNames[i].gameObject.SetActive(true); // Always show name

                if (isUnlocked)
                {
                    entryNames[i].text = GetEntryName(i); // Use real name
                }
                else
                {
                    entryNames[i].text = "???"; // or "Undiscovered"
                }
            }

            // Find first unlocked entry to show
            if (!foundUnlocked && isUnlocked)
            {
                currentIndex = i;
                foundUnlocked = true;
            }
        }

        if (foundUnlocked)
        {
            entries[currentIndex].SetActive(true);
            Debug.Log("First unlocked entry shown: " + currentIndex);
        }
        else
        {
            Debug.LogWarning("No unlocked entries found.");
        }
    }


    /// <summary>
    /// This function will be called by your "Next" button in VR.
    /// </summary>
    public void NextEntry()
    {
        int originalIndex = currentIndex;
        int attempts = 0;

        do
        {
            currentIndex++;
            if (currentIndex >= entries.Length) currentIndex = 0;

            attempts++;
            if (attempts > entries.Length)
            {
                Debug.LogWarning("Tidak ada entry yang terbuka.");
                return;
            }
        }
        while (!progressInfo.encyclopediaProgress.unlockedEntries[currentIndex]);

        EnableEntry(currentIndex);
    }

    /// <summary>
    /// This function will be called by your "Previous" button in VR.
    /// </summary>
    public void PreviousEntry()
    {
        int attempts = 0;

        do
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = entries.Length - 1;

            attempts++;
            if (attempts > entries.Length)
            {
                Debug.LogWarning("Tidak ada entry yang terbuka.");
                return;
            }
        }
        while (!progressInfo.encyclopediaProgress.unlockedEntries[currentIndex]);

        EnableEntry(currentIndex);
    }


    /// <summary>
    /// Activates the chosen entry and deactivates all others.
    /// </summary>
    /// <param name="id">The index of the entry to display.</param>
    private void EnableEntry(int id)
    {
        for (int i = 0; i < entries.Length; i++)
        {
            entries[i].SetActive(false); // Nonaktifkan semua dulu
        }

        if (progressInfo.encyclopediaProgress.unlockedEntries[id])
        {
            entries[id].SetActive(true); // Aktifkan hanya jika unlocked
        }
        else
        {
            Debug.LogWarning("Entry ini belum di-unlock: " + id);
        }
    }

}