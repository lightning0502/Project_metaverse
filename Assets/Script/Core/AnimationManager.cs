using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AnimationManager : Singleton<AnimationManager>
{
    private Dictionary<string, Sprite[]> SpriteDictionary;

    // readonly
    private readonly string Bunny_Idle = "bunny_idle";
    private readonly string Bunny_Running = "bunny_running";
    private readonly string Bunny_Stand = "bunny_stand";

    private readonly string Man_Idle = "man_idle";
    private readonly string Man_Running = "man_running";

    private readonly string Raccoon_Idle = "raccoon_idle";
    private readonly string Raccoon_Running = "raccoon_running";

    // sprite atlas
    public SpriteAtlas Atlas_Raccoon_Idle;
    public SpriteAtlas Atlas_Raccoon_Running;

    private void Awake()
    {
        // initialize
        SpriteDictionary = new Dictionary<string, Sprite[]>();
        // TO DO AtlasManager.GetSprite here

        // bunny
        SpriteDictionary.Add(Bunny_Idle, Resources.LoadAll<Sprite>("Charactor/white_bunny/bunny_idle"));

        SpriteDictionary.Add(Bunny_Running, Resources.LoadAll<Sprite>("Charactor/white_bunny/bunny_running"));
        SpriteDictionary.Add(Bunny_Stand, Resources.LoadAll<Sprite>("Charactor/white_bunny/bunny_stand"));

        // man
        SpriteDictionary.Add(Man_Idle, Resources.LoadAll<Sprite>("Charactor/red_cloth_man/man_idle"));
        SpriteDictionary.Add(Man_Running, Resources.LoadAll<Sprite>("Charactor/red_cloth_man/man_running"));

        // raccoon (atlas)
        SpriteDictionary.Add(Raccoon_Idle, GetSpriteArrayFromAtlas(Atlas_Raccoon_Idle));
        SpriteDictionary.Add(Raccoon_Running, GetSpriteArrayFromAtlas(Atlas_Raccoon_Running));
    }

    private Sprite[] GetSpriteArrayFromAtlas(SpriteAtlas atlas)
    {
        Sprite[] spriteArray = new Sprite[atlas.spriteCount];
        atlas.GetSprites(spriteArray);
        return spriteArray;
    }

    private void SpriteSetter(ref Sprite[] mainSprite, Sprite[] dictionarySprite)
    {
        mainSprite = new Sprite[dictionarySprite.Length];
        mainSprite = dictionarySprite;
    }

    public void SetSpriteAnimation(ref Sprite[] idleArray, ref Sprite[] runningArray, ref Sprite[] etcArray, ref int TEST_SCALE_VALUE)
    {
        int randomValue = Random.Range(0, 3); // TO DO :: DB에서 값을 받아와야함
        switch (randomValue)
        {
            default:
                Debug.Log("Error -> SetSpriteAnimation randomValue : " + randomValue);
                return;

            case 0: // bunny
                SpriteSetter(ref idleArray, SpriteDictionary[Bunny_Idle]);
                SpriteSetter(ref runningArray, SpriteDictionary[Bunny_Running]);
                SpriteSetter(ref etcArray, SpriteDictionary[Bunny_Stand]);
                TEST_SCALE_VALUE = 2;
                return;

            case 1: // man
                SpriteSetter(ref idleArray, SpriteDictionary[Man_Idle]);
                SpriteSetter(ref runningArray, SpriteDictionary[Man_Running]);
                SpriteSetter(ref etcArray, SpriteDictionary[Man_Idle]);
                TEST_SCALE_VALUE = 3;
                return;

            case 2: // raccoon
                SpriteSetter(ref idleArray, SpriteDictionary[Raccoon_Idle]);
                SpriteSetter(ref runningArray, SpriteDictionary[Raccoon_Running]);
                SpriteSetter(ref etcArray, SpriteDictionary[Raccoon_Idle]);
                TEST_SCALE_VALUE = 1;
                return;

            case 3:
                Debug.Log("????????????????????????????????????????????????????");
                return;
        }
    }
}