using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class TextureImportSettingsEditor : AssetPostprocessor
{
    // This function is called when a texture is imported
    public void OnPreprocessTexture()
    {
        string lowerCaseAssetPath = assetPath.ToLower();

        // Skip textures with platform specific import settings already set up
        if (((TextureImporter)assetImporter).GetPlatformTextureSettings("Standalone") != null) return;

        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.maxTextureSize = 2048;
        textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;
        textureImporter.SetPlatformTextureSettings("Standalone", 2048, TextureImporterFormat.BC7);

        if (lowerCaseAssetPath.Contains("normal"))
        {
            textureImporter.textureType = TextureImporterType.NormalMap;
            textureImporter.SetPlatformTextureSettings("Standalone", 2048, TextureImporterFormat.BC5);
        }

        if (lowerCaseAssetPath.Contains("mask"))
        {
            textureImporter.textureType = TextureImporterType.Default;
            textureImporter.sRGBTexture = false;
            textureImporter.SetPlatformTextureSettings("Standalone", 1024, TextureImporterFormat.BC4);
        }

        if (lowerCaseAssetPath.Contains("mochie") ||
            new[] { "metallic", "smoothness", "roughness", "specular" }.Count(word => lowerCaseAssetPath.Contains(word)) >= 2)
        {
            textureImporter.textureType = TextureImporterType.Default;
            textureImporter.sRGBTexture = false;
            textureImporter.SetPlatformTextureSettings("Standalone", 1024, TextureImporterFormat.BC5);
        }

        if (Directory.GetParent(lowerCaseAssetPath).Name.ToLower().Contains("icon"))
        {
            textureImporter.SetPlatformTextureSettings("Standalone", 256, TextureImporterFormat.BC7);
            textureImporter.alphaIsTransparency = true;
        }
    }
}
