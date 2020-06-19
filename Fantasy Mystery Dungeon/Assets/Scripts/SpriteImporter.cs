using UnityEngine;
using UnityEditor;

public class SpriteImporter : AssetPostprocessor
{
    void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;

        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.filterMode = FilterMode.Point;
    }
}
