using UnityEngine;

public class EnvProgressMaterialChanger : MonoBehaviour
{
    public int environmentIndex;
    public ProgressInfo progressInfo;
    public Material completedMaterial;

    private Renderer rend;
    private bool materialChanged = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        CheckAndUpdateMaterial(); // cek langsung saat start
    }

    void Update()
    {
        if (!materialChanged)
        {
            CheckAndUpdateMaterial(); // opsional jika ingin cek berkala
        }
    }

    public void CheckAndUpdateMaterial()
    {
        if (progressInfo == null || rend == null) return;

        var envProgress = progressInfo.environments[environmentIndex];

        if (envProgress.trashCollected >= 100f && envProgress.coralPlanted >= 100f)
        {
            rend.material = completedMaterial;
            materialChanged = true;
        }
    }
}
