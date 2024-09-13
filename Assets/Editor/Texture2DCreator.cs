using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Texture2DCreator
{

    [MenuItem("Custom/SpriteAtlasImporter")]
    static void SaveSprite()
    {
        string resourcesPath = "Assets/Resources/";
        foreach (Object obj in Selection.objects)
        {

            string selectionPath = AssetDatabase.GetAssetPath(obj);

            // 必须最上级是"Assets/Resources/"
            if (selectionPath.StartsWith(resourcesPath))
            {
                string selectionExt = System.IO.Path.GetExtension(selectionPath);
                if (selectionExt.Length == 0)
                {
                    continue;
                }

                // 从路径"Assets/Resources/UI/testUI.png"得到路径"UI/testUI"
                string loadPath = selectionPath.Remove(selectionPath.Length - selectionExt.Length);
                loadPath = loadPath.Substring(resourcesPath.Length);
                Debug.Log(loadPath);
                // 加载此文件下的所有资源
                Sprite[] sprites = Resources.LoadAll<Sprite>(loadPath);
                if (sprites.Length > 0)
                {
                    // 创建导出文件夹
                    string outPath = "E:" + "/outSprite/" + loadPath;
                    System.IO.Directory.CreateDirectory(outPath);

                    foreach (Sprite sprite in sprites)
                    {
                        Debug.Log(sprite.name);
                        // 创建单独的纹理
                        Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height, sprite.texture.format, false);
                        tex.SetPixels(sprite.texture.GetPixels((int)sprite.rect.xMin, (int)sprite.rect.yMin,
                            (int)sprite.rect.width, (int)sprite.rect.height));
                        tex.Apply();

                        // 写入成PNG文件
                        System.IO.File.WriteAllBytes(outPath + "/" + sprite.name + ".png", tex.EncodeToPNG());
                    }
                    Debug.Log("SaveSprite to " + outPath);
                }
            }
        }
        Debug.Log("SaveSprite Finished");
    }

    [MenuItem("Custom/CreatePixelBloom/Strawberry")]
    static void CreatePixelBloom_Strawberry()
    {

        int TextureWidth = 20;
        int TextureHeight = 27;

        Vector2 Center = new Vector2(TextureWidth / 2.0f - 0.5f, TextureHeight / 2.0f - 0.5f);

        Vector2 Raid = new Vector2(8, 10.5f);

        Vector2 CurrentPos = new Vector2();

        Vector2 buf;

        Texture2D Result = new Texture2D(TextureWidth, TextureHeight);

        for (int x = 0; x < TextureWidth; ++x)
        {

            CurrentPos.x = x;

            for (int y = 0; y < TextureHeight; ++y)
            {

                CurrentPos.y = y;

                buf = CurrentPos - Center;

                buf.x /= Raid.x;
                buf.y /= Raid.y;

                Result.SetPixel(x, y, new Color(1, 1, 1, Mathf.Clamp(1 - Mathf.Sqrt(buf.x * buf.x + buf.y * buf.y), 0, 1)));

            }

        }

        Result.Apply();

        System.IO.File.WriteAllBytes("E:\\Unity project\\Celeste\\Assets\\Editor\\bloom.png", Result.EncodeToPNG());

    }

    [MenuItem("Custom/CreatePixelBloom/DashCrystal")]
    static void CreatePixelBloom_DashCrystal()
    {

        int TextureWidth = 20;
        int TextureHeight = 20;

        Vector2 Center = new Vector2(TextureWidth / 2.0f - 0.5f, TextureHeight / 2.0f - 0.5f);

        Vector2 Raid = new Vector2(8.5f, 8.5f);

        Vector2 CurrentPos = new Vector2();

        Vector2 buf;

        Texture2D Result = new Texture2D(TextureWidth, TextureHeight);

        for (int x = 0; x < TextureWidth; ++x)
        {

            CurrentPos.x = x;

            for (int y = 0; y < TextureHeight; ++y)
            {

                CurrentPos.y = y;

                buf = CurrentPos - Center;

                buf.x /= Raid.x;
                buf.y /= Raid.y;

                Result.SetPixel(x, y, new Color(1, 1, 1, Mathf.Clamp(1 - (Mathf.Abs(buf.x) + Mathf.Abs(buf.y)), 0, 1)));

            }

        }

        Result.Apply();

        System.IO.File.WriteAllBytes("E:\\Unity project\\Celeste\\Assets\\Editor\\DashCrystal_bloom.png", Result.EncodeToPNG());

    }

    [MenuItem("Custom/CreateTilemapTexture")]
    static void CreateTileMapTexture()
    {

        var selectItem = Selection.activeGameObject;

        if(selectItem.TryGetComponent(out Tilemap tilemap))
        {

            var bounds = tilemap.cellBounds;

            Texture2D Result = new Texture2D(bounds.size.x * 8, bounds.size.y * 8);

            Sprite bufsprite;

            for (int x = 0; x < bounds.size.x; ++x)
            {

                for (int y = 0; y < bounds.size.y; ++y)
                {

                    bufsprite = tilemap.GetSprite(new Vector3Int(x + bounds.min.x, y + bounds.min.y, 0));

                    if (bufsprite == null)
                    {

                        for(int ix = 0; ix < 8; ++ix)
                        {

                            for(int iy = 0; iy < 8; ++iy)
                            {

                                Result.SetPixel(x * 8 + ix, y * 8 + iy, new Color(1, 1, 1, 0));

                            }

                        }

                        continue;

                    }

                    //Result.SetPixels(x * 8, y * 8, 8, 8, bufsprite.texture.GetPixels((int)bufsprite.rect.xMin, (int)bufsprite.rect.yMin, 8, 8));

                    for (int ix = 0; ix < 8; ++ix)
                    {

                        for (int iy = 0; iy < 8; ++iy)
                        {

                            Result.SetPixel(x * 8 + ix, y * 8 + iy, bufsprite.texture.GetPixel((int)bufsprite.rect.xMin + ix * 4, (int)bufsprite.rect.yMin + iy * 4));

                        }

                    }

                }

            }

            Result.Apply();

            System.IO.File.WriteAllBytes(Application.dataPath+ "\\Editor\\TileMapTexture.png", Result.EncodeToPNG());

        }

    }

}
