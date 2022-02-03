using System.Collections;
using UnityEngine;

public class SpriteAnimationComponent : MonoBehaviour
{
    public bool OnMoveAnimation;

    private SpriteRenderer PlayerSpriteRenderer;
    public bool SpriteFlipX
    {
        set
        {
            PlayerSpriteRenderer.flipX = value;
        }

        get
        {
            return PlayerSpriteRenderer.flipX;
        }
    }

    private readonly int ReadonlyInt_ETCRate = 70; // 0 ~ 99, 71%

    private Sprite[] SpriteArray_Idle;
    private int SpriteLength_Idle;

    private Sprite[] SpriteArray_Running;
    private int SpriteLength_Running;

    private Sprite[] SpriteArray_ETC;
    private int SpriteLength_ETC;

    private enum AnimationState : int
    {
        NONE = 0,

        IDLE = 10,
        RUNNING = 11,

        ETC = 20
    }
    private AnimationState CurrentPlayerState;

    public void AnimationStart(ref int TEST_SCALE_VALUE)
    {
        OnMoveAnimation = false;
        PlayerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        AnimationManager.Instance.SetSpriteAnimation(ref SpriteArray_Idle, ref SpriteArray_Running, ref SpriteArray_ETC, ref TEST_SCALE_VALUE);
        SpriteLength_Idle = SpriteArray_Idle.Length;
        SpriteLength_Running = SpriteArray_Running.Length;
        SpriteLength_ETC = SpriteArray_ETC.Length;

        StartCoroutine(StateAnimation());
    }

    private IEnumerator StateAnimation()
    {
        int spriteCount = 0, randomValue;

        while (gameObject.activeSelf)
        {
            if (OnMoveAnimation)
                CurrentPlayerState = AnimationState.RUNNING;

            else
            {
                randomValue = Random.Range(0, 99);
                CurrentPlayerState = randomValue < ReadonlyInt_ETCRate ? AnimationState.IDLE : AnimationState.ETC;
            }

            switch (CurrentPlayerState)
            {
                case AnimationState.IDLE:
                    while (OnMoveAnimation == false && SpriteLength_Idle != spriteCount)
                    {
                        PlayerSpriteRenderer.sprite = SpriteArray_Idle[spriteCount];
                        yield return Coop.WaitForSeconds(0.15f);
                        ++spriteCount;
                    }

                    spriteCount = 0;
                    break;

                case AnimationState.ETC:
                    while (OnMoveAnimation == false && SpriteLength_ETC != spriteCount)
                    {
                        PlayerSpriteRenderer.sprite = SpriteArray_ETC[spriteCount];
                        yield return Coop.WaitForSeconds(0.2f);
                        ++spriteCount;
                    }

                    spriteCount = 0;
                    break;

                case AnimationState.RUNNING:
                    while (OnMoveAnimation)
                    {
                        PlayerSpriteRenderer.sprite = SpriteArray_Running[spriteCount];
                        yield return Coop.WaitForSeconds(0.1f);
                        ++spriteCount;

                        if (spriteCount == SpriteLength_Running)
                            spriteCount = 0;
                    }

                    spriteCount = 0;
                    break;
            }
        }

        yield return Coop.WaitForSeconds(1);
        Debug.Log("animation end!");
    }
}