using UnityEngine;
using UnityEditor;
using System.IO;

public class MaterialImporter : EditorWindow
{
    [MenuItem("Tools/Set Legacy External Materials")]
    static void SetMaterialsToExternal()
    {
        string folderPath = "Assets/Pandazole_Ultimate_Pack/Pandazole_Nature_Environment_Pack/Models";  // Ganti sesuai dengan lokasi folder kamu
        string[] guids = AssetDatabase.FindAssets("t:Model", new[] { folderPath });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;

            if (importer != null)
            {
                importer.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
                importer.materialLocation = ModelImporterMaterialLocation.External;
                importer.materialName = ModelImporterMaterialName.BasedOnMaterialName;
                importer.materialSearch = ModelImporterMaterialSearch.Everywhere;

                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                Debug.Log($"Updated: {path}");
            }
        }

        Debug.Log("Selesai mengatur semua model ke External (Legacy).");
    }
}
