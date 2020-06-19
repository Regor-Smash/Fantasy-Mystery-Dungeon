using UnityEngine;
using UnityEditor;

public class SpriteImporter : AssetPostprocessor
{
    private int defaultPixSize = 64;
    void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;

        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.filterMode = FilterMode.Point;
        textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;
        if(texture.width == defaultPixSize)
        {
            textureImporter.spritePixelsPerUnit = defaultPixSize;
        }
    }
}
