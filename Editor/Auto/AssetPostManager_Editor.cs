using UnityEditor;
using UnityEngine;

public class AssetPostManager_Editor : AssetPostprocessor
{
    void OnPostprocessTexture(Texture texture)
    {
        //if (assetPath.StartsWith("Assets/Resources/Image"))
        //{
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if ((importer != null) && (importer.textureType != TextureImporterType.Sprite))
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.SaveAndReimport();
            }
       // }
    }
}
