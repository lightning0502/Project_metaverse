using UnityEngine;
using UnityEngine.U2D;
using System.Text;
using Extension;
using System.Collections.Generic;

// GetSprite를 한 번 사용해서 얻은 sprite를 보관해놓고 이후에 또 같은 sprite가 호출된다면 AtlasDictionary안에 있는 sprite를 준다.
public class AtlasManager : Singleton<AtlasManager>
{
    // ui
    // public SpriteAtlas Atlas_NPC;
    public SpriteAtlas Atlas_MapObject;

    // value
    private Dictionary<string, Sprite> AtlasDictionary;

    public enum AtlasType : int
    {
        NPC = 0,
        MapObject = 1,
    }

    // readonly
    // private readonly string ReadonlyString_NPC = "NPC_";
    // private readonly string ReadonlyString_MapObject = "MapObject_";

    public void Awake()
    {
        AtlasDictionary = new Dictionary<string, Sprite>();
    }

    /// <param name="type"> AtlasManager.AtlasType. + Ctrl + Space </param>
    public Sprite GetAtlasSprite(AtlasType type, string imageName)
    {
        switch (type)
        {
            default:
                Debug.LogError("Error -> GetAtlasSprite AtlasType : " + type);
                return null;

            // case AtlasType.NPC:
            // return CheckContains(ref Atlas_NPC, MethodExtension.ToStringBuilder(ReadonlyString_NPC, imageName));

            case AtlasType.MapObject:
                // return CheckContains(ref Atlas_MapObject, MethodExtension.ToStringBuilder(ReadonlyString_MapObject, imageName));
                return null;
        }
    }

    private Sprite CheckContains(ref SpriteAtlas atlas, string spriteName)
    {
        if (AtlasDictionary.ContainsKey(spriteName))
            return AtlasDictionary[spriteName];

        AtlasDictionary.Add(spriteName, atlas.GetSprite(spriteName));
        return AtlasDictionary[spriteName];
    }
}