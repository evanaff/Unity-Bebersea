using UnityEngine;

public class Encyclopedia : MonoBehaviour
{
    public GameObject[] entries; // An array of all your bestiary entry GameObjects
    private int currentIndex = 0; // The index of the currently displayed entry

    void Start()
    {
        // When the scene starts, show the very first entry (index 0)
        // and ensure all others are hidden.
        EnableEntry(currentIndex);
    }

    /// <summary>
    /// This function will be called by your "Next" button in VR.
    /// </summary>
    public void NextEntry()
    {
        // Add this line to see if the function is ever called
        Debug.Log("SUCCESS: NextEntry() was called by the button press!");

        // Increase the index by one
        currentIndex++;

        // If the index goes past the end of the array, loop back to the beginning
        if (currentIndex >= entries.Length)
        {
            currentIndex = 0;
        }

        // Display the new entry
        EnableEntry(currentIndex);
    }

    /// <summary>
    /// This function will be called by your "Previous" button in VR.
    /// </summary>
    public void PreviousEntry()
    {
        // Decrease the index by one
        currentIndex--;

        // If the index goes below zero, loop to the end of the array
        if (currentIndex < 0)
        {
            currentIndex = entries.Length - 1;
        }

        // Display the new entry
        EnableEntry(currentIndex);
    }

    /// <summary>
    /// Activates the chosen entry and deactivates all others.
    /// </summary>
    /// <param name="id">The index of the entry to display.</param>
    private void EnableEntry(int id)
    {
        // Loop through all the entries in your array
        for (int i = 0; i < entries.Length; i++)
        {
            // If the current item's index matches the id, activate it. Otherwise, deactivate it.
            entries[i].SetActive(i == id);
        }
    }
}