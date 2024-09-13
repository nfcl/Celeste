using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteAtlasUtil : Editor
{

    [MenuItem("SpriteAtlas/SpriteAtlasCreate")]
    static void SpriteAtlasCreate()
    {

        var items = Selection.objects;

        SpriteAtlas spriteAtlas = null;

        List<Object> fitableObj = new List<Object>();

        for(int i= 0; i < items.Length; ++i)
        {

            if(spriteAtlas == null)
            {
            
                if (items[i] as SpriteAtlas != null)
                {

                    Debug.Log(items[i].name);

                    spriteAtlas = items[i] as SpriteAtlas;

                }

            }

            if (items[i] as Texture2D != null)
            {

                fitableObj.Add(items[i]);

            }

        }

        if (spriteAtlas == null)
        {

            spriteAtlas = new SpriteAtlas();

            spriteAtlas.Add(fitableObj.ToArray());

            AssetDatabase.CreateAsset(spriteAtlas, "Assets/newAtlas.spriteatlas");

            AssetDatabase.SaveAssets();

        }
        else
        {

            spriteAtlas.Add(fitableObj.ToArray());

        }

    }

}
