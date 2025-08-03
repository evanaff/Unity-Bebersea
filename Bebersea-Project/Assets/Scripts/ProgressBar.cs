//using UnityEngine;
//using UnityEngine.UI;

//[ExecuteInEditMode()]
//public class ProgressBar : MonoBehaviour
//{
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    public int maximum;
//    public int current;
//    public Image mask;

//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        GetCurrentFill();
//    }

//    void GetCurrentFill()
//    {
//        float fillAmount = (float)current / (float)maximum;
//        mask.fillAmount = fillAmount;
//    }
//}


using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    public Image mask;
    public ProgressTypeProvider provider;

    public enum ProgressType
    {
        TrashCollected,
        CoralPlanted,
        AnimalInteracted
    }

    void Update()
    {
        UpdateFill();
    }

    void UpdateFill()
    {
        if (provider == null || provider.progressInfo == null ||
            provider.environmentIndex < 0 || provider.environmentIndex >= provider.progressInfo.environments.Length)
            return;

        var data = provider.progressInfo.environments[provider.environmentIndex];
        float value = 0f;

        switch (provider.progressType)
        {
            case ProgressType.TrashCollected:
                value = data.trashCollected;
                break;
            case ProgressType.CoralPlanted:
                value = data.coralPlanted;
                break;
            case ProgressType.AnimalInteracted:
                value = data.animalInteracted;
                break;
        }

        mask.fillAmount = value / 100f;
    }
}
