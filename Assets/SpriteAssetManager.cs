using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class SpriteAssetManager
{
    
    private const string FolderName = "currentSprites";

    public static Sprite SaveSpriteAsAsset(Sprite sprite, string projPath)
    {
        string folderPath = Path.Combine(Application.dataPath, FolderName);
        string relativePath = Path.Combine("Assets", FolderName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }

        var absPath = Path.Combine(folderPath, projPath);
        projPath = Path.Combine(relativePath, projPath);

        File.WriteAllBytes(absPath, ImageConversion.EncodeToPNG(sprite.texture));
        AssetDatabase.Refresh();

        var ti = AssetImporter.GetAtPath(projPath) as TextureImporter;
        ti.spritePixelsPerUnit = sprite.pixelsPerUnit;
        ti.mipmapEnabled = false;
        ti.textureType = TextureImporterType.Sprite;

        EditorUtility.SetDirty(ti);
        ti.SaveAndReimport();

        return AssetDatabase.LoadAssetAtPath<Sprite>(projPath);
    }

    public static void DeleteSpriteAsset(string spriteFileName)
    {
        string spritePath = Path.Combine("Assets", FolderName, spriteFileName);

        if (AssetDatabase.LoadAssetAtPath<Sprite>(spritePath) != null)
        {
            if (AssetDatabase.DeleteAsset(spritePath))
            {
                Debug.Log($"Successfully deleted sprite: {spriteFileName}");
            }
            else
            {
                Debug.LogError($"Failed to delete sprite: {spriteFileName}");
            }
        }
        else
        {
            Debug.LogWarning($"Sprite {spriteFileName} not found in folder '{FolderName}'");
        }
    }
}