using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int maximum;
    public int current;
    public Image mask;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)current / (float)maximum;
        mask.fillAmount = fillAmount;
    }
}
